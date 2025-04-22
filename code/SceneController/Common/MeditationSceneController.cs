/*********************************************************					
* MeidtationSceneController.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_Phase;
using Dev_SceneManagement;
using Dev_System;
using Dev_UI;
using Dev_Unit;
using MoreMountains.Tools;
using Oculus.Interaction;
using System.Collections;					
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

namespace Dev_SceneControl
{					
	public class MeditationSceneController : SceneControllerBase					
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void Phase_Result()
        {
            // 다른 방법을 통해 씬 로딩의 경우 비활성화 되어서 이 함수로 들어왔을 경우 return ex) 로비로 돌아가기 버튼 클릭
            if (Managers.LoadingHandler.IsLoading) return;

            // Clock UI 종료
            Managers.UIManager.DisableUIModule("MeditationClock", UIGroups.Group_2);
            
            // 랭킹 UI 나오게함
            UIModule_MeditationResult result = Managers.UIManager.EnableUIModule(
                type: Dev_UI.UISceneTypes.Meditation,
                uiState: UIStates.FollowerVR,
                uniqueID: "MeditationResult",
                group: UIGroups.Group_2,
                genpos: Vector3.zero
                ) as UIModule_MeditationResult;


            if (result != null)
            {
                result.Action_End = () =>
                {
                    Managers.UIManager.DisableUIModule("MeditationResult");
                    ChangeScene();
                };
            }
        }
        
        public void EnableSitDownUI()
        {
            UIModule_MeditationLocalization sitdown = Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Meditation,
                uiState:    UIStates.FollowerVR,
                uniqueID:   "MeditationSitdown",
                group:      UIGroups.Group_1,
                followSpeed: 5f,
                followDistanceCam: 10f
                ) as UIModule_MeditationLocalization;


            if (sitdown != null)
            {
                sitdown.Action_End = () =>
                {
                    PhaseManager.CompletePhase();
                };
            }
        }

        IEnumerator SitdownUIDisable()
        {
            yield return new WaitForSeconds(5f);

            Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Meditation,
                uiState:    UIStates.FollowerVR,
                uniqueID:   "MeditationSitdown",
                group:      UIGroups.Group_1,
                followSpeed: 5f,
                followDistanceCam: 10f
                );

            yield return new WaitForSeconds(5f);

            Managers.UIManager.DisableUIModule(uniqueID: "MeditationSitdown");
        }

        public void CanvasGroupAlpha()
        {
            if(CanvasAlphaCor != null)
            {
                StopCoroutine(CanvasAlphaCor);
                CanvasAlphaCor = null;
            }
            CanvasAlphaCor = StartCoroutine(CanvasGroupAlphaCor());
        }

        IEnumerator CanvasGroupAlphaCor()
        {
            float targetAlpha = 1f;
            float StartAlpha = 0;
            float duration = 2.0f;

            float elpesdTime = 0;
            while(elpesdTime < duration)
            {
                elpesdTime += Time.deltaTime;
                float t = (elpesdTime / duration) * Time.deltaTime * 2.0f;
                StartAlpha = Mathf.Lerp(StartAlpha, targetAlpha, t);
                CanvasGroup.alpha = StartAlpha;
                yield return null;
            }
            CanvasGroup.alpha = targetAlpha;
            CanvasAlphaCor = null;
        }

        #region [메디테이션 스트레칭 UI]

        public void EnableStretching(string stretchingStr)
        {
            if (Managers.UIManager.GetUIModule("MeditationStretching") == null)
            {
                UIModule_MeditationStretching gesture = Managers.UIManager.EnableUIModule(
                     type: UISceneTypes.Meditation,
                     uiState: UIStates.Fixed,
                     uniqueID: "MeditationStretching",
                     group: UIGroups.Group_1,
                     genTrans: Managers.Unit.GetCurTurnUnit.pMeditationGestureUIPos
                     ) as UIModule_MeditationStretching;



                //    UIModule_HealthStoneUI stone = Managers.UIManager.EnableUIModule(
                //        type: UISceneTypes.Adventure,
                //        uiState: UIStates.Fixed,
                //        uniqueID: "HealthStone",
                //        group: UIGroups.Group_1,
                //        genTrans: Managers.Unit.GetCurTurnUnit.pStoneUIPos
                //        ) as UI

                if (gesture != null)
                {
                    gesture.PlayGesture(stretchingStr);
                }
            }
        }

        public void PauseStretching()
        {
            UIModule_MeditationStretching gesture = Managers.UIManager.GetUIModule("MeditationStretching") as UIModule_MeditationStretching;

            if (gesture == null) return;

            gesture.PauseGesture();
        }

        public void RestartStretching()
        {
            UIModule_MeditationStretching gesture = Managers.UIManager.GetUIModule("MeditationStretching") as UIModule_MeditationStretching;

            if (gesture == null) return;

            gesture.RestartGesture();
        }

        public void DisableStretching()
        {
            if (Managers.UIManager.GetUIModule("MeditationStretching") != null)
            {
                Managers.UIManager.DisableUIModule("MeditationStretching", UIGroups.Group_1);
            }
        }

        #endregion

        public void CameraFarDistance(float distance)
        {
            DevUtils.CameraFarDistance(distance);
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private PhaseManager PhaseManager;
        [SerializeField] private CanvasGroup CanvasGroup;
        [SerializeField] private Transform GenPos_Player;
        [SerializeField] private string Meditation_BGM;

        private Coroutine CanvasAlphaCor;


        void Start()
        {
            StartCoroutine(CreateUnit());
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override IEnumerator CreateUnit()
        {
            yield return new WaitUntil(() => Managers.AllCreateManagers == true);
            yield return new WaitUntil(() => Managers.LoadingHandler.IsLoading == false);
            Managers.Gesture.Switch_Content(ContentType.Meditation);

            Managers.Game.LoginModeTrigger(
                loginPlayMode: () =>
                {
                    Managers.Unit.GetCurTurnUnit.transform.position = GenPos_Player.position;
                    Managers.Unit.GetCurTurnUnit.transform.rotation = GenPos_Player.rotation;
                },
                debugPlayMode: () =>
                {
                    if (Managers.Unit.GetCurTurnUnit == null)
                    {
                        Managers.Unit.CreateUnit(UnitUniqueID.Player_VR, UnitState.CutScene, GenPos_Player, true, Vector3.one);
                    }
                    else
                    {
                        Managers.Unit.GetCurTurnUnit.transform.position = GenPos_Player.position;
                        Managers.Unit.GetCurTurnUnit.transform.rotation = GenPos_Player.rotation;
                    }
                });

            yield return new WaitForSeconds(DevDefine.fadeOutTime);

            Managers.Sound.PlayBGM(Meditation_BGM);
            PhaseManager.InitRootPhase();

            // 랭킹 UI 나오게함
            UIModule module = Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Meditation,
                uiState:    UIStates.FollowerVR,
                uniqueID:   "MeditationClock",
                group:      UIGroups.Group_2,
                genpos:     Vector3.zero,
                followDistanceCam: 4f
                );



            UIModule_MeditationClock clock = module.GetComponent<UIModule_MeditationClock>();
            if(clock != null)
            {
                clock.Action_End = () =>
                {
                    Phase_Result();
                };
            }

            Managers.Game.LoginModeTrigger(debugPlayMode: () =>
            {
                Managers.LoadingHandler.GetNextTargetSceneType = SceneStructureType.Meditation;
            });

            //Managers.UIManager.EnableUIModule(
            //    type:       UISceneTypes.Common,
            //    uiState:    UIStates.FollowerVR,
            //    uniqueID:   "Exit",
            //    group:      UIGroups.Group_1,
            //    genpos:     Vector3.zero
            //    );


            EventSetting();
        }

        void EventSetting()
        {
            // EventSystem 처리
            EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
            if (eventSystems.Length > 1)
            {
                EventSystem curEventSystem = null;
                foreach (var eventSystem in eventSystems)
                {
                    if (eventSystem.GetComponent<PointableCanvasModule>() != null)
                    {
                        curEventSystem = eventSystem;
                        continue;
                    }
                    else
                    {
                        StandaloneInputModule stand = eventSystem.GetComponent<StandaloneInputModule>();
                        if (stand != null)
                        {
                            Destroy(stand);
                        }
                        Destroy(eventSystem.gameObject);

                        //Destroy(eventSystem);
                    }
                }

                if (curEventSystem == null)
                {
                    new GameObject("EventSystem").AddComponent<UnityEngine.EventSystems.EventSystem>();
                }
            }
        }

        void ChangeScene()
        {
            ResetMeditation();
            DevUtils.CameraFarDistance(70);
            Managers.Game.EndSceneStructure(SceneStructureType.Meditation);
        }

        void ResetMeditation()
        {
            StopAllCoroutines();
        }

    }//end of class								
}//end of namespace					