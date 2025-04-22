/*********************************************************					
* BoatTransport.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.02.24 오전 11:02					
**********************************************************/
using Dev_System;
using MoreMountains.Feedbacks;
using UnityEngine;					
					
namespace Dev_Transport					
{					
	public class BoatTransport : Transport
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------		

        [SerializeField] private MMF_Player IdleFeedback;
        [SerializeField] private MMF_Player RunFeedback;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Init()
        {
            base.Init();
        }

        protected override void Update()
        {
			base.Update();

            //player.CurMoveSpeed = 1.0f;
            Managers.Game.LoginModeTrigger(
                loginPlayMode: () =>
                {
                    player.CurMoveSpeed = Managers.Gesture.pCurActivityGesture == null ? 0f : Managers.Gesture.pCurActivityGesture.PlayerSpeed;
                },
                debugPlayMode: () =>
                {
                    player.CurMoveSpeed = 1.0f;
                });
            if (base.player.CurMoveSpeed > Mathf.Epsilon)
            {
                base.Follower.followSpeed = base.FollowMaxSpeed * base.player.CurMoveSpeed;
                if (RunFeedback?.IsPlaying == false)
                {
                    RunFeedback?.PlayFeedbacks();
                    IdleFeedback?.StopFeedbacks();
                }
            }
            else
            {
                base.Follower.followSpeed = Mathf.Lerp(base.Follower.followSpeed, 1.0f, Time.deltaTime * 3.0f);
                if (IdleFeedback?.IsPlaying == false)
                {
                    IdleFeedback?.PlayFeedbacks();
                    RunFeedback?.StopFeedbacks();
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

    }//end of class					
}//end of namespace					