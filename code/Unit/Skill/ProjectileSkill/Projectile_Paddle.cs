/*********************************************************					
* Projectile_Paddle.cs					
* 작성자 : skjo				
* 작성일 : 2024.05.27 오전 9:48					
**********************************************************/
using Dev_Unit;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dev_UnitSkill
{					
	public class Projectile_Paddle : ProjectileSkill
    {

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public override void Initializing(UnitBase target, string targetTag, GestureReferee curReferee, float scale = 1.0f)
        {
            base.Initializing(target, targetTag, curReferee, scale);
            base.ChangeStateMachine(ProjectileType.Rotate);
            PlayFeedback?.PlayFeedbacks();

            foreach (GameObject value in base.SkillModels)
            {
                value.SetActive(true);
            }
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private MMF_Player PlayFeedback;

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
            base.ChangeStateMachine(ProjectileType.Rotate);
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

        }
        protected override void OnTriggerEnter(Collider col)
        {
            base.OnTriggerEnter(col);


        }




        //--------------------------------------------------------					
        // 추상함수 구현		
        //--------------------------------------------------------	
        public override void OnHit()
        {
            base.ChangeStateMachine(ProjectileType.Idle);
            if (base.pCurReferees == Dev_Unit.GestureReferee.Perfect)
            {
                base.Perfect_HitFeedback?.PlayFeedbacks(this.transform.position);
            }
            else if (base.pCurReferees == Dev_Unit.GestureReferee.Good)
            {

                base.Good_HitFeedback?.PlayFeedbacks(this.transform.position);
            }
        }


    }//end of class					


}//end of namespace					