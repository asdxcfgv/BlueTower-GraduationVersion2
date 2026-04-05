using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;
using static GlobalEnums;

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
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			Time.timeScale = 0f;
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
	}
}
