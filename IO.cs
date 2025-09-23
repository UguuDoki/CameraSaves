using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
namespace CameraSaves
{
	[Serializable]
	public class GlobalXml
	{
		public V2Surrogate IconPosition;
		public V2Surrogate PanelPosition;
		public bool DecouplePanel;
		public bool OpaquePanel;
		public int Slots;
		public bool AlertDeletion;
		public bool NotifyDeletion;
		public bool EnableTooltiping;
		internal static void LoadGlobal()
		{
			if (File.Exists(Data.PathGlobal))
			{
				try
				{
					XmlSerializer serializer = new XmlSerializer(typeof(GlobalXml));
					using (FileStream fs = new FileStream(Data.PathGlobal, FileMode.Open))
					{
						GlobalXml xml = (GlobalXml)serializer.Deserialize(fs);
						Options.IconPosition = xml.IconPosition.ToVector2();
						Options.PanelPosition = xml.PanelPosition.ToVector2();
						Options.DecouplePanel = xml.DecouplePanel;
						Options.OpaquePanel = xml.OpaquePanel;
						Options.Slots = xml.Slots;
						Options.AlertDeletion = xml.AlertDeletion;
						Options.NotifyDeletion = xml.NotifyDeletion;
						Options.EnableTooltiping = xml.EnableTooltiping;
					}
				}
				catch
				{
					Message.Preset("# Global Data #");
				}
			}
		}
		internal static void SaveGlobal()
		{
			GlobalXml xml = new GlobalXml
			{
				IconPosition = V2Surrogate.FromVector2(Options.IconPosition),
				PanelPosition = V2Surrogate.FromVector2(Options.PanelPosition),
				DecouplePanel = Options.DecouplePanel,
				OpaquePanel = Options.OpaquePanel,
				Slots = Options.Slots,
				AlertDeletion = Options.AlertDeletion,
				NotifyDeletion = Options.NotifyDeletion,
				EnableTooltiping = Options.EnableTooltiping,
			};
			XmlSerializer serializer = new XmlSerializer(typeof(GlobalXml));
			using (FileStream fs = new FileStream(Data.PathGlobal, FileMode.Create))
			{
				serializer.Serialize(fs, xml);
			}
		}
	}
	[Serializable]
	public class LocalXml
	{
		public List<Metrics> MetricsList;
		internal static void LoadLocal()
		{
			if (File.Exists(Data.PathLocal))
			{
				try
				{
					XmlSerializer serializer = new XmlSerializer(typeof(LocalXml));
					using (FileStream fs = new FileStream(Data.PathLocal, FileMode.Open))
					{
						LocalXml xml = (LocalXml)serializer.Deserialize(fs);
						Data.MetricsList = xml.MetricsList ?? new List<Metrics>();
					}
				}
				catch
				{
					Message.Preset("# Local Data #");
				}
			}
		}
		internal static void SaveLocal()
		{
			LocalXml xml = new LocalXml
			{
				MetricsList = Data.MetricsList ?? new List<Metrics>()
			};
			XmlSerializer serializer = new XmlSerializer(typeof(LocalXml));
			using (FileStream fs = new FileStream(Data.PathLocal, FileMode.Create))
			{
				serializer.Serialize(fs, xml);
			}
		}
	}
	[Serializable]
	public class V2Surrogate
	{
		public readonly float x;
		public readonly float y;
		public V2Surrogate()
		{
		}
		public V2Surrogate(Vector2 v2)
		{
			x = v2.x;
			y = v2.y;
		}
		internal Vector2 ToVector2()
		{
			return new Vector2(x, y);
		}
		public static V2Surrogate FromVector2(Vector2 v2)
		{
			return new V2Surrogate(v2);
		}
	}
}