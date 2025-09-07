using ColossalFramework.UI;
using UnityEngine;
namespace CameraSaves
{
	internal class CameraSaves_Panel : UIPanel
	{
		public override void Start()
		{
			name = "CameraSaves_Panel";
			backgroundSprite = "EmptySprite";
			color = Data.MidGrey;
			width = Options.OpaquePanel ? Data.PanelWidth() : 2;
			height = Options.OpaquePanel ? Data.PanelHeight() : 2;
			autoSize = false;
			if (Data.PanelInView() && Options.DecouplePanel)
			{
				relativePosition = Options.PanelPosition;
			}
			else
			{
				relativePosition = Options.PanelPosition = Data.PanelPositionDefault();
				try
				{
					GlobalXml.SaveGlobal();
				}
				catch
				{
					Message.Preset("< Panel Position >");
				}
			}
			CameraSaves_Slot.PopulateSlots(this);
			if (CameraSaves_Slot.MetricsAdded)
			{
				LocalXml.SaveLocal();
			}
		}
		protected override void OnMouseDown(UIMouseEventParameter p)
		{
			if (p.buttons.IsFlagSet(UIMouseButton.Right) && Options.DecouplePanel)
			{
				Input.compositionCursorPos = absolutePosition;
				BringToFront();
			}
		}
		protected override void OnMouseMove(UIMouseEventParameter p)
		{
			if (p.buttons.IsFlagSet(UIMouseButton.Right) && Options.DecouplePanel)
			{
				Vector2 pointer = Input.mousePosition;
				pointer.y = UIView.GetAView().fixedHeight - pointer.y;
				absolutePosition = pointer;
			}
		}
		protected override void OnMouseUp(UIMouseEventParameter p)
		{
			if (Input.GetMouseButtonUp(1) && Options.DecouplePanel)
			{
				Options.PanelPosition = relativePosition;
				try
				{
					GlobalXml.SaveGlobal();
				}
				catch
				{
					Message.Preset("< Panel Position >");
				}
			}
		}
	}
}