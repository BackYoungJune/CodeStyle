/*********************************************************					
* CutSceneTimelineController.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Dev_Unit;
using System;
using static DevDefine;
using Dev_System;
using System.Collections;

namespace Dev_CutScene			
{
    public class CutSceneTimelineController : MonoBehaviour
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void PlayCutSceneInspector(string key)
        {
            CutSceneTimeLineEnum enumKey = (CutSceneTimeLineEnum)System.Enum.Parse(typeof(CutSceneTimeLineEnum), key);
            if (System.Enum.IsDefined(typeof(CutSceneTimeLineEnum), enumKey) == false)
            {
                Debug.LogError("key != enum's data");
                return;
            }

            if(CutSceneTable == null)
            {
                CutSceneTimelineTable table = System.Array.Find(cutSceneTables, x => x.Language == Managers.Game.pCurLanguage);
                if (table != null)
                {
                    CutSceneTable = table;
                }
                else
                {
                    CutSceneTimelineTable table2 = System.Array.Find(cutSceneTables, x => x.Language ==  DevSystemLanguage.en_US);
                    if(table2 != null)
                    {
                        CutSceneTable = table2;
                    }
                }
            }

            var info = System.Array.Find(CutSceneTable.TimeLineInfos, x => x.enumKey == enumKey);
            if (info != null)
            {
                timelineTrigger.GetComponent<PlayableDirector>().playableAsset = info.playableAsset;

                for (int i = 0; i < info.InfoDetails.Length; i++)
                {
                    PDBind(timelineTrigger.GetComponent<PlayableDirector>(), info.InfoDetails[i].trackNames, ReturnAnimator(info.InfoDetails[i].animator));
                }
                timelineTrigger.OnUse();
            }
        } 

        Animator ReturnAnimator(CutSceneAnimator animator)
        {
            switch (animator)
            {
                //case CutSceneAnimator.Player: return Managers.Unit.GetCurTurnUnit.pAnimatorbase;
                case CutSceneAnimator.FollowNPC: return Managers.Unit.GetCurFollowNPCUnit.nAnimatorbase;
                case CutSceneAnimator.SubFollowNPC: return Managers.Unit.GetCurSubFollowNPCUnit.nAnimatorbase;
                case CutSceneAnimator.Enemy: return Managers.Unit.GetCurTargetUnit.pAnimatorbase;
                case CutSceneAnimator.Trainer: return Managers.Unit.GetCurTrainerUnit.pAnimatorbase;
            }
            return null;
        }

        public void PDBind(PlayableDirector director, string trackName, Animator animator)
        {
            var timeline = director.playableAsset as TimelineAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == trackName)
                {
                    director.SetGenericBinding(track, animator);
                }
            }
        }

        public void CutSceneSkip()
        {
            if (timelineTrigger.GetComponent<PlayableDirector>().state == PlayState.Playing)
            {
                timelineTrigger.GetComponent<PlayableDirector>().Stop();
                Dev_Dialogue.DevDialogueManager.Controller.StopConversation();
            }
        }

        //--------------------------------------------------------					
        // 배틀 턴오버 컷씬					
        //--------------------------------------------------------	


        // 배틀전에 셋팅함수
        public void SetBattleCutsceneTurnOver()
        {
            turnoverNum = 0;
            BattleCutSceneEvent = null;
            BattleCutSceneEvent += BattleTurnOver;
            battleCutSceneInfos = System.Array.Find(CutSceneTable.BattleCutSceneInfos, x => x.enumKey == Managers.Unit.GetCurTargetUnit.pUniqueID);
            if (battleCutSceneInfos != null)
            {
                battleCutSceneInfos.InitBattleCutSceneInfos();
            }
        }

        public void SetMissionCutsceneTurnOver()
        {
            turnoverNum = 0;
            BattleCutSceneEvent = null;
            battleCutSceneInfos = null;
            BattleCutSceneEvent += BattleTurnOver;
            foreach (var exercise in Managers.Unit.UnitExerciseList)
            {
                if (exercise == null) break;
                if (battleCutSceneInfos != null) break;

                UnitExercise curExercise = exercise.GetComponent<UnitExercise>();
                battleCutSceneInfos = System.Array.Find(CutSceneTable.BattleCutSceneInfos, x => x.enumKey == curExercise.pUniqueID);
            }
            //battleCutSceneInfos = System.Array.Find(CutSceneTable.BattleCutSceneInfos, x => x.enumKey == Managers.Unit.GetCurTargetExerciseUnit.pUniqueID);
            if (battleCutSceneInfos != null)
            {
                battleCutSceneInfos.InitBattleCutSceneInfos();
            }
        }

        public bool PlayCutScene { get; set; }

        public Action BattleCutSceneEvent;

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					

        [SerializeField] private CutSceneTimelineTable[] cutSceneTables;
        [SerializeField] private TimelineTrigger timelineTrigger;
        private BattleCutSceneInfos battleCutSceneInfos;
        private int turnoverNum = 0;
        private CutSceneTimelineTable CutSceneTable;

        void BattleTurnOver()
        {
            turnoverNum++;
            if (battleCutSceneInfos != null)
            {
                var num = System.Array.Find(battleCutSceneInfos.InfoDetails, x => x.CutSceneTurnOver == turnoverNum);
                if (num.CutSceneTurnOver == 0) return;
                PlayCutSceneInspector(battleCutSceneInfos.TurnOverDictionary[num.CutSceneTurnOver]);
                PlayCutScene = true;
            }
        }

    }//end of class				
}//end of namespace					