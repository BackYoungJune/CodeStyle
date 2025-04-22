/*********************************************************					
* UnitExercise_WoodFire.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.06.05 오후 4:21					
**********************************************************/
using Dev_System;
using System.Collections;
using UnityEngine;

// **************Namespace를 반드시 수정하세요**************					
namespace Dev_Unit
{					
	public class UnitExercise_WoodStick : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private Transform smokeTransform;
        private int smokeCount = 0;

        private int curProceedsNum = 0;
        private int curExerciseCount = 0;
        private int curObjectNum = 5;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_WoodStick;
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

        // WoodStick은 한가지 종류로만 되어있어 Prepare함수에서 Prepare Shoot을 나누고 마지막 shoot만 따로 처리한다
        public override void PrePare()
        {
            if (PrePareFeedbacks.Count <= curProceedsNum) return;
            curExerciseCount++;
            int exerciseCount = (int)Managers.Gesture.pCurGestureMaxCount / curObjectNum;

            //if (exerciseCount <= 0) return;

            // Prepare
            if (curExerciseCount % exerciseCount != 0)
            {
                base.PrePareFeedbacks[0]?.StopFeedbacks();
                base.PrePareFeedbacks[0]?.PlayFeedbacks();
                StopCoroutine("TransformUp");
                StartCoroutine(TransformUp(0.018f));
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
            Invoke("InvokeSmokeActive", 2.0f);
            OnDisable();
            ////마지막 연출 feedback 
            //if (ShootFeedbacks.Count <= curProceedsNum)
            //{
            //    StartCoroutine(Active(this.gameObject, false, 1.5f));
            //}
        }

        void InvokeSmokeActive()
        {
            smokeTransform.gameObject.SetActive(false);
        }

        IEnumerator TransformUp(float plusPos)
        {
            smokeCount++;
            Vector3 startPos = smokeTransform.position;
            Vector3 plusVec = new Vector3(0, plusPos + smokeCount * 0.0005f, 0);
            float time = 0;
            while(time <= 1.0f)
            {
                time += Time.deltaTime;
                smokeTransform.position = Vector3.Lerp(smokeTransform.position, startPos + plusVec, Time.deltaTime * 2.0f);
            }
            

            yield return null;
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