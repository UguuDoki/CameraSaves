using ColossalFramework.UI;
using System.IO;
using System.Reflection;
using UnityEngine;
namespace CameraSaves
{
	internal class CameraSaves_Icon : UIButton
	{
		public override void Start()
		{
			name = "CameraSaves_Icon";
			width = height = Data.IconSide;
			atlas = CameraSaves_IconSprite.CreateAtlas("CameraSaves.Icon.png");
			normalBgSprite = hoveredBgSprite = pressedBgSprite = "CameraSaves_IconSprite";
			hoveredColor = Color.black;
			pressedColor = Data.MidGrey;
			playAudioEvents = true;
			relativePosition = Data.IconInView() ? Options.IconPosition : Data.IconPositionDefault;
		}
		protected override void OnClick(UIMouseEventParameter p)
		{
			if (p.buttons == UIMouseButton.Left)
			{
				Unfocus();
				CameraSaves_Panel panel = (CameraSaves_Panel)UIView.GetAView().FindUIComponent("CameraSaves_Panel");
				if (panel == null)
				{
					_ = UIView.GetAView().AddUIComponent(typeof(CameraSaves_Panel));
				}
				else
				{
					Destroy(panel.gameObject);
				}
			}
		}
		protected override void OnMouseDown(UIMouseEventParameter p)
		{
			if (p.buttons == UIMouseButton.Right)
			{
				Input.compositionCursorPos = absolutePosition;
				BringToFront();
			}
		}
		protected override void OnMouseMove(UIMouseEventParameter p)
		{
			if (p.buttons == UIMouseButton.Right)
			{
				Vector2 pointer = Input.mousePosition;
				pointer.y = UIView.GetAView().fixedHeight - pointer.y;
				absolutePosition = pointer;
				if (!Options.DecouplePanel)
				{
					CameraSaves_Panel panel = (CameraSaves_Panel)UIView.GetAView().FindUIComponent("CameraSaves_Panel");
					if (panel != null)
					{
						Vector2 panelPosition = absolutePosition;
						panelPosition.x = absolutePosition.x + 48;
						panel.absolutePosition = panelPosition;
					}
				}
			}
		}
		protected override void OnMouseUp(UIMouseEventParameter p)
		{
			if (p.buttons == UIMouseButton.Right)
			{
				Options.IconPosition = relativePosition;
				if (Options.DecouplePanel)
				{
					try
					{
						GlobalXml.SaveGlobal();
					}
					catch
					{
						Message.Preset("< Icon Position >");
					}
				}
				else
				{
					CameraSaves_Panel panel = (CameraSaves_Panel)UIView.GetAView().FindUIComponent("CameraSaves_Panel");
					if (panel == null)
					{
						try
						{
							GlobalXml.SaveGlobal();
						}
						catch
						{
							Message.Preset("< Icon Position >");
						}
					}
					else
					{
						Options.PanelPosition = panel.relativePosition;
						try
						{
							GlobalXml.SaveGlobal();
						}
						catch
						{
							Message.Preset("< Icon and Panel Positions >");
						}
					}
				}
			}
		}
		private static class CameraSaves_IconSprite
		{
			internal static UITextureAtlas CreateAtlas(string resourceName)
			{
				UITextureAtlas atlas = ScriptableObject.CreateInstance<UITextureAtlas>();
				atlas.name = "CameraSaves_IconAtlas";
				Texture2D texture2d = LoadEmbeddedTexture(resourceName);
				Material material = new Material(UIView.GetAView().defaultAtlas.material)
				{
					mainTexture = texture2d
				};
				atlas.material = material;
				UITextureAtlas.SpriteInfo sprite = new UITextureAtlas.SpriteInfo
				{
					name = "CameraSaves_IconSprite",
					texture = texture2d,
					region = new Rect(0f, 0f, 1f, 1f),
					border = new RectOffset(0, 0, 0, 0)
				};
				atlas.sprites.Clear();
				atlas.AddSprite(sprite);
				return atlas;
			}
			private static Texture2D LoadEmbeddedTexture(string resourceName)
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				using (Stream stream = assembly.GetManifestResourceStream(resourceName))
				{
					byte[] buffer = new byte[stream.Length];
					_ = stream.Read(buffer, 0, buffer.Length);
					Texture2D texture = new Texture2D(46, 46);
					_ = texture.LoadImage(buffer);
					return texture;
				}
			}
		}
	}
}