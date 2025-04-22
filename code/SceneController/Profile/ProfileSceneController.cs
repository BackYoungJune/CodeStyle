/*********************************************************					
* ProfileSceneController.cs					
* 작성자 : lbioh					
* 작성일 : 2024.08.07 오후 3:34					
**********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev_System;
using Dev_Unit;
using Dev_UI;
using Dev_Utils;
using Dev_VideoUtils;
using UnityEngine.Video;
using Dev_Occulus;
using Dev_SceneManagement;

namespace Dev_SceneControl
{
    public class ProfileSceneController : MonoBehaviour					
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					

        [Header("For Scene Settings")]
        [SerializeField] private Transform GenPos_Player;


        [Header("Video Controller")]
        [SerializeField] private UIMovementVR UIMovement_OpenningVideo;
        [SerializeField] private VideoController OpeningVideoController;
        [SerializeField] private VideoClip OpenningClip;





        IEnumerator Start()
        {
            yield return new WaitUntil(() => Managers.AllCreateManagers);
            yield return new WaitUntil(() => Managers.AllClearDLC);
            yield return new WaitUntil(() => Managers.AllClearDB);
            yield return new WaitUntil(() => Managers.Unit != null);


            Managers.UIManager.AllDisableUIModule();


            Managers.Sound.PlayBGM("10022");

            // Player 생성
            //Managers.Unit.CreateUnit(UnitUniqueID.Player_VR, UnitState.None, GenPos_Player, false, Vector3.one);

            //yield return new WaitForSeconds(1f);

            LogoPlay();

            yield return new WaitForSeconds(5f);
            UIMovement_OpenningVideo.gameObject.SetActive(false);

            yield return new WaitUntil(() => UIMovement_OpenningVideo.gameObject.activeSelf == false);

            if (Managers.UIManager.GetUIModule("SelectProfile") == null)
            {
                UIModule selectProfile = Managers.UIManager.EnableUIModule(
                      type: UISceneTypes.Common,
                      uiState: UIStates.FollowerVR,
                      uniqueID: "SelectProfile",
                      group: UIGroups.Group_1,
                      followSpeed: 10f,
                      followDistanceCam: 4f
                      );

                yield return new WaitUntil(() => selectProfile.gameObject.activeSelf == false);
            }

/*            LogoPlay();

            yield return new WaitForSeconds(2f);*/

            //로비로 씬변경
            string targetScene = string.Format("{0}_{1}", SceneStructureType.System.ToString(), SceneDefine_System.Lobby.ToString());
            Managers.LoadingHandler.LoadScene(targetScene, Color.black);
        }


        private void LogoPlay()
        {
            //UIMovement 활성화
            UIMovement_OpenningVideo.gameObject.SetActive(true);

            //오프닝 동영상 재생
            OpeningVideoController.OnPlayVideo(OpenningClip, useLoop: false); ;


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
        }

    }//end of class					
}//end of namespace					