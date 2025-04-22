/*********************************************************					
* AnimationRepeat.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.02.07 오전 9:42					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;

namespace Dev_Animation					
{					
	public class AnimationRepeatEvent : StateMachineBehaviour					
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 처음 진입하거나 반복이 끝나면 초기화 해준다
            if(Init)
            {
                RepeatCount = RepeatNormalCount;
                count = 0;
                Init = false;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isRepeat)
            {
                count++;
                animator.SetTrigger(RepeatAnimation);

                if (count >= RepeatCount)
                {
                    isRepeat = false;
                    Init = true;
                }
            }

            // 다 돌면 다시 반복 bool값을 true로 바꿔준다
            if(count == 0)
            {
                isRepeat = true;
            }


        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }


        public int RepeatNormalCount = 0;
        public string RepeatAnimation = "";
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        private int RepeatCount = 0;
        private int count = 0;
        private bool isRepeat = true;
        private bool Init = true;

    }//end of class					
}//end of namespace					