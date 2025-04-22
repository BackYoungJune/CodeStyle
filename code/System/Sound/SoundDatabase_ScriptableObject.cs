/*********************************************************					
* SoundDatabase.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using System;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_System			
{
	[Serializable]
	public class SoundData
	{
		public string discription;
		public string key;
		public AudioClip clip;
	}


	[CreateAssetMenu(fileName = "SoundDatabase", menuName = "#ScriptableObject/SoundDatabase_")]
	public class SoundDatabase_ScriptableObject : ScriptableObject
	{
		public SoundData[] pDatas { get { return Datas; } }

		public SoundType SoundType = SoundType.None;
		[SerializeField]
		private SoundData[] Datas;



    }
}//end of namespace					