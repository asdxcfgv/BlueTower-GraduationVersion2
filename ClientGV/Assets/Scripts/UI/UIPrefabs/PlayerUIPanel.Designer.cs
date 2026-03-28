using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:04fe291f-9f8e-44d0-a9a8-b7c136d0d3ab
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
		[SerializeField]
		public TMPro.TextMeshProUGUI NormalBulletText;
		[SerializeField]
		public TMPro.TextMeshProUGUI ElectronBulletText;
		[SerializeField]
		public TMPro.TextMeshProUGUI BoomBulletText;
		
		private PlayerUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Gun_Sprite = null;
			BulletIcon_Content = null;
			BulletIcon_Pre = null;
			HeartIcon_Content = null;
			HeartIcon_Pre = null;
			NormalBulletText = null;
			ElectronBulletText = null;
			BoomBulletText = null;
			
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
