/*********************************************************					
* UnitNPC.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.29 오후 7:12					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using Dreamteck.Splines;
using PixelCrushers.DialogueSystem;
using Dev_Spline;
using Dev_System;

namespace Dev_Unit
{
	//public enum NPCState
	//{
	//	None = -1,			//None
	//	CutScene = 0,		//컷씬
	//	MoveSpline = 1,		//플레이어 따라 이동
	//	LookAt = 2			//플레이어 LookAt
	//}

	public struct NPCAnimatorID
	{
		public int AnimID_MotionSpeed;
		public int AnimID_MoveSpeed;
		public int AnimID_HurryUp;
		public int AnimID_OnBattle;
		public int AnimID_Victory;
		public int AnimID_Defeat;
		public int AnimID_Drawn;

		//다른 이동수단
		public int AnimaID_Transport;
		public int AnimID_Climb;
		public int AnimID_Boat;
	}


	[RequireComponent(typeof(Animator))]
	public abstract class UnitNPC : UnitBase
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		protected override void InitUnit(UnitState state)
        {
            //UnitManager 캐싱
            Managers.Unit.AddUnit(this);

			//Player 캐싱
			player = (UnitPlayer)Managers.Unit.GetCurTurnUnit;

            Animatorbase = this.GetComponent<Animator>();
        }

		public Animator nAnimatorbase { get { return Animatorbase; } }
		public UnitPlayer Player { get { return player; } }
		public NPCAnimatorID nAnimatorIDTable { get { return AnimatorIDTable; } }
        public UnitState CurState { get { return curState; } set { curState = value; } }
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------		
        [Header("---------- State Debug ----------")]
        [SerializeField] protected UnitState curState;

        protected NPCAnimatorID AnimatorIDTable;
		protected Animator Animatorbase;
		protected UnitPlayer player;

        protected virtual void Awake(){}
        protected virtual void OnEnable(){}
        protected virtual void Start(){}
		protected virtual void Update(){}

        protected virtual void OnDisable()
		{
            Managers.Unit.RemoveUnit(this);
		}

        protected virtual void AssignAnimatorIDs()
        {
            AnimatorIDTable.AnimID_MoveSpeed = Animator.StringToHash("MovementSpeed");
            AnimatorIDTable.AnimID_MotionSpeed = Animator.StringToHash("MotionSpeed");
            AnimatorIDTable.AnimID_HurryUp = Animator.StringToHash("HurryUp");
            AnimatorIDTable.AnimID_OnBattle = Animator.StringToHash("OnBattle");
            AnimatorIDTable.AnimaID_Transport = Animator.StringToHash("Transport");
            AnimatorIDTable.AnimID_Climb = Animator.StringToHash("Climb");
            AnimatorIDTable.AnimID_Boat = Animator.StringToHash("Boat");

            AnimatorIDTable.AnimID_Victory = Animator.StringToHash("Victory");
            AnimatorIDTable.AnimID_Defeat = Animator.StringToHash("Drawn");
            AnimatorIDTable.AnimID_Drawn = Animator.StringToHash("Defeat");
        }

    }//end of class					


}//end of namespace					