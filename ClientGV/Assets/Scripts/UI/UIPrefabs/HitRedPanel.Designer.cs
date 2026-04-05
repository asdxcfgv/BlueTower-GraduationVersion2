using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:98d6a92d-cc6f-4926-9767-82257b13bfaf
	public partial class HitRedPanel
	{
		public const string Name = "HitRedPanel";
		
		
		private HitRedPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public HitRedPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		HitRedPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new HitRedPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
