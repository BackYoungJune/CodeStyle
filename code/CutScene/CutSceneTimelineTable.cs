/*********************************************************					
* CutSceneTimelineTable.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_System;
using Dev_Unit;
using System;
using System.Collections.Generic;					
using UnityEngine;
using UnityEngine.Playables;
using static DevDefine;
					
namespace Dev_CutScene
{
    [Serializable]
    public struct TimelineInfoDetail
    {
        public string discription;
        public string trackNames;
        public CutSceneAnimator animator;
    }

    [Serializable]
    public class TimelineInfos
    {
        public string discription;
        public CutSceneTimeLineEnum enumKey;
        public PlayableAsset playableAsset;
        public TimelineInfoDetail[] InfoDetails;
    }

    [Serializable]
    public struct BattleCutSceneInfoDetail
    {
        public string discription;
        public int CutSceneTurnOver;
        public string TurnOver_TrackNames;
    }

    [Serializable]
    public class BattleCutSceneInfos
    {
        public string discription;
        public UnitUniqueID enumKey;
        public BattleCutSceneInfoDetail[] InfoDetails;

        [HideInInspector] public Dictionary<int, string> TurnOverDictionary = new Dictionary<int, string>();

        public void InitBattleCutSceneInfos()
        {
            TurnOverDictionary.Clear();
            for (int i = 0; i < InfoDetails.Length; i++)
            {
                TurnOverDictionary.Add(InfoDetails[i].CutSceneTurnOver, InfoDetails[i].TurnOver_TrackNames);
            }
        }
    }

    [CreateAssetMenu(fileName = "Timeline_", menuName = "#ScriptableObject/Timeline")]
    public class CutSceneTimelineTable : ScriptableObject
    {
        public Dev_System.DevSystemLanguage Language;
        public TimelineInfos[] TimeLineInfos;
        public BattleCutSceneInfos[] BattleCutSceneInfos;

    }//end of class					
}//end of namespace					