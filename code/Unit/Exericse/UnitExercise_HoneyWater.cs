/*********************************************************					
* UnitExercise_HoneyWater.cs					
* 작성자 : jkoh					
* 작성일 : 2024.05.22 오전 9:36					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using Dev_System;

namespace Dev_Unit
{					
	public class UnitExercise_HoneyWater : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					

        [SerializeField] private GameObject Bucket;
        [SerializeField] private GameObject[] BeePlanes;
        private int curProceedsNum = 0;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_HoneyWater;

        }


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
            curProceedsNum = 0;
            for (int i = 0; i < BeePlanes.Length; i++)
            {
                int rand = Random.Range(0, BeePlanes.Length - 1);
                BeePlanes[rand].SetActive(true);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (base.StateMachine.CurState != null)
            {
                base.StateMachine.CurState.UpdateExcute();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }


        //--------------------------------------------------------					
        // 추상함수 구현		
        //--------------------------------------------------------	

        public override void Idle()
        {

        }

        public override void PrePare()
        {
            if (PrePareFeedbacks.Count <= curProceedsNum) return;
            StopAllCoroutines();
            base.PrePareFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.PrePareFeedbacks[curProceedsNum]?.PlayFeedbacks();
            Bucket.SetActive(false);
            Bucket.SetActive(true);
            StartCoroutine(Active(Bucket, false, 0.7f));
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;

            base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();
            curProceedsNum++;
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                StartCoroutine(Active(this.gameObject, false, 0f));
                //base.OnDisable();
            }
        }

        IEnumerator Active(GameObject obj, bool active, float time)
        {
            yield return new WaitForSeconds(time);
            yield return new WaitUntil(() => base.ShootFeedbacks[ShootFeedbacks.Count - 1].IsPlaying == false);
            obj.SetActive(active);
        }


    }//end of class					
}//end of namespace					