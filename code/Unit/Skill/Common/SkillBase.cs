/*********************************************************					
* SkillBase.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.19 오후 7:26					
**********************************************************/									
using UnityEngine;					
					
			
namespace Dev_UnitSkill					
{					
	[RequireComponent(typeof(Rigidbody))]
	public abstract class SkillBase : MonoBehaviour					
	{

		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		protected void InitSkillBase(SkillBase skillBase)
		{
			RigidBody = this.GetComponent<Rigidbody>();
			RigidBody.isKinematic = true;
		}
		public void DisableModels()
        {
            foreach (GameObject value in SkillModels)
            {
				value.SetActive(false);
			}
        }

		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		[Header("---------- SkillBase Settings ----------")]
		[SerializeField] protected GameObject[] SkillModels;
		protected Rigidbody RigidBody;



	}//end of class					


}//end of namespace					