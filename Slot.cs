using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
namespace CameraSaves
{
	internal class CameraSaves_Slot : UIButton
	{
		public static void PopulateSlots(CameraSaves_Panel panel)
		{
			int decks = Options.Slots == 100 ? 2 : 1;
			int blocks = Data.BlockCount();
			int rows = Data.SlotRowsPerBlock;
			int columns = Data.SlotColumnsPerBlock;
			int span = Data.SlotSide + Data.SlotSpacing;
			int x = Data.SlotSpacing;
			int y = Data.SlotSpacing;
			for (int d = 0; d < decks; d++)
			{
				for (int b = 0; b < blocks; b++)
				{
					for (int r = 0; r < rows; r++)
					{
						for (int c = 0; c < columns; c++)
						{
							int index = (d * blocks * rows * columns) + (b * rows * columns) + (r * columns) + c;
							CameraSaves_Slot slot = (CameraSaves_Slot)panel.AddUIComponent(typeof(CameraSaves_Slot));
							slot.text = slot.name = index.ToString();
							slot.relativePosition = new Vector2(x, y);
							x += span;
						}
						x -= span * columns;
						y += span;
					}
					x += span * columns;
					y -= span * rows;
				}
				x = Data.SlotSpacing;
				y = Data.SlotSpacing + (span * rows);
			}
		}
		public override void Start()
		{
			width = height = Data.SlotSide;
			textScale = 0.75f;
			textPadding = new RectOffset(0, 0, 4, 0);
			normalBgSprite = hoveredBgSprite = pressedBgSprite = focusedBgSprite = "EmptySprite";
			pressedColor = Data.MidGrey;
			playAudioEvents = true;
			int index = int.Parse(name);
			InitMetrics(index);
			tooltip = Data.MetricsList[index].Tooltip ?? string.Empty;
		}
		private void InitMetrics(int index)
		{
			List<Metrics> list = Data.MetricsList;
			Metrics saved = list.FirstOrDefault(z => z.Slot == index);
			if (saved != null)
			{
				color = hoveredColor = focusedColor = (saved.Position == Vector3.zero ? Data.Pale : Data.Shaded);
			}
			else
			{
				list.Add(new Metrics() { Slot = index, });
				MetricsAdded = true;
				color = hoveredColor = focusedColor = Data.Pale;
			}
		}
		internal static bool MetricsAdded;
		protected override void OnMouseEnter(UIMouseEventParameter p)
		{
			if (color.Equals(Data.Pale))
			{
				textColor = Data.Shaded;
			}
		}
		protected override void OnMouseLeave(UIMouseEventParameter p)
		{
			textColor = Color.white;
		}
		protected override void OnClick(UIMouseEventParameter p)
		{
			if (p.buttons == UIMouseButton.Left)
			{
				KeyCode[] keys = (KeyCode[])Enum.GetValues(typeof(KeyCode));
				bool noKeyAtAll = !keys.Any(k => Input.GetKey(k));
				bool onlyOneAltPressed = (Input.GetKey(KeyCode.LeftAlt) ^ Input.GetKey(KeyCode.RightAlt)) && keys.Where(k => k != KeyCode.LeftAlt && k != KeyCode.RightAlt).All(k => !Input.GetKey(k));
				if (noKeyAtAll || onlyOneAltPressed)
				{
					tooltipBox.Hide();
					Unfocus();
					int index = Data.SlotCurrent = int.Parse(name);
					Metrics saved = Data.MetricsList[index];
					Metrics current = Data.MetricsCurrent();
					if (saved.Position == Vector3.zero)
					{
						if (Options.EnableTooltiping)
						{
							PopTextField(index);
						}
						else
						{
							Save(index);
						}
					}
					else
					{
						if (noKeyAtAll)
						{
							bool offSpot =
								saved.Position != current.Position ||
								saved.Angle != current.Angle ||
								saved.Height != current.Height ||
								saved.Size != current.Size ||
								saved.FOV != current.FOV;
							if (offSpot)
							{
								Teleport(saved);
							}
							else
							{
								Delete(index);
							}
						}
						else
						{
							if (Options.EnableTooltiping)
							{
								PopTextField(index);
							}
							else
							{
								Save(index);
							}
						}
					}
				}
			}
		}
		private void PopTextField(int index)
		{
			string existing = Data.MetricsList[index].Tooltip ?? string.Empty;
			UITextField tf = (UITextField)UIView.GetAView().AddUIComponent(typeof(UITextField));
			tf.autoSize = false;
			tf.builtinKeyNavigation = true;
			tf.horizontalAlignment = UIHorizontalAlignment.Center;
			tf.isVisible = true;
			tf.name = "CameraSaves_Tooltip";
			tf.normalBgSprite = "OptionsDropboxListbox";
			tf.position = Data.TooltipTextFieldPosition;
			tf.selectionSprite = "EmptySprite";
			tf.width = Data.TooltipTextFieldWidth;
			tf.height = Data.TooltipTextFieldHeight;
			tf.submitOnFocusLost = true;
			tf.text = existing;
			tf.padding = new RectOffset(0, 0, 8, 0);
			tf.textScale = 0.875f;
			tf.verticalAlignment = UIVerticalAlignment.Middle;
			tf.Focus();
			tf.eventTextSubmitted += (s, e) =>
			{
				tf.text = Regex.Replace(tf.text, @"\s+", " ").Trim();
				tooltip = tf.text;
				Destroy(tf.gameObject);
				Save(index);
			};
		}
		private void Save(int index)
		{
			Data.MetricsList[index] = Data.MetricsCurrent();
			Data.MetricsList[index].Tooltip = tooltip;
			try
			{
				LocalXml.SaveLocal();
				color = hoveredColor = pressedColor = focusedColor = Data.Shaded;
			}
			catch
			{
				Message.Preset("- Camera -");
			}
		}
		private void Teleport(Metrics saved)
		{
			CameraController cc = Camera.main?.GetComponent<CameraController>();
			if (cc != null)
			{
				cc.m_targetPosition = saved.Position;
				cc.m_targetAngle = saved.Angle;
				cc.m_targetHeight = saved.Height;
				cc.m_targetSize = saved.Size;
				Camera.main.fieldOfView = saved.FOV;
				cc.m_currentPosition = saved.Position;
				cc.m_currentAngle = saved.Angle;
				cc.m_currentHeight = saved.Height;
				cc.m_currentSize = saved.Size;
			}
			else
			{
				string msg = "\n\n" +
					"Teleport failed.\n\n" +
					"InGame camera controller was not available!";
				Message.Custom(msg);
			}
		}
		private void Delete(int index)
		{
			if (Options.AlertDeletion)
			{
				string tooltip = Data.MetricsList[index].Tooltip ?? string.Empty;
				string msg = (string.IsNullOrEmpty(tooltip)
					? string.Empty
					: $"* {tooltip} *\n\n")
					+ "Delete current camera?";
				alert(msg);
				void alert(string message)
				{
					ConfirmPanel.ShowModal(Mod.name, message, response);
				}
				void response(UIComponent component, int result)
				{
					if (result == 1)
					{
						delete(tooltip);
					}
				}
			}
			else
			{
				delete(tooltip);
			}
			void delete(string tooltip)
			{
				Data.MetricsList[index] = new Metrics() { Slot = index };
				try
				{
					LocalXml.SaveLocal();
					color = hoveredColor = focusedColor = Data.Pale;
					this.tooltip = null;
					if (Options.NotifyDeletion)
					{
						string msg = "\n" +
							(string.IsNullOrEmpty(tooltip) ? "\n" : $"* {tooltip} *\n\n\n") +
							"Current camera deleted.\n\n" +
							"Slot ready for a new save.";
						Message.Custom(msg);
					}
				}
				catch
				{
					Message.Preset("= Camera =");
				}
			}
		}
	}
}
