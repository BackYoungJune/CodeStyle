/*********************************************************					
* Physical_Shield.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.28 오전 11:56					
**********************************************************/								
using UnityEngine;					
					
				
namespace Dev_UnitSkill
{					
	public class Physical_Shield : PhysicalSkill
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
            base.ChangeStateMachine(PhysicalType.Fixed);
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
            


        }




    }//end of class					
					
					
}//end of namespace					