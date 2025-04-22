/*********************************************************					
* PhysicalSkill.cs					
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
	public enum PhysicalType
    {
		None,
		Fixed			//고정 소환형
    }


	public abstract class PhysicalSkill : SkillBase
	{
		//--------------------------------------------------------					
		// 추상함수			
		//--------------------------------------------------------	
		public abstract void OnHit();

		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------						
		public void Initializing(string targetTag, GestureReferee curReferee)
        {
			pTargetUnitTag = targetTag;
			CurReferees = curReferee;
		}


		public void ChangeStateMachine(PhysicalType type)
        {
			if (StateMachine == SkillStateCache[type])
            {
				Debug.LogError("[PhysicalSkill] 동일한 State로 변경 불가!");
				return;
			}
			StateMachine.InitState(SkillStateCache[type]);
			CurType = type;
		}


		public string pTargetUnitTag { get; set; }
		public PhysicalType CurType { get; set; }
		public GestureReferee pCurReferees { get { return CurReferees; } }
		public Rigidbody pRigidBody { get { return base.RigidBody; } }

		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		[Header("---------- Projectile Skill Settings ----------")]
		[SerializeField] protected MMF_Player InitFeedback;
		[SerializeField] protected MMF_Player HitFeedback;
		[Header("---------- DEBUG ----------")]
		[SerializeField] protected GestureReferee CurReferees;
		protected SkillStateMachine<PhysicalSkill> StateMachine;
		protected Dictionary<PhysicalType, SkillInterface<PhysicalSkill>> SkillStateCache = new();



		protected virtual void Awake()
		{
			base.InitSkillBase(this);

			//Skill 상태 캐싱
			SkillStateCache.Add(PhysicalType.None, null);
			SkillStateCache.Add(PhysicalType.Fixed, new PhysicalFixed());
			StateMachine = new SkillStateMachine<PhysicalSkill>(this, SkillStateCache[PhysicalType.None]);
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