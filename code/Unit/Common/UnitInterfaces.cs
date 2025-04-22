/*********************************************************					
* UnitInterfaces.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.29 오후 1:30					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Unit
{
	//--------------------------------------------------------					
	//			유닛인터페이스		
	//--------------------------------------------------------					
	public interface UnitInterface<T>
	{
		void Enter(T unit);
		void UpdateExcute();
		void Exit();
		void OnTriggerEnter(Collider col);
	}

	public enum UnitState
    {
		//------------------
		//		공통
		//------------------
		None,
		CutScene,
		MoveSpline,
		BattleIdle,
		BattleAttack,
		BattleDead,
		Idle,

		//------------------
		//		플레이어
		//------------------
		ROM,
		FitnessIdle,
		FitnessAction,
		
		BattleGuard,
		BattleEnd,
		BattleExercise,

		//------------------
		//		에너미
		//------------------
		BattleHit,


		//------------------
		//		NPC
		//------------------
		LookAt,

        //------------------
        //		플레이어
        //------------------
        Climb,
		OtherTransport,

        //------------------
        //		NPC
        //------------------
		FastSpline,
		MoveSpline_NoLookAt,
	}


    public enum UnitMoveType
    {
		Stop, Walk, Run
    }


	//--------------------------------------------------------					
	//			유닛인터페이스 스테이트머신			
	//--------------------------------------------------------			
	[System.Serializable]		
	public class UnitStateMachine<T>
	{
		public UnitInterface<T> CurState { get { return curState; } set { curState = value; } }

		[SerializeField] private UnitInterface<T> curState;
		[SerializeField] private T Sender;

		public UnitStateMachine(T sender, UnitInterface<T> state)
        {
			Sender = sender;
			InitState(state);
		}


		public void InitState(UnitInterface<T> newState)
        {
			if (Sender == null)
            {
				Debug.LogError("[UnitStateMachine] Sender is null");
				return;
            }
			if (curState == newState)
            {
				return;
            }
			if(curState != null)
            {
				curState.Exit();
            }

			curState = newState;
			curState.Enter(Sender);
        }

	}//end of class



}//end of namespace					