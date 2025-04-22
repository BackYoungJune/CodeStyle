/*********************************************************					
* MeadowScene_Controller.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_System;
using Dev_UI;
using Dev_Unit;
using Dreamteck.Splines;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;					
					
namespace Dev_SceneControl
{					
	public class MeadowScene_Controller : AdventureSceneController					
	{

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public override void Mission1_진입()
        {
            base.Mission1_진입();
            base.Phase_미션진입();
        }

        public override void Mission1_운동시작()
        {
            base.Mission1_운동시작();
            base.Phase_미션시작();
        }

        public override void Mission2_진입()
        {
            base.Mission2_진입();
            base.Phase_전투1_Init();
        }

        public override void Mission2_운동시작()
        {
            base.Mission2_운동시작();
            base.Phase_전투1();
        }

        public override void Mission3_진입()
        {
            base.Mission3_진입();
            base.Phase_전투2_Init();
        }

        public override void Mission3_운동시작()
        {
            base.Mission3_운동시작();
            base.Phase_전투2();
        }

        public void MinjiMaskDeactivation()
        {
            Managers.Unit.GetCurTargetUnit?.GetComponent<UnitEnemy_MinJi>()?.MaskDeactivation();
        }

        public void ShongChnageRotation()
        {
            FollowNPC.Follower.motion.rotationOffset = new Vector3(0, 170, 0);
        }

        public void SplineRotationInit()
        {
            StartCoroutine("SplineRotationInitCor");
        }

        IEnumerator SplineRotationInitCor()
        {
            SplineFollower follower = FollowNPC.Follower;
            float duration = 3.0f;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                follower.motion.rotationOffset = Vector3.Lerp(follower.motion.rotationOffset, Vector3.zero, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public void ActivityTutorial()
        {
            UIModule_Activity activity = Managers.UIManager.GetUIModule("Activity") as UIModule_Activity;
            if(activity != null)
            {
                GameObject obj = Managers.ObjectPool.GetObjectFromPool(PoolObjectType.Pool, PoolUniqueID.TutorialRoot, activity.TutorialRoot);
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                if (rect != null)
                {
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;

                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                }
                obj.GetComponent<TutorialRoot>().ImageChange(0);
                obj.GetComponent<TutorialRoot>().FeedbackPlay();
            }
        }

        public void ExitTutorial()
        {
            //UIModule_Exit exit = Managers.UIManager.GetUIModule("Exit") as UIModule_Exit;
            //if (exit != null)
            //{
            //    GameObject obj = Managers.ObjectPool.GetObjectFromPool(PoolObjectType.Pool, PoolUniqueID.TutorialRoot, exit.TutorialRoot);
            //    RectTransform rect = obj.GetComponent<RectTransform>();
            //    rect.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            //    if (rect != null)
            //    {
            //        rect.anchorMin = Vector2.zero;
            //        rect.anchorMax = Vector2.one;

            //        rect.offsetMin = Vector2.zero;
            //        rect.offsetMax = Vector2.zero;
            //    }
            //    obj.GetComponent<TutorialRoot>().ImageChange(1);
            //    obj.GetComponent<TutorialRoot>().FeedbackPlay();
            //}
        }

        public void RankTutorial()
        {
            UIModule_Rank rank = Managers.UIManager.GetUIModule("Rank") as UIModule_Rank;
            if (rank != null)
            {
                GameObject obj = Managers.ObjectPool.GetObjectFromPool(PoolObjectType.Pool, PoolUniqueID.TutorialRoot, rank.TutorialRoot);
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                if (rect != null)
                {
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;

                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                }
                obj.GetComponent<TutorialRoot>().ImageChange(2);
                obj.GetComponent<TutorialRoot>().FeedbackPlay();
            }
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private TextMeshProUGUI meadowSignText;


        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_Carrot, UnitState.None, genpos, true, Vector3.one);
            }
        }

        protected override void Start()					
		{
			base.Start();

            StartCoroutine(InvokeStart());
        }				
        
        IEnumerator InvokeStart()
        {
            yield return new WaitUntil(() => Managers.AllCreateManagers);
            yield return new WaitUntil(() => Managers.LoadingHandler.IsLoading == false);
            Managers.Game.LanguageModeTrigger(
                korea: () =>
                {
                    meadowSignText.text = "당근농장";
                },
                english: () =>
                {
                    meadowSignText.text = "Carrot\nFarm";
                });
        }
							
							
		protected override void Update()					
		{
            base.Update();	
		}					
							
							
							
	}//end of class								
}//end of namespace					