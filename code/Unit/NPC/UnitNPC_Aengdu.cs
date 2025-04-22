/*********************************************************					
* UnitNPC_Parrot.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.09.19 오전 9:33					
**********************************************************/
using UnityEngine;

namespace Dev_Unit
{					
	public class UnitNPC_Aengdu : UnitSubFollowNPC
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
            base.pUniqueID = UnitUniqueID.NPC_Aengdu;

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

        public override void Move()
        {
            throw new System.NotImplementedException();
        }


        //--------------------------------------------------------					
        // 추상함수 구현		
        //--------------------------------------------------------	
        //public override void Move()
        //{

        //}

        //public override void LookAt()
        //{

        //}

    }//end of class							
}//end of namespace					