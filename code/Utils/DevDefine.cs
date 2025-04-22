/*********************************************************					
* DevDefine.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;

public static class DevDefine
{
    public enum ELayer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        PostProcessing = 3,
        Water = 4,
        UI = 5,
        CharLayer = 6,
        LevelLayer = 7,
    }
    
    public enum ETag
    {
        Untagged,
        Respawn,
        Finish,
        EditorOnly,
        MainCamera,
        Player,
        GameController,
        FxTemporaire,
        UIElement,
        UICanvas,
        Weapon,
        Enemy,
    }

    public enum CutSceneTimeLineEnum
    {
        None,

        //------------------------------
        //   어드벤쳐 공용 컷씬
        //------------------------------
        StoryTelling,
        IntroAdventure,
        EnterAdventure,
        InMission,
        InMissionMotionEnd,
        InMissionEnd,
        InStage2Enter,
        InStage2MotionEnd,
        InStage2End,
        InStage3Enter,
        InStage3MotionEnd,
        InStage3End,

        FirstDirection,
        SecondDirection,
        ThirdDirection,
        FourthDirection,


    }

    public enum CutSceneAnimator
    {
        Player,
        FollowNPC,
        SubFollowNPC,
        Enemy,
        Trainer,
    }

    public static int[] TierLevelTable = 
    {
        419,
        1354,
        3101,
        6346,
        10000, // 임시로 마지막 하나 넣음
    };

    public static class SortingLayers
    {
        public const int Message_Depth = 600;
        public const int Ultra_Depth = 500;
        public const int VeryHigh_Depth = 500;
        public const int High_Depth = 300;
        public const int Middle_Depth = 200;
        public const int Small_Depth = 100;
    }

    public const float fadeOutTime = 0.5f;

}//end of class		
			