/*********************************************************					
* UnitPlayer_Follower.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.12.19 오전 10:18					
**********************************************************/									
using UnityEngine;					
					
// **************Namespace를 반드시 수정하세요**************					
namespace Dev_Unit					
{					
	public class UnitPlayer_Follower : MonoBehaviour					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		UnitPlayer UnitPlayer;

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					

        void Awake()
        {
			UnitPlayer = GetComponentInChildren<UnitPlayer>();    
        }

        void Start()					
		{					
							
		}					
							
		void Update()					
		{					
							
		}	
						
	}//end of class					
}//end of namespace					