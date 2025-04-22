/*********************************************************					
* TransportTable.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.03.03 오전 8:55					
**********************************************************/
using System;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Transport					
{
	public enum TransportEnum
	{
		None, Boat, Glider,
	}

	[Serializable]
	public class TransportInfos
	{
		public string discription;
		public TransportEnum enumKey;
		public GameObject prefab;
	}

	[CreateAssetMenu(fileName = "Transport_", menuName = "#ScriptableObject/Transport_")]
	public class TransportTable : ScriptableObject					
	{
		public TransportInfos[] TransportInfos;

    }//end of class					
}//end of namespace					