/*********************************************************					
* DevDialogueController.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.11.28 오전 9:01					
**********************************************************/
using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;
using Dev_System;
using MoreMountains.Tools;
using static MoreMountains.Tools.MMSoundManager;
using Dev_UI;
using Dev_Occulus;
using UnityEngine.UIElements;

namespace Dev_Dialogue
{
    public class DevDialogueController : MonoBehaviour			
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------

		// 대화 - text 노드들이 모여있는 집합

		// 대화를 하나 넘긴다
		public void ContinueConversation(bool UIisOpen = false)
        {
			if(UIisOpen)
            {
				// 대화가 떠있는데 바로 다음 노드가 호출되어야 할 때 사용한다
				// ex) 중앙으로 가주세요 -> 중앙 가자마자 다음 노드 호출되어야함
				if (DialogueManager.standardDialogueUI.conversationUIElements.mainPanel.isOpen)
				{
					DialogueManager.standardDialogueUI.OnContinue();
				}
			}
            else
            {
				DialogueManager.standardDialogueUI.OnContinue();
			}
        }

		// 대화를 멈춘다 (해당 대화 아예 안나옴)
		public void StopConversation()
        {
			DialogueManager.StopConversation();
        }

		// Variable 조건을 넘긴다 (원하는 노드 선택)
		public void SetVariable(string _condition)
        {
            //DevDialogueEnum_Condition condition = (DevDialogueEnum_Condition)System.Enum.Parse(typeof(DevDialogueEnum_Condition), _condition);
            //if (System.Enum.IsDefined(typeof(DevDialogueEnum_Condition), condition) == false)
            //{
            //	Debug.LogError("_condition != enum's data");
            //	return;
            //}

            //Debug.LogError(_condition); 
            DialogueManager.StopConversation();
            DialogueLua.SetVariable("Condition", _condition);
            OnUse();
        }

        #region conversation
        // 어떤 대화가 나올지 선택하고 호출되면 대화가 호출된다
        public void SetConversation(string _conversation)
        {
			// 다언어 적용
			string language = "";
			language = langueString(Managers.Game.pCurLanguage);
			_conversation += language;
			//_conversation += "_KR";

			trigger.conversation = _conversation;
            DialogueManager.StopConversation();
            trigger.OnUse();
            //DialogueManager.StartConversation(_conversation.ToString());
        }

        public void SetMeditationConversation(string _conversation)
        {
            // 다언어 적용
            string language = "";
            language = langueString(Managers.Game.pCurLanguage);
            _conversation += language;
            _conversation += "_" + Managers.Game.MeditationPlayTime.ToString() + "min";

            trigger.conversation = _conversation;
            DialogueManager.StartConversation(_conversation.ToString());
        }

        public void SetFitnessConversatioin(string _conversation)
        {
            // 다언어 적용
            string language = "";
            language = langueString(Managers.Game.pCurLanguage);
            //language = "_KR";
            _conversation += language;

            // 성별 적용
            string gender = "_Female";
            
            _conversation += gender;
            Debug.Log("Conversation : " + _conversation);
            trigger.conversation = _conversation;
            DialogueManager.StartConversation(_conversation.ToString());
        }
        #endregion
        public void CommonVariable(string _condition)
        {
			string originalConversation = trigger.conversation;
			SetConversation("Common");
			SetVariable(_condition);
			trigger.conversation = originalConversation;
            DialogueManager.StartConversation(originalConversation.ToString());
		}

        public void ShowAlert(string message)
        {
			DialogueManager.ShowAlert(message);
        }

		// 원하는 시퀀스를 호출한다
		public void CallSequence(string _sequence)
        {
			Sequencer.Message(_sequence.ToString());
        }

		public void BarkString(string barkText, Transform speaker)
        {
			DialogueManager.BarkString(barkText, speaker);
        }

		private float timelineTime = 20.0f;
		private float defaultTime = 3.0f;
		public void EnterTimeline()
        {
			DialogueManager.instance.displaySettings.subtitleSettings.minSubtitleSeconds = timelineTime;
        }
		
		public void EndTimeline()
        {
			DialogueManager.instance.displaySettings.subtitleSettings.minSubtitleSeconds = defaultTime;
			DialogueManager.StopConversation();
		}

		public void OnUse()
        {
			trigger.OnUse();
        }

		public void StopDialogue()
		{
			DialogueManager.instance.Pause();
            DialogueManager.instance.StopConversation();
		}

        public void SetQuest(QuestState state)
        {
            DialogueLua.SetQuestField("CutScene", "State", state);
            DialogueLua.GetQuestField("CutScene", "State");
        }

        string langueString(Dev_System.DevSystemLanguage language) => language switch
        {
            Dev_System.DevSystemLanguage.en_US => "_EN",
            Dev_System.DevSystemLanguage.ko_KR => "_KR",
			_ => "",
        };

        // Todo : 필요하면 주석풀기
        //string MinuteString(Dev_SceneManagement.MeditationTimeDefine minute) => minute switch
        //{
        //    Dev_SceneManagement.MeditationTimeDefine.None => "",
        //    Dev_SceneManagement.MeditationTimeDefine.minutes_1 => "_1min",
        //    Dev_SceneManagement.MeditationTimeDefine.minutes_3 => "_3min",
        //    Dev_SceneManagement.MeditationTimeDefine.minutes_5 => "_5min",
        //    _ => "",
        //};

        public void Dialogueing(bool value)
		{
			dialogueing = value;
            //if (dialogueing)
            //{
            //    if (MMSoundManager.Instance != null)
            //    {
            //        StopCoroutine("BGMSoundDown");
            //        StartCoroutine("BGMSoundDown");
            //    }
                    
            //}
            //else
            //{
            //    if (MMSoundManager.Instance != null)
            //    {
            //        StopCoroutine("BGMSoundUp");
            //        StartCoroutine("BGMSoundUp");
            //    }
            //}
        }

        IEnumerator BGMSoundDown()
        {
            
            float duration = 2.0f;
            float elapsedTime = 0;

            normalVolume = 1.0f;
            float donwRate = 0.3f;
            if(Managers.LoadingHandler.GetNextTargetSceneType == Dev_SceneManagement.SceneStructureType.Meditation)
            {
                donwRate = 0.7f;
            }
            float desVolume = normalVolume * donwRate;


            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float volume = normalVolume;
                volume = Mathf.Lerp(volume, desVolume, t);
                MMSoundManager.Instance.settingsSo.SetTrackVolume(MMSoundManagerTracks.Music, volume);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            MMSoundManager.Instance.settingsSo.SetTrackVolume(MMSoundManagerTracks.Music, desVolume);
        }

        IEnumerator BGMSoundUp()
        {

            float duration = 2.0f;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float volume = MMSoundManager.Instance.settingsSo.GetTrackVolume(MMSoundManagerTracks.Music);
                volume = Mathf.Lerp(volume, normalVolume, t);
                MMSoundManager.Instance.settingsSo.SetTrackVolume(MMSoundManagerTracks.Music, volume);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            MMSoundManager.Instance.settingsSo.SetTrackVolume(MMSoundManagerTracks.Music, normalVolume);
        }

		public void DialogueSpriteChange(string actor, Sprite sprite, string GetActor)
		{
            DialogueManager.instance.initialDatabase.GetActor(GetActor).Name = actor;
            DialogueManager.instance.initialDatabase.GetActor(GetActor).textureName = actor;
			DialogueManager.instance.initialDatabase.GetActor(GetActor).spritePortrait = sprite;
        }

        public void DialoguePause()
        {
            DialogueManager.instance.Pause();
        }

        public void DialogueUnPause()
        {
            DialogueManager.instance.Unpause();
        }

        public void DialogueMute(bool mute)
        {
            DialogueLua.SetVariable("Mute", mute);
        }

        [SerializeField]
        DialogueSystemTrigger trigger;
        public bool dialogueing = false;

        private float normalVolume = 0;

        [Header("----------UI----------")]
        [SerializeField] private UIMovementVR UIMovement_Dialogue;
        [SerializeField] private float UIDistnace = 3.0f;
        [SerializeField] private Vector3 CorrectValue = Vector3.up;

        IEnumerator Start()
        {
            yield return new WaitUntil(() => Managers.AllCreateManagers);
            yield return new WaitUntil(() => Managers.LoadingHandler.IsLoading == false);
            yield return new WaitUntil(() => Managers.Unit.GetCurTurnUnit != null);

            if(UIMovement_Dialogue != null)
            {
                UIMovementInfos info = new UIMovementInfos();
                info.Initialize(
                    useLerp: true,
                    targetUI: UIMovement_Dialogue.transform,
                    targetCam: Managers.Unit.GetCurTurnUnit.pCurCameraRigMaster.HeadAnchorPos,
                    followSpeed: 20.0f,
                    distanceFromCam: UIDistnace,
                    correctionValue: CorrectValue);

                UIMovement_Dialogue.EnableUI(info);
            }
        }

    }//end of class					
}//end of namespace					