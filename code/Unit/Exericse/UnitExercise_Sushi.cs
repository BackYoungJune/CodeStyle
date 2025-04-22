/*********************************************************					
* UnitExercise_Sushi.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.05.03 오후 3:26					
**********************************************************/
using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// **************Namespace를 반드시 수정하세요**************					
namespace Dev_Unit
{					
	public class UnitExercise_Sushi : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private MMF_Player firstPrepareFeedback;

        private int curProceedsNum = 0;
        private int prepareNum = 0;
        private bool firstFeedback = true;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_Sushi;
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
            if (PrePareFeedbacks.Count <= prepareNum)
            {
                prepareNum = PrePareFeedbacks.Count - 1;
            }

            // 맨처음 Prepare 호출되면
            if (firstFeedback)
            {
                firstFeedback = false;
                firstPrepareFeedback.PlayFeedbacks();
                return;
            }

            base.PrePareFeedbacks[prepareNum]?.StopFeedbacks();
            base.PrePareFeedbacks[prepareNum]?.PlayFeedbacks();
            prepareNum++;
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;
            base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();
            curProceedsNum++;

            //마지막 연출 feedback 
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                StartCoroutine(Active(this.gameObject, false, 1.0f));
            }
        }

        IEnumerator Active(GameObject obj, bool active, float time)
        {
            yield return new WaitForSeconds(time);
            if (obj == this.gameObject)
            {
                base.DisableFeedback.PlayFeedbacks();

                yield return new WaitForSeconds(1.0f);
            }

            OnDisable();
        }
    }//end of class						
}//end of namespace					