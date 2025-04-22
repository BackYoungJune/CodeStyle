/*********************************************************					
* Setting_Graphic.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.08.23 오후 1:19					
**********************************************************/
using Dev_System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dev_UI
{
    public class Setting_Graphic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void Init()
        {
            // Text Language 적용
            Managers.Game.LanguageModeTrigger(
                            korea: () =>
                            {
                                for (int i = 0; i < GraphicDropDown.options.Count; i++)
                                {
                                    GraphicDropDown.options[i].text = dropDownStr_ko[i];
                                }
                            },
                            english: () =>
                            {
                                for (int i = 0; i < GraphicDropDown.options.Count; i++)
                                {
                                    GraphicDropDown.options[i].text = dropDownStr_en[i];
                                }
                            });

            // DB에서 현재 테마를 받아서 적용
            MapTheme thema = DevUtils.StringToEnum<MapTheme>(Managers.LocalDB.pDBForm_AppSystem.pMap_Theme);
            GraphicDropDown.SetValueWithoutNotify((int)thema);
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
                    if (hoverTrans != null)
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

                        if (hoverImageList.Contains(dropdownImage) == false)
                        {
                            hoverImageList.Add(dropdownImage);
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
                    if (hoverTrans != null)
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
        [SerializeField] private TMP_Dropdown GraphicDropDown;

        private string[] dropDownStr_ko = { "티온 월드", "일본", };
        private string[] dropDownStr_en = { "T-ON World", "Japan" };

        private string hoverColor = "2C9EFF";
        private string clickSfx = "10076";

        List<Transform> hoverTransList = new List<Transform>();
        List<Image> hoverImageList = new List<Image>();

        void OnEnable()
        {
            GraphicDropDown.onValueChanged.AddListener(OnGraphicDropDownChanged);
        }

        public void OnGraphicDropDownChanged(int index)
        {
            Managers.Sound.PlaySFX(clickSfx);
            switch (index)
            {
                // 티온월드
                case 0:
                    {
                        Managers.Game.pCurMapTema = MapTheme.TionWorld;
                        Managers.Game.OnMapTemaChanged?.Invoke(Managers.Game.pCurMapTema);
                        break;
                    }
                // 일본
                case 1:
                    {
                        if (Managers.DLCManagers.pSkuDic[Dev_DLC.DLCSkuID.unlock_japan].Purchase)
                        {
                            Managers.Game.pCurMapTema = MapTheme.Japen;
                            Managers.Game.OnMapTemaChanged?.Invoke(Managers.Game.pCurMapTema);
                        }
                        else
                        {
                            GraphicDropDown.SetValueWithoutNotify(0);
                            Managers.Game.pSystemMessage.SetMessage("Canvas_SystemMessage_GraphicJapan");
                        }
                        break;
                    }
            }
        }

        void OnDisable()
        {
            GraphicDropDown.onValueChanged.RemoveAllListeners();

            MapTheme curTheme = (MapTheme)GraphicDropDown.value;
            Managers.LocalDB.DataBaseInser(Dev_LocalDB.QueryType.UPDATE, Dev_LocalDB.LocalDBTables.AppSystem, Dev_LocalDB.FieldType.MAP_THEME, curTheme.ToString());
        }


    }//end of class					
}//end of namespace					