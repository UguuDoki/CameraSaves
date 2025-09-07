using ColossalFramework.Plugins;
using ColossalFramework.UI;
using UnityEngine;
namespace CameraSaves
{
	internal class Message
	{
		private static readonly string prefix = "[" + Mod.name + "] ";
		internal static void Custom(string msg)
		{
			UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(prefix, msg, error: false);
		}
		internal static void Preset(string subject)
		{
			string msg =
				subject + "\n\n" +
				"Failing to " + (
					subject.StartsWith("#") ? "load data" :
					subject.StartsWith("[") ? "change option" :
					subject.StartsWith("<") ? "save new position" :
					subject.StartsWith("*") ? "save tooltip" :
					subject.StartsWith("-") ? "save camera" :
					subject.StartsWith("=") ? "delete camera" : "complete task") +
				" at the moment.\n\n" +
				"There can be various reasons.\n\n" +
				"Most likely due to system I/O or permission issues.";
			UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(prefix, msg, error: true);
		}
		internal static void F7(string msg)
		{
			DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, prefix + msg);
		}
		internal static void Output_log(string msg)
		{
			Debug.Log(prefix + msg);
		}
	}
}