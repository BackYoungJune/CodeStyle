/*********************************************************					
* PlayerAnimationEvent.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.02.13 오후 1:48					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

namespace Dev_Animation					
{					
	public class PlayerAnimationEvent : MonoBehaviour					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		
		public UnityAction halfEvent = null;		// 애니메이션 반 지났을 때 이벤트
		public UnityAction endEvent = null;			// 애니메이션 끝났을 경우 이벤트


		// 피드백 리스트
		//public MMF_Player WalkSoundEvent;			// 걷기 피드백 사운드
		//public MMF_Player RunSoundEvent;             // 달리기 피드백 사운드
		//public MMF_Player PaddleSoundEvent;          // 노젓기 피드백 사운드
		//public MMF_Player GlideSoundEvent;           // 글라이딩 피드백 사운드
		//public MMF_Player RightPaddleEvent;			// 오른쪽 노젓기 이벤트
		//public MMF_Player LeftPaddleEvent;			// 왼쪽 노젓기 이벤트


		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					

		void OnHalf()
        {
            halfEvent?.Invoke();
        }

        void OnEnd()
        {
            endEvent?.Invoke();
        }

        void WalkSound()
		{
			//WalkSoundEvent?.PlayFeedbacks();
		}

		void RunSound()
		{
			//RunSoundEvent?.PlayFeedbacks();
		}

		void PaddleSound()
		{
			//PaddleSoundEvent?.PlayFeedbacks();
		}

		void GlideSound()
		{
			//GlideSoundEvent?.PlayFeedbacks();
		}

		void RiaghtPaddle()
		{
			//RightPaddleEvent?.PlayFeedbacks();
        }

		void LeftPaddle()
		{
			//LeftPaddleEvent?.PlayFeedbacks();
		}
		


	}//end of class					
}//end of namespace					