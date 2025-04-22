/*********************************************************					
* FishEyeScene_Controller.cs					
* 작성자 : skjo					
* 작성일 : 2024.08.07 오후 3:21					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev_System;
using Dev_UI;
using Dev_Unit;
using Dreamteck.Splines;
using TMPro;

namespace Dev_SceneControl
{
    public class TallofofoScene_Contorller : AdventureSceneController
    {

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public override void Mission1_진입()
        {
            base.Mission1_진입();
            Phase_전투1_Init();
        }

        public override void Mission1_운동시작()
        {
            base.Mission1_운동시작();
            base.Phase_전투1();
        }

        protected override void Phase_전투1_Init()
        {
            base.turnState = TurnState.EnemyTurn;
            base.Phase_전투1_Init();
        }

        public override void Mission2_진입()
        {
            base.Mission2_진입();
            base.Phase_미션진입();
        }

        public override void Mission2_운동시작()
        {
            base.Mission2_운동시작();
             base.Phase_미션시작();

        }

        public override void Mission3_진입()
        {
            base.Mission3_진입();
            base.Phase_전투2_Init();
        }

        public override void Mission3_운동시작()
        {
            base.Mission3_운동시작();
            base.Phase_전투2();
        }

        public void ShongChnageRotation()
        {
            FollowNPC.Follower.motion.rotationOffset = new Vector3(0, 170, 0);
        }

        public void SplineRotationInit()
        {
            StartCoroutine("SplineRotationInitCor");
        }

        IEnumerator SplineRotationInitCor()
        {
            SplineFollower follower = FollowNPC.Follower;
            float duration = 3.0f;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                follower.motion.rotationOffset = Vector3.Lerp(follower.motion.rotationOffset, Vector3.zero, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					

        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_WoodStick, UnitState.None, genpos, true, Vector3.one);
            }
        }

        protected override void CreateMission2()
        {
            foreach (var genpos in GenPos_MissionObjects2)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_CampFire, UnitState.None, genpos, true, Vector3.one);
            }
        }

        protected override void Start()
        {
            base.Start();
        }



        protected override void Update()
        {
            base.Update();
        }



    }//end of class					


}//end of namespace					