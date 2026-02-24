using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:df198f6d-f908-4c14-8a5c-85b6e81ffba6
	public partial class MinimapPanel
	{
		public const string Name = "MinimapPanel";
		
		
		private MinimapPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public MinimapPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		MinimapPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new MinimapPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
