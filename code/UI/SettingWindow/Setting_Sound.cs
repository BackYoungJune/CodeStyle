/*********************************************************					
* Setting_Sound.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.08.23 오전 8:57					
**********************************************************/
using UnityEngine;

namespace Dev_UI
{					
	public class Setting_Sound : MonoBehaviour	
	{					
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		public void Init()
		{
			// 초기화
			foreach (var setting in SoundSettings)
			{
				setting.Init();
            }
		}

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private Prefab_SoundSetting[] SoundSettings;

        


    }//end of class					
}//end of namespace					