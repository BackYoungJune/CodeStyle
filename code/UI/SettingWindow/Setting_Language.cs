/*********************************************************					
* Setting_Language.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.08.22 오전 10:00					
**********************************************************/
using Dev_System;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Dev_LocalDB;
using System;

namespace Dev_UI					
{					
	public class Setting_Language : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void Init()
        {
            Managers.Game.LanguageModeTrigger(
                korea: () =>
                {
                    for (int i = 0; i < LanguageDropDown.options.Count; i++)
                    {
                        LanguageDropDown.options[i].text = dropDownStr_ko[i];
                    }

                },
                english: () =>
                {
                    for (int i = 0; i < LanguageDropDown.options.Count; i++)
                    {
                        LanguageDropDown.options[i].text = dropDownStr_en[i];
                    }
                });

            // LocalDB에서 초기 셋팅값 가져온다
            Dev_System.DevSystemLanguage language = DevUtils.StringToEnum<Dev_System.DevSystemLanguage>(Managers.LocalDB.pDBForm_AppSystem.pSystem_Language);
            LanguageDropDown.SetValueWithoutNotify((int)language);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (var trans in hoverTransList)
            {
                if (trans == null) continue;
                trans.gameObject.SetActive(false);
            }

            foreach (var image in hoverImageList)
            {
                if (image == null) continue;
                image.color = Color.white;
            }
            hoverTransList.Clear();
            hoverImageList.Clear();

            foreach (var data in eventData.hovered)
            {
                if (data == null) continue;
                Toggle hoverToggle = data.GetComponent<Toggle>();
                if (hoverToggle != null)
                {
                    Transform hoverTrans = hoverToggle.transform.Find("Hover_Image");
                    if(hoverTrans != null)
                    {
                        hoverTrans.gameObject.SetActive(true);
                        if (hoverTransList.Contains(hoverTrans) == false)
                        {
                            hoverTransList.Add(hoverTrans);
                        }
                    }
                }

                TMP_Dropdown hoverDropdown = data.GetComponent<TMP_Dropdown>();
                if (hoverDropdown != null)
                {
                    Image dropdownImage = hoverDropdown.GetComponent<Image>();
                    if (dropdownImage != null)
                    {
                        Color newColor = DevUtils.HexToColor(hoverColor);
                        dropdownImage.color = newColor;

                        if(hoverImageList.Contains(dropdownImage) == false)
                        {
                            hoverImageList.Add(dropdownImage);
                        }
                    }
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach(var trans in hoverTransList)
            {
                if(trans == null) continue;
                trans.gameObject.SetActive(false);
            }

            foreach(var image in hoverImageList)
            {
                if(image == null) continue;
                image.color = Color.white;
            }
            hoverTransList.Clear();
            hoverImageList.Clear();

            foreach (var data in eventData.hovered)
            {
                if (data == null) continue;
                Toggle hoverToggle = data.GetComponent<Toggle>();
                if (hoverToggle != null)
                {
                    Transform hoverTrans = hoverToggle.transform.Find("Hover_Image");
                    if(hoverTrans != null)
                    {
                        hoverTrans.gameObject.SetActive(false);
                    }
                }

                TMP_Dropdown hoverDropdown = data.GetComponent<TMP_Dropdown>();
                if (hoverDropdown != null)
                {
                    Image dropdownImage = hoverDropdown.GetComponent<Image>();
                    if (dropdownImage != null)
                    {
                        Color newColor = Color.white;
                        dropdownImage.color = newColor;
                    }
                }
            }
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private TMP_Dropdown LanguageDropDown;
        
        private string[] dropDownStr_ko = { "시스템 언어", "English", "한국어" };
        private string[] dropDownStr_en = { "System", "English", "한국어" };

        private string hoverColor = "2C9EFF";
        private string clickSfx = "10076";

        List<Transform> hoverTransList = new List<Transform>();
        List<Image> hoverImageList = new List<Image>();


        void OnEnable()
        {
            LanguageDropDown.onValueChanged.AddListener(OnLanguageDropDownChanged);
        }

        void OnLanguageDropDownChanged(int index)
        {
            Managers.Sound.PlaySFX(clickSfx);
            Managers.Game.pSystemMessage.SetMessage("Canvas_SystemMessage_Language");
        }

        void OnDisable()
        {
            // DB에 바뀐 값을 재셋팅해준다
            DevSystemLanguage curLanguage = (DevSystemLanguage)LanguageDropDown.value;
            Managers.LocalDB.DataBaseInser(QueryType.UPDATE, LocalDBTables.AppSystem, FieldType.SYSTEM_LANGUAGE, curLanguage.ToString());

            LanguageDropDown.onValueChanged.RemoveAllListeners();
        }

    }//end of class					
}//end of namespace					