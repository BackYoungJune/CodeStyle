/*********************************************************					
* UnitDataTable.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.18 오후 6:12					
**********************************************************/					
namespace Dev_Unit
{
	public enum UnitClass
	{
		None, 
		Player, 
		NPC, 
		Enemy,
		Exercise,
		Trainer,
	}

	public enum UnitUniqueID
	{
		None,

		//----- Player -----
		Player_Male,
		Player_Female,
		Player,
		Player_VR,

        //----- Trainer -----
        Trainer_Fitness,

        //----- NPC -----
        NPC_Shong,
        NPC_ShongHood,
        NPC_Aengdu,

        //----- Enemy -----
        Enemy_Mini,             // Chapter1~
        Enemy_MinJi,
        Enemy_Nyangi,
        Enemy_Duckja,
        Enemy_Bee,
        Enemy_Moonfar,
        Enemy_StoneBox,         // Chapter2~
        Enemy_Master,
        Enemy_Siro,
        Enemy_Lamp,
        Enemy_Hana,
        Enemy_Hana2,
        Enemy_Yuki,

        //----- Exercise ----
        Exercise_Carrot,        // Chapter1~
        Exercise_Torch,
        Exercise_Grass,
        Exercise_Honey,
        Exercise_HoneyWater,
        Exercise_Fishing,
        Exercise_Okonomi,       // Chapter2~
        Exercise_Dango,
        Exercise_Wasabi,
        Exercise_Sushi,
        Exercise_MountBread,

        //----- Trainer -----
        Trainer_Adventure,

        //----- Enemy ----
        Enemy_Willy,           // Chapter3~
        Enemy_Willy2,
        Enemy_Max,
        Enemy_Max2,
        Enemy_Aengdu,


        //----- Exercise ----
        Exercise_FishMove,      // Chapter3~
        Exercise_WoodStick,
        Exercise_CampFire,
        Exercise_MeatSkewer
    }


    public static class UnitDataTable
    {

		public static UnitClass GetUnitClass(UnitUniqueID uniqueId)
		{
			string uniqueStr = uniqueId.ToString();

            // uniqueID의 첫번째 _까지 짤라서 사용, 뒤에서부터 찾으려면 LastIndexOf사용
            int index = uniqueStr.IndexOf("_");
            uniqueStr = uniqueStr.Substring(0, index);

            return DevUtils.StringToEnum<UnitClass>(string.Format("{0}", uniqueStr));
        }
    }
}//end of namespace					