/*********************************************************					
* Prefab_SoundSetting.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.08.23 오전 9:35					
**********************************************************/
using Dev_LocalDB;
using Dev_System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;
using static MoreMountains.Tools.MMSoundManager;
using static UnityEngine.Rendering.DebugUI;

namespace Dev_UI
{
    public class Prefab_SoundSetting : MonoBehaviour
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void Init()
        {
            // DB에서 셋팅값 가져오기
            InitSetting();

            // 색깔 초기화
            //Color newColor = new Color(Image_Fiil.color.r, Image_Fiil.color.g, Image_Fiil.color.b, 0f);
            //Image_Fiil.color = newColor;
            //Image_Handle.color = Color.white;

            IsChanging = false;
        }

        public void ChangeHandleImage(Sprite changeSprite)
        {
            //Image_Handle.sprite = changeSprite;
        }

        public void ChangeSoundImage(Sprite changeSprite)
        {
            Image_Sound.sprite = changeSprite;
        }

        public void OnClickSoundButton()
        {
            if (Image_Sound.sprite == SoundImage_Blue)
            {
                Image_Sound.sprite = SoundImage_Mute;
                MMSoundManager.Instance.settingsSo.SetTrackVolume(Type, 0);
            }
            else
            {
                Image_Sound.sprite = SoundImage_Blue;
                MMSoundManager.Instance.settingsSo.SetTrackVolume(Type, SoundSlider.value);
            }
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private MMSoundManagerTracks Type;
        [SerializeField] private Slider SoundSlider;

        [SerializeField] private MMF_Player Feedback_Click;
        [SerializeField] private MMF_Player Feedback_Exit;

        //[SerializeField] private Image Image_Handle;
        [SerializeField] private Image Image_Sound;
        //[SerializeField] private Image Image_Fiil;

        //[SerializeField] private Sprite SoundImage_Normal;
        [SerializeField] private Sprite SoundImage_Blue;
        [SerializeField] private Sprite SoundImage_Mute;

        private string hoverColor = "2C9EFF";
        private bool IsChanging = false;
        private string sfxSound = "10077";
        private string characterSound_kr = "1";
        private string characterSound_en = "2";

        void Update()
        {
            if (IsChanging == false) return;
            if ((OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))))
            {
                IsChanging = false;
                Invoke("InvokeExit", 0.2f);
            }
        }

        void InvokeExit()
        {
            Exit();
        }

        void InitSetting()
        {
            //Color newColor = new Color(Image_Fiil.color.r, Image_Fiil.color.g, Image_Fiil.color.b, 255.0f);
            //Image_Fiil.color = newColor;
            //Color newColor2 = new Color(Image_Handle.color.r, Image_Handle.color.g, Image_Handle.color.b, 255.0f);
            //Image_Handle.color = newColor2;
            IsChanging = false;

            // DB에서 값을 가져와 셋팅한다
            switch(Type)
            {
                case MMSoundManagerTracks.Master:
                    {
                        InitDBSetting(Managers.LocalDB.pDBForm_AppSystem.pMaster_Volume, Managers.LocalDB.pDBForm_AppSystem.pMaster_Mute);
                        break;
                    }
                case MMSoundManagerTracks.Music:
                    {
                        InitDBSetting(Managers.LocalDB.pDBForm_AppSystem.pBGM_Volume, Managers.LocalDB.pDBForm_AppSystem.pBGM_Mute);
                        break;
                    }
                case MMSoundManagerTracks.Sfx:
                    {
                        InitDBSetting(Managers.LocalDB.pDBForm_AppSystem.pSFX_Volume, Managers.LocalDB.pDBForm_AppSystem.pSFX_Mute);
                        break;
                    }
                case MMSoundManagerTracks.UI:
                    {
                        InitDBSetting(Managers.LocalDB.pDBForm_AppSystem.pCharacter_Volume, Managers.LocalDB.pDBForm_AppSystem.pCharacter_Mute);
                        break;
                    }
            }
        }

        // DB에서 volume값, mute 여부 받아서 처리
        void InitDBSetting(float volume, int mute)
        {
            SoundSlider.value = volume;
            if(mute == 0)
            {
                Image_Sound.sprite = SoundImage_Mute;
            }
            else
            {
                Image_Sound.sprite = SoundImage_Blue;
            }
        }

        void Click()
        {
            //Color newColor = new Color(Image_Fiil.color.r, Image_Fiil.color.g, Image_Fiil.color.b, 255.0f);
            //Image_Fiil.color = newColor;
            //Color newColor2 = DevUtils.HexToColor(hoverColor);
            //Image_Handle.color = newColor2;
        }

        void Exit()
        {
            //Color newColor = new Color(Image_Fiil.color.r, Image_Fiil.color.g, Image_Fiil.color.b, 0f);
            //Image_Fiil.color = newColor;
            //Image_Handle.color = Color.white;
            if (Image_Sound.sprite != SoundImage_Mute)
            {
                Image_Sound.sprite = SoundImage_Blue;
            }
            Managers.Sound.StopSFX();
            Managers.Sound.StopCharacter();
        }

        // 슬라이더 값이 변경될 때 호출될 메서드
        public void OnSliderValueChanged()
        {
            if (this.gameObject.activeInHierarchy == false) return;
            Click();

            float value = SoundSlider.value;
            MMSoundManager.Instance.settingsSo.SetTrackVolume(Type, value);

            if (value <= 0)
            {
                Image_Sound.sprite = SoundImage_Mute;
            }
            else
            {
                Image_Sound.sprite = SoundImage_Blue;
            }
            IsChanging = true;

            // DB에 Update 한다
            switch (Type)
            {
                case MMSoundManagerTracks.Master:
                    {
                        break;
                    }
                case MMSoundManagerTracks.Music:
                    {
                        break;
                    }
                case MMSoundManagerTracks.Sfx:
                    {
                        // 만약 소리가 플레이중이면 중복 방지를 위해 return 한다
                        if (Managers.Sound.GetSFX.IsPlaying) return;
                        Managers.Sound.StopSFX();
                        Managers.Sound.PlaySFX(sfxSound);
                        break;
                    }
                case MMSoundManagerTracks.UI:
                    {
                        if (Managers.Sound.GetCharacter.IsPlaying) return;
                        Managers.Sound.StopCharacter();
                        Managers.Game.LanguageModeTrigger(korea: () =>
                        {
                            Managers.Sound.PlayCharacter(characterSound_kr);
                        },
                        english: () =>
                        {
                            Managers.Sound.PlayCharacter(characterSound_en);
                        });
                        break;
                    }
            }
        }

        void OnDisable()
        {
            SoundSlider.onValueChanged.RemoveAllListeners();

            // DB에 Update 한다
            switch (Type)
            {
                case MMSoundManagerTracks.Master:
                    {
                        UpdateDB(FieldType.MASTER_VOLUME, FieldType.MASTER_MUTE);
                        break;
                    }
                case MMSoundManagerTracks.Music:
                    {
                        UpdateDB(FieldType.BGM_VOLUME, FieldType.BGM_MUTE);
                        break;
                    }
                case MMSoundManagerTracks.Sfx:
                    {
                        UpdateDB(FieldType.SFX_VOLUME, FieldType.SFX_MUTE);
                        break;
                    }
                case MMSoundManagerTracks.UI:
                    {
                        UpdateDB(FieldType.CHARACTER_VOLUME, FieldType.CHARACTER_MUTE);
                        break;
                    }
            }
        }

        void UpdateDB(FieldType soundField, FieldType muteField)
        {
            Managers.LocalDB.DataBaseInser(QueryType.UPDATE, LocalDBTables.AppSystem, soundField, SoundSlider.value);
            if (SoundSlider.value <= 0 || Image_Sound.sprite == SoundImage_Mute)
            {
                Managers.LocalDB.DataBaseInser(QueryType.UPDATE, LocalDBTables.AppSystem, muteField, 0);
            }
            else
            {
                Managers.LocalDB.DataBaseInser(QueryType.UPDATE, LocalDBTables.AppSystem, muteField, 1);
            }
        }

    }//end of class					
}//end of namespace					