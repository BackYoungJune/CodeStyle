/*********************************************************					
* UnitExercise_Okonomi.cs					
* 작성자 : Hmit					
* 작성일 : 2024.07.05 오전 9:20					
**********************************************************/
using System.Collections;
using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;
using Dev_System;

// **************Namespace를 반드시 수정하세요**************					
namespace Dev_Unit
{					
	public class UnitExercise_Okonomi : UnitExercise
    {

        [SerializeField] private MMF_Player firstPrepareFeedback;
        [SerializeField] private MMF_Player firstPrepareFeedback2;
        [SerializeField] private SkinnedMeshRenderer OkonomiMeshRenderer;
        [SerializeField] private Material FiredMaterial;
        [SerializeField] private Material FinishMaterial;
        [SerializeField] private GameObject Spatula;
        [SerializeField] private GameObject Brush;
        [SerializeField] private ParticleSystem SmokeParticle;
        private int curProceedsNum = 0;

        private bool firstFeedback = true;
        private bool firstFeedback2 = true;
        private Material createFinishMaterial;
        private float MaterialRatio;
        private ParticleSystem.MainModule mainModule;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_Okonomi;
        }


        protected override void OnEnable()
        {
            base.OnEnable();

        }

        protected override void Start()
        {
            base.Start();
            curProceedsNum = 0;
            createFinishMaterial = Instantiate(FinishMaterial);
            Color createColor = createFinishMaterial.color;
            createColor.a = 0f;
            createFinishMaterial.color = createColor;

            ParticleInit();
        }

        void ParticleInit()
        {
            // 메인 모듈을 적용한다
            mainModule = SmokeParticle.main;

            // 스모크 파티클 Transform 조정
            float size = Random.Range(0.3f, 1.0f);
            float multifier = 0;
            if (size > 0.8f) multifier = -0.03f;
            else if (size > 0.4f && size <= 0.8f) multifier = 0.04f;
            else if (size <= 0.4f) multifier = 0.07f;
            SmokeParticle.transform.localPosition = new Vector3(-0.071f, size + multifier, -0.073f);
            SmokeParticle.transform.localScale = new Vector3(1.0f, size, 1.0f);

            // 스모크 파티클 Simulation Speed 조정
            float randSpeed = Random.Range(0.35f, 0.5f);
            mainModule.simulationSpeed = randSpeed;
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
            StopAllCoroutines();
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
                firstPrepareFeedback?.PlayFeedbacks();
                return;
            }


            // 두번째 Prepare라면
            if (curProceedsNum == 1)
            {
                float ratio = 0.5f;
                if (firstFeedback2)
                {
                    firstFeedback2 = false;
                    firstPrepareFeedback2?.PlayFeedbacks();
                    Invoke("BrushActiveFalse", 1.6f);
                    FriedMaterialChange(FiredMaterial, createFinishMaterial);
                    StartCoroutine(ChangeAlpha(ratio));
                    return;
                }


                Brush.SetActive(false);

                //int curObjectNum = Managers.Exercise.ExerciseObjectNums[Managers.Unit.GetCurTargetExerciseUnit.GetComponent<UnitExercise>().ExerciseStage];
                //MaterialRatio = Managers.Gesture.pCurGestureCount / curObjectNum;
                MaterialRatio = 1f;

                //float ratio = 1 / (MaterialRatio - 1);
                //FriedMaterialChange(FiredMaterial, createFinishMaterial);
                //StopCoroutine(ChangeAlpha(ratio));
                StartCoroutine(ChangeAlpha(ratio));
                Invoke("BrushActiveFalse", 1.6f);
            }
            base.PrePareFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.PrePareFeedbacks[curProceedsNum]?.PlayFeedbacks();
        }

        void BrushActiveFalse()
        {
            Brush.SetActive(false);
        }

        IEnumerator ChangeAlpha(float raito)
        {
            Color createColor = createFinishMaterial.color;
            float desAlpha = createColor.a + raito;
            while (createColor.a <= desAlpha - 0.05f)
            {
                createColor.a = Mathf.Lerp(createColor.a, desAlpha, Time.deltaTime * 3.0f);
                createFinishMaterial.color = createColor;
                yield return null;

                if (createColor.a > 1.0f) break;
            }
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;

            firstFeedback = false;

            base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();
            // 첫번째 Shoot이라면
            if (curProceedsNum == 0)
            {
                Invoke("FiredMaterialInvoke", 0.4f);
                Invoke("SpatulaActive", 0.8f);
            }
            curProceedsNum++;

            //마지막 연출 feedback 
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                FriedMaterialChange(FiredMaterial, FinishMaterial);
                base.OnDisable();
                //StartCoroutine(Active(this.gameObject, false, 1.5f));
            }
        }

        void SpatulaActive()
        {
            Spatula.SetActive(false);
        }

        void FiredMaterialInvoke()
        {
            FriedMaterialChange(FiredMaterial, FiredMaterial);
        }

        void FriedMaterialChange(Material firstMaterial, Material SecondMaterial)
        {
            Material[] materials = OkonomiMeshRenderer.materials;
            materials[0] = firstMaterial;
            materials[1] = SecondMaterial;
            OkonomiMeshRenderer.materials = materials;
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