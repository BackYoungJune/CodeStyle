/*********************************************************					
* ProjectileSkill.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.19 오후 7:35					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using Dev_Unit;
using MoreMountains.Feedbacks;

namespace Dev_UnitSkill
{					
	public enum ProjectileType
    {
		None,
		Idle,			// 가만히 있는 발사체
		Straight,		// 직선형 발사체
		Rotate,			// 회전형 발사체
		Arc,			// 포물선형 발사체
			
    }



	public abstract class ProjectileSkill : SkillBase
	{
		//--------------------------------------------------------					
		// 추상함수			
		//--------------------------------------------------------	
		public abstract void OnHit();

		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		public virtual void Initializing(UnitBase target, string targetTag, GestureReferee curReferee, float _scale = 1.0f)
		{
			TargetUnit = target;
			pTargetUnitTag = targetTag;
			CurReferees = curReferee;
			this.transform.localScale = Vector3.one * normalScale * _scale;
			this.tag = DevUtils.GetTag(DevDefine.ETag.Weapon);
		}


		public void ChangeStateMachine(ProjectileType type)
        {
			if (StateMachine == SkillStateCache[type])
            {
				Debug.LogError("[ProjectileSkill] 동일한 State로 변경 불가!");
				return;
			}
			StateMachine.InitState(SkillStateCache[type]);
			CurType = type;
		}

		
		public ProjectileType CurType { get; set; }
		public UnitBase pTargetUnit { get { return TargetUnit; } set { TargetUnit = value; } }
		public string pTargetUnitTag { get; set; }
		public float pMoveSpeed { get { return MoveSpeed; } }
		public float pRotSpeed { get { return RotSpeed; } }
		public GestureReferee pCurReferees { get { return CurReferees; } }
		public Rigidbody pRigidBody { get { return base.RigidBody; } }
		public Vector3 pRotateVector { get { return RotateVector; } }
		public float pTargetUpPercent { get { return TargetUpPercent; } }
		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------	
		[Header("---------- Projectile Skill Settings ----------")]
		[SerializeField] protected float MoveSpeed;
		[SerializeField] protected float RotSpeed;
		[SerializeField] protected MMF_Player Perfect_HitFeedback;
		[SerializeField] protected MMF_Player Good_HitFeedback;
		[SerializeField] protected float normalScale = 1.0f;
		[SerializeField] protected Vector3 RotateVector = Vector3.up;
		[SerializeField] protected float TargetUpPercent = 0;
		[Header("---------- DEBUG ----------")]
		[SerializeField] protected UnitBase TargetUnit;
		[SerializeField] protected GestureReferee CurReferees;
		protected SkillStateMachine<ProjectileSkill> StateMachine;				
		protected Dictionary<ProjectileType, SkillInterface<ProjectileSkill>> SkillStateCache = new();


		protected virtual void Awake()
        {
			base.InitSkillBase(this);

			//Skill 상태 캐싱
			SkillStateCache.Add(ProjectileType.None, null);
			SkillStateCache.Add(ProjectileType.Idle, new ProjectileIdle());
			SkillStateCache.Add(ProjectileType.Straight, new ProjectileStraight());
			SkillStateCache.Add(ProjectileType.Rotate, new ProjectileRotate());
			SkillStateCache.Add(ProjectileType.Arc, new ProjectileArc());
			StateMachine = new SkillStateMachine<ProjectileSkill>(this, SkillStateCache[ProjectileType.None]);
		}

		protected virtual void OnEnable()
		{

		}

		protected virtual void Start()
        {

        }

		protected virtual void Update()
        {
			if (StateMachine.CurState != null)
			{
				StateMachine.CurState.UpdateExcute();
			}

		}

		protected virtual void OnDisable()
        {

        }


		protected virtual void OnTriggerEnter(Collider col)
        {
			if (StateMachine.CurState != null)
			{
				StateMachine.CurState.OnTriggerEnter(col);
			}
		}




	}//end of class					
					
					
}//end of namespace					