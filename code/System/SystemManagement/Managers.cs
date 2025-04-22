using System.Collections;
using UnityEngine;
using Dev_Unit;
using Dev_Data;
using Dev_Gesture;
using UnityEngine.SceneManagement;

using Dev_SceneManagement;
using Dev_UI;
using SuperUI.Systems.Tweening;
using Dev_VideoUtils;
using Dev_LocalDB;
using Dev_DLC;

namespace Dev_System
{
    public enum Manager_UniqueID
    {
        None,
        GameManager,
        UnitManager,
        ObjectPoolManager,
        SoundManager,
        DataManager,
        GestureManager,
        SeamlessLoadingHandler,
        StageStructureManager,
        UIModuleManager,
        TweenMaster,
        UserStatusManager,
        VideoStructureManager,
        LocalDB,
        DLCManager,
    }

    public class Managers : MonoBehaviour
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------		

        // ------------------ 생성형 Manager들------------------------- 
        public static Managers Instance { get {  return instance; } }
        public static GameManager Game { get { return Instance?.game; } }
        public static UnitManager Unit { get { return Instance?.unit; } }
        public static ObjectPoolManager ObjectPool { get { return Instance?.objectPool; } }
        public static SoundManager Sound { get { return Instance?.sound; } }
        public static DataManager Data { get { return Instance?.data; } }
        public static GestureManager Gesture { get { return Instance?.gesture; } }
        public static SeamlessLoadingHandler LoadingHandler { get { return Instance?.seamlessLoadingHandler; } }
        public static StageStructureManager StageStructureMng { get { return Instance?.stageStructureManager; } }
        public static UIModuleManager UIManager { get { return Instance?.uiModuleManager; } }
        public static TweenMaster TweenMaster { get { return Instance?.tweenMaster; } }
        public static UserStatusManager UserStatusManager { get { return Instance?.userStatusManager; } }
        public static VideoStructureManager VideoManager { get { return Instance?.videoStructureManager; } }
        public static LocalDBManager LocalDB { get { return Instance?.LocalDBMng; } }
        public static DLCManager DLCManagers { get { return Instance?.dlcManager; } }

        // ------------------ 생성하지않는 Manager들-------------------- 
        public static BattleManager Battle
        {
            get
            {
                if (battle == null)
                {
                    battle = FindObjectOfType<BattleManager>();
                    if (battle == null)
                    {
                        GameObject obj = new GameObject("#BattleManager");
                        battle = obj.AddComponent<BattleManager>();
                    }
                }
                return battle;
            }
        }

        // 어드벤쳐 미션 매니저
        public static ExerciseManager Exercise
        {
            get
            {
                if (exercise == null)
                {
                    exercise = FindObjectOfType<ExerciseManager>();
                    if (exercise == null)
                    {
                        GameObject obj = new GameObject("#ExerciseManager");
                        exercise = obj.AddComponent<ExerciseManager>();
                    }
                }
                return exercise;
            }
        }

        public static bool Initialized { get; set; } = false;

        // 기다리는 변수들
        public static bool AllCreateManagers = false;
        public static bool AllClearDLC = false;
        public static bool AllClearDB = false;

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------	
        [SerializeField] private ManagerDataTable managerDataTable;

        // ------------------ 생성형 Manager들------------------------- 
        private static Managers instance;
        private GameManager game;
        private UnitManager unit;
        private ObjectPoolManager objectPool;
        private SoundManager sound;
        private DataManager data;
        private GestureManager gesture;
        private SeamlessLoadingHandler seamlessLoadingHandler;
        private StageStructureManager stageStructureManager;
        private UIModuleManager uiModuleManager;
        private TweenMaster tweenMaster;
        private UserStatusManager userStatusManager;
        private VideoStructureManager videoStructureManager;
        private LocalDBManager LocalDBMng;
        private DLCManager dlcManager;

        // ------------------ 생성하지않는 Manager들-------------------- 
        private static BattleManager battle;
        private static ExerciseManager exercise;

        void Awake()
        {
            if(instance == null && Initialized == false)
            {
                Initialized = true;
                StartCoroutine(Init());
            }
        }

        void Start()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded2;
            SceneManager.sceneLoaded += OnSceneLoaded2;
        }

        void OnSceneLoaded2(Scene scene, LoadSceneMode mode)
        {
            battle = null;
            exercise = null;
            Managers.Gesture.ResetGestureActions();
        }

        IEnumerator Init()
        {
            AllCreateManagers = false;

            yield return  StartCoroutine(CrateManager());

            AllCreateManagers = true;
        }

        IEnumerator CrateManager()
        {
            if (instance == null)
            {
                var manager = FindObjectOfType<Managers>();
                if (manager == null)
                {
                    GameObject obj = new GameObject("#Managers");
                    manager = obj.AddComponent<Managers>();
                }
                instance = manager;
                DontDestroyOnLoad(instance);
            }
            yield return new WaitUntil(() => instance != null);

            foreach (var info in managerDataTable.pManagerInfos)
            {
                GameObject obj = Instantiate(info.Prefab, this.transform, true);

                switch (info.ID)
                {
                    case Manager_UniqueID.GameManager:
                        {
                            game = obj.GetComponent<GameManager>();
                            break;
                        }
                    case Manager_UniqueID.UnitManager:
                        {
                            unit = obj.GetComponent<UnitManager>();
                            break;
                        }
                    case Manager_UniqueID.ObjectPoolManager:
                        {
                            objectPool = obj.GetComponent<ObjectPoolManager>();
                            break;
                        }
                    case Manager_UniqueID.SoundManager:
                        {
                            sound = obj.GetComponent<SoundManager>();
                            break;
                        }
                    case Manager_UniqueID.DataManager:
                        {
                            data = obj.GetComponent<DataManager>();
                            break;
                        }
                    case Manager_UniqueID.GestureManager:
                        {
                            gesture = obj.GetComponent<GestureManager>();
                            break;
                        }

                    case Manager_UniqueID.SeamlessLoadingHandler:
                        {
                            seamlessLoadingHandler = obj.GetComponent<SeamlessLoadingHandler>();
                            break;
                        }

                    case Manager_UniqueID.StageStructureManager:
                        {
                            stageStructureManager = obj.GetComponent<StageStructureManager>();
                            break;
                        }

                    case Manager_UniqueID.UIModuleManager:
                        {
                            uiModuleManager = obj.GetComponent<UIModuleManager>();
                            break;
                        }
                    case Manager_UniqueID.TweenMaster:
                        {
                            tweenMaster = obj.GetComponent<TweenMaster>();
                            break;
                        }
                    case Manager_UniqueID.UserStatusManager:
                        {
                            userStatusManager = obj.GetComponent<UserStatusManager>();
                            break;
                        }
                    case Manager_UniqueID.VideoStructureManager:
                        {
                            videoStructureManager = obj.GetComponent<VideoStructureManager>();
                            break;
                        }
                    case Manager_UniqueID.LocalDB:
                        {
                            LocalDBMng = obj.GetComponent<LocalDBManager>();
                            break;
                        }
                    case Manager_UniqueID.DLCManager:
                        {
                            dlcManager = obj.GetComponent<DLCManager>();
                            break;
                        }

                }
            }
            yield return null;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded2;
        }
    }
}