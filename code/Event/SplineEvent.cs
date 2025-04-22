/*********************************************************					
* SplineEvent.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.12.01 오후 3:21					
**********************************************************/					
using UnityEngine;
using Dev_Unit;
using Dreamteck.Splines;
using Dev_System;
using System.Collections;
using Dev_Transport;

namespace Dev_Spline
{					
    public enum SplineSpeedState
    {
        None, Up, Down, Climb,
    }

	public class SplineEvent : MonoBehaviour					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		public void SplineInit()
        {
            player = (UnitPlayer)Managers.Unit.GetCurTurnUnit;
            followNPC = Managers.Unit.GetCurFollowNPCUnit;
            if(Managers.Unit.UnitSubFollowNPCList.Count > 0)
            {
                subFollowNPC = Managers.Unit.GetCurSubFollowNPCUnit;
            }
        }
        
        public void ChangeSpline(SplineComputer changeComputer)
        {
            player.Follower.spline = changeComputer;
            player.Follower.Restart(0);

            if (followNPC != null)
            {
                SplineEnum SplineEnum = changeComputer.gameObject.GetComponent<SplineEnum>();
                if (SplineEnum.SplineStage == SplineStage.Stage)
                {
                    followNPC.SplineSetting(SplineStage.Stage, changeComputer);
                }
                else if (SplineEnum.SplineStage == SplineStage.Mission)
                {
                    followNPC.SplineSetting(SplineStage.Mission, changeComputer);
                }
                else if (SplineEnum.SplineStage == SplineStage.RunStraight)
                {
                    followNPC.SplineSetting(SplineStage.RunStraight, changeComputer);
                }
                else
                {
                    followNPC.SplineSetting(SplineStage.None, changeComputer);
                }
            }

            if (subFollowNPC != null)
            {
                SplineEnum SplineEnum = changeComputer.gameObject.GetComponent<SplineEnum>();
                if (SplineEnum.SplineStage == SplineStage.Stage)
                {
                    subFollowNPC.SplineSetting(SplineStage.Stage, changeComputer);
                }
                else if (SplineEnum.SplineStage == SplineStage.Mission)
                {
                    subFollowNPC.SplineSetting(SplineStage.Mission, changeComputer);
                }
                else if (SplineEnum.SplineStage == SplineStage.RunStraight)
                {
                    subFollowNPC.SplineSetting(SplineStage.RunStraight, changeComputer);
                }
                else
                {
                    subFollowNPC.SplineSetting(SplineStage.None, changeComputer);
                }
            }
        }

        public void ChangeSplineNPC(SplineComputer changeComputer)
        {
            if (followNPC != null)
            {
                SplineEnum SplineEnum = changeComputer.gameObject.GetComponent<SplineEnum>();
                followNPC.Follower.spline = changeComputer;
                followNPC.Follower.SetClipRange(0, 1);
                followNPC.Follower.followSpeed = 0;
                followNPC.Follower.Restart(0);
                followNPC.Follower.motion._offset.x = 0;
                followNPC.Follower.motion._offset.y = 0;
            }

            if (subFollowNPC != null)
            {
                SplineEnum SplineEnum = changeComputer.gameObject.GetComponent<SplineEnum>();
                subFollowNPC.Follower.spline = changeComputer;
                subFollowNPC.Follower.SetClipRange(0, 1);
                subFollowNPC.Follower.followSpeed = 0;
                subFollowNPC.Follower.Restart(0);
                subFollowNPC.Follower.motion._offset.x = 1.0f;
                subFollowNPC.Follower.motion._offset.y = 0;
            }
        }

        public void ChangeSplineTransport(Transport transport, SplineComputer changeComputer)
        {
            transport.Follower.spline = changeComputer;
            transport.Follower.Restart(0);
        }

        public void ChangeSplineSpeed(string _state)
        {
            SplineSpeedState state = (SplineSpeedState)System.Enum.Parse(typeof(SplineSpeedState), _state);
            float speed = 0;
            switch(state)
            {
                case SplineSpeedState.None:
                    {
                        speed = player.NormalFollowSpeed;
                        break;
                    }
                    case SplineSpeedState.Up:
                    {
                        speed = player.NormalFollowSpeed - 1;
                        break;
                    }
                    case SplineSpeedState.Down:
                    {
                        speed = player.NormalFollowSpeed + 1;
                        break;
                    }
                case SplineSpeedState.Climb:
                    {
                        speed = player.NormalFollowSpeed - 3;
                        break;
                    }
            }

            player.ChangeSpeed(speed);
        }
        
        public void ChangeSplinePlayer(SplineComputer changeComputer)
        {
            player.Follower.spline = changeComputer;
            player.Follower.Restart(0);
        }

        public void ChangeSplineTrainer(SplineComputer changeComputer)
        {
            UnitTrainer trainer = Managers.Unit.GetCurTrainerUnit;
            if(trainer != null)
            {
                trainer.Follower.spline = changeComputer;
                trainer.Follower.Restart(0);
            }
        }

        public void NullSplinePlayer()
        {
            player.Follower.spline = null;
        }


        private UnitPlayer player;
        private UnitFollowNPC followNPC;
        private UnitSubFollowNPC subFollowNPC;


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					

    }//end of class					
}//end of namespace					