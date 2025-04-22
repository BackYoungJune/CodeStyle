/*********************************************************					
* UnitExercise_CampFire.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.06.07 오전 9:54					
**********************************************************/
using Dev_System;
using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;

// **************Namespace를 반드시 수정하세요**************					
namespace Dev_Unit
{					
	public class UnitExercise_CampFire : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] MMF_Player EnableFeedback;

        private int curProceedsNum = 0;
        private int curExerciseCount = 0;
        private int curObjectNum = 5;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_CampFire;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            EnableFeedback?.PlayFeedbacks();
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

        // WoodStick은 한가지 종류로만 되어있어 Prepare함수에서 Prepare Shoot을 나누고 마지막 shoot만 따로 처리한다
        public override void PrePare()
        {
            if (PrePareFeedbacks.Count <= curProceedsNum) return;
            curExerciseCount++;

            int exerciseCount = (int)Managers.Gesture.pCurGestureMaxCount / curObjectNum;
            // Prepare
            if (curExerciseCount % exerciseCount != 0)
            {
                base.PrePareFeedbacks[0]?.StopFeedbacks();
                base.PrePareFeedbacks[0]?.PlayFeedbacks();
            }
            // Shoot
            else
            {
                base.PrePareFeedbacks[1]?.StopFeedbacks();
                base.PrePareFeedbacks[1]?.PlayFeedbacks();
            }
        }

        // Last Shoot
        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;
            base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();
            curProceedsNum++;
            OnDisable();
            ////마지막 연출 feedback 
            //if (ShootFeedbacks.Count <= curProceedsNum)
            //{
            //    StartCoroutine(Active(this.gameObject, false, 1.5f));
            //}
        }

        IEnumerator Active(GameObject obj, bool active, float time)
        {
            yield return new WaitForSeconds(time);
            if (obj == this.gameObject)
            {
                base.DisableFeedback.PlayFeedbacks();

                yield return new WaitUntil(() => base.DisableFeedback.IsPlaying == false);
            }

            obj.SetActive(active);
        }

    }//end of class					
}//end of namespace					