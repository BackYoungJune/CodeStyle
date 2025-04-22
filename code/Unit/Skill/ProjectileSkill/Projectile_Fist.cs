/*********************************************************					
* Projectile_Fist.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.19 오후 7:37					
**********************************************************/
using Dev_Unit;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;


namespace Dev_UnitSkill
{					
	public class Projectile_Fist : ProjectileSkill
	{

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public override void Initializing(UnitBase target, string targetTag, GestureReferee curReferee, float scale = 1.0f)
        {
            base.Initializing(target, targetTag, curReferee, scale);
            Fist.SetActive(true);
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private GameObject Fist;



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
            base.ChangeStateMachine(ProjectileType.Straight);
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