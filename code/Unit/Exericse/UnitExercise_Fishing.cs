/*********************************************************					
* UnitExercise_Fishing.cs					
* 작성자 : HMIT					
* 작성일 : 2024.05.23 오후 12:43					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev_Utils;
using MoreMountains.Feedbacks;
//using Cinemachine;
using Unity.VisualScripting;

namespace Dev_Unit
{
    public class UnitExercise_Fishing : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private MMF_Player FirstPrepareFeedback;
        [SerializeField] private GameObject[] Fishs;
        //[SerializeField] private ParticleSystem disalbeParticle;

        private int curProceedsNum = 0;
        private bool firstFeedback = true;
        private GameObject activeFish = null;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_Fishing;
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
                StartCoroutine(FishActive());
                return;
            }
            //base.PrePareFeedbacks[curProceedsNum]?.StopFeedbacks();
            FirstPrepareFeedback?.StopFeedbacks();
            base.PrePareFeedbacks[curProceedsNum]?.PlayFeedbacks();
        }

        IEnumerator FishActive()
        {
            int index = Random.Range(0, Fishs.Length);
            activeFish = Fishs[index];
            yield return new WaitUntil(() => FirstPrepareFeedback.IsPlaying == false);
            yield return new WaitForSeconds(0.7f);
            activeFish.SetActive(true);
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;
            //base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            FirstPrepareFeedback?.StopFeedbacks();
            base.PrePareFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();

            curProceedsNum++;
            firstFeedback = true;
            StartCoroutine(Active(this.gameObject, false, 1.0f));
            //if (ShootFeedbacks.Count <= curProceedsNum)
            //{

            //}
        }

        IEnumerator Active(GameObject obj, bool active, float time)
        {
            DisableFeedback.PlayFeedbacks();
            yield return new WaitForSeconds(time);
            yield return new WaitUntil(() => base.DisableFeedback.IsPlaying == false);
            //disalbeParticle.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);

            if (activeFish != null)
            {
                activeFish.SetActive(active);
            }

          
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                //if (lastExercise)
                //{
                //    UnitManager.Instance.CurTurnExerciseUnitIndex--;
                //}
                obj.SetActive(active);
            }
        }

    }//end of class					
}//end of namespace						