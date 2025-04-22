/*********************************************************					
* Setting_Control.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.08.23 오후 4:23					
**********************************************************/
using Dev_System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Dev_UI
{
    public class Setting_Control : MonoBehaviour
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void Init()
        {

        }

        public void OnClickButton()
        {
            Managers.Sound.PlaySFX(clickSfx);
            isWaitForInput = true;
            // 텍스트에 해당 문구 셋팅
            Locale currentLanguage = LocalizationSettings.SelectedLocale;
            SelectText.text = LocalizationSettings.StringDatabase.GetLocalizedString("SystemMessage", ButtonStr_key, currentLanguage);
            HoverImage.SetActive(true);
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private Button ContolButton;
        [SerializeField] private TextMeshProUGUI SelectText;
        [SerializeField] private GameObject HoverImage;

        private string ButtonStr_key = "Canvas_ControlSetting_Button";

        private bool isWaitForInput = false;
        private string curKey = "B";
        private string clickSfx = "10076";

        void OnEnable()
        {
            // DB에서 키 받아오기
            SelectText.text = Managers.LocalDB.pDBForm_AppSystem.pControl;
            curKey = Managers.LocalDB.pDBForm_AppSystem.pControl;
            HoverImage.SetActive(false);
        }

        void Update()
        {
            if(isWaitForInput)
            {
                if(CheckButtonInput())
                {
                    ExitChangeMode();
                }
            }
        }

        bool CheckButtonInput()
        {
            // OVR Input으로 모든 주요 버튼 입력을 감지합니다.
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                curKey = "A";
                SelectText.text = "A";
                Managers.Game.pButtonKey = OVRInput.Button.One;
                return true;
            }
            else if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                curKey = "B";
                SelectText.text = "B";
                Managers.Game.pButtonKey = OVRInput.Button.Two;
                return true;
            }
            else if (OVRInput.GetDown(OVRInput.Button.Three))
            {
                curKey = "X";
                SelectText.text = "X";
                Managers.Game.pButtonKey = OVRInput.Button.Three;
                return true;
            }
            else if (OVRInput.GetDown(OVRInput.Button.Four))
            {
                curKey = "Y";
                SelectText.text = "Y";
                Managers.Game.pButtonKey = OVRInput.Button.Four;
                return true;
            }

            return false;
        }

        void ExitChangeMode()
        {
            isWaitForInput = false;
            HoverImage.SetActive(false);
        }

        void OnDisable()
        {
            // DB Key 업데이트
            Managers.LocalDB.DataBaseInser(Dev_LocalDB.QueryType.UPDATE, Dev_LocalDB.LocalDBTables.AppSystem, Dev_LocalDB.FieldType.CONTROL, curKey);
            string controlKey = Managers.LocalDB.pDBForm_AppSystem.pControl;
            switch (controlKey)
            {
                case "A":
                    {
                        Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.SettingUIActive(false, controlKey);
                        break;
                    }
                case "B":
                    {
                        Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.SettingUIActive(false, controlKey);
                        break;
                    }
                case "X":
                    {
                        Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.SettingUIActive(true, controlKey);
                        break;
                    }
                case "Y":
                    {
                        Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.SettingUIActive(true, controlKey);
                        break;
                    }
                default:
                    {
                        Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.SettingUIActive(true, controlKey);
                        break;
                    }
            }
            
        }

    }//end of class					
}//end of namespace					