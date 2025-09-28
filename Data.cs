using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace CameraSaves
{
	internal class Data
	{
		private static readonly int ViewWidth = UIView.GetAView().fixedWidth;
		private static readonly int ViewHeight = UIView.GetAView().fixedHeight;
		internal const int IconSide = 46;
		internal const int IconSpacing = 2;
		internal static Vector2 IconPositionDefault = new Vector2((ViewWidth - IconSide) / 2f, (ViewHeight - IconSide) / 2f);
		internal static bool IconInView()
		{
			return Options.IconPosition.x > 0 && Options.IconPosition.x <= ViewWidth - IconSide && Options.IconPosition.y > 0 && Options.IconPosition.y <= ViewHeight - IconSide;
		}
		internal static readonly Color32 MidGrey = new Color32(147, 148, 152, 255);
		internal static int PanelWidth() { return SlotSpacing + (BlockCount() * BlockWidth); }
		internal static int PanelHeight() { return SlotSpacing + (BlockHeight * (Options.Slots == 100 ? 2 : 1)); }
		internal static Vector2 PanelPositionDefault()
		{
			Vector2 iconPosition = Options.IconPosition == Vector2.zero? IconPositionDefault : Options.IconPosition;
			return new Vector2(iconPosition.x + IconSide + IconSpacing, iconPosition.y);
		}
		internal static bool PanelInView()
		{
			return Options.PanelPosition.x > 0 && Options.PanelPosition.x <= ViewWidth - PanelWidth()
				&& Options.PanelPosition.y > 0 && Options.PanelPosition.y <= ViewHeight - PanelHeight();
		}
		internal static int BlockCount()
		{
			return (Options.Slots == 100 ? 50 : Options.Slots) / SlotRowsPerBlock / SlotColumnsPerBlock;
		}
		internal const int BlockWidth = SlotColumnsPerBlock * (SlotSide + SlotSpacing);
		internal const int BlockHeight = SlotRowsPerBlock * (SlotSide + SlotSpacing);
		internal static readonly Color32 Pale = new Color32(188, 190, 192, 255);
		internal static readonly Color32 Shaded = new Color32(109, 110, 113, 255);
		internal static readonly Color32 Lime = new Color32(0, 255, 0, 255);
		internal const int SlotSide = 20;
		internal const int SlotColumnsPerBlock = 5;
		internal const int SlotRowsPerBlock = 2;
		internal const int SlotSpacing = 2;
		internal const int SlotsDefault = 10;
		internal static int SlotCurrent = default;
		internal const int TooltipTextFieldWidth = 448;
		internal const int TooltipTextFieldHeight = 28;
		internal static Vector2 TooltipTextFieldPosition = new Vector2(0 - TooltipTextFieldWidth / 2, 0 - TooltipTextFieldHeight / 2);
		internal static Metrics MetricsCurrent()
		{
			CameraController controller = Camera.main?.GetComponent<CameraController>();
			Metrics metrics = new Metrics()
			{
				Slot = SlotCurrent,
				Tooltip = string.Empty,
				Position = controller.m_targetPosition,
				Angle = controller.m_targetAngle,
				Height = controller.m_targetHeight,
				Size = controller.m_targetSize,
				FOV = Camera.main.fieldOfView
			};
			return metrics;
		}
		internal static List<Metrics> MetricsList = new List<Metrics>();
		internal static readonly string PathGlobal = Path.Combine(DataLocation.localApplicationData, "CameraSaves.xml");
		internal static string PathLocal;
		internal static string PathMod()
		{
			string assemblyTitle = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false)).Title;
			string pfid = Singleton<PluginManager>.instance.GetPluginsInfo().FirstOrDefault(z => z.GetInstances<IUserMod>().Length != 0 && z.publishedFileID.AsUInt64 != ulong.MaxValue && z.assembliesString.StartsWith(assemblyTitle))?.publishedFileID.ToString();
			return $"https://steamcommunity.com/sharedfiles/filedetails/?id={pfid}";
		}
	}
	public class Metrics
	{
		public int Slot = default;
		public string Tooltip = string.Empty;
		public Vector3 Position = default;
		public Vector2 Angle = default;
		public float Height = default;
		public float Size = default;
		public float FOV = default;
	}
}