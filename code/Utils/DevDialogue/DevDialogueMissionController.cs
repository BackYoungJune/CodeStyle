/*********************************************************					
* DevDialogueMissionController.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.12.26 오후 3:55					
**********************************************************/					
using UnityEngine;
using Dev_Unit;
using Dev_System;

namespace Dev_Dialogue
{					
	public class DevDialogueMissionController : MonoBehaviour					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					

		// 미션 오브젝트가 생성되고 0.1f후에 호출되도록 하기
		public void SetMissionObject()
        {
			int missionNums = Managers.Unit.UnitExerciseList.Count;
			prepareNum = 0;
			shootNum = 0;
			for (int i = 0; i < missionNums; i++)
            {
				UnitExercise Exercise = (UnitExercise)Managers.Unit.UnitExerciseList[i];
				Exercise.PrePareEvent += MissionPrepare;
				Exercise.ShootEvent += MissionShoot;
			}
			dialogueInfos = System.Array.Find(dialogueTable.MissionDialogueInfos, x => x.Stage == Managers.Unit.GetCurTargetExerciseUnit.ExerciseStage);
		}


		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		[SerializeField] private DevDialogueTable dialogueTable;
		private int prepareNum = 0;
		private int shootNum = 0;
		private MissionDialogueInfos dialogueInfos;

        // Exercise Prepare와 Scriptable DevDialogueTable가 같으면 호출
        void MissionPrepare()
        {
            prepareNum++;
            if (dialogueInfos != null)
            {
                // shoot이 같으면 호출
                var num = System.Array.Find(dialogueInfos.PrepareInfo, x => x.Shoot_Num == shootNum);
                // // shoot이 같으면 호출 (혹시 모를 보호코드)
                if (num.Shoot_Num == shootNum)
                {
                    if (num.DialoguePrepare == 0) return;
                    if (num.DialoguePrepare == prepareNum)
                    {
                        DevDialogueManager.Controller.SetVariable(num.Prepare_Condition);
                    }
                }
            }
        }

        // Exercise Shoot과 Scriptable DevDialogueTable가 같으면 호출
        void MissionShoot()
        {
            //ToDo : ShootNum 초기화가 되어 Shoot 2 Event가 안걸립니다!
            shootNum++;

            // dialogueInfos 도 값이 안들어가 있습니다!
            prepareNum = 0;
            if (dialogueInfos != null)
            {
                var num = System.Array.Find(dialogueInfos.ShootInfo, x => x.DialogueShoot == shootNum);
                if (num.DialogueShoot == 0) return;
                DevDialogueManager.Controller.SetQuest(PixelCrushers.DialogueSystem.QuestState.Unassigned);
                
                DevDialogueManager.Controller.SetVariable(num.Shoot_Condition);
            }
        }

    }//end of class					
}//end of namespace					