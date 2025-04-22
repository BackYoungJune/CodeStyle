/*********************************************************					
* NPCInterfaces.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.29 오후 7:14					
**********************************************************/							
using UnityEngine;
using Dreamteck.Splines;
using Dev_System;

namespace Dev_Unit
{
    public class FollowNPCCutScene : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;



        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            CurNPC.Follower.followSpeed = 0;
            CurNPC.DialogueSystemTrigger.OnBarkEnd(CurNPC.transform);
        }

        public void UpdateExcute()
        {
            if (CurNPC.nAnimatorbase.GetBool(CurNPC.nAnimatorIDTable.AnimID_OnBattle) == false)
            {
                //NPC 컷씬동작 - BattleIdle
                CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimID_OnBattle, true);
            }
        }

        public void Exit()
        {
            //NPC 컷씬동작 - BattleIdle
            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimID_OnBattle, false);
        }

        public void OnTriggerEnter(Collider col)
        {

        }


    }//end of class



    public class FollowNPCFollowSpline : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;
        private float targetSpeed;
        float curTime;
        float distance;
        private float restrictSpeed;

        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            restrictSpeed = CurNPC.Player.FollowMaxSpeed - 1.0f;
        }

        public void UpdateExcute()
        {
            if (CurNPC.Player == null) return;
            if(CurNPC.Follower == null) return;
            curTime += Time.deltaTime;
            targetSpeed = CurNPC.Player.Follower.followSpeed;

            if (CurNPC.Follower.result.percent >= 1.0f)
            {
                CurNPC.ChangeStateMachine(UnitState.LookAt);
            }

            if (targetSpeed > Mathf.Epsilon)
            {
                CurNPC.Follower.followSpeed = targetSpeed + CurNPC.Curve.Evaluate(curTime);
                // 숑이가 뒤쳐질수도 있도록 restrictSpeed의 스피드를 넘으면 속도를 줄인다
                if (CurNPC.Follower.followSpeed > restrictSpeed && CurNPC.Follower.GetPercent() >= CurNPC.Player.Follower.GetPercent() + 0.03f)
                {
                    CurNPC.Follower.followSpeed -= 0.5f;
                }
            }
            else
            {
                CurNPC.Follower.followSpeed = 0;
            }

            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, targetSpeed);

            distance = Vector3.Distance(CurNPC.transform.position, CurNPC.Player.transform.position);
            if (distance > 9.0f)
            {
                CurNPC.ChangeStateMachine(UnitState.LookAt);
            }

            Vector3 dir = CurNPC.Follower.motion.splineResult.forward;
            CurNPC.transform.rotation = Quaternion.Slerp(CurNPC.transform.rotation, Quaternion.LookRotation(dir), 1.0f);
        }

        public void Exit()
        {
            
        }

        public void OnTriggerEnter(Collider col)
        {
            
        }

    }//end of class


    public class FollowNPCFollowSplineLookAt : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;
        private float distance;

        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            CurNPC.Follower.followSpeed = 0f;

            //Hurry 동작 시작
            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimID_HurryUp, true);

            if (CurNPC.Follower.result.percent < 1.0f)
            {
                //HurryUp 다이얼로그 출력
                CurNPC.BarkDialogue();
            }
        }

        public void UpdateExcute()
        {
            distance = Vector3.Distance(CurNPC.transform.position, CurNPC.Player.transform.position);
            
            if(distance < 5.0f && CurNPC.Follower.result.percent < 1.0f)
            {
                CurNPC.ChangeStateMachine(UnitState.MoveSpline);
            }

            Vector3 dir = CurNPC.Player.transform.position - CurNPC.transform.position;
            CurNPC.transform.rotation = Quaternion.Slerp(CurNPC.transform.rotation, Quaternion.LookRotation(dir), 1.0f);

            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, 0);
        }

        public void Exit()
        {
            //Hurry 동작 해제
            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimID_HurryUp, false);
            CurNPC.transform.localRotation = Quaternion.identity;
        }

        public void OnTriggerEnter(Collider col)
        {

        }

    }//end of class

    public class FollowNPCBattleIdle : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;

        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            CurNPC.Follower.followSpeed = 0;
            CurNPC.transform.localRotation = Quaternion.identity;

            CurNPC.DialogueSystemTrigger.OnBarkEnd(CurNPC.transform);
            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimID_OnBattle, true);
        }

        public void UpdateExcute()
        {
            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, 0);
        }

        public void Exit()
        {
            //NPC 컷씬동작 - BattleIdle
            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimID_OnBattle, false);
        }

        public void OnTriggerEnter(Collider col)
        {

        }


    }//end of class

    public class FollowNPCBattleEnd : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;

        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;

            // NPC 전투 승/패 애니메이션
            Managers.Battle.BattleVictoryTrigger(
                victoryAction: () =>
                {
                    CurNPC.nAnimatorbase.SetTrigger(CurNPC.nAnimatorIDTable.AnimID_Victory);
                },
                defeatAction: () =>
                {
                    CurNPC.nAnimatorbase.SetTrigger(CurNPC.nAnimatorIDTable.AnimID_Drawn);
                },
                drawnAction: () =>
                {
                    CurNPC.nAnimatorbase.SetTrigger(CurNPC.nAnimatorIDTable.AnimID_Defeat);
                });
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

    public class FollowNPCClimb : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;
        private UnitTrainer curTrainer;
        private float targetSpeed = 0;
        private float restrictSpeed = 0;
        private float curTime = 0;

        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;
            curTrainer = Managers.Unit.GetCurTrainerUnit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            CurNPC.Follower.followSpeed = 0;

            // Temp : 영준 - Player -> Trainer로 변경 되었지만 수치값이 같아 우선 Player의 max 스피드로 그냥 둠
            restrictSpeed = CurNPC.Player.FollowMaxSpeed - 1.0f;

            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimaID_Transport, true);
        }

        public void UpdateExcute()
        {
            curTime += Time.deltaTime;
            targetSpeed = curTrainer.Follower.followSpeed;

            if (targetSpeed > Mathf.Epsilon)
            {
                CurNPC.Follower.followSpeed = targetSpeed + CurNPC.Curve.Evaluate(curTime) * 1.2f;
                // 숑이가 뒤쳐질수도 있도록 restrictSpeed의 스피드를 넘으면 속도를 줄인다
                if (CurNPC.Follower.GetPercent() >= curTrainer.Follower.GetPercent() + 0.03f)
                {
                    CurNPC.Follower.followSpeed = targetSpeed;
                }
                CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
                CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, targetSpeed);
            }
            else
            {
                CurNPC.Follower.followSpeed = 0;
                CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
                CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, 0);
            }

        }

        public void Exit()
        {
            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimaID_Transport, false);
            CurNPC.NPCRotation(Vector3.zero);
        }

        public void OnTriggerEnter(Collider col)
        {

        }

        
    }//end of class

    public class FollowNPCOtherTransport : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;
        private float targetSpeed = 0;


        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimaID_Transport, true);
        }

        public void UpdateExcute()
        {
            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, 1.0f);
        }

        public void Exit()
        {
            CurNPC.nAnimatorbase.SetBool(CurNPC.nAnimatorIDTable.AnimaID_Transport, false);
            CurNPC.transform.SetParent(CurNPC.Follower.transform);
            CurNPC.transform.localPosition = Vector3.zero;
            CurNPC.transform.localRotation = Quaternion.identity;
            CurNPC.transform.localScale= Vector3.one * 0.3f;
        }

        public void OnTriggerEnter(Collider col)
        {

        }
    }

    public class FollowNPCFastFollowSpline : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;
        private float targetSpeed = 0;
        private float curTime;

        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;
            targetSpeed = 8.0f;
        }

        public void UpdateExcute()
        {
            if(CurNPC.Follower.result.percent >= 1.0f)
            {
                Vector3 dir = CurNPC.Player.transform.position - CurNPC.transform.position;
                CurNPC.transform.rotation = Quaternion.Slerp(CurNPC.transform.rotation, Quaternion.LookRotation(dir), 1.0f);
                CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, 0.0f);
                return;
            }

            curTime += Time.deltaTime;
            CurNPC.Follower.followSpeed = targetSpeed + CurNPC.Curve.Evaluate(curTime);

            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, 1.0f);
        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider col)
        {

        }
    }

    public class FollowNPCMoveSplineNoLookAt : UnitInterface<UnitFollowNPC>
    {
        private UnitFollowNPC CurNPC;
        private float targetSpeed;
        float curTime;
        float distance;
        private float restrictSpeed;

        public void Enter(UnitFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            restrictSpeed = CurNPC.Player.FollowMaxSpeed - 1.0f;
        }

        public void UpdateExcute()
        {
            if (CurNPC.Player == null) return;
            curTime += Time.deltaTime;
            targetSpeed = CurNPC.Player.Follower.followSpeed;

            if (CurNPC.Follower.result.percent >= 1.0f)
            {
                CurNPC.ChangeStateMachine(UnitState.LookAt);
            }

            if (targetSpeed > Mathf.Epsilon)
            {
                CurNPC.Follower.followSpeed = targetSpeed + CurNPC.Curve.Evaluate(curTime);
                // 숑이가 뒤쳐질수도 있도록 restrictSpeed의 스피드를 넘으면 속도를 줄인다
                if (CurNPC.Follower.followSpeed > restrictSpeed && CurNPC.Follower.GetPercent() >= CurNPC.Player.Follower.GetPercent())
                {
                    CurNPC.Follower.followSpeed -= 0.5f;
                }
            }
            else
            {
                CurNPC.Follower.followSpeed = 0;
            }

            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MotionSpeed, 1.0f);
            CurNPC.nAnimatorbase.SetFloat(CurNPC.nAnimatorIDTable.AnimID_MoveSpeed, targetSpeed);

            // 각 구간 넣었을것 같기는 하지만 만약에 0.5까지 왔는데 FastSpline이 안된다면 바꿔준다
            if(CurNPC.Follower.result.percent >= 0.5f)
            {
                CurNPC.ChangeStateMachine(UnitState.FastSpline);
            }

            Vector3 dir = CurNPC.Follower.motion.splineResult.forward;
            CurNPC.transform.rotation = Quaternion.Slerp(CurNPC.transform.rotation, Quaternion.LookRotation(dir), 1.0f);
        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider col)
        {

        }
    }

}//end of namespace					