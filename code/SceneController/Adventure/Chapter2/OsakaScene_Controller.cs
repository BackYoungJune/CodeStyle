/*********************************************************					
* OsakaScene_Controller.cs					
* 작성자 : skjo					
* 작성일 : 2024.07.01 오후 5:10					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using Dev_System;
using Dev_UI;
using Dev_Unit;
using Dreamteck.Splines;
using UnityEngine;					
					
namespace Dev_SceneControl					
{					
	public class OsakaScene_Controller : AdventureSceneController
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

        public void OpenStoneBox()
        {
            UnitEnemy_StoneBox stoneBox = Managers.Unit.GetCurTargetUnit.GetComponent<UnitEnemy_StoneBox>();
            if (stoneBox != null)
            {
                stoneBox.OpenFeedbackPlay();
            }
        }

        public override void Climb(SplineComputer changeComputer)
        {
            base.Climb(changeComputer);
            Player.transform.localRotation = Quaternion.Euler(new Vector3(-89.052f, -67.281f, -112.625f));
            Trainer.transform.localRotation *= Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }

        public void ChangeAnimatorSpeed()
        {
            Trainer.pAnimatorbase.speed = 0f;
        }

        public void ChangeExitButtonParent(Transform target)
        {
            //UIModule_Exit module = Managers.UIManager.GetUIModule("Exit") as UIModule_Exit;
            //module.SetParent(target);
            //target_Canvas.SetParent(target);
        }

        public void InitExitButtonParent()
        {
            //UIModule_Exit module = Managers.UIManager.GetUIModule("Exit") as UIModule_Exit;
            //module.InitParent();
            //target_Canvas.SetParent(parent);
        }

        public void PlayerRateSpeed(float speed)
        {
            base.Player.RateMoveSpeed(speed);
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------	
        [SerializeField] private Transform target_Canvas;
        private Transform parent;
        				
        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_Okonomi, UnitState.None, genpos, true, Vector3.one);
            }
        }

        protected override void Start()
        {
            base.Start();
            parent = target_Canvas.parent;
        }


        protected override void Update()
        {
            base.Update();
        }


    }//end of class					
					
					
}//end of namespace					