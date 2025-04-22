/*********************************************************					
* UnitExercise.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.12.21 오후 1:03					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
using Dev_System;

// **************Namespace를 반드시 수정하세요**************					
namespace Dev_Unit					
{					
	public enum ExerciseState
    {
		None, Idle, Prepare, Shoot,
    }

	public enum ExerciseStage
    {
		None, Carrot, Torch, Grass, Honey, HoneyWater, Fishing, Okonomi, Dango, 
		Washiabi, Sushi, MountBread, WoodStick, CampFire, Plank, PlankHammer,
		FishMove, MeatSkewer, Statue, WishingCandle, ArrangeItem, ArrangeItem2,
		MapUse, MapUse2, Concert,
    }

	public abstract class UnitExercise : UnitBase
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		protected override void InitUnit(UnitState state)
        {
            // UnitManager 캐싱
            Managers.Unit.AddUnit(this);

            //Exercise 상태 캐싱
            ExerciseStateCache.Add(ExerciseState.None, null);
            ExerciseStateCache.Add(ExerciseState.Idle, new ExerciseIdle());
            ExerciseStateCache.Add(ExerciseState.Prepare, new ExercisePrepare());
            ExerciseStateCache.Add(ExerciseState.Shoot, new ExerciseShoot());
            StateMachine = new UnitStateMachine<UnitExercise>(this, ExerciseStateCache[ExerciseState.None]);
			ChangeStateMachine(ExerciseState.Idle);
        }
		
		public void ChangeStateMachine(ExerciseState state)
		{
			if (StateMachine == ExerciseStateCache[state])
			{
				Debug.LogError("[UnitNPC] 동일한 State로 변경 불가!");
				return;
			}

			StateMachine.InitState(ExerciseStateCache[state]);
		}


		[SerializeField] protected List<MMF_Player> PrePareFeedbacks;
		[SerializeField] protected List<MMF_Player> ShootFeedbacks;
		[SerializeField] protected MMF_Player DisableFeedback;

		//[HideInInspector] public int curExercisingNum;			// 현재 운동 횟수
		[HideInInspector] public int ExerciseObjectNum;			// Exercise 오브젝트 갯수 (당근갯수) 
		[HideInInspector] public int ExerciseCount;             // 오브젝트당 진행될 운동 갯수
		[HideInInspector] public UnityAction PrePareEvent;		// PrePare시 호출될 이벤트
		[HideInInspector] public UnityAction ShootEvent;        // Shoot시 호출될 이벤트
		[HideInInspector] public ExerciseStage ExerciseStage { get { return exerciseStage; } }   // Exercise 스테이지
		[HideInInspector] public bool FirstExercise = false;	// 첫번째 오브젝트인지
		[HideInInspector] public bool lastExercise = false;		// 마지막 오브젝트인지
		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		[SerializeField] private ExerciseStage exerciseStage;

		protected UnitStateMachine<UnitExercise> StateMachine;
		protected Dictionary<ExerciseState, UnitInterface<UnitExercise>> ExerciseStateCache = new();

		

		//--------------------------------------------------------					
		// 추상함수			
		//--------------------------------------------------------		
		public abstract void Idle();
		public abstract void PrePare();
		public abstract void Shoot();

		protected virtual void Awake()
		{

		}

		protected virtual void OnEnable()
		{

		}

		protected virtual void Start()
		{

		}

		protected virtual void Update()
		{
			//if(GameManager.Instance.IsDebug)
			//{
			//	if(Input.GetKeyDown(KeyCode.Q))
			//	{
			//		PrePare();
			//	}

			//	if(Input.GetKeyUp(KeyCode.W))
			//	{
			//		Shoot();
			//	}
			//}
		}

		protected virtual void OnDisable()
		{
            Managers.Unit.RemoveUnit(this);
			Managers.Unit.CurTurnExerciseUnitIndex--;
        }

	}//end of class					
}//end of namespace					