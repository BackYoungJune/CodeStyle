/*********************************************************					
* UnitExercise_Dango.cs					
* 작성자 : lbioh					
* 작성일 : 2024.07.05 오후 5:09					
**********************************************************/
using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev_System;


namespace Dev_Unit
{					
	public class UnitExercise_Dango : UnitExercise
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        
        public UnityAction first2PrepareAction;

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					



        [SerializeField] private MMF_Player firstPrepareFeedback;
        [SerializeField] private MMF_Player first2PrepareFeedback;
        [SerializeField] private MeshRenderer[] DangoMeshRenderer;
        [SerializeField] private Material SoftMaterial;
        [SerializeField] private Material HardMaterial;
        [SerializeField] private ParticleSystem SmokeParticle;
        [SerializeField] private GameObject Dango;
        [SerializeField] private int curProceedsNum = 0;

        private bool firstFeedback = true;
        private bool first2Feedback = false;
        private Material createFinishMaterial;
        private float MaterialRatio;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_Dango;
        }


        protected override void OnEnable()
        {
            base.OnEnable();

        }

        protected override void Start()
        {
            base.Start();
            curProceedsNum = 0;
            createFinishMaterial = Instantiate(HardMaterial);
            Color createColor = createFinishMaterial.color;
            createColor.a = 0f;
            createFinishMaterial.color = createColor;
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
            first2PrepareAction = null;
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
            // 맨처음 Prepare 호출되면
            if (firstFeedback)
            {
                firstFeedback = false;
                firstPrepareFeedback.PlayFeedbacks();
                return;
            }

            if (first2Feedback)
            {
                first2Feedback = false;
                first2PrepareFeedback.PlayFeedbacks();
                first2PrepareAction?.Invoke();
                return;
            }

            // 두번째 Prepare라면
            if (curProceedsNum == 1)
            {
                Dango.SetActive(false);
                int curObjectNum = Managers.Exercise.ExerciseObjectNums[Managers.Unit.GetCurTargetExerciseUnit.GetComponent<UnitExercise>().ExerciseStage];
                MaterialRatio = (int)Managers.Gesture.pCurGestureCount / curObjectNum;

                float ratio = 1 / (MaterialRatio - 1);
                FriedMaterialChange(SoftMaterial, createFinishMaterial);
                ChangeAlpha(ratio);
            }
            base.PrePareFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.PrePareFeedbacks[curProceedsNum]?.PlayFeedbacks();
        }


        void ChangeAlpha(float raito)
        {
            Color createColor = createFinishMaterial.color;
            float desAlpha = createColor.a + raito;
            createColor.a = desAlpha;
            createFinishMaterial.color = createColor;
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;

            base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();

            // 첫번째 Shoot이라면
            if (curProceedsNum == 0)
            {
                Invoke("SmokeInvoke", 0.4f);
                Invoke("DangoInvoke", 0.4f);
                first2Feedback = true;
            }


            curProceedsNum++;

            //마지막 연출 feedback 
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                //FriedMaterialChange(FiredMaterial, FinishMaterial);
                base.OnDisable();
                //StartCoroutine(Active(this.gameObject, false, 1.5f));
            }
        }

        void SmokeInvoke()
        {
            SmokeParticle.Play();
        }

        void DangoInvoke()
        {
            Dango.SetActive(false);
        }

        void FriedMaterialChange(Material firstMaterial, Material SecondMaterial)
        {
            foreach (var renderer in DangoMeshRenderer)
            {
                Material[] materials = renderer.materials;
                materials[0] = firstMaterial;
                materials[1] = SecondMaterial;
                renderer.materials = materials;
            }
        }

        IEnumerator Active(GameObject obj, bool active, float time)
        {
            yield return new WaitForSeconds(time);
            //if (obj == this.gameObject)
            //{
            //    base.DisableFeedback.PlayFeedbacks();

            //    yield return new WaitUntil(() => base.DisableFeedback.IsPlaying == false);
            //}

            obj.SetActive(active);
        }

    }//end of class					
}//end of namespace					