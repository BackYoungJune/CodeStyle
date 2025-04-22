/*********************************************************					
* UnitManager.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.03 오후 4:52					
**********************************************************/								
using System.Collections.Generic;					
using UnityEngine;
using System;

namespace Dev_Unit
{
	public class UnitManager : MonoBehaviour					
	{

		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------		
		
		/// <summary>
		/// 유닛 생성 함수
		/// </summary>
		/// <param name="uniqueId">생성하려는 유닛 이름</param>
		/// <param name="genpos">생성위치</param>
		/// <param name="isParent">생성위치의 자식으로 넣을까?</param>
		public void CreateUnit(UnitUniqueID uniqueId, UnitState state, Transform genpos, bool isParent, Vector3 scale, bool revertGender = false)
        {
			UnitClass targetClass = UnitDataTable.GetUnitClass(uniqueId);
			UnitScriptableObject targetScriptable = GetUnitScriptable(targetClass);

			//유닛 생성
			targetScriptable.CreateUnitObject(uniqueId, state, genpos, isParent, scale, revertGender);
		}


		//유닛 생성시 등록 함수
		public void AddUnit(UnitBase unit)
        {
			if(unit is UnitPlayer)
            {
				UnitPlayerList.Add(unit);
			}
			if(unit is UnitNPC)
            {
				if(unit is UnitFollowNPC)
				{
                    UnitFollowNPCList.Add(unit);
                }
				else if(unit is UnitSubFollowNPC)
				{
					UnitSubFollowNPCList.Add(unit);
				}
			}
			if(unit is UnitEnemy)
            {
				UnitEnemyList.Add(unit);
			}
			if(unit is UnitExercise)
            {
				UnitExerciseList.Add(unit);
			}
			if(unit is UnitTrainer)
			{
				UnitTrainerList.Add(unit);
			}
		}


		//유닛 사망시 등록해제 함수
		public void RemoveUnit(UnitBase unit)
        {
			if (unit is UnitPlayer)
			{
				if (UnitPlayerList.Contains(unit))
                {
					UnitPlayerList.Remove(unit);
				}
			}
			if (unit is UnitNPC)
			{
				if (UnitFollowNPCList.Contains(unit))
				{
					UnitFollowNPCList.Remove(unit);
				}
				else if (UnitSubFollowNPCList.Contains(unit))
				{
					UnitSubFollowNPCList.Remove(unit);
				}
			}
			if (unit is UnitEnemy)
			{
				if (UnitEnemyList.Contains(unit))
				{
					UnitEnemyList.Remove(unit);
				}
			}
			if (unit is UnitExercise)
			{
				if (UnitExerciseList.Contains(unit))
				{
					UnitExerciseList.Remove(unit);
				}
			}
			if(unit is UnitTrainer)
			{
				if(UnitTrainerList.Contains(unit))
				{
					UnitTrainerList.Remove(unit);
				}
			}
		}


		//특정 Table의 유닛 전체 등록 해제 함수
		public void RemoveAllUnit(UnitClass unitClass)
        {
            switch (unitClass)
            {
                case UnitClass.Player:
					UnitPlayerList.Clear();
					break;
                case UnitClass.NPC:
					UnitFollowNPCList.Clear();
					UnitSubFollowNPCList.Clear();
                    break;
                case UnitClass.Enemy:
					UnitEnemyList.Clear();
					break;
				case UnitClass.Exercise:
					UnitExerciseList.Clear();
					break;
				case UnitClass.Trainer:
					UnitTrainerList.Clear();
					break;
			}
        }


		public UnitPlayer GetCurTurnUnit
        {
            get
            {
				if (UnitPlayerList.Count <= 0)
				{
#if UNITY_EDITOR
					Debug.Log("[UnitManager] 대기중인 Player 없음");
#endif
					return null;
				}
				//플레이어가 1명일때 or 플레이어 여러명일때
				if (UnitPlayerList.Count == 1 || CurTurnUnitIndex >= UnitPlayerList.Count)
				{
					CurTurnUnitIndex = 0;
				}
				return (UnitPlayer)UnitPlayerList[CurTurnUnitIndex];
			}
	
		}


		public UnitEnemy GetCurTargetUnit
        {
            get
            {
				if (UnitEnemyList.Count <= 0)
				{
#if UNITY_EDITOR
                    Debug.Log("[UnitManager] 대기중인 Enemy 없음");
#endif
                    return null;
				}
				return (UnitEnemy)UnitEnemyList[0];
			}		
		}

        public UnitExercise GetCurTargetExerciseUnit
        {
            get
            {
                if (UnitExerciseList.Count <= 0)
                {
#if UNITY_EDITOR
                    Debug.Log("[UnitManager] 대기중인 UnitExercise 없음");
#endif
                    return null;
                }

                return (UnitExercise)UnitExerciseList[CurTurnExerciseUnitIndex];
            }
        }

		public UnitFollowNPC GetCurFollowNPCUnit
        {
            get
            {
				if(UnitFollowNPCList.Count <= 0)
                {
#if UNITY_EDITOR
                    Debug.Log("[UnitManager] 대기중인 UnitFollowNPC 없음");
#endif
                    return null;
				}

				return (UnitFollowNPC)UnitFollowNPCList[0];
            }
        }

		public UnitSubFollowNPC GetCurSubFollowNPCUnit
		{
			get
			{
				if(UnitSubFollowNPCList.Count <= 0)
				{
#if UNITY_EDITOR
                    Debug.Log("[UnitManager] 대기중인 UnitSubFollowNPC 없음");
#endif
                    return null;
                }
				return (UnitSubFollowNPC)UnitSubFollowNPCList[0];
			}
		}

		public UnitTrainer GetCurTrainerUnit
		{
			get
			{
				if(UnitTrainerList.Count <= 0)
				{
#if UNITY_EDITOR
                    Debug.Log("[UnitManager] 대기중인 UnitUnitTrainer 없음");
#endif
                    return null;
                }
                return (UnitTrainer)UnitTrainerList[0];
            }
    }

        public void ResetBattle()
        {			
			//CurTurnUnit = null;
			CurTurnUnitIndex = 0;


            for (int i = 0; i < UnitEnemyList.Count; i++)
            {
				Destroy(UnitEnemyList[i].gameObject);
            }
            //UnitEnemyList.Clear();



			Debug.Log("---------- 전투 초기화 완료 ----------");
		}


		public void ResetExerciseIndex()
        {
			CurTurnExerciseUnitIndex = 0;
		}



		public UnitScriptableObject GetUnitScriptable(UnitClass unitClass)
        {
			return Array.Find(UnitScriptables, element => element.pUnitClassType == unitClass);
        }


		public int CurTurnUnitIndex { get; set; }
		public int CurTurnExerciseUnitIndex { get; set; }
		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------		

		[Header("---------- Setting Unit ScriptableObjects ----------")]
		[SerializeField] private UnitScriptableObject[] UnitScriptables;


		[Header("----------All Playable Unit List----------")]
		public List<UnitBase> UnitPlayerList = new();
		public List<UnitBase> UnitFollowNPCList = new();
		public List<UnitBase> UnitSubFollowNPCList = new();
		public List<UnitBase> UnitEnemyList = new();
		public List<UnitBase> UnitExerciseList = new();
		public List<UnitBase> UnitTrainerList = new();




		void Awake()
        {
			if(UnitScriptables.Length <= 0)
            {
                Debug.LogError("[UnitManager] UnitScriptables 세팅하세요");
            }
		}

	


    }//end of class					


}//end of namespace					