/*********************************************************					
* TwoLoversPointScene_Controller.cs					
* 작성자 : skjo				
* 작성일 : 2024.08.16 오전 11:31					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev_System;
using Dev_Unit;

namespace Dev_SceneControl
{
    public class TwoLoversPointScene_Controller : AdventureSceneController
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

        public override void Mission2_진입()
        {
            base.Mission2_진입();
            Phase_전투2_Init();
        }

        public override void Mission2_운동시작()
        {
            base.Mission2_운동시작();
            base.Phase_전투2();
        }

        public override void Mission3_진입()
        {
            base.Mission3_진입();
            base.Phase_미션진입();
            
        }

        public override void Mission3_운동시작()
        {
            base.Mission3_운동시작();
            base.Phase_미션시작();
        }

        protected override void Phase_전투1_Init()
        {
            base.turnState = TurnState.EnemyTurn;
            base.Phase_전투1_Init();
        }

        protected override void Phase_전투2_Init()
        {
            base.turnState = TurnState.EnemyTurn;
            base.Phase_전투2_Init();
        }

        public void ChangeAnimatorSpeed()
        {
            Trainer.pAnimatorbase.speed = 0f;
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_MeatSkewer, UnitState.None, genpos, true, Vector3.one);
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