/*********************************************************					
* DevDialogueData.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.12.23 오후 3:59					
**********************************************************/					
using UnityEngine;
using System;
using Dev_Unit;

namespace Dev_Dialogue					
{					
	[Serializable]
	public struct PrepareInfo
    {
		public string discription;
		public int DialoguePrepare;
        public int Shoot_Num;
        public string Prepare_Condition;
	}

	[Serializable]
	public struct ShootInfo
	{
		public string discription;
		public int DialogueShoot;
		public string Shoot_Condition;
	}

	[Serializable]
	public class MissionDialogueInfos
    {
		public string discription;
		public ExerciseStage Stage;
		public PrepareInfo[] PrepareInfo;
		public ShootInfo[] ShootInfo;
    }

	[CreateAssetMenu(fileName = "Dev_Dialogue_", menuName = "#ScriptableObject/Dev_Dialogue/Stage")]
	public class DevDialogueTable : ScriptableObject
	{
		public MissionDialogueInfos[] MissionDialogueInfos;
						
	}//end of class			


	
}//end of namespace					