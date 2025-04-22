/*********************************************************					
* TrainerInterfaces.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_System;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Unit
{
    public class TrainerCutScene : UnitInterface<UnitTrainer>
    {
        private UnitTrainer CurTrainer;

        public void Enter(UnitTrainer unit)
        {
            CurTrainer = unit;
            CurTrainer.pAnimatorbase.SetFloat(CurTrainer.pAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurTrainer.pAnimatorbase.SetFloat(CurTrainer.pAnimatorIDTable.AnimID_MoveSpeed, 0);
        }

        public void UpdateExcute()
        {

        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider col)
        {


        }

    }//end of class			

    public class TrainerFitnessIdle : UnitInterface<UnitTrainer>
    {
        private UnitTrainer CurTrainer;

        public void Enter(UnitTrainer unit)
        {
            CurTrainer = unit;
            Managers.Battle.BattleManagerMode = BattleManagerMode.Fitness;

            //플레이어 Fitness Idle True
            CurTrainer.OnIdle(CurTrainer.pAnimatorIDTable.AnimID_OnFitness, true, 1.5f);

            //플레이어 애니메이션 보호코드
            CurTrainer.pAnimatorbase.SetFloat(CurTrainer.pAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurTrainer.pAnimatorbase.SetFloat(CurTrainer.pAnimatorIDTable.AnimID_MoveSpeed, 0);

            if (Managers.Battle.IsFirstFitness == false)
            {
                Managers.Battle.IsFirstFitness = true;
                CurTrainer.SetTriggerCompare(CurTrainer.pAnimatorIDTable.AnimID_Emo_Greeting, true);
                Debug.Log("----- 피트니스 최초 시작 11111-----");
            }
        }

        public void UpdateExcute()
        {
            //if (Managers.Battle.IsFirstFitness == false)
            //{
            //    Managers.Battle.IsFirstFitness = true;
            //    CurPlayer.pAnimatorbase.SetTrigger(CurPlayer.pAnimatorIDTable.AnimID_Emo_Greeting);
            //    Debug.LogError("----- 피트니스 최초 시작 22222-----");
            //}


        }

        public void Exit()
        {
            if(Managers.Battle.pCurTurnState == TurnState.EndBattleTurn)
            {
                CurTrainer.OnIdle(CurTrainer.pAnimatorIDTable.AnimID_OnFitness, false, 0f);
            }
        }

        public void OnTriggerEnter(Collider col)
        {

        }
    }//end of class

    public class TrainerFitnessAction : UnitInterface<UnitTrainer>
    {
        private UnitTrainer CurTrainer;

        public void Enter(UnitTrainer unit)
        {
            CurTrainer = unit;

            //플레이어 애니메이션 보호코드
            CurTrainer.pAnimatorbase.SetFloat(CurTrainer.pAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurTrainer.pAnimatorbase.SetFloat(CurTrainer.pAnimatorIDTable.AnimID_MoveSpeed, 0);

            //현재 타겟 피트니스운동 수행
            CurTrainer.SetTriggerCompare(Managers.Battle.pCurGestureAnimID);

            //운동수행 후 FitnessIdle로 변경
            CurTrainer.ChangeStateMachine(UnitState.FitnessIdle);
        }

        public void UpdateExcute()
        {


        }

        public void Exit()
        {
            //플레이어 Fitness Idle false
            //CurPlayer.pAnimatorbase.SetBool(CurPlayer.pAnimatorIDTable.AnimID_OnFitness, false);
        }

        public void OnTriggerEnter(Collider col)
        {

        }
    }//end of class

    public class TrainerClimb : UnitInterface<UnitTrainer> 
    {
        private UnitTrainer CurTrainer;
        private float targetSpeed = 0;
        private float climbMaxSpeed = 2.0f;

        public void Enter(UnitTrainer unit)
        {
            CurTrainer = unit;
            CurTrainer.Follower.followSpeed = 0;
            CurTrainer.pAnimatorbase.SetTrigger(CurTrainer.pAnimatorIDTable.AnimID_Climb);
        }

        public void UpdateExcute()
        {
            targetSpeed = Managers.Gesture.pCurActivityGesture == null ? 0f : Managers.Gesture.pCurActivityGesture.PlayerSpeed;
            Managers.Game.LoginModeTrigger(
                debugPlayMode: () =>
                {
                    targetSpeed = 1.0f;
                });

            if (targetSpeed > 0.1f) targetSpeed += CurTrainer.climbPlusPercent;

            CurTrainer.Follower.followSpeed = targetSpeed * climbMaxSpeed;
            CurTrainer.pAnimatorbase.SetFloat(CurTrainer.pAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurTrainer.pAnimatorbase.SetFloat(CurTrainer.pAnimatorIDTable.AnimID_MoveSpeed, targetSpeed);
        }

        public void Exit()
        {
            CurTrainer.pAnimatorbase.SetBool(CurTrainer.pAnimatorIDTable.AnimID_Transport, false);
            CurTrainer.PlayerRotation(Vector3.zero);
            CurTrainer.climbPlusPercent = 0;
        }

        public void OnTriggerEnter(Collider col)
        {

        }

    }







}//end of namespace					