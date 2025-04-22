/*********************************************************					
* UnitPlayer_VR.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.29 오후 7:19					
**********************************************************/								
using UnityEngine;
using Dev_UnitSkill;
using Dev_System;

namespace Dev_Unit
{
    public class UnitPlayer_VR : UnitPlayer
    {

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					




        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------	

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
        }

        protected override void Update()
        {
            base.Update();
        }

        //--------------------------------------------------------					
        // 추상함수 구현		
        //--------------------------------------------------------	
        public override void OnIdle(int animatorID, bool bo, float delay)
        {
            StartCoroutine(base.SelectIdle(animatorID, bo, delay));
        }

        public override void OnAttack(Transform genPos)
        {
            float scale = 1.0f;
            Managers.Game.GestureRangeTrigger(
                 punchAction: () =>
                 {
                     GameObject fistProjectile = Managers.ObjectPool.GetObjectFromPool(PoolObjectType.Pool, PoolUniqueID.Effect_Fist);// Instantiate(Projectile_Fist, genPos.position, genPos.rotation);
                     fistProjectile.transform.position = genPos.position;
                     fistProjectile.transform.rotation = genPos.rotation;
                     
                     if (Managers.Unit.GetCurTargetUnit != null)
                     {
                         if (Managers.Unit.GetCurTargetUnit.pEnemyScale == EnemyScale.Small)
                         {
                             scale = 0.3f;
                         }
                     }
                     fistProjectile.GetComponent<ProjectileSkill>().Initializing(Managers.Unit.GetCurTargetUnit, DevUtils.GetTag(DevDefine.ETag.Enemy), Managers.Battle.pGestureReferees, scale);
                 },
                  kickAction: () =>
                  {
                      GameObject kickProjectile = Managers.ObjectPool.GetObjectFromPool(PoolObjectType.Pool, PoolUniqueID.Effect_Kick);// Instantiate(Projectile_Kick, genPos.position, genPos.rotation);
                      kickProjectile.transform.position = genPos.position;
                      kickProjectile.transform.rotation = genPos.rotation;
                      if (Managers.Unit.GetCurTargetUnit != null)
                      {
                          if (Managers.Unit.GetCurTargetUnit.pEnemyScale == EnemyScale.Small)
                          {
                              scale = 0.3f;
                          }
                      }
                      kickProjectile.GetComponent<ProjectileSkill>().Initializing(Managers.Unit.GetCurTargetUnit, DevUtils.GetTag(DevDefine.ETag.Enemy), Managers.Battle.pGestureReferees, scale);
                  });

            //[obsolete]
            /*
            int random = Random.Range(0, 2);

               if (random == 0)
               {
                   GameObject fistProjectile = Instantiate(Projectile_Fist, genPos.position, genPos.rotation);
                   fistProjectile.GetComponent<ProjectileSkill>().Initializing(UnitManager.Instance.GetCurTargetUnit, "Enemy", Managers.Battle.pGestureReferees);
                   //Debug.LogError("-------- Player Female Fist 공격! ----------");
               }
               else
               {
                   GameObject kickProjectile = Instantiate(Projectile_Kick, genPos.position, genPos.rotation);
                   kickProjectile.GetComponent<ProjectileSkill>().Initializing(UnitManager.Instance.GetCurTargetUnit, "Enemy", Managers.Battle.pGestureReferees);
                   //Debug.LogError("-------- Player Female Kick 공격! ----------");
               }
             */


        }

        public override void Guard(GestureReferee referee, Transform genPos)
        {
            if (referee == GestureReferee.Perfect)
            {
                base.Perfect_GuardFeedback?.PlayFeedbacks(transform.position);
            }
            else if (referee == GestureReferee.Good)
            {
                base.Good_GuardFeedback?.PlayFeedbacks(transform.position);
            }


            //PhysicalSkill_Shield.SetActive(true);
            //GameObject skillShield = Instantiate(PhysicalSkill_Shield, genPos.position, genPos.rotation);
            //skillShield.GetComponent<PhysicalSkill>().Initializing("Skill", Managers.Battle.pGestureReferees);
            //Debug.LogError("-------- Player Female 방어 ! ----------");
        }

        public override void OnExercise()
        {

        }

        public override void Dead()
        {
        }


    }//end of class					
}//end of namespace					