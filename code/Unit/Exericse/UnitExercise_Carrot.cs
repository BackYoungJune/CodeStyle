/*********************************************************					
* UnitExercise_Carrot.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.12.21 오후 1:03					
**********************************************************/					
using System.Collections;									
using UnityEngine;

namespace Dev_Unit					
{					
	public class UnitExercise_Carrot : UnitExercise					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					


		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		[SerializeField] private GameObject WaterCan;
		private int curProceedsNum = 0;


		protected override void Awake()
		{
			base.Awake();
		}


		protected override void OnEnable()
		{
			base.OnEnable();
		}

		protected override void Start()
		{
			base.Start();
			curProceedsNum = 0;
			this.transform.localScale = Vector3.one;
		}

		protected override void Update()
		{
			base.Update();

			if (base.StateMachine.CurState != null)
			{
				base.StateMachine.CurState.UpdateExcute();
			}
		}

        protected override void OnDisable()
        {
			base.OnDisable();
        }


		//--------------------------------------------------------					
		// 추상함수 구현		
		//--------------------------------------------------------	

		public override void Idle()
		{

		}

		public override void PrePare()
        {
			if (PrePareFeedbacks.Count <= curProceedsNum) return;
			base.PrePareFeedbacks[curProceedsNum]?.StopFeedbacks();
			base.PrePareFeedbacks[curProceedsNum]?.PlayFeedbacks();
			WaterCan.SetActive(true);
			StartCoroutine(Active(WaterCan, false, 1.5f));
		}

        public override void Shoot()
        {
			if (ShootFeedbacks.Count <= curProceedsNum) return;
			base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
			base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();
			curProceedsNum++;

			//마지막 연출 feedback 
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
				StartCoroutine(Active(this.gameObject, false, 1.5f));
            }
        }

		IEnumerator Active(GameObject obj, bool active, float time)
        {
			yield return new WaitForSeconds(time);
			if(obj == this.gameObject)
            {
				base.DisableFeedback.PlayFeedbacks();

				yield return new WaitUntil(() => base.DisableFeedback.IsPlaying == false);
			}

            obj.SetActive(active);
        }

    }//end of class					
}//end of namespace					