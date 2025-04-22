/*********************************************************					
* EndingCredit_SceneController.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.06.21 오후 1:15					
**********************************************************/
using Dev_Phase;
using Dev_SceneManagement;
using Dev_System;
using Dev_UI;
using Dev_Unit;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dev_LocalDB;

namespace Dev_SceneControl
{
    public class EndingCredit_SceneController : SceneControllerBase
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void EndingCreditAppear()
        {
            UIModule_EndingCredit ending = Managers.UIManager.EnableUIModule(Dev_UI.UISceneTypes.Common, Dev_UI.UIStates.Fixed, uniqueID:"EndingCredit", 
                genTrans: GenPos_EndingUI, group:Dev_UI.UIGroups.Group_1) as UIModule_EndingCredit;

            if(ending != null)
            {
                ending.Action_End = () =>
                {
                    PhaseManager.CompletePhase();
                };
            }
        }

        public void ChangeScene()
        {
            //로비로 씬변경
            string targetScene = string.Format("{0}_{1}", SceneStructureType.System.ToString(), SceneDefine_System.Lobby.ToString());
            Managers.LoadingHandler.LoadScene(targetScene, Color.black);

            DBSetting();
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private PhaseManager PhaseManager;
        [SerializeField] private Transform GenPos_Player;
        [SerializeField] private Transform GenPos_EndingUI;
        [SerializeField] private string Direction_BGM;

        // LocalDB
        private int chapter = 0;
        private int stage = 0;
        private int clear = 0;
        private int firstClear = 0;

        void Start()
        {
            StartCoroutine(CreateUnit());
        }

        protected override IEnumerator CreateUnit()
        {
            yield return new WaitUntil(() => Managers.AllCreateManagers == true);
            yield return new WaitUntil(() => Managers.LoadingHandler.IsLoading == false);

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

            Managers.Sound.PlayBGM(Direction_BGM);
            PhaseManager.InitRootPhase();

            Managers.Game.LoginModeTrigger(debugPlayMode: () =>
            {
                Managers.LoadingHandler.GetNextTargetSceneType = SceneStructureType.System;
            });
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
        void DBSetting()
        {
            Managers.Game.LoginTrigger(
                isLocaledEvent: () =>
                {
                    if (Managers.LocalDB.pDBForm_ContentProgress.pChapterDBForm_Array != null)
                    {
                        chapter = Managers.LocalDB.pDBForm_ContentProgress.pChapterDBForm;
                        stage = Managers.LocalDB.pDBForm_ContentProgress.pStageDBForm;
                        clear = Managers.LocalDB.pDBForm_ContentProgress.pIsClearDBForm;
                        // 0: 최초 클리어 안함, 1: 1챕터 클리어 하고 엔딩 씬, 2: 2챕터 클리어 하고 엔딩씬 
                        firstClear = Managers.LocalDB.pDBForm_AppSystem.pFirstClear;

                        if (firstClear == 1)
                        {
                            Managers.LocalDB.DataBaseInser(QueryType.UPDATE, LocalDBTables.AppSystem, FieldType.FIRSTCLEAR, 2);
                        }

                        else if (firstClear == 2 && chapter == 2 && stage == 4 && clear == 1)
                        {
                            Managers.LocalDB.DataBaseInser(QueryType.UPDATE, LocalDBTables.AppSystem, FieldType.FIRSTCLEAR, 3);
                        }
                    }
                },
                isLoginedEvent: () =>
                {
                    if (Managers.LocalDB.pDBForm_UserInfo.pCurUserFirstClear != 0)
                    {
                        chapter = Managers.LocalDB.pDBForm_UserInfo.pCurUserChapter;
                        stage = Managers.LocalDB.pDBForm_UserInfo.pCurUserStage;
                        clear = Managers.LocalDB.pDBForm_UserInfo.pCurUserIsClear;
                        // 0: 최초 클리어 안함, 1: 1챕터 클리어 하고 엔딩 씬, 2: 2챕터 클리어 하고 엔딩씬 
                        firstClear = Managers.LocalDB.pDBForm_UserInfo.pCurUserFirstClear;

                        if (firstClear == 1)
                        {
                            Managers.LocalDB.DataBaseInser(QueryType.UPDATE, LocalDBTables.AppSystem, FieldType.FIRSTCLEAR, 2);
                        }

                        else if (firstClear == 2 && chapter == 2 && stage == 4 && clear == 1)
                        {
                            Managers.LocalDB.DataBaseInser(QueryType.UPDATE, LocalDBTables.AppSystem, FieldType.FIRSTCLEAR, 3);
                        }
                    }
                });




        }
    }//end of class					
}//end of namespace					