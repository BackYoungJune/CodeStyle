/*********************************************************					
* UnitExercise_MeatSkewer.cs					
* 작성자 : skjo			
* 작성일 : 2024.08.16 오후 3:41					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Unit
{
    public class UnitExercise_MeatSkewer : UnitExercise
    {

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] private Material freshMaterial;
        [SerializeField] private Material sucessMaterial;
        [SerializeField] private ParticleSystem smokeParticle;

        private int curProceedsNum = 0;
        private ParticleSystem.MainModule mainModule;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_MeatSkewer;
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
            SmokeParticleInit();
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
            if (curProceedsNum == 0)
            {
                StartCoroutine(ChangeMaterial(meshRenderer, freshMaterial));
            }
            curProceedsNum++;

            //마지막 연출 feedback 
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                base.OnDisable();
            }
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

        void SmokeParticleInit()
        {
            // 메인 모듈을 적용한다
            mainModule = smokeParticle.main;

            // 스모크 파티클 Transform 조정 및 비율계산
            float size = Random.Range(0.2f, 0.6f);
            float multifier = (size / 0.6f) * 0.2f;

            Vector3 normalPosition = smokeParticle.transform.localPosition;
            smokeParticle.transform.localPosition = new Vector3(normalPosition.x, multifier, normalPosition.z);
            smokeParticle.transform.localScale = new Vector3(0.4f, size, 0.4f);

            // 스모크 파티클 Simulation Speed 조정
            float randSpeed = Random.Range(0.35f, 0.5f);
            mainModule.simulationSpeed = randSpeed;

            // 스모크 파티클 플레이
            smokeParticle.Play();
        }

        IEnumerator ChangeMaterial(MeshRenderer original, Material change)
        {
            yield return new WaitForSeconds(0.4f);

            original.material = change;
        }




    }//end of class					


}//end of namespace					