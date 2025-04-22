/*********************************************************					
* Setting_User.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.08.23 오후 5:44					
**********************************************************/
using Dev_Network;
using Dev_SceneManagement;
using Dev_System;
using MoreMountains.Feedbacks;
using System.Collections;					
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace Dev_UI					
{					
	public class Setting_User : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------	
        public void Init()
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (var trans in hoverTransList)
            {
                if (trans == null) continue;
                trans.gameObject.SetActive(false);
            }
            hoverTransList.Clear();

            foreach (var data in eventData.hovered)
            {
                if (data == null) continue;
                Button hoverButton = data.GetComponent<Button>();
                if (hoverButton != null)
                {
                    Transform hoverTrans = hoverButton.transform.Find("Hover_Image");
                    if(hoverTrans != null)
                    {
                        hoverTrans.gameObject.SetActive(true);
                        if (hoverTransList.Contains(hoverTrans) == false)
                        {
                            hoverTransList.Add(hoverTrans);
                            ButtonHover(hoverButton.name);
                        }
                    }
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (var trans in hoverTransList)
            {
                if (trans == null) continue;
                trans.gameObject.SetActive(false);
            }
            hoverTransList.Clear();

            foreach (var data in eventData.hovered)
            {
                if (data == null) continue;
                Button hoverButton = data.GetComponent<Button>();
                if (hoverButton != null)
                {
                    Transform hoverTrans = hoverButton.transform.Find("Hover_Image");
                    if(hoverTrans != null)
                    {
                        hoverTrans.gameObject.SetActive(false);
                    }
                    //ButtonExit(hoverButton.name);
                }
            }
        }

        public void OnClick_Profile()
        {
            Managers.Sound.PlaySFX(clickSfx);
            Managers.Game.pConfirmMessage.SetMessage("Canvas_ConfirmMessage_ReturnToProfile",
                confirmAction: () =>
                {
                    //씬변경
                    string targetScene = string.Format("{0}_{1}", SceneStructureType.System.ToString(), SceneDefine_System.Profile.ToString());
                    Managers.LoadingHandler.LoadScene(targetScene, Color.black);
                });
        }

        public void OnClick_Lobby()
        {
            Managers.Sound.PlaySFX(clickSfx);
            Managers.Game.pConfirmMessage.SetMessage("Canvas_ConfirmMessage_ReturnToLobby",
                confirmAction: () =>
                {
                    //씬변경
                    string targetScene = string.Format("{0}_{1}", SceneStructureType.System.ToString(), SceneDefine_System.Lobby.ToString());
                    Managers.LoadingHandler.LoadScene(targetScene, Color.black);
                });
        }

        public void OnClick_Exit()
        {
            Managers.Sound.PlaySFX(clickSfx);
            Managers.Game.pConfirmMessage.SetMessage("Canvas_ConfirmMessage_ExitGame",
                confirmAction: () =>
                {
#if UNITY_EDITOR
                    // Unity 에디터에서 실행 중인 경우
                    UnityEditor.EditorApplication.isPlaying = false;
#else
            // 다른 모든 플랫폼에서 애플리케이션 종료
            Application.Quit();
#endif
                });
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private Image PlayerImage;
        [SerializeField] private TextMeshProUGUI NicknameText;
        [SerializeField] private MMF_Player Feedback_NicknameHover;
        [SerializeField] private GameObject LobbyButton_Obj;
        [SerializeField] private Image ProfileImage;
        [SerializeField] private Sprite[] ProfileSprites;

        List<Transform> hoverTransList = new List<Transform>();

        private string clickSfx = "10076";

        void OnEnable()
        {
            // DB Nicname 연결
            string name = Managers.Game.IsLogined ? Managers.Game.pCurUserData.DATA.NickName : "Guest";
            name = ShortenString(name);
            NicknameText.text = name;

            // 이미지 DB or 서버 받아서 처리 - 우선 고정
            CheckSystemScene();

            // 프로필 이미지를 바꿔준다
            if(Managers.Game.IsLogined)
            {
                int prpfileNum = Managers.LocalDB.pDBForm_UserInfo.GetCurUserIndex();
                if(prpfileNum < ProfileSprites.Length)
                {
                    ProfileImage.sprite = ProfileSprites[prpfileNum];
                }
            }
        }

        void CheckSystemScene()
        {
            StageRef curScene = Managers.LoadingHandler.GetNextTargetStageRef();
            if(curScene != null)
            {
                if(curScene.pSystemDefine() == SceneDefine_System.Profile || curScene.pSystemDefine() == SceneDefine_System.Lobby)
                {
                    LobbyButton_Obj.SetActive(false);
                }
                else
                {
                    LobbyButton_Obj.SetActive(true);
                }
            }
            else
            {
                LobbyButton_Obj.SetActive(true);
            }
        }

        // 7자 이상이면 뒤에 ...만 붙인다
        string ShortenString(string name, int maxLength = 6)
        {
            if (name.Length > maxLength)
            {
                return name.Substring(0, maxLength) + "...";
            }
            else
            {
                return name;
            }
        }

        void ButtonHover(string objName)
        {
            switch(objName)
            {
                case "NickName_Button":
                    {
                        //Managers.Game.pSystemMessage.SetMessage("Canvas_SystemMessage_ProfileHover", setBlock: false);
                        Feedback_NicknameHover?.PlayFeedbacks();
                        break;
                    }
                case "Lobby_Button":
                    {

                        break;
                    }
                case "Exit_Button":
                    {

                        break;
                    }
            }
        }

        void ButtonExit(string objName)
        {
            switch (objName)
            {
                case "NickName_Button":
                    {

                        break;
                    }
                case "Lobby_Button":
                    {

                        break;
                    }
                case "Exit_Button":
                    {

                        break;
                    }
            }
        }


    }//end of class					
}//end of namespace					