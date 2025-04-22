/*********************************************************					
* BattleManager.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.30 오후 11:04					
**********************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;
using Dev_System;
using Dev_Gesture;
using Dev_CutScene;
//using Dev_SceneManagement;

namespace Dev_Unit
{
    public enum BattleManagerMode { None, Battle, Exercise, Fitness, Profit }
    public enum TurnState { None = 0, PlayerTurn = 1, EnemyTurn = 2, EndBattleTurn = 3 }
    public enum BattleResult { None, Victory, Defeat, Drawn }
    public enum GestureReferee //전투동작 인식 상태
    {
        None,
        Perfect,    //Perfect판정시
        Good        //Good판정시
    }

    public class BattleManager : MonoBehaviour
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void InitBattle(BattleManagerMode battleMode, TurnState _turnState = TurnState.PlayerTurn)
        {
            BattleMode = battleMode;
            CurTurnState = _turnState;
            Managers.Gesture.ResetGestureActions();

        }

        public void TurnOver()
        {
            TurnOverAction?.Invoke();
            StartCoroutine(ITurnOver());
        }

        public void BattleVictoryTrigger(Action victoryAction, Action defeatAction, Action drawnAction)
        {
            switch (CurBattleResult)
            {
                case BattleResult.Victory:
                    if (victoryAction != null)
                        victoryAction.Invoke();
                    break;
                case BattleResult.Defeat:
                    if (defeatAction != null)
                        defeatAction.Invoke();
                    break;
                case BattleResult.Drawn:
                    if (drawnAction != null)
                        drawnAction.Invoke();
                    break;
            }
        }

        public bool pIsEndBattle { get { return IsEndBattle; } }
        public int pCurGestureIndex { get; set; }
        public int pCurGestureAnimID { get { return CurGestureAnimID; } set { CurGestureAnimID = value; } }
        public TurnState pCurTurnState { get { return CurTurnState; } }
        public BattleResult pCurBattleResult { get { return CurBattleResult; } }
        public GestureReferee pGestureReferees { get { return GestureReferees; } }
        public bool pIsProgress { get { return IsProgress; } set { IsProgress = value; } }
        public BattleManagerMode BattleManagerMode { get { return BattleMode; } set { BattleMode = value; } }
        public bool IsFirstFitness { get; set; }
        public UnityAction DontSkipTurn;        // 턴 스킵을 하지 않는 이벤트
        public UnityAction TurnOverAction;
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------		
        [Header("-------- Setting Battle Manager ----------")]
        [SerializeField] private UnityEvent VictoryEvent;       //전투 승리 이벤트
        [SerializeField] private UnityEvent DefeatEvent;        //전투 패배 이벤트
        [SerializeField] private UnityEvent DrawnEvent;         //전투 무승부 이벤트
        [SerializeField] private UnityEvent BattleEndEvent;     //전투 종료 이벤트
        private bool noSkip = false;

        [Header("------- Component --------")]
        [SerializeField] private CutSceneTimelineController CutSceneController;

        [Header("---------- DEBUG ----------")]
        //[SerializeField] private bool UseDebug = false;
        [SerializeField] private BattleManagerMode BattleMode;
        [SerializeField] private TurnState CurTurnState;        //현재턴인 세력
        [SerializeField] private BattleResult CurBattleResult;  //전투승패결과                           //전투중
        [SerializeField] private bool IsProgress;                                //전투연출중
        private GestureReferee GestureReferees;                 //player가 회피성공/실패 판정 변수
        private int CurGestureAnimID;                           //현재 타겟 운동동작
        private Gesture CurGestureState;
        private float autoTime = 0f;

        void Start()
        {
            DontSkipTurn -= DoNotSkipTurn;
            DontSkipTurn += DoNotSkipTurn;
        }

        void Update()
        {
            if (Managers.AllCreateManagers == false) return;
            if (Managers.LoadingHandler.IsLoading) return;

            //-------------------------------------------------------------------------------
            //          전투 턴 전환 및 유닛상태 업데이트
            //-------------------------------------------------------------------------------
            if (BattleMode == BattleManagerMode.Battle && Managers.Gesture != null)
            {
                if (Managers.Gesture.TurnOverAction == null)
                {
                    //운동 TurnOver 판정 함수 세팅
                    Managers.Gesture.TurnOverAction = TurnOver;

                    //Debug.LogError("턴오버 함수 세팅");
                }

                //--------------------------------------------------------------
                //					플레이어 턴일때
                //--------------------------------------------------------------
                if (CurTurnState == TurnState.PlayerTurn)
                {
                    //운동 Perfect 판정 함수 세팅
                    if (Managers.Gesture.PerfectGestureAction == null)
                    {
                        Managers.Gesture.PerfectGestureAction = () =>
                        {
                            GestureReferees = GestureReferee.Perfect;

                            //공격자 - Player
                            Managers.Unit.GetCurTurnUnit.ChangeStateMachine(UnitState.BattleAttack);


                            //스테미나 감소
                            Managers.Game.Calculate_Stemina(consumePercent: 0.3f, isPlus: false, action: null);
                        };
                    }

                    //운동 Good 판정 함수 세팅
                    if (Managers.Gesture.GoodGestureAction == null)
                    {
                        Managers.Gesture.GoodGestureAction = () =>
                        {
                            GestureReferees = GestureReferee.Good;

                            //공격자 - Player
                            Managers.Unit.GetCurTurnUnit.ChangeStateMachine(UnitState.BattleAttack);


                            //스테미나 감소
                            Managers.Game.Calculate_Stemina(consumePercent: 0.3f, isPlus: false, action: null);
                        };
                    }
                }

                //--------------------------------------------------------------
                //					에너미 턴일때
                //--------------------------------------------------------------
                else if (CurTurnState == TurnState.EnemyTurn && Managers.Gesture != null)
                {
                    //운동 Perfect 판정 함수 세팅
                    if (Managers.Gesture.PerfectGestureAction == null)
                    {
                        Managers.Gesture.PerfectGestureAction = () =>
                        {
                            GestureReferees = GestureReferee.Perfect;

                            //공격자 - Enemy
                            if (Managers.Unit.GetCurTargetUnit != null)
                                Managers.Unit.GetCurTargetUnit.ChangeStateMachine(UnitState.BattleAttack);

                            //스테미나 감소
                            Managers.Game.Calculate_Stemina(consumePercent: 0.3f, isPlus: false, action: null);
                        };
                    }

                    //운동 Good 판정 함수 세팅
                    if (Managers.Gesture.GoodGestureAction == null)
                    {
                        Managers.Gesture.GoodGestureAction = () =>
                        {
                            GestureReferees = GestureReferee.Good;

                            //공격자 - Enemy
                            if (Managers.Unit.GetCurTargetUnit != null)
                                Managers.Unit.GetCurTargetUnit.ChangeStateMachine(UnitState.BattleAttack);

                            //스테미나 감소
                            Managers.Game.Calculate_Stemina(consumePercent: 0.3f, isPlus: false, action: null);
                        };
                    }
                }

                //--------------------------------------------------------------
                //					Battle 끝났을때
                //--------------------------------------------------------------
                else if (CurTurnState == TurnState.EndBattleTurn)
                {
                    Managers.Unit.GetCurTurnUnit.ChangeStateMachine(UnitState.BattleEnd);
                    if (Managers.Unit.GetCurFollowNPCUnit != null)
                    {
                        Managers.Unit.GetCurFollowNPCUnit.ChangeStateMachine(UnitState.BattleEnd);
                    }
                    if (Managers.Unit.GetCurSubFollowNPCUnit != null)
                    {
                        Managers.Unit.GetCurSubFollowNPCUnit.ChangeStateMachine(UnitState.BattleEnd);
                    }
                }

            }//end of Battle

            else if (BattleMode == BattleManagerMode.Fitness && Managers.Gesture != null)
            {
                if (Managers.Gesture.TurnOverAction == null)
                {
                    //운동 TurnOver 판정 함수 세팅
                    Managers.Gesture.TurnOverAction = TurnOver;
                }

                //운동 Perfect 판정 함수 세팅
                if (Managers.Gesture.PerfectGestureAction == null)
                {
                    Managers.Gesture.PerfectGestureAction = () =>
                    {
                        GestureReferees = GestureReferee.Perfect;
                        //Happy2
                        //Dance2
                        //Cheerup2
                        //Cheerup3
                        //Player Emotion
                        //Player Emotion

                        Managers.Unit.GetCurTurnUnit.FitnessFeedbackEffect();

                        //스테미나 감소
                        Managers.Game.Calculate_Stemina(consumePercent: 0.1f, isPlus: false, action: null);
                    };
                }

                //운동 Good 판정 함수 세팅
                if (Managers.Gesture.GoodGestureAction == null)
                {
                    Managers.Gesture.GoodGestureAction = () =>
                    {
                        GestureReferees = GestureReferee.Good;


                        Managers.Unit.GetCurTurnUnit.FitnessFeedbackEffect();

                        //스테미나 감소
                        Managers.Game.Calculate_Stemina(consumePercent: 0.1f, isPlus: false, action: null);
                    };
                }

                //--------------------------------------------------------------
                //					피트니스 동작 따라하는거 구현
                //--------------------------------------------------------------
                // 트레이너 동작 따라하도록 구현
                if(Managers.Gesture.PreviewGestureAction == null)
                {
                    Managers.Gesture.PreviewGestureAction = () =>
                    {
                        CurGestureState = Managers.Gesture.pGetCurGesture.MyData.GestureEnum;
                        CurGestureAnimID = CompareAnimGesture(CurGestureState);

                        // 피트니스 수행자 - Trainer
                        Managers.Unit.GetCurTrainerUnit.ChangeStateMachine(UnitState.FitnessAction);
                    };
                }

                //--------------------------------------------------------------
                //					피트니스 끝났을때
                //--------------------------------------------------------------
                if (CurTurnState == TurnState.EndBattleTurn)
                {
                    Managers.Unit.GetCurTrainerUnit.ChangeStateMachine(UnitState.FitnessIdle);
                }

                //--------------------------------------------------------------
                //					피트니스 감정동작
                //--------------------------------------------------------------
                if (Managers.Gesture.Fitness_ReadyAction == null)
                {
                    Managers.Gesture.Fitness_ReadyAction = () =>
                    {
                        Managers.Unit.GetCurTrainerUnit.SetTriggerCompare(Managers.Unit.GetCurTrainerUnit.pAnimatorIDTable.AnimID_Emo_Fast);
                    };
                }
                if (Managers.Gesture.Fitness_KeepGoing == null)
                {
                    Managers.Gesture.Fitness_KeepGoing = () =>
                    {
                        Managers.Unit.GetCurTrainerUnit.SetTriggerCompare(Managers.Unit.GetCurTrainerUnit.pAnimatorIDTable.AnimID_Emo_Clapping2);
                    };
                }
                if (Managers.Gesture.Fitness_GoodCheerup == null)
                {
                    Managers.Gesture.Fitness_GoodCheerup = () =>
                    {
                        Managers.Unit.GetCurTrainerUnit.SetTriggerCompare(Managers.Unit.GetCurTrainerUnit.pAnimatorIDTable.AnimID_Emo_ThumbUp);
                    };
                }
                if (Managers.Gesture.Fitness_VeryGood == null)
                {
                    Managers.Gesture.Fitness_VeryGood = () =>
                    {
                        Managers.Unit.GetCurTrainerUnit.SetTriggerCompare(Managers.Unit.GetCurTrainerUnit.pAnimatorIDTable.AnimID_Emo_ThumbUp2);
                    };
                }
                if (Managers.Gesture.Fitness_NeverGiveup == null)
                {
                    Managers.Gesture.Fitness_NeverGiveup = () =>
                    {
                        Managers.Unit.GetCurTrainerUnit.SetTriggerCompare(Managers.Unit.GetCurTrainerUnit.pAnimatorIDTable.AnimID_Emo_HurryUp);
                    };
                }


            }//end of Fitness

            //--------------------------------------------------------------
            //					Exercise 일때
            //--------------------------------------------------------------
            else if (BattleMode == BattleManagerMode.Exercise && Managers.Gesture != null)
            {
                if (Managers.Gesture.TurnOverAction == null)
                {
                    //운동 TurnOver 판정 함수 세팅
                    Managers.Gesture.TurnOverAction = TurnOver;

                }


                //운동 Perfect 판정 함수 세팅
                if (Managers.Gesture.PerfectGestureAction == null)
                {
                    Managers.Gesture.PerfectGestureAction = () =>
                    {
                        GestureReferees = GestureReferee.Perfect;

                        //운동오브젝트 상태 변경 - perfect
                        if (Managers.Unit.GetCurTargetExerciseUnit != null)
                            Managers.Unit.GetCurTargetExerciseUnit.ChangeStateMachine(ExerciseState.Prepare);

                        //운동수행자 - Player
                        if (Managers.Unit.GetCurTurnUnit != null)
                            Managers.Unit.GetCurTurnUnit.ChangeStateMachine(UnitState.BattleExercise);

                        //스테미나 감소
                        Managers.Game.Calculate_Stemina(consumePercent: 0.5f, isPlus: false, action: null);
                    };
                }

                //운동 Good 판정 함수 세팅
                if (Managers.Gesture.GoodGestureAction == null)
                {
                    Managers.Gesture.GoodGestureAction = () =>
                    {
                        GestureReferees = GestureReferee.Good;

                        //운동수행자 - Player
                        if (Managers.Unit.GetCurTurnUnit != null)
                            Managers.Unit.GetCurTurnUnit.ChangeStateMachine(UnitState.BattleExercise);

                        //운동오브젝트 상태 변경 - Good
                        if (Managers.Unit.GetCurTargetExerciseUnit != null)
                            Managers.Unit.GetCurTargetExerciseUnit.ChangeStateMachine(ExerciseState.Prepare);

                        //스테미나 감소
                        Managers.Game.Calculate_Stemina(consumePercent: 0.5f, isPlus: false, action: null);
                    };
                }

                //--------------------------------------------------------------
                //					Exercise 끝났을때
                //--------------------------------------------------------------
                if (CurTurnState == TurnState.EndBattleTurn)
                {
                    Managers.Unit.GetCurTurnUnit.ChangeStateMachine(UnitState.BattleEnd);
                    if (Managers.Unit.GetCurFollowNPCUnit != null)
                    {
                        Managers.Unit.GetCurFollowNPCUnit.ChangeStateMachine(UnitState.BattleEnd);
                    }
                    if (Managers.Unit.GetCurSubFollowNPCUnit != null)
                    {
                        Managers.Unit.GetCurSubFollowNPCUnit.ChangeStateMachine(UnitState.BattleEnd);
                    }
                }

            }

            if (BattleMode != BattleManagerMode.Fitness)
            {
                if (IsFirstFitness == true)
                    IsFirstFitness = false;

            }
        }//update

        IEnumerator ITurnOver()
        {
            // 바로 끝나는게 이상하대서 1초걸음
            yield return new WaitForSeconds(1.0f);

            //칼로리 계산
            //Managers.Game.Calculate_Calorie();

            //현재 플레이어 공격턴일때
            if (CurTurnState == TurnState.PlayerTurn)
            {
                if (!noSkip)
                {
                    CurTurnState = TurnState.EnemyTurn;
                }
                else
                {
                    noSkip = false;
                }
                Managers.Unit.CurTurnUnitIndex++;
            }
            //현재 플레이어 회피턴일때
            else if (CurTurnState == TurnState.EnemyTurn)
            {
                if (!noSkip)
                {
                    CurTurnState = TurnState.PlayerTurn;
                }
                else
                {
                    noSkip = false;
                }
            }


            //공격성공 / 공격실패 Action 초기화
            Managers.Gesture.ResetGestureActions();

            /// 원래는 바로 아래 if문과 순서가 바뀌어 있었음(컷씬 때문에 순서 변경)
            // 만약 마지막 운동이 끝나고 다이얼로그 남아있다면
            if (BattleMode == BattleManagerMode.Exercise)
            {
                // 다이얼로그 끝날떄까지 기다린다
                yield return new WaitUntil(() => !Dev_Dialogue.DevDialogueManager.Controller.dialogueing);
            }

            if (BattleMode != BattleManagerMode.Fitness)
            {
                yield return new WaitForSeconds(1.5f);
                CutSceneController.BattleCutSceneEvent?.Invoke();

                yield return new WaitUntil(() => CutSceneController.PlayCutScene == false);
            }

            //전투 끝났는지 확인
            if (IsEndBattle)
            {
                //에너미 마지막 피격 연출
                if (Managers.Unit.GetCurTargetUnit != null)
                    Managers.Unit.GetCurTargetUnit.ChangeStateMachine(UnitState.BattleDead);

                //플레이어 전투종료 상태 변경
                CurTurnState = TurnState.EndBattleTurn;

                if (BattleMode != BattleManagerMode.Fitness)
                {
                    StartCoroutine(EndBattleEvent());

                }
                Managers.Gesture.Change_Gesture();

#if UNITY_EDITOR
                Debug.Log("<color=#FF00DD>[BattleManager] 전투 종료됨</color>");
#endif
                yield break;
            }

#if UNITY_EDITOR
            Debug.Log("<color=#1DDB16><b>턴오버 실행 !!</b></color>");
#endif
            yield return new WaitForSeconds(2.0f);

            //다음 운동슬롯으로 업데이트
            Managers.Gesture.Change_Gesture();

        }

        bool IsEndBattle
        {
            get
            {

                if (BattleMode == BattleManagerMode.Battle)
                {
                    //모든 운동슬롯을 소모했을때
                    if (Managers.Gesture.GestureClear)
                    {
                        return true;
                    }
                    // player진영이나 enemy진영중 한쪽이 다 죽으면 전투종료
                    return Managers.Unit.GetCurTurnUnit == null || Managers.Unit.GetCurTargetUnit == null ? true : false;
                }

                else if (BattleMode == BattleManagerMode.Exercise)
                {
                    return Managers.Gesture.GestureClear ? true : false;
                }

                else if (BattleMode == BattleManagerMode.Fitness)
                {

                    return Managers.Gesture.GestureClear ? true : false;
                }

                else
                {
                    Debug.LogError(string.Format("[BattleManager] ERROR 전투모드 설정오류 = <color=#FF0000><b>{0}</b></color>", BattleMode));
                    return true;
                }


            }
        }

        IEnumerator EndBattleEvent()
        {
            //승패판정
            승패판정();

            yield return new WaitForSeconds(1.0f);

            //승패판정에따른 이벤트호출
            BattleVictoryTrigger(
                 victoryAction: () =>
                 {
                     VictoryEvent.Invoke();

                 },
                 defeatAction: () =>
                 {
                     DefeatEvent.Invoke();

                 },
                 drawnAction: () =>
                 {
                     DrawnEvent.Invoke();

                 });

            //LidarGestureManager에 공격성공 / 공격실패 Action 초기화
            Managers.Gesture.ResetGestureActions();

            //전투종료 이벤트
            BattleMode = BattleManagerMode.None;
            BattleEndEvent.Invoke();

            yield return new WaitForSeconds(5.0f);

            //전투유닛 리셋
            Managers.Unit.ResetBattle();
        }

        void 승패판정()
        {
            if (Managers.Unit.GetCurTargetUnit != null)
            {
                UnitEnemy Enemy = Managers.Unit.GetCurTargetUnit.GetComponent<UnitEnemy>();
                if (Enemy != null)
                {
                    switch (Enemy.pEnemyResult)
                    {
                        //무승부 게임오버 - player와 enemy모두 살아남았지만 운동슬롯이 비었을때
                        case BattleResult.Drawn:
                            {
                               
                                CurBattleResult = BattleResult.Drawn;
                                return;
                            }
                        //플레이어 전투 승리
                        case BattleResult.Victory:
                            {
                        
                                CurBattleResult = BattleResult.Victory;
                                return;
                            }
                        //플레이어 전투 패배
                        case BattleResult.Defeat:
                            {
                        
                                CurBattleResult = BattleResult.Defeat;
                                return;
                            }
                    }
                }
            }
            else
            {
                //무승부 게임오버 - player와 enemy모두 살아남았지만 운동슬롯이 비었을때
                if (Managers.Gesture.GestureClear &&
                    Managers.Unit.GetCurTurnUnit != null && Managers.Unit.GetCurTargetUnit != null)
                {
               
                    CurBattleResult = BattleResult.Drawn;
                }

                //플레이어 전투 승리
                else if (Managers.Unit.GetCurTurnUnit != null && Managers.Unit.GetCurTargetUnit == null)
                {
               
                    CurBattleResult = BattleResult.Victory;
                }

                //플레이어 전투 패배
                else
                {
              
                    CurBattleResult = BattleResult.Defeat;
                }
            }
        }

        // 현재 Trainer만 사용하는중
        int CompareAnimGesture(Gesture gesture)
        {
            UnitTrainer CurTrainer = Managers.Unit.GetCurTrainerUnit;
            //플레이어 애니메이션 방향설정
            CurTrainer.pGetureDirection = !CurTrainer.pGetureDirection;
            CurTrainer.pAnimatorbase.SetBool(CurTrainer.pAnimatorIDTable.AnimID_Direction, CurTrainer.pGetureDirection);

            int animationHash = 0;
            string animationName = gesture.ToString();
            if(CurTrainer.pAnimatorbase.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                animationHash = Managers.Unit.GetCurTrainerUnit.pAnimatorIDTable.AnimID_Emo_ThumbUp;

#if UNITY_EDITOR
                Debug.LogFormat("[BattleManager] : 해당 {0}를 체크해주세여", gesture);
#endif
            }
            else
            {
                animationHash = Animator.StringToHash(gesture.ToString());
            }

            return animationHash;
        }

        void DoNotSkipTurn()
        {
            noSkip = true;
        }

    }//end of class					
}//end of namespace					