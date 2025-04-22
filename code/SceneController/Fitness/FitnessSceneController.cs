/*********************************************************					
* FitnessSceneController.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_Dialogue;
using Dev_Phase;
using Dev_SceneManagement;
using Dev_Spline;
using Dev_System;
using Dev_UI;
using Dev_Unit;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dev_SceneControl
{					
	public class FitnessSceneController : SceneControllerBase					
	{

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void Phase_RankAppear()
        {
            Managers.UIManager.EnableUIModule(
                type:        UISceneTypes.Adventure,
                uiState:    UIStates.Fixed,
                uniqueID:   "Rank",
                group:      UIGroups.Group_1,
                genpos:     Vector3.zero,
                genTrans:   GenPos_Rank);
        }

        public void Phase_RankDisAppear()
        {
            Managers.UIManager.DisableUIModule("Rank");
        }

        public void CameraDistance(float dist)
        {
            DevUtils.CameraFarDistance(dist);
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------		
        [SerializeField] private PhaseManager Phase;
        [SerializeField] private Transform GenPos_Player;
        [SerializeField] private Transform GenPos_Trainer;
        [SerializeField] private Transform GenPos_GestureList;
        [SerializeField] private Transform GenPos_Exercise;
        [SerializeField] private Transform GenPos_Result;
        [SerializeField] private Transform GenPos_Rank;
        [SerializeField] private DevDialogueController DialogueController;
        [SerializeField] private Sprite FemaleDialogueSprite;
        [SerializeField] private Sprite MaleDialogueSprite;



        protected virtual void Start()
        {
            StartCoroutine(CreateUnit());          
        }

        public void Start_Fitness()
        {
            DialogueController.SetVariable(Managers.Gesture.CurFitnessProgram.ToString());
        }

        public void End_Fitness()
        {
            if(Managers.Gesture.MyContent == ContentType.FITNESS)
            DialogueController.SetVariable("EndExcisie");
            else
                DialogueController.SetVariable("EndPFT");
        }



        public void Start_Exercise()
        {
            DialogueController.SetFitnessConversatioin("Exercise");
            Managers.Gesture.Start_Gesture(GenPos_Exercise);            
        }


        void InitFitnessScene()
        {
            On_TimerUI();
            //Managers.Gesture.Switch_Content(ContentType.FITNESS);
            Managers.Gesture.Load_Gesture(0);                                    //운동 받아오기
            DialogueController.SetFitnessConversatioin("FlowCommon");           //다이얼로그 세팅
            Managers.Sound.PlayBGM("10063");
            DialogueChangeSprite();

            //Managers.UIManager.EnableUIModule(
            //    type:       UISceneTypes.Common,
            //    uiState:    UIStates.FollowerVR,
            //    uniqueID:   "Exit",
            //    group:      UIGroups.Group_1,
            //    genpos:     Vector3.zero
            //    );
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
                    if(Managers.Unit.GetCurTurnUnit == null)
                    {
                        Managers.Unit.CreateUnit(UnitUniqueID.Player_VR, UnitState.CutScene, GenPos_Player, true, Vector3.one);
                    }
                    else
                    {
                        Managers.Unit.GetCurTurnUnit.transform.position = GenPos_Player.position;
                        Managers.Unit.GetCurTurnUnit.transform.rotation = GenPos_Player.rotation;
                    }
                });

            //Player 생성
            yield return new WaitUntil(() => Managers.Unit.GetCurTurnUnit != null);
            Managers.Unit.CreateUnit(UnitUniqueID.Trainer_Fitness, UnitState.FitnessIdle, GenPos_Trainer, true, Vector3.one);
            Managers.Unit.GetCurTurnUnit.ChangeStateMachine(UnitState.FitnessIdle);

            Managers.Game.LoginModeTrigger(debugPlayMode: () =>
            {
                Managers.LoadingHandler.GetNextTargetSceneType = SceneStructureType.Fitness;
            });

            yield return new WaitForSeconds(DevDefine.fadeOutTime);
            InitFitnessScene();
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

        public void UI_GestureList()
        {
            if(Managers.Gesture.MyContent == ContentType.FITNESS)
            DialogueController.SetVariable("StartExercise");
            else if(Managers.Gesture.MyContent == ContentType.PFT)
                DialogueController.SetVariable("StartPFT");
            Managers.UIManager.EnableUIModule(
                type:       Dev_UI.UISceneTypes.Fitness,
                uiState:    UIStates.Fixed,
                uniqueID:   "GestureList",
                group:      UIGroups.Group_1,
                genpos:     new Vector3(0, 0, 0),
                genTrans:   GenPos_GestureList);            
        }


        public void UI_CloseGestureList()
        {
            Managers.UIManager.DisableUIModule("GestureList");
        }


        public void UI_Result()
        {

            if (Managers.Gesture.MyContent == ContentType.FITNESS)
            {
                if (Managers.UIManager.GetUIModule("FitnessResult") == null)
                {
                    UIModule module = Managers.UIManager.EnableUIModule(
                        type: Dev_UI.UISceneTypes.Fitness,
                        uiState: UIStates.Fixed,
                        uniqueID: "FitnessResult",
                        group: UIGroups.Group_2,
                        genpos: Vector3.zero,
                        genTrans: GenPos_Result);


                    UIModule_Result result = module.GetComponent<UIModule_Result>();
                    if (result != null)
                    {
                        result.Action_End = () =>
                        {
                            Managers.UIManager.DisableUIModule("FitnessResult");
                            Phase.CompletePhase();
                        };
                    }
                }
            }
            else
            {
                if (Managers.UIManager.GetUIModule("PFTResult") == null)
                {
                    UIModule module = Managers.UIManager.EnableUIModule(
                        type: Dev_UI.UISceneTypes.Fitness,
                        uiState: UIStates.Fixed,
                        uniqueID: "PFTResult",
                        group: UIGroups.Group_2,
                        genpos: Vector3.zero,
                        genTrans: GenPos_Result);


                    UIModule_PFTResult result = module.GetComponent<UIModule_PFTResult>();
                    if (result != null)
                    {
                        result.Action_End = () =>
                        {
                            Managers.UIManager.DisableUIModule("PFTResult");
                            Phase.CompletePhase();
                        };
                    }
                }             
            }
        }


        // 추후 ChangeScene으로 변경 필요할 것 같습니다.
        public void Load_Lobby()
        {
            //string targetScene = string.Format("{0}_{1}", SceneStructureType.System.ToString(), SceneDefine_System.Lobby.ToString());
            //Managers.LoadingHandler.LoadScene(targetScene, Color.black);
            Managers.Game.EndSceneStructure(SceneStructureType.Fitness);
        }
        protected override void Update()
        {
            base.Update();
        }

        void ResetFitness()
        {
            StopAllCoroutines();
        }

        void DialogueChangeSprite()
        {
            string gender = "";
            Sprite sprite = null;
            Managers.Game.CharGenderTrigger(
            male: () =>
            {
                gender = "Dino";
                sprite = MaleDialogueSprite;
            },
            female: () =>
            {
                gender = "Kiki";
                sprite = FemaleDialogueSprite;
            });
            DevDialogueManager.Controller.DialogueSpriteChange(gender, sprite, "System");
        }

        void On_TimerUI()
        {


        }

     

    }//end of class								
}//end of namespace					