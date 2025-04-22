/*********************************************************					
* SoundManager.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_System			
{					
	public enum SoundType
	{
		None,
		BGM,
		SFX,
		Voice,
	}

	public class SoundManager : MonoBehaviour					
	{					
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		public void PlayBGM(string _key)
		{
			if(BGMCor != null)
			{
				StopBGM();
            }
            BGMCor = StartCoroutine(PlayBGMCor(_key));
		}

		IEnumerator PlayBGMCor(string _key)
		{
            MMF_MMSoundManagerSound mmsound = Feedback_BGM.GetFeedbackOfType<MMF_MMSoundManagerSound>();
            if (mmsound != null)
			{
                StopBGM();

                yield return null;
                yield return new WaitUntil(() => Feedback_StopBGM.IsPlaying == false);

				var data =  Array.Find(Database_BGM.pDatas, x => x.key == _key);
				if(data != null)
				{
					mmsound.Sfx = data.clip;
					Feedback_BGM.PlayFeedbacks();
                }
				else
				{
					Debug.LogError("[SoundManager] : BGM 해당 key의 AudioClip이 없음");
				}
            }
			BGMCor = null;
        }

        public void StopBGM()
		{
			Feedback_StopBGM?.PlayFeedbacks();
			if(BGMCor != null)
			{
                StopCoroutine(BGMCor);
                BGMCor = null;
            }
        }

		public void StopStoneBGM()
		{
			Feedback_StopStoneBGM?.PlayFeedbacks();
        }

		public void PlaySFX(string _key)
		{
            MMF_MMSoundManagerSound mmsound = Feedback_SFX.GetFeedbackOfType<MMF_MMSoundManagerSound>();
			if(mmsound != null)
			{
                var data = Array.Find(Database_SFX.pDatas, x => x.key == _key);
                if (data != null)
                {
                    mmsound.Sfx = data.clip;
                    Feedback_SFX.PlayFeedbacks();
                }
                else
                {
                    Debug.LogError("[SoundManager] : SFX 해당 key의 AudioClip이 없음");
                }
            }
        }

		public void StopSFX()
		{
			Feedback_StopSFX?.PlayFeedbacks();
        }

        public void PlayCharacter(string _key)
        {
            MMF_MMSoundManagerSound mmsound = Feedback_Character.GetFeedbackOfType<MMF_MMSoundManagerSound>();
            if (mmsound != null)
            {
                var data = Array.Find(Database_Character.pDatas, x => x.key == _key);
                if (data != null)
                {
                    mmsound.Sfx = data.clip;
                    Feedback_Character.PlayFeedbacks();
                }
                else
                {
                    Debug.LogError("[SoundManager] : Character 해당 key의 AudioClip이 없음");
                }
            }
        }

        public void StopCharacter()
        {
            Feedback_StopCharacter?.PlayFeedbacks();
        }

        public MMF_Player GetSFX { get { return Feedback_SFX; } }
        public MMF_Player GetCharacter { get { return Feedback_Character; } }
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private SoundDatabase_ScriptableObject Database_BGM;
		[SerializeField] private SoundDatabase_ScriptableObject Database_SFX;
		[SerializeField] private SoundDatabase_ScriptableObject Database_Character;

		[SerializeField] private MMF_Player Feedback_BGM;
		[SerializeField] private MMF_Player Feedback_StopBGM;
		[SerializeField] private MMF_Player Feedback_StopStoneBGM;
		[SerializeField] private MMF_Player Feedback_SFX;
		[SerializeField] private MMF_Player Feedback_StopSFX;
		[SerializeField] private MMF_Player Feedback_Character;
		[SerializeField] private MMF_Player Feedback_StopCharacter;

		[SerializeField] private MMSoundManagerSettingsSO settingsSo;

		private Coroutine BGMCor = null;

        IEnumerator Start()
        {
			yield return new WaitUntil(() => MMSoundManager.Instance != null);
			if(MMSoundManager.Instance.settingsSo == null)
			{
				MMSoundManager.Instance.settingsSo = settingsSo;
            }
        }

    }//end of class					
}//end of namespace					