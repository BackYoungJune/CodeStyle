/*********************************************************					
* UnitExercise_Torch.cs					
* 작성자 : skjo					
* 작성일 : 2024.05.23 오후 1:39					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Dev_Utils;

namespace Dev_Unit
{
    public class UnitExercise_Torch : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        private int curProceedsNum = 0;


        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_Torch;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
            curProceedsNum = 0;
            this.transform.localScale = Vector3.one;

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
            base.PrePareFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.PrePareFeedbacks[curProceedsNum]?.PlayFeedbacks();
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;
            base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();
            curProceedsNum++;

            //Debug.LogError(string.Format("ShootFeedbacks.Count : {0}, curProceedsNum : {1}", ShootFeedbacks.Count, curProceedsNum));
            if (ShootFeedbacks.Count <= curProceedsNum)
            {

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