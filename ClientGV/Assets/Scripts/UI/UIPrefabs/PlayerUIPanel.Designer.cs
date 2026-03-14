using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:73281033-013a-48ba-86f4-1244764a8f88
	public partial class PlayerUIPanel
	{
		public const string Name = "PlayerUIPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image Gun_Sprite;
		[SerializeField]
		public UnityEngine.GameObject BulletIcon_Content;
		[SerializeField]
		public UnityEngine.GameObject BulletIcon_Pre;
		[SerializeField]
		public UnityEngine.GameObject HeartIcon_Content;
		[SerializeField]
		public UnityEngine.GameObject HeartIcon_Pre;
		
		private PlayerUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Gun_Sprite = null;
			BulletIcon_Content = null;
			BulletIcon_Pre = null;
			HeartIcon_Content = null;
			HeartIcon_Pre = null;
			
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
