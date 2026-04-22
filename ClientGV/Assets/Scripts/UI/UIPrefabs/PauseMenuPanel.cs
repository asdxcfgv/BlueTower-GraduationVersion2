using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace QFramework.Example
{
	public class PauseMenuPanelData : UIPanelData
	{
	}
	public partial class PauseMenuPanel : UIPanel
	{
		
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PauseMenuPanelData ?? new PauseMenuPanelData();
			ResumeButton.onClick.AddListener(PauseGameMenu);
			QuitButton.onClick.AddListener(Exit);
			MusicIncreaseBtn.onClick.AddListener(IncreaseMusicVolume);
			MusicDecreaseBtn.onClick.AddListener(DecreaseMusicVolume);
			SoundIncreaseBtn.onClick.AddListener(IncreaseSoundsVolume);
			SoundDecreaseBtn.onClick.AddListener(DecreaseSoundsVolume);
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			Time.timeScale = 0f;
			
			// Initialise UI text
			StartCoroutine(InitializeUI());
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			Time.timeScale = 1f;
		}

		private void Exit()
		{
			SceneManager.LoadScene("MainMenuScene");
		}
		public void PauseGameMenu()
		{
			GameManager.Instance.PauseGameMenu();
		}
		
		private IEnumerator InitializeUI()
		{
			// Wait a frame to ensure the previous music and sound levels have been set
			yield return null;

			// Initialise UI text
			SoundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
			MusicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
		}
		
		/// <summary>
		/// Increase music volume - linked to from music volume increase button in UI
		/// </summary>
		public void IncreaseMusicVolume()
		{
			MusicManager.Instance.IncreaseMusicVolume();
			MusicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
		}

		/// <summary>
		/// Decrease music volume - linked to from music volume decrease button in UI
		/// </summary>
		public void DecreaseMusicVolume()
		{
			MusicManager.Instance.DecreaseMusicVolume();
			MusicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
		}

		/// <summary>
		/// Increase sounds volume - linked to from sounds volume increase button in UI
		/// </summary>
		public void IncreaseSoundsVolume()
		{
			SoundEffectManager.Instance.IncreaseSoundsVolume();
			SoundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
		}

		/// <summary>
		/// Decrease sounds volume - linked to from sounds volume decrease button in UI
		/// </summary>
		public void DecreaseSoundsVolume()
		{
			SoundEffectManager.Instance.DecreaseSoundsVolume();
			SoundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
		}
	}
}
