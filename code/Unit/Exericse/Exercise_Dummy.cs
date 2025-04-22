/*********************************************************					
* Exercise_Dummy.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.05.03 오후 2:16					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Linq;

// **************Namespace를 반드시 수정하세요**************					
namespace Dev_Unit					
{					
	public class Exercise_Dummy : MonoBehaviour					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		public void PrepareFeedback()
		{
            if (PrepareFeedbacks.Length <= curProceedsNum) return;

			PrepareFeedbacks[curProceedsNum].PlayFeedbacks();
        }

		public void ShootFeedback()
		{
            if (ShootFeedbacks.Length <= curProceedsNum) return;

			ShootFeedbacks[curProceedsNum].PlayFeedbacks();
			curProceedsNum++;
        }

		public void FirstPrepareFeedback()
		{
			if(firstfeedback)
			{
				FirstFeedback.PlayFeedbacks();
				firstfeedback = false;
            }
		}

		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		[SerializeField] private MMF_Player[] PrepareFeedbacks;
		[SerializeField] private MMF_Player[] ShootFeedbacks;
		[SerializeField] private MMF_Player FirstFeedback;

        private int curProceedsNum = 0;
		private bool firstfeedback = true;

        void Start()
        {
			curProceedsNum = 0;
        }

    }//end of class					
}//end of namespace					