using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class MinimapPanelData : UIPanelData
	{
	}
	public partial class MinimapPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as MinimapPanelData ?? new MinimapPanelData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			DungeonMap.Instance.minimapUI = this.gameObject	;
		}
		
				
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
