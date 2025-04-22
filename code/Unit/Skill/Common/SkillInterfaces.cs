/*********************************************************					
* SkillInterfaces.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.19 오후 7:47					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
			
namespace Dev_UnitSkill
{
	//--------------------------------------------------------					
	//			스킬인터페이스		
	//--------------------------------------------------------					
	public interface SkillInterface<T>
    {
		void Enter(T skill);
		void UpdateExcute();
		void Exit();
		IEnumerator Hit();

		void OnTriggerEnter(Collider col);
	}


	//--------------------------------------------------------					
	//			스킬인터페이스 스테이트머신			
	//--------------------------------------------------------			
	[System.Serializable]
	public class SkillStateMachine<T>
    {
		public SkillInterface<T> CurState { get; set; }
		[SerializeField] private T Sender;


		public SkillStateMachine(T sender, SkillInterface<T> state)
        {
			Sender = sender;
			InitState(state);
		}


		public void InitState(SkillInterface<T> newState)
        {
			if (Sender == null)
            {
				Debug.LogError("[SkillStateMachine] Sender is null");
				return;
			}
			if (CurState == newState)
            {
				return;
			}
			if (CurState != null)
            {
				CurState.Exit();
			}

			CurState = newState;
			CurState.Enter(Sender);
        }


	}//end of class










}//end of namespace					