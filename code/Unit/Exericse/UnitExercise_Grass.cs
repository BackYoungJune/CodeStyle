/*********************************************************					
* UnitExercise_Grass.cs					
* 작성자 : skjo					
* 작성일 : 2024.05.24 오후 3:00					
**********************************************************/
using System.Collections;
using UnityEngine;
using MoreMountains.Feedbacks;
using Dev_System;

namespace Dev_Unit
{					
	public class UnitExercise_Grass : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        [HideInInspector] public bool currectAnswer = false;

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private MMF_Player currectShootFeedback;
        [SerializeField] private MMF_Player currectDisableFeedback;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material currectMaterial;

        private int curProceedsNum = 0;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_Grass;
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
            //CinemachineManager.m_instance.SetExerciseRotate();
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;
            if (base.lastExercise)
            {
                Material[] materials = new Material[1];
                materials[0] = currectMaterial;
                meshRenderer.materials = materials;
                currectShootFeedback?.StopFeedbacks();
                currectShootFeedback?.PlayFeedbacks();
            }
            else
            {
                base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
                base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();
            }

            curProceedsNum++;

            //Debug.LogError(string.Format("ShootFeedbacks.Count : {0}, curProceedsNum : {1}", ShootFeedbacks.Count, curProceedsNum));
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                StartCoroutine(Active(this.gameObject, false, 1.5f));
            }
        }

        IEnumerator Active(GameObject obj, bool active, float time)
        {

            if (currectAnswer)
            {
                currectDisableFeedback.PlayFeedbacks();
            }
            else
            {
                DisableFeedback.PlayFeedbacks();
            }
            yield return new WaitForSeconds(time);
            yield return new WaitUntil(() => base.DisableFeedback.IsPlaying == false);
            if (lastExercise)
            {
                Managers.Unit.CurTurnExerciseUnitIndex--;
                //UnitManager.Instance.CurTurnExerciseUnitIndex--;
            }
            obj.SetActive(active);
        }


    }//end of class					


}//end of namespace					