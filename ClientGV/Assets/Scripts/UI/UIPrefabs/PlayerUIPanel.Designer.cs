using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:f1e1aa3a-7198-4f4d-af7e-95cf43446ef5
	public partial class PlayerUIPanel
	{
		public const string Name = "PlayerUIPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image Gun_Sprite;
		
		private PlayerUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Gun_Sprite = null;
			
			mData = null;
		}
		
		public PlayerUIPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		PlayerUIPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PlayerUIPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
