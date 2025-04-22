/*********************************************************					
* PlayerInterfaces.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.29 오후 1:32					
**********************************************************/
using System.Collections;
using UnityEngine;
using Dev_System;
using PixelCrushers.DialogueSystem.UnityGUI;
using System.Collections.Generic;
using Dev_Gesture;

namespace Dev_Unit
{
    public class PlayerCutScene : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;

        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;
            if (CurPlayer.Follower != null)
            {
                CurPlayer.Follower.followSpeed = 0f;
            }
            CurPlayer.RunFeedback.StopFeedbacks();
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

    public class PlayerFitnessIdle : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;

        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;
            Managers.Battle.BattleManagerMode = BattleManagerMode.Fitness;


            //플레이어 애니메이션 보호코드

            if (Managers.Battle.IsFirstFitness == false)
            {
                Managers.Battle.IsFirstFitness = true;
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
        }

        public void OnTriggerEnter(Collider col)
        {

        }


    }//end of class


    public class PlayerFitnessAction : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;

        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;


            //운동수행 후 FitnessIdle로 변경
            CurPlayer.ChangeStateMachine(UnitState.FitnessIdle);
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


    public class PlayerMove : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;
        private float targetSpeed;
        private float pantTimer = 0;
        private float pantUpdateTimer = 0;
        private float 팬트트리거시간 = 3.0f;
        private float 팬트유지시간 = 4.0f;
        private bool isPantUpdating = false;
        private bool isPant = false;
        private bool isFirstStep;


        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;
            targetSpeed = 0;
            isFirstStep = true;
            if (CurPlayer.ExistFollower)
            {
                CurPlayer.Follower.followSpeed = 0;
                CurPlayer.MeterFollower = CurPlayer.Follower;
            }

            //걷기 뛰기 칼로리 계산
            CurPlayer.Call_Calculate_WalkRunTime(true);

            CurPlayer.pRateSpeed = 1.0f;
        }

        private float testTime = 1.0f;

        public void UpdateExcute()
        {
            if (CurPlayer.ExistFollower == false) return;
            targetSpeed = Managers.Gesture.pCurActivityGesture == null ? 0f : Managers.Gesture.pCurActivityGesture.PlayerSpeed;

            // 디버그 이동
            Managers.Game.LoginModeTrigger(
                debugPlayMode: () =>
                {
                    targetSpeed = 1.0f;
                });

            if(CurPlayer.pRateSpeed >= 1.0f)
            {
                targetSpeed /= CurPlayer.pRateSpeed;
            }

            //Follow 목적지 도착시 강제 정지
            if (CurPlayer.Follower.result.percent >= 1.0f)
            {
                targetSpeed = 0f;
            }

            //speed가 0.1보다 작으면 정지
            //speed가 0.1보다 크면 걷기
            //speed가 0.6보다 크면 뛰기
            //speed가 0.6보다 클때, 3초이상 유지되면 usePant = true되고 speed가 0이되면 IsPant모션 5초간 재생 후 UsePant = false

            CurPlayer.CurMoveSpeed = targetSpeed;
            CurPlayer.Follower.followSpeed = targetSpeed * CurPlayer.FollowMaxSpeed;


            //멈춤
            if (targetSpeed <= 0.1f)
            {
                if (isFirstStep == false)
                    PantUpdate();

                CurPlayer.pCurMoveType = UnitMoveType.Stop;
                CurPlayer.isMoving = false;
                isPant = false;
                pantTimer = 0;

                if (CurPlayer.RunFeedback?.IsPlaying == true)
                    CurPlayer.RunFeedback?.StopFeedbacks();

            }
            //걷는중일때
            else if (targetSpeed >= 0.2f && targetSpeed < 0.71f)
            {
                CurPlayer.pCurMoveType = UnitMoveType.Walk;
                Managers.Game.Calculate_Calorie_WalkTimeCount();
            }
            //달리기중일때
            else if (targetSpeed >= 0.71f)
            {
                if (isFirstStep == true)
                    isFirstStep = false;

                //달리기 Feedback play
                if (CurPlayer.RunFeedback?.IsPlaying == false)
                    CurPlayer.RunFeedback?.PlayFeedbacks();

                IsPantTimer();

                CurPlayer.pCurMoveType = UnitMoveType.Run;
                Managers.Game.Calculate_Calorie_RunTimeCount();
            }


            //움직이는 중일때 - 걷기/뛰기 포함
            if (targetSpeed >= 0.2f)
            {
                CurPlayer.isMoving = true;
                isPantUpdating = false;
                pantUpdateTimer = 0;

                if (CurPlayer.StopFeedback?.IsPlaying == true)
                    CurPlayer.StopFeedback?.StopFeedbacks();
            }

        }

        public void Exit()
        {
            CurPlayer.Call_Calculate_WalkRunTime(false);

            CurPlayer.isMoving = false;

            if (CurPlayer.StopFeedback?.IsPlaying == true)
                CurPlayer.StopFeedback?.StopFeedbacks();

            CurPlayer.pRateSpeed = 1.0f;
        }

        public void OnTriggerEnter(Collider col)
        {


        }

        void IsPantTimer()
        {
            if (isPant == false)
            {
                if (pantTimer <= 팬트트리거시간)
                {
                    pantTimer += Time.deltaTime;
                    //Debug.LogError("PantTimer 증가");

                    //3초간 뛰기 지속
                    if (pantTimer > 팬트트리거시간)
                    {
                        isPant = true;
                        //Debug.LogError("Is Pant = True");
                    }
                }
            }
        }

        void PantUpdate()
        {
            if (isPantUpdating == false)
            {
                if (pantUpdateTimer <= 팬트유지시간)
                {
                    pantUpdateTimer += Time.deltaTime;

                    //5초간 pant동작
                    if (isPant)
                    {
                        if (CurPlayer.StopFeedback?.IsPlaying == false)
                            CurPlayer.StopFeedback?.PlayFeedbacks();

                        //Debug.LogError("Pant 실행중...");
                    }

                    //5초간 pant동작 후 해제
                    if (pantUpdateTimer > 팬트유지시간)
                    {
                        isPantUpdating = true;

                        if (CurPlayer.StopFeedback?.IsPlaying == true)
                            CurPlayer.StopFeedback?.StopFeedbacks();

                        //Debug.LogError("Pant 종료");
                    }
                }
            }

        }


    }//end of class
    public class PlayerBattleIdle : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;
        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;

            //플레이어 전투Idle True
            if (CurPlayer.ExistFollower)
            {
                CurPlayer.Follower.followSpeed = 0;
            }
        }

        public void UpdateExcute()
        {

        }

        public void Exit()
        {


        }

        public void OnTriggerEnter(Collider col)
        {
            //방어 동작으로 변경
            CurPlayer.ChangeStateMachine(UnitState.BattleGuard);
        }

    }//end of class



    public class PlayerBattleAttack : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;
        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;

            //BattleManager 연출시작
            Managers.Battle.pIsProgress = true;

            //총알 발사!
            if (Managers.Unit.GetCurTargetUnit != null)
                CurPlayer.OnAttack(Managers.Unit.GetCurTargetUnit.pOnDamagePivot);

            CurPlayer.ChangeStateMachine(UnitState.BattleIdle);
        }

        public void UpdateExcute()
        {



        }

        public void Exit()
        {
            //BattleManager 연출종료
            Managers.Battle.pIsProgress = false;
        }

        public void OnTriggerEnter(Collider col)
        {


        }

    } //end of class



    public class PlayerBattleGuard : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;

        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;

            //BattleManager 연출시작
            Managers.Battle.pIsProgress = true;

            //플레이어 방어 연출
            CurPlayer.Guard(Managers.Battle.pGestureReferees, CurPlayer.pWeaponPivot);
            CurPlayer.ChangeStateMachine(UnitState.BattleIdle);
        }

        public void UpdateExcute()
        {



        }

        public void Exit()
        {
            //BattleManager 연출종료
            Managers.Battle.pIsProgress = false;
        }

        public void OnTriggerEnter(Collider col)
        {

        }



    }//end of class


    public class PlayerBattleEnd : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;

        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;

            //BattleManager 연출시작
            Managers.Battle.pIsProgress = true;
        }

        public void UpdateExcute()
        {




        }

        public void Exit()
        {
            //BattleManager 연출종료
            Managers.Battle.pIsProgress = false;
        }

        public void OnTriggerEnter(Collider col)
        {


        }

    }//end of class



    public class PlayerExercise : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;

        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;

            //플레이어 운동 연출
            CurPlayer.OnExercise();

            CurPlayer.ChangeStateMachine(UnitState.BattleIdle);
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

    public class PlayerClimb : UnitInterface<UnitPlayer>
    {
        private UnitPlayer CurPlayer;
        private UnitTrainer curTrainer;
        private float targetSpeed = 0;
        private float motionSpeed = 0;

        public void Enter(UnitPlayer unit)
        {
            CurPlayer = unit;
            curTrainer = Managers.Unit.GetCurTrainerUnit;
            CurPlayer.Follower.followSpeed = 0;
            CurPlayer.RunFeedback?.StopFeedbacks();
        }

        public void UpdateExcute()
        {
            targetSpeed = curTrainer.Follower.followSpeed;
            //Managers.Game.LoginModeTrigger(
            //    debugPlayMode: () =>
            //    {
            //        targetSpeed = 1.0f;
            //    });

            CurPlayer.CurMoveSpeed = Managers.Gesture.pCurActivityGesture == null ? 0f : Managers.Gesture.pCurActivityGesture.PlayerSpeed;
            CurPlayer.Follower.followSpeed = targetSpeed;
        }

        public void Exit()
        {
            CurPlayer.PlayerRotation(Vector3.zero);
        }

        public void OnTriggerEnter(Collider col)
        {

        }


        public class PlayerOtherTransport : UnitInterface<UnitPlayer>
        {
            private UnitPlayer CurPlayer;
            //private float targetSpeed = 0;

            public void Enter(UnitPlayer unit)
            {
                CurPlayer = unit;
                CurPlayer.ExerciseInputCount = 0;
                if(CurPlayer.Follower != null)
                {
                    CurPlayer.Follower.followSpeed = 0;
                }
            }

            public void UpdateExcute()
            {


            }

            public void Exit()
            {
                if (CurPlayer.Follower != null)
                {
                    CurPlayer.transform.SetParent(CurPlayer.Follower.transform);
                }
                CurPlayer.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                CurPlayer.transform.localScale = Vector3.one;

                if (CurPlayer.CurTransport.gameObject != null)
                {
                    CurPlayer.CurTransport.ArriveTransport();
                }
            }

            public void OnTriggerEnter(Collider col)
            {

            }
        }
    }
}//end of namespace		