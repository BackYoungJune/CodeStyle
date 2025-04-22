/*********************************************************					
* SceneControllerBase.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_Unit;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_SceneControl
{					
	public abstract class SceneControllerBase : MonoBehaviour					
	{

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        protected UnitPlayer Player;

		protected abstract IEnumerator CreateUnit();
		
        protected virtual void Update()
        {
        }
							
							
	}//end of class								
}//end of namespace					