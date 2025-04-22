/*********************************************************					
* UnitExercise_Honey.cs					
* 작성자 : jkoh					
* 작성일 : 2024.05.22 오전 9:33					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


namespace Dev_Unit
{					
	public class UnitExercise_Honey : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					

        [SerializeField] MMF_Player FirstPrepareFeedback;
        [SerializeField] private GameObject HoneyJar;
        private int curProceedsNum = 0;
        private bool firstFeedback = true;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_Honey;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
            curProceedsNum = 0;
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
            if (firstFeedback)
            {
                FirstPrepareFeedback.PlayFeedbacks();
                firstFeedback = false;
                return;
            }
            HoneyJar.SetActive(false);
            base.PrePareFeedbacks[curProceedsNum].StopFeedbacks();
            base.PrePareFeedbacks[curProceedsNum].PlayFeedbacks();
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;
            base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();
            curProceedsNum++;

            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                //StartCoroutine(Active())
                base.OnDisable();
            }
        }

        IEnumerator Active(GameObject obj, bool active, float time)
        {
            yield return new WaitForSeconds(time);
            obj.SetActive(active);
        }

    }//end of class					
}//end of namespace					