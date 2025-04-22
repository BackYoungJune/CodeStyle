/*********************************************************					
* UnitNPC_Shong.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.29 오후 7:18					
**********************************************************/									
using UnityEngine;


namespace Dev_Unit
{
    public class UnitNPC_Shong : UnitFollowNPC
    {

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
      
        public void NPCRotation(float rot)
        {
            transform.rotation = Quaternion.Euler(0, rot, 0);
        }



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
            AssignAnimatorIDs();
        }

        protected override void AssignAnimatorIDs()
        {
            base.AssignAnimatorIDs();
        }

        protected override void Update()
        {
            base.Update();
          
        }


        protected override void OnDisable()
        {
            base.OnDisable();
        }



        //--------------------------------------------------------					
        // 추상함수 구현		
        //--------------------------------------------------------	
        public override void Move()
        {
            
        }

        public override void LookAt()
        {

        }

    }//end of class					


}//end of namespace					