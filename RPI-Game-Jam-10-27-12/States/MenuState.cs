using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using QuickFont;

using MineEscape.Graphics;

namespace MineEscape.States
{
	public class MenuState : IState
	{
		private string title, subtitle;
		private QFont titleFont, subtitleFont;

		public event EventHandler OnPopped;

		private bool fading;
		private float fadePercent;
		private Mesh fadeMesh;

		public MenuState(string title, string subtitle)
		{
			this.title = title;
			this.subtitle = subtitle;
			fadeMesh = new Mesh(1024, 768, Texture.Zero);
			fadePercent = 1f;
		}

		public void OnLoad(EventArgs e)
		{
			titleFont = new QFont(@".\Resources\Fonts\anarchysans.ttf", 40);
			subtitleFont = new QFont(@".\Resources\Fonts\anarchysans.ttf", 26);
			titleFont.Options.Colour = Color4.Red;
			subtitleFont.Options.Colour = Color4.Red;

			GL.ClearColor(Color.Black);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		public void OnUpdateFrame(FrameEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
			if (fading)
			{
				fadePercent += (float)e.Time;

				if (fadePercent >= 1)
				{
					StateManager.PopState();
					if (OnPopped != null)
						OnPopped(this, null);
				}
			}
			else
			{
				fadePercent -= (float)e.Time;
				if (fadePercent <= 0f)
					fadePercent = 0f;
			}

		}

		public void OnRenderFrame(FrameEventArgs e)
		{
			GL.MatrixMode(MatrixMode.Projection);
			var proj = Matrix4.CreateOrthographicOffCenter(-512, 512, 384, -384, 0, 1);
			GL.LoadMatrix(ref proj);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref Matrix4.Identity);

			GL.PushMatrix();
			GL.Translate(0, -60, 0);
			titleFont.Print(title, QFontAlignment.Centre);
			GL.PopMatrix();

			GL.PushMatrix();
			GL.Translate(0, 40, 0);
			subtitleFont.Print(subtitle, QFontAlignment.Centre);
			GL.PopMatrix();

			GL.Color4(new Color4(0f, 0f, 0f, fadePercent));

			fadeMesh.Draw();

			GL.Color4(new Color4(1f, 1f, 1f, 1f));
		}

		public void OnResize(EventArgs e, Size ClientSize)
		{
		}

		public void OnKeyDown(object sender, KeyboardKeyEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
			fading = true;
		}

		public void OnKeyUp(object sender, KeyboardKeyEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
		}

		public void OnMouseDown(object sender, MouseButtonEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
			fading = true;
		}

		public void OnMouseUp(object sender, MouseButtonEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
		}

		public void OnUnload(EventArgs e)
		{
			titleFont.Dispose();
			subtitleFont.Dispose();
		}

		public void Reset()
		{
			fading = false;
		}
	}
}
