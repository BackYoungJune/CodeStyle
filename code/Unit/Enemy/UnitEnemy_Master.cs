/*********************************************************					
* UnitEnemy_Master.cs					
* 작성자 : Hmit					
* 작성일 : 2024.07.08 오후 1:16					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev_UnitSkill;
using Dev_System;

// **************Namespace를 반드시 수정하세요**************					
namespace Dev_Unit
{					
	public class UnitEnemy_Master : UnitEnemy
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void SpatulaOn()
        {
            SmokeParticle.Play();
            Spatula.SetActive(true);
        }

        public void SpatulaOff()
        {
            Spatula.SetActive(false);
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------			
        //총알파트는 Projectile이랑 Launcher로 이전할 것 !!!!!!!!
        [Header("---------- 임시 총알 ----------")]
        [SerializeField] private PoolUniqueID Projectile_Attack;
        [SerializeField] private ParticleSystem SmokeParticle;
        [SerializeField] private GameObject Spatula;


        protected override void Awake()
        {
            base.Awake();
        }


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
            AssignAnimatorIDs();
        }

        protected override void Update()
        {
            base.Update();

        }



        void AssignAnimatorIDs()
        {
            base.AnimatorIDTable.AnimID_AttackThrow = Animator.StringToHash("AttackThrow");
            base.AnimatorIDTable.AnimID_OnBattle = Animator.StringToHash("OnBattle");
            base.AnimatorIDTable.AnimID_Hits = new int[4];
            base.AnimatorIDTable.AnimID_Hits[0] = Animator.StringToHash("Hit1");
            base.AnimatorIDTable.AnimID_Hits[1] = Animator.StringToHash("Hit2");
            base.AnimatorIDTable.AnimID_Hits[2] = Animator.StringToHash("Hit3");
            base.AnimatorIDTable.AnimID_Hits[3] = Animator.StringToHash("Hit4");
            base.AnimatorIDTable.AnimID_Dead = Animator.StringToHash("Dead");
        }

        IEnumerator IOnDamaged()
        {
            base.ModelMeshRenderer.material = base.OnDamageMat;
            yield return new WaitForSeconds(1.0f);
            base.ModelMeshRenderer.material = base.NormalMat;
        }


        //--------------------------------------------------------					
        // 추상함수 구현		
        //--------------------------------------------------------	
        public override void Idle()
        {

        }

        public override void OnAttack(Transform genpos)
        {
            GameObject projectile = Managers.ObjectPool.GetObjectFromPool(PoolObjectType.Pool, Projectile_Attack);
            projectile.transform.position = genpos.position;
            projectile.transform.rotation = genpos.rotation;
            projectile.GetComponent<ProjectileSkill>().Initializing(Managers.Unit.GetCurTurnUnit, DevUtils.GetTag(DevDefine.ETag.Player), Managers.Battle.pGestureReferees);

        }

        public override void OnDamage()
        {
            //랜덤 히트동작
            int random = Random.Range(0, base.AnimatorIDTable.AnimID_Hits.Length);
            base.pAnimatorbase.SetTrigger(AnimatorIDTable.AnimID_Hits[random]);
            base.HitFeedback?.PlayFeedbacks(this.transform.position);

            StopCoroutine("IOnDamaged");
            base.ModelMeshRenderer.material = base.NormalMat;
            StartCoroutine("IOnDamaged");
        }

        public override void Dead()
        {
            base.ModelMeshRenderer.material = base.OnFinalDamageMat;
            base.pAnimatorbase.SetBool(AnimatorIDTable.AnimID_Dead, true);
            base.DeadFeedback?.PlayFeedbacks(this.transform.position);
        }



    }//end of class					
					
					
}//end of namespace					