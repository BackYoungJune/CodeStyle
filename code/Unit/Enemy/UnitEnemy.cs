/*********************************************************					
* UnitEnemy.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.29 오후 1:24					
**********************************************************/									
using System.Collections.Generic;					
using UnityEngine;
using MoreMountains.Feedbacks;
using Dev_System;

namespace Dev_Unit
{
	//public enum EnemyState
	//{
	//	None = -1,		//None
	//	Idle = 1,		//전투 idle
	//	Attack,			//전투 Attack
	//	Hit				//전투 Hit
	//}

	public struct EnemyAnimatorID
    {
		public int AnimID_AttackThrow;       //Attack
		public int AnimID_OnBattle;			 //OnBattle
		public int[] AnimID_Hits;			 //Hit
		public int AnimID_Dead;				 //Dead
	}

	public enum EnemyScale
	{
		None,
		Small,
		Medium,
		Large,
	}

	[RequireComponent(typeof(Animator))]
	public abstract class UnitEnemy : UnitBase
	{

		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------
		
		protected override void InitUnit(UnitState state)
        {
            //UnitManager 캐싱
            Managers.Unit.AddUnit(this);

            //Enemy 상태 캐싱
            EnemyStateCache.Add(UnitState.None, null);
			EnemyStateCache.Add(UnitState.BattleIdle, new EnemyBattleIdle());
			EnemyStateCache.Add(UnitState.BattleAttack, new EnemyBattleAttack());
			EnemyStateCache.Add(UnitState.BattleHit, new EnemyBattleHit());
			EnemyStateCache.Add(UnitState.BattleDead, new EnemyBattleDead());
			StateMachine = new UnitStateMachine<UnitEnemy>(this, EnemyStateCache[UnitState.None]);
			Managers.Battle.TurnOverAction = null;
			Managers.Battle.TurnOverAction += TurnOver;

            ChangeStateMachine(state);
		}



		public void ChangeStateMachine(UnitState state)
        {
			if (StateMachine == EnemyStateCache[state])
            {
				Debug.LogError("[UnitEnemy] 동일한 State로 변경 불가!");
				return;
            }

			StateMachine.InitState(EnemyStateCache[state]);
			curState = state;
		}

		//public bool IsCurAnimationEndFrame
		//{
		//	get
		//	{
		//		return Animatorbase.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f ? true : false;
		//	}
		//}


		public Animator pAnimatorbase { get { return Animatorbase; } }
		public EnemyAnimatorID pAnimatorIDTable { get { return AnimatorIDTable; } }
		public UnitState CurState { get { return curState; } set { curState = value; } }
		public string AttackerTag = "Weapon";
		public EnemyScale pEnemyScale { get { return EnemyScale; } }
		public BattleResult pEnemyResult { get { return EnemyResult; } }
		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------	
		[Header("---------- Feel Feedback Settings ----------")]
		[SerializeField] protected MMF_Player AttackFeedback;
		[SerializeField] protected MMF_Player HitFeedback;
		[SerializeField] protected MMF_Player DeadFeedback;

		[Header("---------- 임시 연출 매터리얼 ----------")]
		[SerializeField] protected SkinnedMeshRenderer ModelMeshRenderer;
		[SerializeField] protected Material NormalMat;
		[SerializeField] protected Material OnDamageMat;
		[SerializeField] protected Material OnFinalDamageMat;

		[Header("---------- State Debug ----------")]
		[SerializeField] UnitState curState;

		[Header("-------------Enemy Setting---------------")]
		[SerializeField] protected EnemyScale EnemyScale = EnemyScale.None;
		[SerializeField] protected BattleResult EnemyResult = BattleResult.Victory;

        protected UnitStateMachine<UnitEnemy> StateMachine;
		protected Dictionary<UnitState, UnitInterface<UnitEnemy>> EnemyStateCache = new();
		protected Animator Animatorbase;
		protected EnemyAnimatorID AnimatorIDTable;
		

		//--------------------------------------------------------					
		// 추상함수			
		//--------------------------------------------------------		
		public abstract void Idle();						//대기동작 - 전투
		public abstract void OnAttack(Transform genpos);	//공격 - 전투
		public abstract void OnDamage();					//데미지받기 - 전투
		public abstract void Dead();						//에너미 사망



		protected virtual void Awake()
		{	
			Animatorbase = this.GetComponent<Animator>();
		}
		 

		protected virtual void OnEnable()
		{
			
		}


		protected virtual void Start()
		{
			

		}


		protected virtual void Update()
        {
			if (StateMachine != null)
			{
				if (StateMachine.CurState != null)
				{
					StateMachine.CurState.UpdateExcute();
				}
			}
		}

		protected virtual void OnDisable()
        {
			Managers.Unit.RemoveUnit(this);
		}

		protected virtual void OnTriggerEnter(Collider col)
		{
			if (StateMachine.CurState != null)
			{
				if(Managers.Battle != null)
				{
					if(Managers.Battle.pCurTurnState == TurnState.PlayerTurn)
					{
                        StateMachine.CurState.OnTriggerEnter(col);
                    }
				}
			}
		}

		protected virtual void TurnOver()
		{

		}

		//--------------------------------------------------------					
		//          Animation Events	
		//--------------------------------------------------------	



	}//end of class					


}//end of namespace					