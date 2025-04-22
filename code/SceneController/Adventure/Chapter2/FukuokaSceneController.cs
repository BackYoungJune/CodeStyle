/*********************************************************					
* FukuokaSceneController.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.05.02 오전 8:40					
**********************************************************/
using Dev_Dialogue;
using Dev_System;
using Dev_Transport;
using Dev_Unit;
using Dreamteck.Splines;
using MoreMountains.Feedbacks;
using UnityEngine;
			
namespace Dev_SceneControl
{					
	public class FukuokaSceneController : AdventureSceneController
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


        //public void BoatTransport(SplineComputer changeComputer)
        //{
        //    if (Player.pCurState == UnitState.OtherTransport) return;
        //    // 탈것 생성 및 초기화
        //    var info = System.Array.Find(TransportTable.TransportInfos, x => x.enumKey == TransportEnum.Boat);
        //    if (info != null)
        //    {
        //        Transport createBoat = Instantiate(info.prefab).GetComponent<Transport>();
        //        createBoat.InitTransport();
        //        SplineEvent.ChangeSplineTransport(createBoat, changeComputer);
        //    }

        //    // 플레이어 보트 셋팅
        //    Player.ChangeStateMachine(UnitState.OtherTransport);
        //    Player.SetTriggerCompare(Player.pAnimatorIDTable.AnimID_Boat, true);

        //    // NPC 보트 셋팅
        //    FollowNPC.ChangeStateMachine(UnitState.OtherTransport);
        //    FollowNPC.nAnimatorbase.SetTrigger(FollowNPC.nAnimatorIDTable.AnimID_Boat);
        //}




        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        //[SerializeField] private Sprite MaleSprite;
        //[SerializeField] private Sprite FemaleSprite;

        [Header("Mission Sushi")]
        [SerializeField] private Exercise_Dummy[] SyshiDummy;
        [SerializeField] private MMF_Player MissionEndFeedback;
        private int shootCount = 0;

        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_Wasabi, UnitState.None, genpos, true, Vector3.one);
            }

            foreach (var genpos in GenPos_MissionObjects2)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_Sushi, UnitState.None, genpos, true, Vector3.one);
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
            SushiMissionSetting();
        }

        void SushiMissionSetting()
        {
            shootCount = 0;
            int missionNums = Managers.Unit.UnitExerciseList.Count;
            for (int i = 0; i < missionNums; i++)
            {
                UnitExercise Exercise = (UnitExercise)Managers.Unit.UnitExerciseList[i];
                Exercise.ShootEvent += SushitDummyShoot;
            }
        }

        void SushitDummyShoot()
        {
            if (SyshiDummy.Length <= shootCount) return;

            if (shootCount < 4)
            {
                SyshiDummy[shootCount]?.ShootFeedback();
            }
            else if (shootCount == 4)
            {
                SyshiDummy[shootCount]?.ShootFeedback();
                MissionEndFeedback?.PlayFeedbacks();
                int missionNums = Managers.Unit.UnitExerciseList.Count;
                for (int i = 0; i < missionNums; i++)
                {
                    UnitExercise Exercise = (UnitExercise)Managers.Unit.UnitExerciseList[i];
                    if (Exercise.ExerciseStage == ExerciseStage.Sushi)
                    {
                        Exercise.PrePare();
                    }
                }
            }
            shootCount++;
        }

    }//end of class									
}//end of namespace					