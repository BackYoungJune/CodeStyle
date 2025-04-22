/*********************************************************					
* AdventureSceneController.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_CutScene;
using Dev_Dialogue;
using Dev_Phase;
using Dev_SceneManagement;
using Dev_Spline;
using Dev_System;
using Dev_Transport;
using Dev_UI;
using Dev_Unit;
using Dreamteck.Splines;
using Oculus.Interaction;
using System.Collections;					
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Dev_SceneControl
{					
	public class AdventureSceneController : SceneControllerBase					
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					

        #region 연출
        public void PhaseEvnet_에피소드()
        {
            UIModule_Episode epUI = Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Adventure,
                uiState:    UIStates.FollowerVR,
                uniqueID:   "Episode",
                group:      UIGroups.Group_1,
                genpos:     Vector3.zero
                ) as UIModule_Episode;

            if (epUI != null)
            {
                epUI.Action_End += () =>
                {
                    CompletePhase();
                };
            }
        }

        public void PhaseEvnet_StoryStlling()
        {
            // UI
            UIModule_StoryTelling story = Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Adventure,
                uiState:    UIStates.Fixed,
                uniqueID:   "StoryTelling",
                group:      UIGroups.Group_1,
                genpos:     Vector3.zero,
                genTrans:   UIGenPos_StroyTelling
                ) as UIModule_StoryTelling;


            if (story != null)
            {
                story.Action_End += () =>
                {
                    CompletePhase();
                };
            }

            // 컷씬
            CutSceneController.PlayCutSceneInspector("StoryTelling");

            //// 다이얼로그
            //DevDialogueManager.Controller.SetConversation("ActivityTrack1");
            //DevDialogueManager.Controller.SetVariable("StoryTelling");
        }

        // ----------------------------------------------
        // 트랜지션
        // ----------------------------------------------
        public void Phase_맵트랜지션()
        {
            Managers.LoadingHandler.ScreenTransition(ScreenTransitionEffect.FadeIn, Color.black, 1.0f, finished: () =>
            {
                CompletePhase();
                Invoke("InvokeFadeOut", 1.0f);
            });
            //TitleTextDisappear();
        }

        void InvokeFadeOut()
        {
            Managers.LoadingHandler.ScreenTransition(ScreenTransitionEffect.FadeOut, Color.black, 1.0f, null);
        }
        #endregion

        #region Activity
        // ----------------------------------------------
        // Activity
        // ----------------------------------------------
        public void Phase_플레이어런(SplineComputer changeComputer)
        {
            Player.ChangeStateMachine(UnitState.MoveSpline);
            FollowNPC.ChangeStateMachine(UnitState.MoveSpline);
            SplineEvent.ChangeSplineSpeed("None");
            if (SubFollowNPC != null)
            {
                SubFollowNPC.ChangeStateMachine(UnitState.MoveSpline);
            }
            SplineEvent.ChangeSpline(changeComputer);

            // Activity 운동 Ui On
            Managers.Gesture.Switch_Activity(Dev_Gesture.ActivityGesture.walk);

            Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Adventure,
                uiState:    UIStates.FollowerVR,
                uniqueID:   "Activity",
                group:      UIGroups.Group_1,
                genpos:     Vector3.zero
                );

            Invoke("InvokeRank", 1.0f);

            // BGM Play
            Managers.Sound.PlayBGM(ActivityRunSound);

            // Exit Enable
            //if (Managers.UIManager.GetUIModule("Exit") == null)
            //{
            //    Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Common,
            //        uiState: UIStates.FollowerVR,
            //        uniqueID: "Exit",
            //        group: UIGroups.Group_1,
            //        genpos: Vector3.zero
            //        );
            //}
        }

        void InvokeRank()
        {
            // 랭킹 UI On
            if (Managers.UIManager.GetUIModule("Rank") == null)
            {

                Managers.UIManager.EnableUIModule(
                    type:       UISceneTypes.Adventure,
                    uiState:    UIStates.FollowerVR,
                    uniqueID:   "Rank",
                    group:      UIGroups.Group_1,
                    genpos:     Vector3.zero, 
                    followDistanceCam: 4.37f
                    );
            }
        }

        public void Phase_Activity도착()
        {
            // BGM 스탑
            Managers.Sound.StopBGM();

            // Activity 운동 UI Off
            Managers.UIManager.DisableUIModule("Activity", UIGroups.Group_1);

            // 랭킹 UI Off
            Managers.UIManager.DisableUIModule("Rank", UIGroups.Group_1);
        }

        public virtual void Climb(SplineComputer changeComputer)
        {
            // 플레이어 절벽 셋팅
            Player.PlayerRotation(new Vector3(71.017f, 24.163f, 25.382f));
            Player.ChangeStateMachine(UnitState.Climb);

            // Trainer 절벽 셋팅
            Trainer.PlayerRotation(new Vector3(80, 0, 0));
            Trainer.pAnimatorbase.SetBool(Trainer.pAnimatorIDTable.AnimID_Transport, true);
            Trainer.ChangeStateMachine(UnitState.Climb);
            SplineEvent.ChangeSplineTrainer(changeComputer);

            // NPC 절벽 셋팅
            FollowNPC.ChangeStateMachine(UnitState.Climb);
            Invoke("InvokeClimb", 0.8f);

            // 스프라인 절벽 셋팅
            SplineEvent.ChangeSpline(changeComputer);
            SplineEvent.ChangeSplineSpeed("Climb");

            // 액티비티 셋팅
            Managers.Gesture.Switch_Activity(Dev_Gesture.ActivityGesture.climbing);

            Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Adventure,
                uiState:    UIStates.FollowerVR,
                uniqueID:   "Activity",
                group:      UIGroups.Group_1,
                genpos:     Vector3.zero
                );

            // BGM Play
            Managers.Sound.PlayBGM(ActivityRunSound);

            Invoke("InvokeRank", 1.0f);

            // Exit Enable
            //if (Managers.UIManager.GetUIModule("Exit") == null)
            //{
            //    Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Common,
            //        uiState: UIStates.FollowerVR,
            //        uniqueID: "Exit",
            //        group: UIGroups.Group_1,
            //        genpos: Vector3.zero
            //        );
            //}
        }

        void InvokeClimb()
        {
            FollowNPC.NPCRotation(new Vector3(90.0f, 0, 0));
            FollowNPC.nAnimatorbase.SetTrigger(FollowNPC.nAnimatorIDTable.AnimID_Climb);
        }

        public void BoatTransport(SplineComputer changeComputer)
        {
            // 탈것 생성 및 초기화
            Transport createBoat = Managers.ObjectPool.GetObjectFromPool(PoolObjectType.Pool, PoolUniqueID.Transport_Boat).GetComponent<Transport>();
            createBoat.InitTransport();
            SplineEvent.ChangeSplineTransport(createBoat, changeComputer);

            // 플레이어 보트 셋팅
            Player.ChangeStateMachine(UnitState.OtherTransport);

            // NPC 보트 셋팅
            FollowNPC.ChangeStateMachine(UnitState.OtherTransport);
            Invoke("InvokeBoat", 0.8f);

            // 액티비티 셋팅
            Managers.Gesture.Switch_Activity(Dev_Gesture.ActivityGesture.rowing);
        
            Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Adventure,
                uiState:    UIStates.FollowerVR,
                uniqueID:   "Activity",
                group:      UIGroups.Group_1,
                genpos:     Vector3.zero
                );

            // BGM Play
            Managers.Sound.PlayBGM(ActivityRunSound);

            Invoke("InvokeRank", 1.0f);

            // Exit Enable
            //if (Managers.UIManager.GetUIModule("Exit") == null)
            //{
            //    Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Common,
            //        uiState: UIStates.FollowerVR,
            //        uniqueID: "Exit",
            //        group: UIGroups.Group_1,
            //        genpos: Vector3.zero
            //        );
            //}
        }

        void InvokeBoat()
        {
            FollowNPC.nAnimatorbase.SetTrigger(FollowNPC.nAnimatorIDTable.AnimID_Boat);
        }

        public void GliderTransport(SplineComputer changeComputer)
        {
            // 탈것 생성 및 초기화
            GameObject obj = Managers.ObjectPool.GetObjectFromPool(PoolObjectType.Pool, PoolUniqueID.Transport_Glider, GenPos_Glider);
            obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            obj.transform.localScale = Vector3.one;
            Transport createGlider = obj.GetComponent<Transport>();
            createGlider.InitTransport(false);
            SplineEvent.ChangeSplineTransport(createGlider, changeComputer);

            // 플레이어 글라이딩 셋팅
            Player.ChangeStateMachine(UnitState.OtherTransport);

            // 액티비티 셋팅
            Managers.Gesture.Switch_Activity(Dev_Gesture.ActivityGesture.gliding);
          
            Managers.UIManager.EnableUIModule(
                type:       UISceneTypes.Adventure,
                uiState:    UIStates.FollowerVR,
                uniqueID:   "Activity",
                group:      UIGroups.Group_1,
                genpos:     Vector3.zero
                );

            // BGM Play
            Managers.Sound.PlayBGM(ActivityRunSound);

            // Exit Enable
            //if (Managers.UIManager.GetUIModule("Exit") == null)
            //{
            //    Managers.UIManager.EnableUIModule(
            //        type: UISceneTypes.Common,
            //        uiState: UIStates.FollowerVR,
            //        uniqueID: "Exit",
            //        group: UIGroups.Group_1,
            //        genpos: Vector3.zero
            //        );
            //}
        }
        #endregion

        #region Mission & Battle
        // ----------------------------------------------
        // Mission
        // ----------------------------------------------
        public virtual void Mission1_진입()
        {
            // BGM Play
            Managers.Sound.PlayBGM(Mission1_Sound);
        }

        public virtual void Mission1_운동시작()
        {
            Managers.Gesture.Load_Gesture(1);
            // Gestrue 운동 시작
            Managers.Gesture.Start_Gesture(UIGenPos_Mission1);
        }

        public virtual void Mission2_진입()
        {
            // BGM Play
            Managers.Sound.PlayBGM(Mission2_Sound);
        }

        public virtual void Mission2_운동시작()
        {
            Managers.Gesture.Load_Gesture(2);
            // Gestrue 운동 시작
            Managers.Gesture.Start_Gesture(UIGenPos_Mission2);
        }

        public virtual void Mission3_진입()
        {
            // BGM Play
            Managers.Sound.PlayBGM(Mission3_Sound);
        }

        public virtual void Mission3_운동시작()
        {
            Managers.Gesture.Load_Gesture(3);
            // Gestrue 운동 시작
            Managers.Gesture.Start_Gesture(UIGenPos_Mission3);
        }

        protected void Phase_미션진입()
        {
            // 플레이어, 숑이 회전값 초기화
            Player.PlayerRotation(Vector3.zero);
            FollowNPC.NPCRotation(Vector3.zero);

            //임시코드 없어질예정
            Player.ChangeStateMachine(UnitState.BattleIdle);
            FollowNPC.ChangeStateMachine(UnitState.BattleIdle);

            ////Exerceis - object 생성
            Managers.Unit.ResetExerciseIndex();
            CreateMission();

            //전투매니저 초기화
            Managers.Battle.InitBattle(BattleManagerMode.Exercise);

            // 턴오버 컷씬 초기화
            CutSceneController.SetMissionCutsceneTurnOver();
            Invoke("InvokeMissionSetting", 1.0f);
        }

        protected void Phase_미션시작()
        {           

            //플레이어 상태변경 = BattleIdle
            Player.ChangeStateMachine(UnitState.BattleIdle);
            FollowNPC.ChangeStateMachine(UnitState.BattleIdle);
        }

        public void Phase_미션2시작()
        {
            ////Exerceis - Torchh 생성
            Managers.Unit.ResetExerciseIndex();
            CreateMission2();

            // 턴오버 컷씬 초기화
            CutSceneController.SetMissionCutsceneTurnOver();
            Invoke("InvokeMissionSetting", 1.0f);
        }

        // ----------------------------------------------
        // Battle
        // ----------------------------------------------
        protected virtual void Phase_전투1_Init()
        {
            //전투매니저 초기화
            Managers.Battle.InitBattle(BattleManagerMode.Battle, turnState);

            //Enemy - Enemy 생성
            if(UniqueID_Enemy != UnitUniqueID.None)
            {
                Managers.Unit.CreateUnit(UniqueID_Enemy, UnitState.BattleIdle, GenPos_Enemy, true, Vector3.one);
            }
        }

        protected void Phase_전투1()
        {          
            //플레이어 상태변경 = BattleIdle
            Player.ChangeStateMachine(UnitState.BattleIdle);
            FollowNPC.ChangeStateMachine(UnitState.BattleIdle);
            if (SubFollowNPC != null)
            {
                SubFollowNPC.ChangeStateMachine(UnitState.BattleIdle);
            }

            // 턴오버 컷씬 초기화
            CutSceneController.SetBattleCutsceneTurnOver();
        }

        protected virtual void Phase_전투2_Init()
        {
            //전투매니저 초기화
            Managers.Battle.InitBattle(BattleManagerMode.Battle, turnState);

            //Enemy - MinJi 생성
            if(UniqueID_Enemy2 != UnitUniqueID.None)
            {
                Managers.Unit.CreateUnit(UniqueID_Enemy2, UnitState.BattleIdle, GenPos_Enemy2, true, Vector3.one);
            }
        }

        protected void Phase_전투2()
        {           

            //플레이어 상태변경 = BattleIdle
            Player.ChangeStateMachine(UnitState.BattleIdle);
            FollowNPC.ChangeStateMachine(UnitState.BattleIdle);
            if (SubFollowNPC != null)
            {
                SubFollowNPC.ChangeStateMachine(UnitState.BattleIdle);
            }

            // 턴오버 컷씬 초기화
            CutSceneController.SetBattleCutsceneTurnOver();
        }

        public virtual void Mission1_End()
        {
            // BGM Stop
            Managers.Sound.StopBGM();
            Managers.Sound.StopStoneBGM();
            
            // Win SFX Start
            Managers.Sound.PlaySFX("10040");
        }

        public virtual void Mission2_End()
        {
            // BGM Stop
            Managers.Sound.StopBGM();
            Managers.Sound.StopStoneBGM();

            // Win SFX Start
            Managers.Sound.PlaySFX("10040");
        }

        public virtual void Mission3_End()
        {
            //Off_Exit();

            // BGM Stop
            Managers.Sound.StopBGM();
            Managers.Sound.StopStoneBGM();

            // Win SFX Start
            Managers.Sound.PlaySFX("10040");
        }
        #endregion

        public void NullSplineComputer()
        {
            SplineEvent.NullSplinePlayer();
        }

        public void Phase_Result()
        {
            //ResultUI();
            //Managers.UIManager.DisableUIModule("Exit");
            
            if (Managers.UIManager.GetUIModule("HealthStone") == null)
            {
                UIModule_HealthStoneUI stone = Managers.UIManager.EnableUIModule(
                    type: UISceneTypes.Adventure,
                    uiState: UIStates.Fixed,
                    uniqueID: "HealthStone",
                    group: UIGroups.Group_1,
                    genTrans: Managers.Unit.GetCurTurnUnit.pStoneUIPos
                    ) as UIModule_HealthStoneUI;
            
                if(stone != null)
                {
                    stone.Action_End = () =>
                    {
                        ResultUI();
                    };
                }
            }
        }

        private void ResultUI()
        {
            //On_Exit();


            // 랭킹 UI 나오게함
            UIModule_Result result = Managers.UIManager.EnableUIModule(
                type: UISceneTypes.Adventure,
                uiState: UIStates.FollowerVR,
                uniqueID: "AdventureResult",
                group: UIGroups.Group_2,
                genpos: Vector3.zero
                ) as UIModule_Result;


            if (result != null)
            {
                result.Action_End += () =>
                {
                    CompletePhase();
                };
            }
        }

        // ----------------------------------------------
        // TitleText
        // ----------------------------------------------

        public void TitleText()
        {
            StopCoroutine("TitleTextCor");
            StartCoroutine("TitleTextCor");
        }

        IEnumerator TitleTextCor()
        {
            yield return new WaitForSeconds(2.0f);

            bool maintain = false;
            int index = -1;
            string titleText = "";

            for (int i = 0; i < TitleTextMissionPhase.Length; i++)
            {
                if (TitleTextMissionPhase[i].activeInHierarchy)
                {
                    maintain = true;
                    index = i;
                }
            }

            for (int i = 0; i < TitleTextActivityPhase.Length; i++)
            {
                if (TitleTextActivityPhase[i].activeInHierarchy)
                {
                    maintain = false;
                    index = i;
                }
            }

            if (maintain)
            {
                if (index >= 0 && index < TitleMissionTexts.Length)
                {
                    titleText = TitleMissionTexts[index];
                }
            }
            else
            {
                if (index >= 0 && index < TitleActivityTexts.Length)
                {
                    titleText = TitleActivityTexts[index];
                }
            }

            // Todo : 우선 주석처리
            //if (Adventure_UI.instance != null && titleText != "")
            //{
            //    Adventure_UI.instance.TitleTextAppear(titleText, maintain);
            //}
        }

        public void TitleTextDisappear()
        {
            // Todo : 우선 주석처리
            //if (Adventure_UI.instance != null)
            //{
            //    Adventure_UI.instance.TitleTextDisappear();
            //}
        }

        

        public void EnterCutScene()
        {
            Player?.ChangeStateMachine(UnitState.CutScene);
            FollowNPC?.ChangeStateMachine(UnitState.CutScene);

            DevDialogueManager.Controller.EnterTimeline();
        }

        public void EndCutScene()
        {
            DevDialogueManager.Controller.EndTimeline();
            CutSceneController.PlayCutScene = false;

            // 컷씬이 끝날시 배틀상태라면 배틀 Idle로 다시 보내준다
            if (Managers.Battle.BattleManagerMode == BattleManagerMode.Battle)
            {
                //플레이어 상태변경 = BattleIdle
                Player.ChangeStateMachine(UnitState.BattleIdle);
                FollowNPC.ChangeStateMachine(UnitState.BattleIdle);
            }
        }

        public void ChangeScene()
        {
            ResetAdventure();

            Managers.Gesture.Switch_Content(ContentType.FITNESS);
            Managers.Game.EndSceneStructure(SceneStructureType.Adventure);
        }

        //public void Off_Exit()
        //{

        //    if (Managers.UIManager.GetUIModule("Exit") != null)
        //    {
        //        Managers.UIManager.DisableUIModule("Exit");
        //    }
        //}


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [Header("Component")]
		[SerializeField] protected PhaseManager PhaseManager;
		[SerializeField] protected SplineEvent SplineEvent;
        [SerializeField] protected CutSceneTimelineController CutSceneController;

		[Header("Unit")]
		[SerializeField] protected UnitUniqueID UniqueID_FollowerNPC = UnitUniqueID.None;
		[SerializeField] protected UnitUniqueID UniqueID_SubFollowerNPC = UnitUniqueID.None;
        protected UnitFollowNPC FollowNPC;
        protected UnitSubFollowNPC SubFollowNPC;
        [SerializeField] protected UnitUniqueID UniqueID_Enemy = UnitUniqueID.None;
        [SerializeField] protected UnitUniqueID UniqueID_Enemy2 = UnitUniqueID.None;
        protected UnitTrainer Trainer;
        [SerializeField] protected UnitUniqueID UniqueID_Trainer = UnitUniqueID.None;
        protected TurnState turnState = TurnState.PlayerTurn;

        [Header("Genpos")]
        [SerializeField] protected Transform GenPos_Player;
        [SerializeField] protected Transform GenPos_FollowNPC;
        [SerializeField] protected Transform GenPos_SubFollowNPC;
        [SerializeField] protected Transform GenPos_Enemy;
        [SerializeField] protected Transform GenPos_Enemy2;
        [SerializeField] protected Transform UIGenPos_StroyTelling;
        [SerializeField] protected Transform UIGenPos_Mission1;
        [SerializeField] protected Transform UIGenPos_Mission2;
        [SerializeField] protected Transform UIGenPos_Mission3;
        [SerializeField] protected List<Transform> GenPos_MissionObjects;
        [SerializeField] protected List<Transform> GenPos_MissionObjects2;
        [SerializeField] protected Transform GenPos_Trainer;

		[Header("Spline")]
		[SerializeField] protected SplineComputer[] AdventureSpline100;
		[SerializeField] protected SplineComputer[] AdventureSpline200;
		[SerializeField] protected SplineComputer[] AdventureSpline300;

		[Header("TitleText")]
		[SerializeField] private GameObject[] TitleTextActivityPhase;
		[SerializeField] private GameObject[] TitleTextMissionPhase;
        private string[] TitleActivityTexts;
        private string[] TitleMissionTexts;

        [Header("Transport")]
        [SerializeField] private TransportTable TransportTable;
        [SerializeField] private Transform GenPos_Boat;
        [SerializeField] private Transform GenPos_Glider;

        [Header("Sound")]
        [SerializeField] private string ActivityRunSound;
        [SerializeField] private string Mission1_Sound;
        [SerializeField] private string Mission2_Sound;
        [SerializeField] private string Mission3_Sound;

        [Header("Sprite")]
        [SerializeField] private Sprite MaleSprite;
        [SerializeField] private Sprite FemaleSprite;

        private void Awake()
        {
            StartCoroutine(CreateUnit());
        }

        protected virtual void Start()					
		{
			
		}					
							
        protected override IEnumerator CreateUnit()
        {
			yield return new WaitUntil(() => Managers.AllCreateManagers);
            yield return new WaitUntil(() => Managers.LoadingHandler.IsLoading == false);

            // Player 생성, 위치 변경
            Managers.Game.LoginModeTrigger(
                loginPlayMode: () =>
                {
                    Managers.Unit.GetCurTurnUnit.transform.SetParent(GenPos_Player);
                    Managers.Unit.GetCurTurnUnit.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                },
                debugPlayMode: () =>
                {
                    if (Managers.Unit.GetCurTurnUnit == null)
                    {
                        Managers.Unit.CreateUnit(UnitUniqueID.Player_VR, UnitState.CutScene, GenPos_Player, true, Vector3.one);
                    }
                    else
                    {
                        Managers.Unit.GetCurTurnUnit.transform.SetParent(GenPos_Player);
                        Managers.Unit.GetCurTurnUnit.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    }
                });
            Managers.Unit.GetCurTurnUnit.Follower = Managers.Unit.GetCurTurnUnit.transform.parent?.GetComponent<SplineFollower>();
            

            // NPC 생성
            if (UniqueID_FollowerNPC != UnitUniqueID.None)
			{
				Managers.Unit.CreateUnit(UniqueID_FollowerNPC, UnitState.CutScene, GenPos_FollowNPC, true, Vector3.one * 0.3f);
				yield return new WaitUntil(() => Managers.Unit.GetCurFollowNPCUnit != null);
                FollowNPC = Managers.Unit.GetCurFollowNPCUnit;
            }

			// SubNPC 생성
			if(UniqueID_SubFollowerNPC != UnitUniqueID.None)
			{
				Managers.Unit.CreateUnit(UniqueID_SubFollowerNPC, UnitState.CutScene, GenPos_SubFollowNPC, true, Vector3.one * 0.3f);
				yield return new WaitUntil(() => Managers.Unit.GetCurSubFollowNPCUnit != null);
                SubFollowNPC = Managers.Unit.GetCurSubFollowNPCUnit;
            }

            // Trainer 생성
            if(UniqueID_Trainer != UnitUniqueID.None)
            {
                Managers.Unit.CreateUnit(UniqueID_Trainer, UnitState.None, GenPos_Trainer, true, Vector3.one);
                yield return new WaitUntil(() => Managers.Unit.GetCurTrainerUnit != null);
                Trainer = Managers.Unit.GetCurTrainerUnit;
                Trainer.Follower = GenPos_Trainer.GetComponent<SplineFollower>();
            }
			
			yield return new WaitUntil(() => Managers.Unit.GetCurTurnUnit != null);
            Player = Managers.Unit.GetCurTurnUnit;

            yield return new WaitForSeconds(DevDefine.fadeOutTime);
            Managers.Sound.PlayBGM(ActivityRunSound);
            InitScene();
        }

		protected virtual void InitScene()
		{
            Managers.LocalDB.DataBaseRead_ContentProgress("Select * From ContentsInfo");
            SplineEvent.SplineInit();
            PhaseManager.InitRootPhase();
            EventSetting();
            CalculeateSpline();
            TitleTextDummy();
            Load_Gesture();
            //On_Exit();
            DialogueChangeSprite();
            turnState = TurnState.PlayerTurn;

            // Debug
            Managers.Game.LoginModeTrigger(debugPlayMode: () =>
            {
                Managers.LoadingHandler.GetNextTargetSceneType = SceneStructureType.Adventure;
            });
        }

        //void On_Exit()
        //{
        //    if (Managers.UIManager.GetUIModule("Exit") == null)
        //    {
        //        Managers.UIManager.EnableUIModule(
        //        type: UISceneTypes.Common,
        //        uiState: UIStates.FollowerVR,
        //        uniqueID: "Exit",
        //        group: UIGroups.Group_1,
        //        genpos: Vector3.zero
        //        );
        //    }
        //}


		void EventSetting()
		{
            //PhaseManager.ComplePhaseEvent += TitleText;

            // EventSystem 처리
            EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
            if(eventSystems.Length > 1)
            {
                EventSystem curEventSystem = null;
                foreach(var eventSystem in eventSystems)
                {
                    if(eventSystem.GetComponent<PointableCanvasModule>() != null)
                    {
                        curEventSystem = eventSystem;
                        continue;
                    }
                    else
                    {
                        StandaloneInputModule stand = eventSystem.GetComponent<StandaloneInputModule>();
                        if(stand != null)
                        {
                            Destroy(stand);
                        }
                        Destroy(eventSystem.gameObject);
                    }
                }

                if(curEventSystem == null)
                {
                    new GameObject("EventSystem").AddComponent<UnityEngine.EventSystems.EventSystem>();
                }
            }
        }

        void Load_Gesture()
        {
            //운동 받아오기   
            Managers.Gesture.Switch_Content(ContentType.ADVENTURE);           
        }

        void TitleTextDummy()
        {
            TitleActivityTexts = new string[] { "당근 농장 가는 길", "미니 찾으러 가는 길", "미니 찾으러 가는 길" };
            TitleMissionTexts = new string[] { "당근 농장 -\r\n 당근 뽑기", "민지 텃밭", "미니네 집" };
        }

        void CalculeateSpline()
        {
            // Todo : 우선 주석처리
            if (Managers.Game.pDistances == null)
            {
                Managers.Game.pDistances = new List<float>();
            }
            Managers.Game.pDistances.Clear();
            Managers.Game.pTotalMoveDistance = 0;
            Managers.Game.pMoveDistance = 0;

            float distance = 0;
            foreach (var spline in AdventureSpline100)
            {
                distance += spline.CalculateLength();
            }
            Managers.Game.pDistances.Add(distance);
            distance = 0;

            foreach (var spline in AdventureSpline200)
            {
                distance += spline.CalculateLength();
            }
            Managers.Game.pDistances.Add(distance);
            distance = 0;

            foreach (var spline in AdventureSpline300)
            {
                distance += spline.CalculateLength();
            }
            Managers.Game.pDistances.Add(distance);

            foreach (var dist in Managers.Game.pDistances)
            {
                Managers.Game.pTotalMoveDistance += dist;
            }

        }

        // Todo : 우선 주석처리
        //void ReceiveTitleText()
        //{
        //    StartCoroutine(ReceiveTitleTextCor());
        //}

        // Todo : 우선 주석처리
        //IEnumerator ReceiveTitleTextCor()
        //{
        //    yield return new WaitUntil(() => TionXR_UserData.Instance != null);
        //    yield return new WaitUntil(() => TionXR_UserData.Instance._AdventureTitle != null);
        //    int arrSize = TionXR_UserData.Instance._AdventureTitle.DATA.ActivityArry.Length;
        //    if (TitleActivityTexts != null)
        //    {
        //        System.Array.Clear(TitleActivityTexts, 0, TitleActivityTexts.Length);
        //    }
        //    TitleActivityTexts = null;
        //    TitleActivityTexts = new string[arrSize];
        //    for (int i = 0; i < arrSize; i++)
        //    {
        //        TitleActivityTexts[i] = TionXR_UserData.Instance._AdventureTitle.DATA.ActivityArry[i].Activity;
        //    }
        //    arrSize = TionXR_UserData.Instance._AdventureTitle.DATA.MissionArry.Length;
        //    if (TitleMissionTexts != null)
        //    {
        //        System.Array.Clear(TitleMissionTexts, 0, TitleMissionTexts.Length);
        //    }
        //    TitleMissionTexts = null;
        //    TitleMissionTexts = new string[arrSize];
        //    for (int i = 0; i < arrSize; i++)
        //    {
        //        TitleMissionTexts[i] = TionXR_UserData.Instance._AdventureTitle.DATA.MissionArry[i].Mission;
        //    }

        //    TionXR_UserData.Instance._AdventureTitle = null;
        //}

        void CompletePhase()
        {
            PhaseManager.CompletePhase();
        }

        void DialogueChangeSprite()
        {
            string gender = "";
            Sprite sprite = null;
            Managers.Game.CharGenderTrigger(
            male: () =>
            {
                gender = "Dino";
                sprite = MaleSprite;
            },
            female: () =>
            {
                gender = "Kiki";
                sprite = FemaleSprite;
            });
            DevDialogueManager.Controller.DialogueSpriteChange(gender, sprite, "Player");
        }

        protected override void Update()
        {
            base.Update();
        }

        protected virtual void CreateMission() { }
        protected virtual void CreateMission2() { }

        protected virtual void InvokeMissionSetting()
        {
            DevDialogueManager.Mission.SetMissionObject();
            Managers.Exercise.InitExercise();
        }

        protected virtual void 전투BattleInit(TurnState turnState = TurnState.PlayerTurn)
        {
            Managers.Battle.InitBattle(BattleManagerMode.Battle, turnState);
        }

        protected virtual void 전투2BattleInit(TurnState turnState = TurnState.PlayerTurn)
        {
            Managers.Battle.InitBattle(BattleManagerMode.Battle, turnState);
        }

        void ResetAdventure()
        {
            StopAllCoroutines();
        }

    }//end of class								
}//end of namespace					