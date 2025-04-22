/*********************************************************					
* SimpleObjectPoolData.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.07.25 오전 9:29					
**********************************************************/
using System;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_System					
{					
	public enum PoolObjectType
	{
		None,
		Pool,
		UI,

	}

	public enum PoolUniqueID
	{
		None,
		// ----------------------------------
		//				Effect
		// ----------------------------------
		Effect_Fist,
		Effect_Kick,
		Effect_Fireball,
		Effect_Foot,
		Effect_Hand,
		Effect_Hip,
		Effect_Paddle,
		Effect_Iceball,
		Effect_Carrot,
		Effect_HoneyJar,
		Effect_BeeSting,

        // ----------------------------------
        //				UI Dummy
        // ----------------------------------
		TutorialRoot,

        // ----------------------------------
        //				UI Dummy
        // ----------------------------------
		Transport_Boat,
		Transport_Glider,
    }

    [Serializable]
	public class PoolDataTable
	{
		public string discription;
		public PoolUniqueID enumKey;
		public GameObject prefab;
		//public int poolSize;
	}

	[Serializable]
	public class PoolUIDataTable
	{

	}


    [CreateAssetMenu(fileName = "ObjectPoolTable_", menuName = "#ScriptableObject/ObjectPool")]
    public class ObjectPoolData : ScriptableObject
    {
		public PoolObjectType PoolType { get { return poolType; } }
		public List<PoolDataTable> PoolData { get { return poolData; } set { poolData = value; } }
		public bool AutoSetting_EnumPool = false;
		public bool AutoSetting_Initailizing = false;

		[SerializeField] private PoolObjectType poolType;
        [SerializeField] private List<PoolDataTable> poolData;

						
	}//end of class					
}//end of namespace					