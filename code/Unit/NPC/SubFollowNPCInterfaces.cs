/*********************************************************					
* SubFollowNPCInterfaces.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.09.19 오후 5:37					
**********************************************************/
using UnityEngine;
using Dev_System;
using Dreamteck.Splines;

namespace Dev_Unit
{
    public class SubFollowNPCCutScene : UnitInterface<UnitSubFollowNPC>
    {
        private UnitSubFollowNPC CurNPC;

        public void Enter(UnitSubFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            CurNPC.Follower.followSpeed = 0;
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
    }

    public class SubFollowNPCIdle : UnitInterface<UnitSubFollowNPC>
    {
        private UnitSubFollowNPC CurNPC;

        public void Enter(UnitSubFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            CurNPC.Follower.followSpeed = 0;
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
    }

    public class SubFollowNPCFollowSpline : UnitInterface<UnitSubFollowNPC>
    {
        private UnitSubFollowNPC CurNPC;
        private float targetSpeed;
        float curTime;
        float distance;
        private float restrictSpeed;

        public void Enter(UnitSubFollowNPC unit)
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
                CurNPC.ChangeStateMachine(UnitState.Idle);
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

            distance = Vector3.Distance(CurNPC.transform.position, CurNPC.Player.transform.position);
            if (distance > 9.9f)
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

    public class SubFollowNPCFollowSplineLookAt : UnitInterface<UnitSubFollowNPC>
    {
        private UnitSubFollowNPC CurNPC;
        private float distance;

        public void Enter(UnitSubFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            CurNPC.Follower.followSpeed = 0f;

        }

        public void UpdateExcute()
        {
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

    public class SubFollowNPCBattleIdle : UnitInterface<UnitSubFollowNPC>
    {
        private UnitSubFollowNPC CurNPC;

        public void Enter(UnitSubFollowNPC unit)
        {
            CurNPC = unit;
            CurNPC.Follower = CurNPC.GetComponentInParent<SplineFollower>();
            CurNPC.Follower.followSpeed = 0;
            CurNPC.transform.localRotation = Quaternion.identity;

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

    public class SubFollowNPCBattleEnd : UnitInterface<UnitSubFollowNPC>
    {
        private UnitSubFollowNPC CurNPC;

        public void Enter(UnitSubFollowNPC unit)
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

    public class SubFollowNPCOtherTransport : UnitInterface<UnitSubFollowNPC>
    {
        private UnitSubFollowNPC CurNPC;
        private float targetSpeed = 0;


        public void Enter(UnitSubFollowNPC unit)
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
            CurNPC.transform.localScale = Vector3.one * 0.3f;
        }

        public void OnTriggerEnter(Collider col)
        {

        }
    }

}//end of namespace					