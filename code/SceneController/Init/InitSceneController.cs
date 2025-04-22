/*********************************************************					
* InitSceneController.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev_System;
using Dev_Unit;
using Dev_UI;
using Dev_Occulus;
using Dev_Utils;
using Dev_VideoUtils;
using MoreMountains.Feedbacks;
using Dev_SceneManagement;


namespace Dev_SceneControl					
{					
	public class InitSceneController : MonoBehaviour					
	{

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------		





        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------				
        [Header("For Scene Settings")]	
        [SerializeField] private Transform          GenPos_Player;
        [SerializeField] private UIMovementVR       UIMovement_OpenningVideo;
        [SerializeField] private VideoController    OpnningVideoController;
        [SerializeField] private MMF_Player         Feedback_OpenningUI_Open;


        void Awake()
        {     
            //UIMovement_OpenningVideo.gameObject.transform.localScale = Vector3.zero;
            //UIMovement_OpenningVideo.gameObject.SetActive(false);
        }


        void Start()
        {
            //StartCoroutine(SceneProcess());

            StartCoroutine(SimpleSceneProcess());

        }


        void OnDisable()
        {

            //OpnningVideoController.OnStop();
            //UIMovement_OpenningVideo.DisableUI(isInstanceNull: true);
        }

        IEnumerator SimpleSceneProcess()
        {
            yield return new WaitUntil(() => Managers.AllCreateManagers);
            yield return new WaitUntil(() => Managers.Unit != null);

            // Player 생성
            Managers.Unit.CreateUnit(UnitUniqueID.Player_VR, UnitState.None, GenPos_Player, false, Vector3.one);

            yield return new WaitUntil(() => Managers.Unit.GetCurTurnUnit != null);

            //씬변경
            string targetScene = string.Format("{0}_{1}", SceneStructureType.System.ToString(), SceneDefine_System.Profile.ToString());
            Managers.LoadingHandler.LoadScene(targetScene, Color.black);

        }



        IEnumerator SceneProcess()
        {
            yield return new WaitUntil(() => Managers.AllCreateManagers);
            yield return new WaitUntil(() => Managers.AllClearDLC);
            yield return new WaitUntil(() => Managers.AllClearDB);
            yield return new WaitUntil(() => Managers.Unit != null);

            // Player 생성
            Managers.Unit.CreateUnit(UnitUniqueID.Player_VR, UnitState.None, GenPos_Player, false, Vector3.one);

            yield return new WaitForSeconds(1f);

            //RayColor 변경
            Color hoverColor = DevUtils.HexToColor("#36A3FE");
            Color selectColor = DevUtils.HexToColor("#FF922D", 50f);
            CameraRigMaster.Instance.SetControllerRayColor_Hover(hoverColor);
            CameraRigMaster.Instance.SetControllerRayColor_Select(selectColor);

            //UIMovement 활성화
            UIMovement_OpenningVideo.gameObject.SetActive(true);

            //오프닝 동영상 재생
            VideoDataStructure videoStructure = Managers.VideoManager.GetVideoDataStructure(VideoRefType.Common);
            OpnningVideoController.OnPlayVideo(videoStructure.GetVideoClip(VideoCategory_Common.Openning.ToString()), useLoop: false); ;

            //오프닝영상패널 추적 초기화
            UIMovementInfos uiMovement = new();
            uiMovement.Initialize(
             useLerp: true,
             targetUI: UIMovement_OpenningVideo.transform,
             targetCam: CameraRigMaster.Instance.HeadAnchorPos,
             followSpeed: 5f,
             distanceFromCam: 2.7f,
             correctionValue: Vector3.zero
             );

            //오프닝영상패널 추적 시작
            UIMovement_OpenningVideo.EnableUI(uiMovement);

            //Feedback open
            Feedback_OpenningUI_Open?.PlayFeedbacks();
        }





    }//end of class					


}//end of namespace					