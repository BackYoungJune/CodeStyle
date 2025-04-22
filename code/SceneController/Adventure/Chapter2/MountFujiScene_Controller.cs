/*********************************************************					
* MountFuji_SceneController.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.07.04 오후 1:04					
**********************************************************/
using Dev_System;
using Dev_Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;					
					
namespace Dev_SceneControl					
{					
	public class MountFujiScene_Controller : AdventureSceneController
    {
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

        public void scaleZero(Transform target)
        {
            StartCoroutine(scaleZeroCor(target));
        }

        IEnumerator scaleZeroCor(Transform target)
        {
            float elapsedTime = 0;
            float duration = 2.0f;
            float t = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                t = (elapsedTime/ duration) * Time.deltaTime;
                target.transform.localScale = Vector3.Lerp(target.transform.localScale, Vector3.zero, t);

                yield return null;
            }
            target.transform.localScale = Vector3.zero;
        }

        public void CameraDistance(float dist)
        {
            DevUtils.CameraFarDistance(dist);
        }

        public void ClimbPlusSpeed(float speed)
        {
            Managers.Unit.GetCurTrainerUnit.climbPlusPercent = speed;
        }

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_MountBread, UnitState.None, genpos, true, Vector3.one);
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