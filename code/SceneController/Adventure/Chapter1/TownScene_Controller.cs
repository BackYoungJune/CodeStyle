/*********************************************************					
* TownScene_Controller.cs					
* 작성자 : jkOh					
* 작성일 : 2024.05.21.					
**********************************************************/
using Dev_System;
using Dev_Unit;
using Dreamteck;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System;
using Unity.VisualScripting;
using UnityEngine;


namespace Dev_SceneControl
{					
	public class TownScene_Controller : AdventureSceneController
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public override void Mission1_진입()
        {
            base.Mission1_진입();
            base.Phase_미션진입();
        }

        public override void Mission1_운동시작()
        {
            base.Mission1_운동시작();
            base.Phase_미션시작();
        }

        public override void Mission2_진입()
        {
            base.Mission2_진입();
            base.Phase_전투1_Init();
        }

        public override void Mission2_운동시작()
        {
            base.Mission2_운동시작();
            base.Phase_전투1();
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

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------		

        //핸드 카트 위에 있는 HoneyJar MMF_Player
        [Header("HoneyJarActive")]
        [SerializeField] private MMF_Player[] honeyJar_HandCart;
        [SerializeField] private NeutralNPC[] missionNPCs;

        private int shootCount = 0;
	
        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_Honey, UnitState.None, genpos, true, Vector3.one);
            }
        }

        protected override void CreateMission2()
        {
            foreach (var genpos in GenPos_MissionObjects2)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_HoneyWater, UnitState.None, genpos, true, Vector3.one);
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

        protected override void InvokeMissionSetting()
        {
            base.InvokeMissionSetting();
            HoneyWaterMissionSettion();
        }

        private void HoneyWaterMissionSettion()
        {
            shootCount = 0;
            int missionNums = Managers.Unit.UnitExerciseList.Count;
            for(int i =0; i < missionNums; i++)
            {
                UnitExercise exercise = (UnitExercise)Managers.Unit.UnitExerciseList[i];
                exercise.ShootEvent += NPCMissionInteraction;
                if (exercise.pUniqueID == UnitUniqueID.Exercise_HoneyWater)
                {
                    exercise.ShootEvent += HoneyJarActiveObejct;
                }
            }
        }

        private void HoneyJarActiveObejct()
        {
            honeyJar_HandCart[shootCount].PlayFeedbacks();
            shootCount++;
        }

        private void NPCMissionInteraction()
        {
            /*            for(int i =0; i < missionNPCs.Length; i++)
                        {
                            missionNPCs[i].Interaction();
                        }*/
            missionNPCs[0].Interaction();
            missionNPCs[0].InteractionAfterPrevAnimation();
            missionNPCs[1].Interaction();
        }

    }//end of class					
}//end of namespace					