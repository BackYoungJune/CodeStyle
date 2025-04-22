/*********************************************************					
* UnitScriptableObject.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.18 오후 6:12					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using System;

using Dev_System;

namespace Dev_Unit
{
	[Serializable]
	public class UnitPrefabInfo
    {
		public string discription;
		public UnitBase Prefab;
		public UnitUniqueID UniqueID;
	}


	[CreateAssetMenu(fileName = "UnitObjectTable_", menuName = "#ScriptableObject/UnitObjectTable")]
	public class UnitScriptableObject : ScriptableObject
	{

		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------	
		public Dictionary<UnitClass, Dictionary<UnitUniqueID, GameObject>> UnitPrefabTable = new();
		public UnitClass pUnitClassType { get { return UnitClassType; } }

		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		[Header("---------- Setting informations ----------")]
		[SerializeField] private UnitClass UnitClassType;
		[SerializeField] private UnitPrefabInfo[] UnitPrefabInfos;


		//void Awake()
		//{
		//	if (UnitClassType == UnitClass.None)
		//	{
		//		throw new Exception(string.Format("{[0]} 유닛클래스타입 설정하시오!", this.name));
		//	}
		//}


		public void CreateUnitObject(UnitUniqueID uniqueId, UnitState state, Transform genPos, bool isParent, Vector3 scale, bool revertGender)
        {
			GameObject targetPrefab = null;

			if (uniqueId == UnitUniqueID.Player)
            {
				Managers.Game.CharGenderTrigger(
					male: () =>
					{
						if (revertGender == false)
							targetPrefab = ComparePrefab(UnitUniqueID.Player_Male);
						else
                            targetPrefab = ComparePrefab(UnitUniqueID.Player_Female);

                    },
					female: () =>
					{
						if (revertGender == false)
							targetPrefab = ComparePrefab(UnitUniqueID.Player_Female);
						else
							targetPrefab = ComparePrefab(UnitUniqueID.Player_Male);

                    });
			}
            else
            {
				targetPrefab = ComparePrefab(uniqueId);
			}

			//Debug.Log(string.Format("---------- <color=#FF0000>{0}</color> 유닛 프리팹 생성 ----------", targetPrefab.name));

			GameObject newUnit;

			if (isParent)
            {
                newUnit = Instantiate(targetPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                newUnit.transform.SetParent(genPos);
				newUnit.transform.localPosition = Vector3.zero;
				newUnit.transform.localRotation = Quaternion.identity;
				newUnit.transform.localScale = scale;
			}
            else
            {
                newUnit = Instantiate(targetPrefab, genPos.position, genPos.rotation) as GameObject;
				newUnit.transform.localScale = scale;
			}

			UnitBase unit = newUnit.GetComponent<UnitBase>();
			if(unit != null)
			{
				unit.InitUnitBase(UnitClassType, uniqueId, state);
            }
        }


		GameObject ComparePrefab(UnitUniqueID uniqueId)
        {
			return Array.Find(UnitPrefabInfos, element => element.UniqueID == uniqueId).Prefab.gameObject;
		}



	}//end of class					
					
					
}//end of namespace					