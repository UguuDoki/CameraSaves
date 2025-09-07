using ColossalFramework.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
namespace CameraSaves
{
	internal class Options
	{
		public static Vector2 IconPosition;
		public static Vector2 PanelPosition;
		public static bool DecouplePanel;
		public static bool OpaquePanel;
		public static int Slots = Data.SlotsDefault;
		public static bool AlertDeletion = true;
		public static bool NotifyDeletion = true;
		public static bool EnableTooltiping = true;
		public static List<Metrics> MetricsList = new List<Metrics>();
		public static void CreateOptionsPanel(UIHelper helper)
		{
			bool InGame = ToolsModifierControl.toolController != null
				&& ToolsModifierControl.toolController.gameObject.scene.name != "IntroScreen";
			UIScrollablePanel sp = (UIScrollablePanel)helper.self;
			sp.name = "CameraSaves_Options";
			sp.autoLayout = true;
			sp.autoLayoutDirection = LayoutDirection.Vertical;
			sp.autoLayoutStart = LayoutStart.TopLeft;
			sp.autoSize = true;
			_ = helper.AddSpace(16);
			{
				UIPanel pn0 = OptionsUI.PanelVertical(sp);
				{
					UILabel lb00 = OptionsUI.Label(pn0);
					lb00.text = "Camera Saves";
					lb00.textColor = new Color32(240, 230, 140, 255);
					lb00.textScale = 2f;
					UIPanel pn00 = OptionsUI.PanelHorizontal(pn0);
					pn00.padding = new RectOffset(32, 0, 48, 0);
					{
						UIButton bt000 = OptionsUI.Button(pn00);
						bt000.eventClicked += (s, e) => HowToUse_ButtonClicked();
						UILabel lb000 = OptionsUI.Label(pn00);
						lb000.padding = new RectOffset(16, 0, 0, 0);
						lb000.text = "How To Use\n\n" + Data.PathMod();
						lb000.textScale = 1.125f;
					}
				}
			}
			_ = helper.AddSpace(48);
			{
				UIPanel pn1 = OptionsUI.PanelVertical(sp);
				pn1.padding = new RectOffset(32, 0, 0, 0);
				{
					UILabel lb10 = OptionsUI.Label(pn1);
					lb10.text = "Icon";
					lb10.textColor = new Color32(238, 232, 170, 255);
					lb10.textScale = 1.5f;
					UIPanel pn10 = OptionsUI.PanelHorizontal(pn1);
					pn10.padding = new RectOffset(32, 0, 16, 0);
					{
						UIButton bt100 = OptionsUI.Button(pn10);
						bt100.eventClicked += (s, e) => ResetIcon_ButtonClicked();
						bt100.isEnabled = InGame;
						bt100.text = InGame ? "" : "X";
						UIPanel pn100 = OptionsUI.PanelHorizontal(pn10);
						pn100.padding = new RectOffset(16, 0, 0, 0);
						{
							UILabel lb1000 = OptionsUI.Label(pn100);
							lb1000.text = "[InGame]";
							lb1000.textColor = InGame ? Color.white : Color.gray;
							lb1000.textScale = 1.125f;
							UILabel lb1001 = OptionsUI.Label(pn100);
							lb1001.padding = new RectOffset(16, 0, 0, 0);
							lb1001.text = "Reset icon";
							lb1001.textScale = 1.125f;
						}
					}
				}
			}
			_ = helper.AddSpace(48);
			{
				UIPanel pn2 = OptionsUI.PanelVertical(sp);
				pn2.padding = new RectOffset(32, 0, 0, 0);
				{
					UILabel lb20 = OptionsUI.Label(pn2);
					lb20.text = "Panel";
					lb20.textColor = new Color32(238, 232, 170, 255);
					lb20.textScale = 1.5f;
					{
						UIPanel pn20 = OptionsUI.PanelHorizontal(pn2);
						pn20.padding = new RectOffset(32, 0, 16, 0);
						{
							UIButton bt200 = OptionsUI.Button(pn20);
							bt200.eventClicked += (s, e) => ResetPanel_ButtonClicked();
							bt200.isEnabled = InGame;
							bt200.text = InGame ? "" : "X";
							UIPanel pn200 = OptionsUI.PanelHorizontal(pn20);
							pn200.padding = new RectOffset(16, 0, 0, 0);
							{
								UILabel lb2000 = OptionsUI.Label(pn200);
								lb2000.text = "[InGame]";
								lb2000.textColor = InGame ? Color.white : Color.gray;
								lb2000.textScale = 1.125f;
								UILabel lb2001 = OptionsUI.Label(pn200);
								lb2001.padding = new RectOffset(16, 0, 0, 0);
								lb2001.text = "Reset panel";
								lb2001.textScale = 1.125f;
							}
						}
						UIPanel pn21 = OptionsUI.PanelHorizontal(pn2);
						pn21.padding = new RectOffset(32, 0, 16, 0);
						{
							UICheckBox cb210 = OptionsUI.CheckBox(pn21);
							cb210.eventClicked += (s, e) => DecouplePanel_CheckBoxClicked(cb210);
							cb210.isChecked = DecouplePanel;
							cb210.name = "cb210";
							cb210.label.text = "Decouple from icon";
						}
						UIPanel pn22 = OptionsUI.PanelHorizontal(pn2);
						pn22.padding = new RectOffset(32, 0, 16, 0);
						{
							UICheckBox cb211 = OptionsUI.CheckBox(pn22);
							cb211.eventClicked += (s, e) => OpaquePanel_CheckBoxClicked(cb211);
							cb211.isChecked = OpaquePanel;
							cb211.name = "cb211";
							cb211.label.text = "Opaque";
						}
					}
				}
			}
			_ = helper.AddSpace(48);
			{
				UIPanel pn3 = OptionsUI.PanelVertical(sp);
				pn3.padding = new RectOffset(32, 0, 0, 0);
				{
					UILabel lb30 = OptionsUI.Label(pn3);
					lb30.text = "Slots";
					lb30.textColor = new Color32(238, 232, 170, 255);
					lb30.textScale = 1.5f;
					UIPanel pn30 = OptionsUI.PanelHorizontal(pn3);
					pn30.padding = new RectOffset(32, 0, 16, 0);
					{
						UICheckBox cb300 = OptionsUI.CheckBox(pn30);
						cb300.group = pn30;
						cb300.name = "cb300";
						cb300.label.text = "10";
						cb300.isChecked = cb300.label.text == Slots.ToString() ||
							!new int[] { 10, 20, 30, 40, 50, 100 }.Contains(Slots);
						cb300.isInteractive = !cb300.isChecked;
						cb300.eventClicked += (s, e) => Slots_CheckBoxClicked(cb300);
						UICheckBox cb301 = OptionsUI.CheckBox(pn30);
						cb301.group = pn30;
						cb301.name = "cb301";
						cb301.label.text = "20";
						cb301.isChecked = cb301.label.text == Slots.ToString();
						cb301.isInteractive = !cb301.isChecked;
						cb301.eventClicked += (s, e) => Slots_CheckBoxClicked(cb301);
						UICheckBox cb302 = OptionsUI.CheckBox(pn30);
						cb302.group = pn30;
						cb302.name = "cb302";
						cb302.label.text = "30";
						cb302.isChecked = cb302.label.text == Slots.ToString();
						cb302.isInteractive = !cb302.isChecked;
						cb302.eventClicked += (s, e) => Slots_CheckBoxClicked(cb302);
						UICheckBox cb303 = OptionsUI.CheckBox(pn30);
						cb303.group = pn30;
						cb303.name = "cb303";
						cb303.label.text = "40";
						cb303.isChecked = cb303.label.text == Slots.ToString();
						cb303.isInteractive = !cb303.isChecked;
						cb303.eventClicked += (s, e) => Slots_CheckBoxClicked(cb303);
						UICheckBox cb304 = OptionsUI.CheckBox(pn30);
						cb304.group = pn30;
						cb304.name = "cb304";
						cb304.label.text = "50";
						cb304.isChecked = cb304.label.text == Slots.ToString();
						cb304.isInteractive = !cb304.isChecked;
						cb304.eventClicked += (s, e) => Slots_CheckBoxClicked(cb304);
						UICheckBox cb305 = OptionsUI.CheckBox(pn30);
						cb305.group = pn30;
						cb305.name = "cb305";
						cb305.label.text = "100";
						cb305.isChecked = cb305.label.text == Slots.ToString();
						cb305.isInteractive = !cb305.isChecked;
						cb305.eventClicked += (s, e) => Slots_CheckBoxClicked(cb305);
					}
					UIPanel pn31 = OptionsUI.PanelHorizontal(pn3);
					pn31.padding = new RectOffset(32, 0, 16, 0);
					{
						UICheckBox cb310 = OptionsUI.CheckBox(pn31);
						cb310.eventClicked += (s, e) => AlertDeletion_CheckBoxClicked(cb310);
						cb310.isChecked = AlertDeletion;
						cb310.name = "cb310";
						cb310.label.text = "Alert deletion";
					}
					UIPanel pn32 = OptionsUI.PanelHorizontal(pn3);
					pn32.padding = new RectOffset(32, 0, 16, 0);
					{
						UICheckBox cb320 = OptionsUI.CheckBox(pn32);
						cb320.eventClicked += (s, e) => NotifyDeletion_CheckBoxClicked(cb320);
						cb320.isChecked = NotifyDeletion;
						cb320.name = "cb320";
						cb320.label.text = "Notify deletion";
					}
					UIPanel pn33 = OptionsUI.PanelHorizontal(pn3);
					pn33.padding = new RectOffset(32, 0, 16, 0);
					{
						UICheckBox cb330 = OptionsUI.CheckBox(pn33);
						cb330.eventClicked += (s, e) => EnableTooltiping_CheckBoxClicked(cb330);
						cb330.isChecked = EnableTooltiping;
						cb330.name = "cb330";
						cb330.label.text = "Enable tooltiping";
					}
				}
			}
		}
		public static void HowToUse_ButtonClicked()
		{
			Process process = new Process();
			process.StartInfo.FileName = Data.PathMod();
			process.StartInfo.UseShellExecute = true;
			_ = process.Start();
		}
		private static void ResetIcon_ButtonClicked()
		{
			CameraSaves_Icon icon = (CameraSaves_Icon)UIView.GetAView().FindUIComponent("CameraSaves_Icon");
			icon.relativePosition = Data.IconPositionDefault;
			IconPosition = icon.relativePosition;
			try
			{
				GlobalXml.SaveGlobal();
			}
			catch
			{
				Message.Preset("< Reset icon >");
			}
		}
		private static void ResetPanel_ButtonClicked()
		{
			CameraSaves_Panel panel = (CameraSaves_Panel)UIView.GetAView().FindUIComponent("CameraSaves_Panel");
			panel.relativePosition = Data.PanelPositionDefault();
			PanelPosition = panel.relativePosition;
			try
			{
				GlobalXml.SaveGlobal();
			}
			catch
			{
				Message.Preset("< Reset panel >");
			}
		}
		private static void DecouplePanel_CheckBoxClicked(UIComponent component)
		{
			DecouplePanel = ((UICheckBox)component).isChecked;
			try
			{
				GlobalXml.SaveGlobal();
			}
			catch
			{
				DecouplePanel = ((UICheckBox)component).isChecked = !((UICheckBox)component).isChecked;
				Message.Preset("[ " + ((UICheckBox)component).label.text + " ]");
			}
		}
		private static void OpaquePanel_CheckBoxClicked(UIComponent component)
		{
			OpaquePanel = ((UICheckBox)component).isChecked;
			CameraSaves_Panel panel = (CameraSaves_Panel)UIView.GetAView().FindUIComponent("CameraSaves_Panel");
			try
			{
				GlobalXml.SaveGlobal();
				panel.width = OpaquePanel ? Data.PanelWidth() : 2;
				panel.height = OpaquePanel ? Data.PanelHeight() : 2;
			}
			catch
			{
				OpaquePanel = ((UICheckBox)component).isChecked = !((UICheckBox)component).isChecked;
				Message.Preset("[ " + ((UICheckBox)component).label.text + " ]");
			}
		}
		private static void Slots_CheckBoxClicked(UIComponent component)
		{
			UICheckBox thisCB = (UICheckBox)component;
			UICheckBox previousCB = UIView.GetAView()
				.GetComponentsInChildren<UIScrollablePanel>().First(z => z.name == "CameraSaves_Options")
				.GetComponentsInChildren<UICheckBox>().First(z => z.label.text == Slots.ToString());
			Slots = int.Parse(thisCB.label.text);
			CameraSaves_Panel panel = (CameraSaves_Panel)UIView.GetAView().FindUIComponent("CameraSaves_Panel");
			if (panel != null)
			{
				IEnumerable<Component> slots = panel.GetComponentsInChildren(typeof(CameraSaves_Slot));
				foreach (Component slot in slots)
				{
					Object.Destroy(slot.gameObject);
				}
				panel.width = OpaquePanel ? Data.PanelWidth() : 2;
				panel.height = OpaquePanel ? Data.PanelHeight() : 2;
				CameraSaves_Slot.PopulateSlots(panel);
				try
				{
					GlobalXml.SaveGlobal();
					thisCB.isInteractive = false;
					previousCB.isInteractive = true;
				}
				catch
				{
					Slots = int.Parse(previousCB.label.text);
					panel.width = OpaquePanel ? Data.PanelWidth() : 2;
					panel.height = OpaquePanel ? Data.PanelHeight() : 2;
					CameraSaves_Slot.PopulateSlots(panel);
					thisCB.isChecked = false;
					previousCB.isChecked = true;
					Message.Preset("[ Slots " + ((UICheckBox)component).label.text + " ]");
				}
			}
		}
		private static void AlertDeletion_CheckBoxClicked(UIComponent component)
		{
			AlertDeletion = ((UICheckBox)component).isChecked;
			try
			{
				GlobalXml.SaveGlobal();
			}
			catch
			{
				AlertDeletion = ((UICheckBox)component).isChecked = !((UICheckBox)component).isChecked;
				Message.Preset("[ " + ((UICheckBox)component).label.text + " ]");
			}
		}
		private static void NotifyDeletion_CheckBoxClicked(UIComponent component)
		{
			NotifyDeletion = ((UICheckBox)component).isChecked;
			try
			{
				GlobalXml.SaveGlobal();
			}
			catch
			{
				NotifyDeletion = ((UICheckBox)component).isChecked = !((UICheckBox)component).isChecked;
				Message.Preset("[ " + ((UICheckBox)component).label.text + " ]");
			}
		}
		private static void EnableTooltiping_CheckBoxClicked(UIComponent component)
		{
			EnableTooltiping = ((UICheckBox)component).isChecked;
			try
			{
				GlobalXml.SaveGlobal();
			}
			catch
			{
				EnableTooltiping = ((UICheckBox)component).isChecked = !((UICheckBox)component).isChecked;
				Message.Preset("[ " + ((UICheckBox)component).label.text + " ]");
			}
		}
	}
	internal class OptionsUI
	{
		internal static UIPanel PanelHorizontal(UIComponent parent)
		{
			UIPanel p = parent.AddUIComponent<UIPanel>();
			p.autoLayout = true;
			p.autoLayoutDirection = LayoutDirection.Horizontal;
			p.autoFitChildrenHorizontally = true;
			p.autoFitChildrenVertically = true;
			p.autoSize = true;
			return p;
		}
		internal static UIPanel PanelVertical(UIComponent parent)
		{
			UIPanel p = parent.AddUIComponent<UIPanel>();
			p.autoLayout = true;
			p.autoLayoutDirection = LayoutDirection.Vertical;
			p.autoFitChildrenHorizontally = true;
			p.autoFitChildrenVertically = true;
			p.autoSize = true;
			return p;
		}
		internal static UILabel Label(UIComponent parent)
		{
			UILabel l = parent.AddUIComponent<UILabel>();
			l.font = UIDynamicFont.FindByName("OpenSans-Semibold (ColossalFramework.UI.UIDynamicFont)");
			return l;
		}
		internal static UIButton Button(UIComponent parent)
		{
			UIButton b = parent.AddUIComponent<UIButton>();
			{
				b.disabledBgSprite = "GenericPanel";
				b.disabledTextColor = Color.black;
				b.hoveredBgSprite = "ButtonMenuHovered";
				b.normalBgSprite = "GenericPanel";
				b.size = new Vector2(16, 16);
				return b;
			}
		}
		internal static UICheckBox CheckBox(UIComponent parent)
		{
			UICheckBox cb = parent.AddUIComponent<UICheckBox>();
			{
				UISprite uncheck = cb.AddUIComponent<UISprite>();
				uncheck.autoSize = false;
				uncheck.size = new Vector2(16, 16);
				uncheck.spriteName = "AchievementCheckedFalse";
				UISprite check = cb.AddUIComponent<UISprite>();
				check.autoSize = false;
				check.size = new Vector2(16, 16);
				check.spriteName = "AchievementCheckedTrue";
				cb.checkedBoxObject = check;
				UILabel label = cb.AddUIComponent<UILabel>();
				label.font = UIDynamicFont.FindByName("OpenSans-Semibold (ColossalFramework.UI.UIDynamicFont)");
				label.padding = new RectOffset(32, 0, 0, 0);
				label.textScale = 1.125f;
				cb.label = label;
			}
			cb.size = new Vector2(96, 20);
			return cb;
		}
	}
}