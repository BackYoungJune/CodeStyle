/*********************************************************					
* RiverScene_Controller.cs					
* 작성자 : HMIT					
* 작성일 : 2024.05.23 오전 9:45					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using Dev_Unit;
using Dev_Spline;
using Dreamteck.Splines;
using Dev_System;
using Dev_UI;
using UnityEngine.UIElements;
//using Dev_Transport;

namespace Dev_SceneControl
{
    public class RiverScene_Controller : AdventureSceneController
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

        public void ChangeUIPos(float yPos)
        {
            StartCoroutine(ChangeUIPosCor(yPos));
        }

        IEnumerator ChangeUIPosCor(float yPos)
        {
            yield return new WaitUntil(() => Managers.UIManager.GetUIModule("Activity"));
            yield return new WaitUntil(() => Managers.UIManager.GetUIModule("Rank"));
            //yield return new WaitUntil(() => Managers.UIManager.GetUIModule("Exit"));

            UIModule activityUI = Managers.UIManager.GetUIModule("Activity");
            UIModule rankUI = Managers.UIManager.GetUIModule("Rank");
            //UIModule exitUI = Managers.UIManager.GetUIModule("Exit");


            Vector3 LerpPos = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, yPos, 0), 0.5f);
         
            activityUI?.MovePos(LerpPos);
            rankUI?.MovePos(LerpPos);
            //exitUI?.MovePos(LerpPos);

            yield return new WaitForSeconds(0.5f);
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_Fishing, UnitState.None, genpos, true, Vector3.one);
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