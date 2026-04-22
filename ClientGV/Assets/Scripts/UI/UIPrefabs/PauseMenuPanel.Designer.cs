using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:409f0340-9ddf-41c5-9f51-231bec3c8f46
	public partial class PauseMenuPanel
	{
		public const string Name = "PauseMenuPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button ResumeButton;
		[SerializeField]
		public UnityEngine.UI.Button QuitButton;
		[SerializeField]
		public TMPro.TextMeshProUGUI MusicLevelText;
		[SerializeField]
		public UnityEngine.UI.Button MusicIncreaseBtn;
		[SerializeField]
		public UnityEngine.UI.Button MusicDecreaseBtn;
		[SerializeField]
		public TMPro.TextMeshProUGUI SoundLevelText;
		[SerializeField]
		public UnityEngine.UI.Button SoundIncreaseBtn;
		[SerializeField]
		public UnityEngine.UI.Button SoundDecreaseBtn;
		
		private PauseMenuPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ResumeButton = null;
			QuitButton = null;
			MusicLevelText = null;
			MusicIncreaseBtn = null;
			MusicDecreaseBtn = null;
			SoundLevelText = null;
			SoundIncreaseBtn = null;
			SoundDecreaseBtn = null;
			
			mData = null;
		}
		
		public PauseMenuPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		PauseMenuPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PauseMenuPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
