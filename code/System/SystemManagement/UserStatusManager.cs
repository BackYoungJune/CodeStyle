/*********************************************************					
* UserStatusManager.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dreamteck.Splines;
using System.Collections;					
using UnityEngine;
using UnityEngine.SceneManagement;
using Dev_SceneManagement;

namespace Dev_System
{					
	public class UserStatusManager : MonoBehaviour					
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        private float CurMoveDistance;
        private double prevLength;
        private SplineFollower PlayerSpline;
		private SceneStructureType curSceneType = SceneStructureType.None;

        void Start()					
		{
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            StartCoroutine(InitCor());
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
            StopAllCoroutines();
			StartCoroutine(InitCor());
        }

		IEnumerator InitCor()
		{
			yield return new WaitUntil(() => Managers.AllCreateManagers);
			yield return new WaitUntil(() => Managers.LoadingHandler.IsLoading == false);
            PlayerSpline = null;
			curSceneType = Managers.LoadingHandler.GetNextTargetSceneType;
            CurMoveDistance = 0;
            if (curSceneType == SceneStructureType.Adventure)
			{
                yield return new WaitUntil(() => Managers.Unit.GetCurTurnUnit != null);
                yield return new WaitUntil(() => Managers.Unit.GetCurTurnUnit.MeterFollower != null);
				PlayerSpline = Managers.Unit.GetCurTurnUnit.MeterFollower;
            }
        }

        void Update()					
		{
            Update_MoveDistance();
        }	

		void Update_MoveDistance()
		{
            Managers.Game.LoginModeTrigger(loginPlayMode: () =>
            {
                if (curSceneType != SceneStructureType.Adventure)
                {
                    return;
                }
            });
            if (PlayerSpline == null) return;

            if (PlayerSpline != Managers.Unit.GetCurTurnUnit.MeterFollower)
            {
                PlayerSpline = Managers.Unit.GetCurTurnUnit.MeterFollower;
            }

            double percent = PlayerSpline.GetPercent();
			double curDistance = PlayerSpline.CalculateLength() * percent;

            if (prevLength != curDistance && curDistance - prevLength > 0)
            {
                double difference = curDistance - prevLength;
                CurMoveDistance += (float)difference;
            }

            Managers.Game.pMoveDistance = CurMoveDistance;
            prevLength = curDistance;
        }

	}//end of class					
}//end of namespace					