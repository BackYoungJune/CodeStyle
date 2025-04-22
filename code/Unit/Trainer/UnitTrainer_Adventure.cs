/*********************************************************					
* UnitTrainer_Adventure.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.06.13 오전 9:17					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Unit
{					
	public class UnitTrainer_Adventure : UnitTrainer
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
            AssignAnimatorIDs();
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
        protected override void AssignAnimatorIDs()
        {
            base.AssignAnimatorIDs();
        }

        public override void OnIdle(int animatorID, bool bo, float delay)
        {
            StartCoroutine(base.SelectIdle(animatorID, bo, delay));
        }

    }//end of class					
}//end of namespace					