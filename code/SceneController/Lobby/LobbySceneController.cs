/*********************************************************					
* LobbySceneController.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dev_System;
using Dev_UI;
using Dev_Occulus;
using Dev_SceneManagement;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using Dev_LocalDB;
using System.Diagnostics;
using System;
using Dev_Network;

namespace Dev_SceneControl
{
    public class LobbySceneController : MonoBehaviour
    {

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------


        public void SearchToken(string token)
        {
            // DB에서 토큰 찾아 존재하면 로그인, 아니면 게스트 모드

            foreach (string str in Managers.LocalDB.pDBForm_UserInfo.pTokenDBForm)
            {
                if (token == str)
                {
                    // 해당 토큰 로그인, 함수 종료
                    // 기존 로그인 함수 사용 X UI 없이 바로 데이터 넣고 로그인될 수 있도록 하는 함수 작성
                    return;
                }
            }
            // 게스트 모드 로그인 코드 추후에 아이디어 검토 후 작성
        }



        public void Active_EndingCredit()
        {
            Managers.Sound.PlaySFX("10076");
            Managers.Game.Hosting_EventScene(SceneDefine_Event.EndingCredit);
        }


        public void Active_OverAllResult(bool On)
        {
            if (On)
            {
                Managers.Sound.PlaySFX("10076");
                //켜져있는 다른 콘텐츠 선택 UI 비활성화
                Managers.UIManager.DisableUIModule("SelectChapter");
                Managers.UIManager.DisableUIModule("SelectAdventure");
                Managers.UIManager.DisableUIModule("FitnessList");
                Managers.UIManager.DisableUIModule("SelectMeditation");
                Managers.UIManager.DisableUIModule("Tutorial");


                //수동모드일때 : 어드벤쳐 씬 선택 UI 활성화
                if (Managers.UIManager.GetUIModule("FinalResult") == null)
                {
                    Managers.UIManager.EnableUIModule(
                        type: UISceneTypes.Lobby,
                        uiState: UIStates.Fixed,
                        uniqueID: "FinalResult",
                        group: UIGroups.Group_1,
                        genpos: UIpos.transform.position
                        );
                }
            }
            else if (!On)
            {
                Managers.UIManager.DisableUIModule("FinalResult");
            }
        }


        public void Active_Tutorial(bool On)
        {

            //UIFollowerElements follower_tutorial = new UIFollowerElements(
            //    lookatCamera: true,
            //    targetCamera: Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.pHeadCamera,
            //    rotateWithCamera: true,
            //    followSpeed: 10.0f,
            //    distanceFromCamera: 5f,
            //    inValidMoveY: false
            //    );


            if (On)
            {
                Managers.Sound.PlaySFX("10076");
                //켜져있는 다른 콘텐츠 선택 UI 비활성화
                Managers.UIManager.DisableUIModule("SelectChapter");
                Managers.UIManager.DisableUIModule("SelectAdventure");
                Managers.UIManager.DisableUIModule("FitnessList");
                Managers.UIManager.DisableUIModule("SelectMeditation");
                Managers.UIManager.DisableUIModule("FinalResult");

                //수동모드일때 : 어드벤쳐 씬 선택 UI 활성화
                if (Managers.UIManager.GetUIModule("Tutorial") == null)
                {
                    Managers.UIManager.EnableUIModule(
                        type: UISceneTypes.Lobby,
                        uiState: UIStates.FollowerVR,
                        uniqueID: "Tutorial",
                        group: UIGroups.Group_1,
                        followSpeed: 10f,
                        followDistanceCam: 7f
                        );
                }
            }
            else if (!On)
            {
                Managers.UIManager.DisableUIModule("Tutorial");
            }
        }

        public void ChangeSceneInspector_Adventure()
        {
            //if(isSceneChanging)
            //{
            //    return;
            //}
            //isSceneChanging = true;

            //string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Adventure.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
            //Managers.LoadingHandler.LoadScene(targetScene, Color.black);


            if (Managers.UIManager.GetUIModule("SelectAdventure") != null)
            {
                return;
            }

            UIModule uiModule = Managers.UIManager.GetUIModule("SelectChapter");

            if (uiModule != null && uiModule.transform.position == Genpos_AdventureSelectUI.position)
            {
                return;
            }

            //if (uiModule != null)
            //{
            //    if (uiModule.transform.position == Genpos_AdventureSelectUI.position)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        UIModule_SelectChapter module = Managers.UIManager.EnableUIModule(
            //            type: UISceneTypes.Lobby,
            //            uiState: UIStates.Fixed,
            //            uniqueID: "SelectChapter",
            //            group: UIGroups.Group_1,
            //            genTrans: Genpos_AdventureSelectUI,
            //            followSpeed: 10f,
            //            followDistanceCam: 4f
            //            ) as UIModule_SelectChapter;

            //        module.SettingParent(Genpos_AdventureSelectUI, UISceneTypes.Adventure);


            //        uiModule.SettingParent(Genpos_AdventureSelectUI, UISceneTypes.Adventure);
            //        Managers.Sound.PlaySFX("10076");
            //        return;
            //    }
            //}


            //********************************
            //		Adventure Select UI - follower
            //********************************
            //켜져있는 다른 콘텐츠 선택 UI 비활성화
            Managers.UIManager.DisableUIModule("SelectChapter");
            Managers.UIManager.DisableUIModule("FitnessList", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("SelectMeditation", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("FinalResult", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("Tutorial", UIGroups.Group_1);

            //수동모드일때 : 어드벤쳐 씬 선택 UI 활성화
            //if (Managers.UIManager.GetUIModule("SelectAdventure") == null)
            //{
            //    Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Lobby,
            //        uiState: UIStates.Fixed,
            //        uniqueID: "SelectAdventure",
            //        group: UIGroups.Group_1,
            //        genTrans:Genpos_AdventureSelectUI,
            //        followSpeed: 10f,
            //        followDistanceCam: 4f
            //        );
            //}


            //todo : 원본 - 조승근매니저 소스
            //if (Managers.UIManager.GetUIModule("SelectChapter") == null)
            //{
            //    UIModule_SelectChapter module = Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Lobby,
            //        uiState: UIStates.Fixed,
            //        uniqueID: "SelectChapter",
            //        group: UIGroups.Group_1,
            //        genTrans: Genpos_AdventureSelectUI,
            //        followSpeed: 10f,
            //        followDistanceCam: 4f
            //        ) as UIModule_SelectChapter;

            //    module.SettingParent(Genpos_AdventureSelectUI, UISceneTypes.Adventure);
            //}

            StartCoroutine(EnableSelect(SceneStructureType.Adventure));

            //todo : 박서진테스트 - SelectChapter 리뉴얼
            //if (Managers.UIManager.GetUIModule("SelectChapter_ver2") != null)
            //{
            //    UIModule_SelectChapter_ver2 module = Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Lobby,
            //        uiState: UIStates.Fixed,
            //        uniqueID: "SelectChapter_ver2",
            //        group: UIGroups.Group_1,
            //        genTrans: Genpos_MeditationSelectUI,
            //        followSpeed: 10f,
            //        followDistanceCam: 4f
            //        ) as UIModule_SelectChapter_ver2;

            //    module.Initializing(Genpos_MeditationSelectUI, UISceneTypes.Adventure);
            //}



            Managers.Sound.PlaySFX("10076");
        }

        public void ChangeSceneInspector_Fitness()
        {
            //if (isSceneChanging)
            //{
            //    return;
            //}
            //isSceneChanging = true;

            //string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Fitness.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
            //Managers.LoadingHandler.LoadScene(targetScene, Color.black);


            if (Managers.UIManager.GetUIModule("FitnessList") != null)
            {
                return;
            }


            //********************************
            //		Fitness Select UI - follower
            //********************************
            //켜져있는 다른 콘텐츠 선택 UI 비활성화
            Managers.UIManager.DisableUIModule("SelectChapter");
            Managers.UIManager.DisableUIModule("SelectAdventure", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("SelectMeditation", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("FinalResult", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("Tutorial", UIGroups.Group_1);

            //수동모드일때 : 피트니스  운동선택 UI 활성화
            if (Managers.UIManager.GetUIModule("FitnessList") == null)
            {
                Managers.UIManager.EnableUIModule(
                      type: UISceneTypes.Lobby,
                      uiState: UIStates.Fixed,
                      uniqueID: "FitnessList",
                      group: UIGroups.Group_1,
                      genTrans: Genpos_FitnessSelectUI,
                      followSpeed: 10f,
                      followDistanceCam: 4f
                      );
            }

            Managers.Sound.PlaySFX("10076");
        }

        public void ChangeSceneInspector_Meditation()
        {
            //if (isSceneChanging)
            //{
            //    return;
            //}
            //isSceneChanging = true;

            //string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Meditation.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
            //Managers.LoadingHandler.LoadScene(targetScene, Color.black);


            if (Managers.UIManager.GetUIModule("SelectMeditation") != null)
            {
                return;
            }


            UIModule uiModule = Managers.UIManager.GetUIModule("SelectChapter");

            if (uiModule != null && uiModule.transform.position == Genpos_MeditationSelectUI.position)
            {
                return;
            }

            //********************************
            //		Meditation Select UI - follower
            //********************************
            //켜져있는 다른 콘텐츠 선택 UI 비활성화
            Managers.UIManager.DisableUIModule("SelectChapter");
            Managers.UIManager.DisableUIModule("SelectAdventure", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("FitnessList", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("FinalResult", UIGroups.Group_1);
            Managers.UIManager.DisableUIModule("Tutorial", UIGroups.Group_1);

            //수동모드일때 : 매디테이션 씬 선택 UI 활성화
            //if (Managers.UIManager.GetUIModule("SelectMeditation") == null)
            //{
            //    Managers.UIManager.EnableUIModule(
            //         type: UISceneTypes.Lobby,
            //         uiState: UIStates.Fixed,
            //         uniqueID: "SelectMeditation",
            //         group: UIGroups.Group_1,
            //         genTrans:Genpos_MeditationSelectUI,
            //         followSpeed: 10f,
            //         followDistanceCam: 4f
            //         );
            //}


            //todo : 원본 - 조승근매니저 소스
            //if (Managers.UIManager.GetUIModule("SelectChapter") == null)
            //{
            //    UIModule_SelectChapter module = Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Lobby,
            //        uiState: UIStates.Fixed,
            //        uniqueID: "SelectChapter",
            //        group: UIGroups.Group_1,
            //        genTrans: Genpos_MeditationSelectUI,
            //        followSpeed: 10f,
            //        followDistanceCam: 4f
            //        ) as UIModule_SelectChapter;

            //    module.SettingParent(Genpos_MeditationSelectUI, UISceneTypes.Meditation);
            //}

            StartCoroutine(EnableSelect(SceneStructureType.Meditation));

            //todo : 박서진테스트 - SelectChapter 리뉴얼
            //if (Managers.UIManager.GetUIModule("SelectChapter_ver2") != null)
            //{
            //    UIModule_SelectChapter_ver2 module = Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Lobby,
            //        uiState: UIStates.Fixed,
            //        uniqueID: "SelectChapter_ver2",
            //        group: UIGroups.Group_1,
            //        genTrans: Genpos_MeditationSelectUI,
            //        followSpeed: 10f,
            //        followDistanceCam: 4f
            //        ) as UIModule_SelectChapter_ver2;

            //    module.Initializing(Genpos_MeditationSelectUI, UISceneTypes.Meditation);
            //}




            Managers.Sound.PlaySFX("10076");
        }

        public void HoverAction_Inspector(int num)
        {
            switch (num)
            {
                //어드벤쳐 Hover
                case 1:
                    Feedback_Hover_AdventureUI?.PlayFeedbacks();
                    break;

                //피트니스 Hover
                case 2:
                    Feedback_Hover_FitnessUI?.PlayFeedbacks();
                    break;

                //매디테이션 Hover
                case 3:
                    Feedback_Hover_MeditationUI?.PlayFeedbacks();
                    break;
                case 4:
                    TutorialUI.SetActive(true);
                    Feedback_Hover_TutorialUI.PlayFeedbacks();
                    // 튜토리얼 UI Active & Feedback
                    break;
                case 5:
                    OverallResultUI.SetActive(true);
                    Feedback_Hover_OverallUI.PlayFeedbacks();
                    // 종합 결과창 뜨기
                    break;
                //엔딩 크레딧 Hover
                case 6:
                    EndingCreditUI.SetActive(true);
                    Feedback_Hover_EndingCreditUI?.PlayFeedbacks();
                    break;
                //Exit Hover
                case 7:
                    Feedback_Hover_Exit?.PlayFeedbacks();
                    break;
                //AutoMode Hover
                case 8:
                    AutoModeUI.SetActive(true);
                    Feedback_Hover_AutoModeUI?.PlayFeedbacks();
                    break;
            }

            //hover 효과음
            Managers.Sound.PlaySFX("10042");
        }
        public void NormalAction_Inspector(int num)
        {
            switch (num)
            {
                //어드벤쳐 Normal
                case 1:
                    Feedback_Normal_AdventureUI?.PlayFeedbacks();
                    break;

                //피트니스 Normal
                case 2:
                    Feedback_Normal_FitnessUI?.PlayFeedbacks();
                    break;

                //매디테이션 Normal
                case 3:
                    Feedback_Normal_MeditationUI?.PlayFeedbacks();
                    break;
                case 4:
                    TutorialUI.SetActive(false);
                    break;
                case 5:
                    OverallResultUI.SetActive(false);
                    break;
                //엔딩 크레딧 Normal
                case 6:
                    EndingCreditUI.SetActive(false);
                    break;
                //Exit Normal
                case 7:
                    Feedback_Normal_Exit?.PlayFeedbacks();
                    break;
                //AutoMode Normal
                case 8:
                    AutoModeUI.SetActive(false);
                    break;
            }

            //hover 효과음
            Managers.Sound.PlaySFX("10041");
        }

        public void OnClickExit()
        {
            Managers.Sound.PlaySFX("10076");
            Managers.Game.pConfirmMessage.SetMessage("Canvas_ConfirmMessage_ExitGame",
                confirmAction: () =>
                {
#if UNITY_EDITOR
                    // Unity 에디터에서 실행 중인 경우
                    UnityEditor.EditorApplication.isPlaying = false;
#else
            // 다른 모든 플랫폼에서 애플리케이션 종료
            Application.Quit();
#endif
                });
        }

        public void OnAutoModeUI()
        {
            if (Managers.UIManager.GetUIModule("TodayList") == null)
            {
                Managers.Sound.PlaySFX("10076");

                //투데이리스트UI 생성
                UIModule today = Managers.UIManager.EnableUIModule(
                    type: UISceneTypes.Lobby,
                    uiState: UIStates.FollowerVR,
                    uniqueID: "TodayList",
                    group: UIGroups.Group_1,
                    followSpeed: 10f,
                    followDistanceCam: 5f
                    );
            }
        }



        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [Header("*************** Island Title UI - MovementUI ***************")]
        [SerializeField] private UIMovementVR MovementUI_Adventure;
        [SerializeField] private UIMovementVR MovementUI_Fitness;
        [SerializeField] private UIMovementVR MovementUI_Meditation;
        [SerializeField] private UIMovementVR MovementUI_OverAllResult;
        [SerializeField] private UIMovementVR MovementUI_Tutorial;
        [SerializeField] private UIMovementVR MovementUI_EndingCredit;
        [SerializeField] private UIMovementVR MovementUI_AutoMode;

        [Header("*************** Feedback Settings ***************")]
        [SerializeField] private MMF_Player Feedback_Normal_AdventureUI;
        [SerializeField] private MMF_Player Feedback_Hover_AdventureUI;
        [SerializeField] private MMF_Player Feedback_Normal_FitnessUI;
        [SerializeField] private MMF_Player Feedback_Hover_FitnessUI;
        [SerializeField] private MMF_Player Feedback_Normal_MeditationUI;
        [SerializeField] private MMF_Player Feedback_Hover_MeditationUI;
        [SerializeField] private MMF_Player Feedback_Hover_TutorialUI;
        [SerializeField] private MMF_Player Feedback_Hover_OverallUI;
        [SerializeField] private MMF_Player Feedback_Hover_EndingCreditUI;
        [SerializeField] private MMF_Player Feedback_Hover_AutoModeUI;
        [SerializeField] private MMF_Player Feedback_Hover_Exit;
        [SerializeField] private MMF_Player Feedback_Normal_Exit;

        [Header("*************** UI GenPos ***************")]
        [SerializeField] private Transform OverAllResult_GenPos;
        [SerializeField] private Transform UIpos;
        [SerializeField] private Transform Genpos_AdventureSelectUI;
        [SerializeField] private Transform Genpos_FitnessSelectUI;
        [SerializeField] private Transform Genpos_MeditationSelectUI;

        [Header("*************** Tutorial UI Enable Disable ***************")]

        [SerializeField] private GameObject TutorialUI;
        [SerializeField] private Image FlagImg;
        [SerializeField] private Sprite TutorialUIKor;
        [SerializeField] private Sprite TutorialUIEng;


        [Header("*************** Ove UI Enable Disable ***************")]

        [SerializeField] private GameObject OverallResultUI;
        [SerializeField] private Image OverallFlagImg;
        [SerializeField] private Sprite OverallUIKor;
        [SerializeField] private Sprite OverallUIEng;


        [Header("*************** EndingCredit UI Enable Disable ***************")]
        [SerializeField] private GameObject EndingCreditUI;
        [SerializeField] private GameObject EndingCreditProp;
        [SerializeField] private GameObject EndingCreditCollider;


        [Header("*************** EndingCredit UI Enable Disable ***************")]
        [SerializeField] private GameObject AutoModeUI;

        [Header("*************** MapTema ***************")]
        [SerializeField] private GameObject MapTheme_Chapter1;
        [SerializeField] private GameObject MapTheme_Chapter2;

        private Dictionary<MapTheme, GameObject> mapThemeDic;

        //private UIFollowerElements FollowerElement;
        //private bool isSceneChanging = false;


        private void LoadToServer_Score()
        {
            if (Managers.Game.IsLogined == false) return;

            Managers.Game.StartCoroutine(CallbackAPI_AdventureStarScore.Callback_AdventureStarScore(
                userId: Managers.Game.pCurUserData.DATA.USER_ID,
                successEvent: () =>
                {

                },
                failEvent: () =>
                {
                    //실패 시 더미 값 넣어주기
                    Managers.Game.pCurProfileUserAdventureScores.DATA = new Response_Adventure_StarScore.DataTable[4];

                    for (int i = 0; i < Managers.Game.pCurProfileUserAdventureScores.DATA.Length; i++)
                    {
                        Managers.Game.pCurProfileUserAdventureScores.DATA[i].Score = 0;
                    }
                },
                outCallback: (response) =>
                {
                    //데이터파싱 성공!
                    if (response != null)
                    {
                        Managers.Game.pCurProfileUserAdventureScores = response;
                    }
                }));

        }


        /// <summary>
        /// SelectChapter 생성하는 함수
        /// </summary>
        /// <param name="sceneType">어드벤처인지 메디테이션인지 선택</param>
        /// <returns></returns>
        IEnumerator EnableSelect(SceneStructureType sceneType)
        {
            yield return null;      // 한 프레임 후에 호출해야 중복 생성이 되지 않는다
            if (Managers.UIManager.GetUIModule("SelectChapter") == null)
            {
                UIModule_SelectChapter_re module;
                switch (sceneType)
                {
                    case SceneStructureType.Adventure:
                        module = Managers.UIManager.EnableUIModule(
                            type: UISceneTypes.Lobby,
                            uiState: UIStates.Fixed,
                            uniqueID: "SelectChapter",
                            group: UIGroups.Group_1,
                            genTrans: Genpos_AdventureSelectUI,
                            followSpeed: 10f,
                            followDistanceCam: 4f
                            ) as UIModule_SelectChapter_re;

                        module.Initializing(Genpos_AdventureSelectUI, UISceneTypes.Adventure);
                        break;
                    case SceneStructureType.Meditation:
                        module = Managers.UIManager.EnableUIModule(
                            type: UISceneTypes.Lobby,
                            uiState: UIStates.Fixed,
                            uniqueID: "SelectChapter",
                            group: UIGroups.Group_1,
                            genTrans: Genpos_MeditationSelectUI,
                            followSpeed: 10f,
                            followDistanceCam: 4f
                            ) as UIModule_SelectChapter_re;

                        module.Initializing(Genpos_MeditationSelectUI, UISceneTypes.Meditation);
                        break;
                    default:
                        break;
                }
            }
        }



        IEnumerator Start()
        {
            LoadToServer_Score();

            //Player Camerarig 위치 초기화
            Managers.Unit.GetCurTurnUnit.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);


            //로컬 DB 데이터 불러오기
            Managers.LocalDB.AllReadDB();

            //엔딩 크레딧 섬 등장 조건
            if (Managers.LocalDB.pDBForm_AppSystem.pFirstClear <= 0)
            {
                EndingCreditCollider.SetActive(false);
                EndingCreditProp.SetActive(false);
            }


            yield return new WaitUntil(() => Managers.LoadingHandler.IsLoading == false);
            yield return new WaitForSeconds(DevDefine.fadeOutTime);
            //isSceneChanging = false;

            //Lobby BGM 호출
            Managers.Sound.PlayBGM("10001");


            //RayDistance 초기화
            if (CameraRigMaster.Instance != null)
            {
                CameraRigMaster.Instance.SetRayDistance(10.5f);
            }

            //어드벤처 섬 MovementUI
            UIMovementInfos adventureUI = new();
            adventureUI.Initialize(
             useLerp: true,
             targetUI: MovementUI_Adventure.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero,
             fixXYZ: true
             );

            MovementUI_Adventure.EnableUI(adventureUI);


            //피트니스 섬 MovementUI
            UIMovementInfos fitnessUI = new();
            fitnessUI.Initialize(
             useLerp: true,
             targetUI: MovementUI_Fitness.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero,
             fixXYZ: true
             );

            MovementUI_Fitness.EnableUI(fitnessUI);


            //매디테이션 섬 MovementUI
            UIMovementInfos meditationUI = new();
            meditationUI.Initialize(
             useLerp: true,
             targetUI: MovementUI_Meditation.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero,
             fixXYZ: true
             );

            MovementUI_Meditation.EnableUI(meditationUI);

            //종합 결과 UI MovementUI
            UIMovementInfos overAllResultUI = new();
            overAllResultUI.Initialize(
             useLerp: true,
             targetUI: MovementUI_OverAllResult.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero,
             fixXYZ: true
             );

            MovementUI_OverAllResult.EnableUI(overAllResultUI);


            //튜토리얼 UI MovementUI
            UIMovementInfos tutorialUI = new();
            tutorialUI.Initialize(
             useLerp: true,
             targetUI: MovementUI_Tutorial.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero,
             fixXYZ: true
             );

            MovementUI_Tutorial.EnableUI(tutorialUI);

            Managers.Game.LanguageModeTrigger(
                korea: () =>
                {
                    FlagImg.sprite = TutorialUIKor;
                },
                english: () =>
                {
                    FlagImg.sprite = TutorialUIEng;
                });


            //종합결과 UI MovementUI
            UIMovementInfos overallUI = new();
            overallUI.Initialize(
             useLerp: true,
             targetUI: OverallResultUI.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero,
             fixXYZ: true
             );

            MovementUI_OverAllResult.EnableUI(overallUI);

            Managers.Game.LanguageModeTrigger(
                korea: () =>
                {
                    OverallFlagImg.sprite = OverallUIKor;
                },
                english: () =>
                {
                    OverallFlagImg.sprite = OverallUIEng;
                });

            //엔딩 크레딧 UI MovementUI
            UIMovementInfos endigCreditUI = new();
            endigCreditUI.Initialize(
             useLerp: true,
             targetUI: EndingCreditUI.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero,
             fixXYZ: true
             );

            MovementUI_EndingCredit.EnableUI(endigCreditUI);

            //AutoMode UI MovemnetUI
            UIMovementInfos autioModeUI = new();
            autioModeUI.Initialize(
             useLerp: true,
             targetUI: AutoModeUI.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero,
             fixXYZ: true
             );

            MovementUI_AutoMode.EnableUI(autioModeUI);

            // 테마 변경 이벤트 추가
            Managers.Game.OnMapTemaChanged -= MapThemeChange;
            Managers.Game.OnMapTemaChanged += MapThemeChange;

            if(mapThemeDic != null)
            {
                mapThemeDic.Clear();
                mapThemeDic = null;
            }

            mapThemeDic = new Dictionary<MapTheme, GameObject>
            {
                {MapTheme.TionWorld, MapTheme_Chapter1 },
                {MapTheme.Japen, MapTheme_Chapter2 },
            };

            MapThemeChange(Managers.Game.pCurMapTema);

            //공용 follower 초기화
            //if (FollowerElement == null)
            //{
            //    FollowerElement = new UIFollowerElements(
            //        lookatCamera: true,
            //        targetCamera: CameraRigMaster.Instance.pHeadCamera,
            //        rotateWithCamera: true,
            //        followSpeed: 10.0f,
            //        distanceFromCamera: 3.1f,
            //        inValidMoveY: false
            //        );
            //}


            yield return StartCoroutine(SceneProcess());
        }



        IEnumerator SceneProcess()
        {
            yield return new WaitForSeconds(2f);

            if (Managers.Game.pIsFirstConnection)
            {

                if (Managers.LocalDB.IsFirstAppPlay())
                {
                    //********************************
                    //		튜토리얼 UI
                    //********************************
                    //튜토리얼UI
                    //UIFollowerElements follower_tutorial = new UIFollowerElements(
                    //    lookatCamera: true,
                    //    targetCamera: Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.pHeadCamera,
                    //    rotateWithCamera: true,
                    //    followSpeed: 10.0f,
                    //    distanceFromCamera: 5f,
                    //    inValidMoveY: false
                    //    );

                    Managers.Sound.PlaySFX("10076");

                    //튜토리얼UI 활성화
                    UIModule tutorital = Managers.UIManager.EnableUIModule(
                        type: UISceneTypes.Lobby,
                        uiState: UIStates.FollowerVR,
                        uniqueID: "Tutorial",
                        group: UIGroups.Group_1,
                        followSpeed: 10f,
                        followDistanceCam: 7f
                        );

                    Managers.LocalDB.DataBaseInser("Update AppSystem Set FIRSTCONNECT = 1");
                    Managers.LocalDB.DataBaseRead_AppSystem();

                    if (Input.GetKeyDown(KeyCode.K))
                        tutorital.gameObject.SetActive(false);

                    //튜토리얼 UI 사라질때까지 대기
                    yield return new WaitUntil(() => tutorital.gameObject.activeSelf == false);
                }

                //********************************
                //		안전가이드 UI
                //********************************
                //안전가이드UI
                //UIFollowerElements follower_SafeGuide = new UIFollowerElements(
                //    lookatCamera: true,
                //    targetCamera: Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.pHeadCamera,
                //    rotateWithCamera: true,
                //    followSpeed: 10.0f,
                //    distanceFromCamera: 5f,
                //    inValidMoveY: false
                //    );

                Managers.Sound.PlaySFX("10076");

                //안전가이드UI 활성화
                UIModule safety = Managers.UIManager.EnableUIModule(
                    type: UISceneTypes.Lobby,
                    uiState: UIStates.FollowerVR,
                    uniqueID: "Safety",
                    group: UIGroups.Group_1,
                    followSpeed: 10f,
                    followDistanceCam: 5f
                    );

                //안전가이드UI 비활성화
                yield return new WaitForSeconds(8f);
                Managers.UIManager.DisableUIModule("Safety");

                //yield return new WaitUntil(() => safety.gameObject.activeSelf == false);

                yield return new WaitForSeconds(1.5f);




                //********************************
                //		투데이리스트 UI
                //********************************
                //투데이리스트UI
                //UIFollowerElements follower_TodayList = new UIFollowerElements(
                //    lookatCamera: true,
                //    targetCamera: Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.pHeadCamera,
                //    rotateWithCamera: true,
                //    followSpeed: 10.0f,
                //    distanceFromCamera: 5f,
                //    inValidMoveY: false
                //    );

                Managers.Sound.PlaySFX("10076");

                //투데이리스트UI 생성
                UIModule today = Managers.UIManager.EnableUIModule(
                    type: UISceneTypes.Lobby,
                    uiState: UIStates.FollowerVR,
                    uniqueID: "TodayList",
                    group: UIGroups.Group_1,
                    followSpeed: 10f,
                    followDistanceCam: 5f
                    );


                //투데이리스트UI 사라질때까지 대기
                yield return new WaitUntil(() => today.gameObject.activeSelf == false);
            }


            if (Managers.Game.TempIsFirstLobby == false)
            {
                Managers.Game.TempIsFirstLobby = true;
            }
            else
            {
                Managers.Game.ChangePlayMode(GamePlayMode.FreeSelect);
            }


            //자동선택모드 / 수동선택모드
            Managers.Game.GamePlayModeTrigger(
                autoSelect: () =>
                {

                    //Managers.Game.AutoHosting(SceneStructureType.Adventure);

                },
                freeSelect: () =>
                {

                    //RayDistance 초기화
                    if (CameraRigMaster.Instance != null)
                    {
                        CameraRigMaster.Instance.SetRayDistance(100f);
                    }

                });


            //********************************
            //		로그인버튼 UI 활성화
            //********************************
            yield return StartCoroutine(Managers.UIManager.AllDisableUIModule());

            // 시스템 메세지 생성
            UIModule module = Managers.UIManager.EnableUIModule(
                type: UISceneTypes.Common,
                uiState: UIStates.FollowerVR,
                uniqueID: "SystemMessage",
                group: UIGroups.Group_1,
                genpos: Vector3.zero,
                followDistanceCam: 2.5f);

            if (module != null)
            {
                Managers.Game.pSystemMessage = module as UIModule_SystemMessage;
            }

            // 확인 UI 생성
            module = Managers.UIManager.EnableUIModule(
                type: UISceneTypes.Common,
                uiState: UIStates.FollowerVR,
                uniqueID: "ConfirmMessage",
                group: UIGroups.Group_1,
                genpos: Vector3.zero,
                followDistanceCam: 2.5f);

            if (module != null)
            {
                Managers.Game.pConfirmMessage = module as UIModule_ConfirmMessage;
            }

            //Managers.Game.EnableLoginSyncButton();

            //********************************
            //		종합 결과 UI
            //********************************
            //if (Managers.LocalDB.IsActiveFinalResultUI())
            //{
            //    if (Managers.UIManager.GetUIModule("OverAllResult") == null)
            //    {
            //        Managers.UIManager.EnableUIModule(
            //              type: UISceneTypes.Lobby,
            //              uniqueID: "OverAllResult",
            //              genpos: OverAllResult_GenPos.position
            //              //genTrans: OverAllResult_GenPos
            //              );
            //    }
            //}

        }

        private void Update()
        {
            //if (OVRInput.GetDown(OVRInput.RawButton.A))
            //{
            //    Managers.Game.Hosting_SelectStage(SceneStructureType.Fitness, ChaptersDefines.Chapter_1, StagesDefines.Stage_2);
            //}
            //else if (OVRInput.GetDown(OVRInput.RawButton.B))
            //{
            //    Managers.Game.Hosting_SelectStage(SceneStructureType.Fitness, ChaptersDefines.Chapter_1, StagesDefines.Stage_3);
            //}
            //else if (OVRInput.GetDown(OVRInput.RawButton.X))
            //{
            //    Managers.Game.Hosting_SelectStage(SceneStructureType.Fitness, ChaptersDefines.Chapter_1, StagesDefines.Stage_4);
            //}
            //else if (OVRInput.GetDown(OVRInput.RawButton.Y))
            //{
            //    Managers.Game.Hosting_SelectStage(SceneStructureType.Fitness, ChaptersDefines.Chapter_1, StagesDefines.Stage_1);
            //}

            // DLC 디버그
            if (OVRInput.Get(OVRInput.RawButton.A) && OVRInput.Get(OVRInput.RawButton.X))
            {
                if (OVRInput.Get(OVRInput.RawButton.LHandTrigger) && OVRInput.Get(OVRInput.RawButton.RHandTrigger))
                {
                    if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
                    {
                        Managers.DLCManagers.PurchaseDLC(Dev_DLC.DLCSkuID.unlock_japan);
                    }

                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Managers.DLCManagers.pSkuDic[Dev_DLC.DLCSkuID.unlock_japan].Purchase = true;
                Managers.DLCManagers.PurchaseSucess?.Invoke();
            }
        }

        void MapThemeChange(MapTheme tema)
        {
            foreach(var obj in mapThemeDic.Values)
            {
                obj.SetActive(false);
            }

            if(mapThemeDic.ContainsKey(tema))
            {
                mapThemeDic[tema].SetActive(true);
            }
            else
            {
                mapThemeDic[MapTheme.TionWorld].SetActive(true);
            }
        }


        private void OnDestroy()
        {
            Managers.Game.OnMapTemaChanged -= MapThemeChange;
            StopAllCoroutines();
        }


        //        void Update()
        //        {

        //#if UNITY_EDITOR

        //            if (Input.GetKeyDown(KeyCode.Alpha1))
        //            {

        //                string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Adventure.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
        //                Managers.LoadingHandler.LoadScene(targetScene, Color.black);
        //            }

        //            if (Input.GetKeyDown(KeyCode.Alpha2))
        //            {

        //                string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Fitness.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
        //                Managers.LoadingHandler.LoadScene(targetScene, Color.black);
        //            }

        //            if (Input.GetKeyDown(KeyCode.Alpha3))
        //            {
        //                string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Meditation.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
        //                Managers.LoadingHandler.LoadScene(targetScene, Color.black);
        //            }

        //#endif


        //            //if (OVRInput.GetDown(OVRInput.RawButton.X))
        //            //{
        //            //    // B 버튼이 눌렸을 때 수행할 동작 작성
        //            //    string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Adventure.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
        //            //    LoadScene(targetScene, Color.black);
        //            //}

        //            //if (OVRInput.GetDown(OVRInput.RawButton.Y))
        //            //{
        //            //    // B 버튼이 눌렸을 때 수행할 동작 작성
        //            //    string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Fitness.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
        //            //    LoadScene(targetScene, Color.black);
        //            //}

        //            //if (OVRInput.GetDown(OVRInput.RawButton.B))
        //            //{
        //            //    // B 버튼이 눌렸을 때 수행할 동작 작성
        //            //    string targetScene = string.Format("{0}_{1}_{2}", SceneStructureType.Meditation.ToString(), ChaptersDefines.Chapter_1.ToString(), StagesDefines.Stage_1.ToString());
        //            //    LoadScene(targetScene, Color.black);
        //            //}





        //        }










    }//end of class					


}//end of namespace					