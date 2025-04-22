/*********************************************************					
* TokyoScene_Controller.cs					
* 작성자 : lbioh					
* 작성일 : 2024.07.05 오후 2:45					
**********************************************************/
using Dev_SceneControl;
using Dev_System;
using Dev_UI;
using Dev_Unit;
using Dreamteck.Splines;
using MoreMountains.Feedbacks;
using Newtonsoft.Json.Serialization;
using System.Collections;
using TMPro;
using UnityEngine;


namespace Dev_SceneControl
{
    public class TokyoScene_Controller : AdventureSceneController
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
        //[SerializeField] private Sprite MaleSprite;
        //[SerializeField] private Sprite FemaleSprite;

        //[SerializeField] private Transform Genpos_Lamp;
        [Header("Dango")]
        [SerializeField] private MMF_Player[] GenPos_Dango;
        [SerializeField] private MMF_Player[] GenPos_Dango2;
        [SerializeField] private MMF_Player[] First2PrepareFeedbacks;


        [Header("Fortune Paper")]
        [SerializeField] private SkinnedMeshRenderer fortunePaperRenderer;
        [SerializeField] private Material fortunePaperMat_KR;
        [SerializeField] private Material fortunePaperMat_EN;


        private int prepareCount = 0;
        private int shootCount = 0;

        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_Dango, UnitState.None, genpos, true, Vector3.one);
            }
        }

        protected override void CreateMission2()
        {

        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(WaitToCreateManagers());
        }

        protected override void InitScene()
        {
            base.InitScene();
        }


        protected override void Update()
        {
            base.Update();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        protected override void InvokeMissionSetting()
        {
            base.InvokeMissionSetting();
            DangoMissionSetting();
            DangoMissionFirst2PrepareSetting();
        }

        void DangoMissionSetting()
        {
            shootCount = 0;
            int missionNums = Managers.Unit.UnitExerciseList.Count;
            for (int i = 0; i < missionNums; i++)
            {
                UnitExercise Exercise = (UnitExercise)Managers.Unit.UnitExerciseList[i];
                if (Exercise.pUniqueID == UnitUniqueID.Exercise_Dango)
                {
                    Exercise.ShootEvent += DangoActiveObject;
                }
            }
        }

        void DangoMissionFirst2PrepareSetting()
        {
            prepareCount = 0;
            int missionNums = Managers.Unit.UnitExerciseList.Count;
            for (int i = 0; i < missionNums; i++)
            {
                UnitExercise Exercise = (UnitExercise)Managers.Unit.UnitExerciseList[i];
                if (Exercise.pUniqueID == UnitUniqueID.Exercise_Dango)
                {
                    Exercise.GetComponent<UnitExercise_Dango>().first2PrepareAction += DangoFirst2Prepare;
                }
            }
        }

        void DangoFirst2Prepare()
        {
            First2PrepareFeedbacks[prepareCount].PlayFeedbacks();
            prepareCount++;
        }

        void DangoActiveObject()
        {
            GenPos_Dango[shootCount].PlayFeedbacks();
            shootCount++;
        }

        private IEnumerator WaitToCreateManagers()
        {
            yield return new WaitUntil(() => Managers.AllCreateManagers);

            //Todo

            Managers.Game.LanguageModeTrigger(

              korea: () =>
              {
                  fortunePaperRenderer.material = fortunePaperMat_KR;
              },
              english: () =>
              {
                  fortunePaperRenderer.material = fortunePaperMat_EN;
              });
        }

    }//end of class					
}//end of namespace					