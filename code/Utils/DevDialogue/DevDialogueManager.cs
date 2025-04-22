/*********************************************************					
* DevDialogueManager.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.11.28 오전 9:39					
**********************************************************/

using UnityEngine;

namespace Dev_Dialogue
{					
	public static class DevDialogueManager					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//-----------------------------------------------------
		public static DevDialogueController Controller
        {
			get
            {
				if (controller == null) controller = GameObject.FindObjectOfType<DevDialogueController>();
				return controller;
            }
        }

        public static DevDialogueMissionController Mission
        {
            get
            {
                if (M_instance == null) M_instance = GameObject.FindObjectOfType<DevDialogueMissionController>();
                return M_instance;
            }
        }


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        private static DevDialogueController controller = null;
        private static DevDialogueMissionController M_instance = null;


    }//end of class					
}//end of namespace					