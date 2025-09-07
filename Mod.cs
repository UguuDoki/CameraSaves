using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.UI;
using ICities;
using System.IO;
namespace CameraSaves
{
	public class Mod : IUserMod
	{
		internal static readonly string name = "Camera Saves";
		public string Name => name;
		public string Description => "Helps save multiple cameras per city for teleportation to exact views";
		public void OnSettingsUI(UIHelper helper)
		{
			GlobalXml.LoadGlobal();
			Options.CreateOptionsPanel(helper);
		}
		public class Loader : LoadingExtensionBase
		{
			public override void OnLevelLoaded(LoadMode mode)
			{
				if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame)
				{
					string cityName = Singleton<SimulationManager>.instance.m_metaData.m_CityName;
					Data.PathLocal = Path.Combine(DataLocation.localApplicationData, "CameraSaves_" + cityName + ".xml");
					LocalXml.LoadLocal();
					_ = UIView.GetAView().AddUIComponent(typeof(CameraSaves_Icon));
				}
			}
		}
	}
}