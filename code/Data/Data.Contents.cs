/*********************************************************					
* Data.Contents.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using System;
					
namespace Dev_Data				
{
    #region SoundData
    [Serializable]
	public class SoundData
	{
		public int DataID;
        public string Type;
        public string Name;
		public string Description;
	}
    #endregion

    #region SkillData
    [Serializable]
    public class SkillData
    {
        public int DataID;
        public string Name;
        public string ClassName;
        public string ComponentName;
        public string Description;
        public int ProjectileId;
        public string PrefabLabel;
        public string IconLabel;
        public string AnimName;
        public float CoolTime;
        public float DamageMultiplier;
        public float Duration;
        public float NumProjectiles;
        public string CastingSound;
        public float AngleBetweenProj;
        public float SkillRange;
        public float RotateSpeed;
        public float ScaleMultiplier;
        public float AngleRange;
    }
    #endregion
}//end of namespace					