using System;
using System.Collections.Generic;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using MineEscape.Graphics;
using MineEscape.Entities;

namespace MineEscape.States
{
	public class GameState : IState
	{
		private Camera camera;
		private Player player;
		private Pickaxe pickaxe;
		private LevelMap map;

		public void OnLoad(EventArgs e)
		{
			camera = new Camera();
			camera.LoadProjection();

			player = new Player();
			pickaxe = new Pickaxe(new Vector2(120, 120), MathHelper.PiOver6 * 1.2f);
			map = new LevelMap(Resources.Textures["testmap.png"].Size.Width, Resources.Textures["testmap.png"].Size.Height, Resources.Textures["testmap.png"]);

			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		public void OnUpdateFrame(FrameEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
			float time = (float)e.Time;
			bool up = Keyboard[Key.W], left = Keyboard[Key.A], down = Keyboard[Key.S], right = Keyboard[Key.D];

			Vector2 previousSpot = player.Position;

			if (up && left)
				player.MoveUpLeft(time);
			else if (up && right)
				player.MoveUpRight(time);
			else if (down && left)
				player.MoveDownLeft(time);
			else if (down && right)
				player.MoveDownRight(time);
			else if (up)
				player.MoveUp(time);
			else if (left)
				player.MoveLeft(time);
			else if (down)
				player.MoveDown(time);
			else if (right)
				player.MoveRight(time);

			if (map.IsColliding(player.BoundingBox))
				player.Position = previousSpot;

			camera.Position = player.Position;

			if (Keyboard[Key.Escape])
				StateManager.PopState();
		}

		public void OnRenderFrame(FrameEventArgs e)
		{
			GL.PushMatrix();
			//GL.Enable(EnableCap.Texture2D);

			camera.LoadView();

			map.Draw();
			player.Draw();
			pickaxe.Draw();

			//GL.Disable(EnableCap.Texture2D);
			GL.PopMatrix();
		}

		public void OnResize(EventArgs e, Size ClientSize)
		{
		}

		public void OnKeyDown(object sender, KeyboardKeyEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
		}

		public void OnKeyUp(object sender, KeyboardKeyEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
		}

		public void OnMouseDown(object sender, MouseButtonEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
		}

		public void OnMouseUp(object sender, MouseButtonEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
		}

		public void OnUnload(EventArgs e)
		{
		}
	}
}
