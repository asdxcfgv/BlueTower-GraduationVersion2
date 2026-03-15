using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:f94e2370-4ac2-46db-9256-3f70eece12fa
	public partial class PauseMenuPanel
	{
		public const string Name = "PauseMenuPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button ResumeButton;
		[SerializeField]
		public UnityEngine.UI.Button QuitButton;
		
		private PauseMenuPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ResumeButton = null;
			QuitButton = null;
			
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
