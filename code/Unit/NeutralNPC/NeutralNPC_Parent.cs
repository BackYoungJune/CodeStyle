/*********************************************************					
* NeutralNPC2_Parent.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.08.18 오후 6:50					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Unit
{					
	public class NeutralNPC_Parent : MonoBehaviour					
	{					
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		public void ChildInteraction()
		{
			if (chileNeutrals == null) return;
			if(chileNeutrals.Length > 0)
			{
				foreach(var child in chileNeutrals)
				{
					if(child.gameObject.activeInHierarchy)
					{
                        child.Interaction();
                    }
				}
			}
		}

		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		private NeutralNPC[] chileNeutrals;
		

		void OnEnable()					
		{
			chileNeutrals = null;
            chileNeutrals = GetComponentsInChildren<NeutralNPC>(true);
		}					
							

						
	}//end of class					
}//end of namespace					