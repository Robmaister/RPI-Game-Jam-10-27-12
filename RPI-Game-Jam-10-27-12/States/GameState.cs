using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using MineEscape.Audio;
using MineEscape.Graphics;
using MineEscape.Entities;
using MineEscape.Physics;
using QuickFont;

namespace MineEscape.States
{
	public class GameState : IState
	{
		public const int maxLevel = 5;

		private Camera camera;
		private Player player;
		private Generator generator;
		private Lift lift;
		private List<Entity> entities;
		private LevelMap map;

		private Entity lastCollidedWith;

		private Mesh fadeIn;
		private float fadePercent;
		private bool fadingIn, fadingOut;

		private QFont helpFont;

		private int level;
		private MenuState loseState;

		public GameState(int level)
		{
			this.level = level;
			this.loseState = new MenuState("You lost", "\n\n\nTry again?");
		}

		public void OnLoad(EventArgs e)
		{
			camera = new Camera();
			camera.LoadProjection();
			entities = new List<Entity>();

			player = new Player();
			map = new LevelMap(new Texture("Resources/Levels/" + level + ".png"), new Texture("Resources/Levels/" + level + "_col.png"));

			helpFont = new QFont("Resources/Fonts/anarchysans.ttf", 22);
			helpFont.Options.Colour = Color4.DarkRed;

			using (StreamReader reader = new StreamReader("Resources/Levels/" + level + ".txt"))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					string[] values = line.Split(' ');
					Vector2 pos = new Vector2(int.Parse(values[1]), int.Parse(values[2]));

					switch (values[0])
					{
						case "Spawn":
							player.Position = pos;
							player.Position = pos;
							break;
						case "Generator":
							generator = new Generator(pos);
							entities.Add(generator);
							break;
						case "Lift":
							lift = new Lift(pos);
							entities.Add(lift);
							break;
						case "Pickaxe":
							entities.Add(new Pickaxe(pos, 0, true));
							break;
						case "PickaxeB":
							entities.Add(new Pickaxe(pos, 0, false));
							break;
						case "Skeleton":
							entities.Add(new Skeleton(0, pos));
							break;
						case "Goblin":
							entities.Add(new Goblin(pos));
							break;
						case "GameEnd":
							entities.Add(new Endgame(pos));
							break;
					}
				}
			}

			fadeIn = new Mesh(1024, 768, Texture.Zero);
			fadePercent = 1f;
			fadingIn = true;

			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		public void OnUpdateFrame(FrameEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
			if (fadingIn)
			{
				fadePercent -= (float)e.Time;
				if (fadePercent <= 0)
					fadingIn = false;
			}

			if (fadingOut)
			{
				fadePercent += (float)e.Time;
				if (fadePercent >= 1)
				{
					StateManager.PopState();
					if (level + 1 > maxLevel)
						StateManager.PushState(new MenuState("You escaped!", "\n\n\nPlay again?"));
					else
						StateManager.PushState(new GameState(level + 1));
				}
			}

			float time = (float)e.Time;
			bool up = Keyboard[Key.W], left = Keyboard[Key.A], down = Keyboard[Key.S], right = Keyboard[Key.D];

			player.Update(time);

			Entity prevCollidedWith = lastCollidedWith;
			lastCollidedWith = null;

			Vector2 realPrevPos = player.Position;

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
			else
			{
				lastCollidedWith = prevCollidedWith; //if no key was pressed, restore the old one. HACK
				player.moving = false;
			}

			if (map.IsColliding(player.BoundingBox))
				player.Position = realPrevPos;

			foreach (var ent in entities)
			{
				if (ent is Pickaxe)
					((Pickaxe)ent).AIUpdate(player);

				if (ent is Goblin)
					((Goblin)ent).AIUpdate(player);
			}

			for (int i = entities.Count - 1; i >= 0; i--)
			{
				Entity ent = entities[i];

				ent.Update(time);
				if (map.IsColliding(ent.BoundingBox))
					ent.ResetPos();

				if (PhysicsManager.IsColliding(player.BoundingBox, ent.BoundingBox))
				{
					lastCollidedWith = ent;

					if (ent is Generator)
					{
						player.Position = realPrevPos;
					}
					else if (ent is Lift)
					{
						if (((Lift)ent).enabled)
						{
							((Lift)ent).source.Play();
							fadingOut = true;
						}
					}
					else if (ent is Pickaxe)
					{
						if (((Pickaxe)ent).active)
						{
							entities.Remove(ent);
							player.health--;
						}
					}
					else if (ent is Goblin)
					{
						entities.Remove(ent);
						player.health -= 2;
					}
					else if (ent is Endgame)
					{
						fadingOut = true;
					}
				}
			}

			if (Keyboard[Key.E] && lastCollidedWith != null)
			{
				if (lastCollidedWith is Generator)
				{
					((Generator)lastCollidedWith).Used = true;
					player.drawShadow = false;
					lift.Enable();
				}
			}

			camera.Position = player.Position;
			AudioManager.ListenerPos = new Vector3(player.Position);

			if (generator != null && generator.Used)
			{
				for (int i = entities.Count - 1; i >= 0; i--)
				{
					var ent = entities[i];
					if (ent is Goblin)
						entities.Remove(ent);
					if (ent is Pickaxe)
						((Pickaxe)ent).active = false;
				}
			}

			if (player.health <= 0)
			{
				StateManager.PopState();
				StateManager.PushState(loseState);
			}
		}

		public void OnRenderFrame(FrameEventArgs e)
		{
			GL.PushMatrix();

			camera.LoadView();

			map.Draw();

			foreach (var ent in entities)
				ent.Draw();

			player.Draw();

			GL.PopMatrix();

			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			var projection = Matrix4.CreateOrthographicOffCenter(-512, 512, 384, -384, 0, 1);
			GL.LoadMatrix(ref projection);
			GL.MatrixMode(MatrixMode.Modelview);

			if (lastCollidedWith != null && !string.IsNullOrEmpty(lastCollidedWith.helpText))
			{
				GL.PushMatrix();
				GL.Translate(-500, -360, 0);
				helpFont.Print(lastCollidedWith.helpText);
				GL.PopMatrix();
			}

			GL.PushMatrix();
			GL.Translate(-500, 340, 0);
			helpFont.Print("Health: " + player.health);
			GL.PopMatrix();
			GL.Color4(1f, 1f, 1f, 1f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
			GL.MatrixMode(MatrixMode.Modelview);

			if (fadingIn || fadingOut)
			{
				GL.Color4(0f, 0f, 0f, fadePercent);
				fadeIn.Draw();
				GL.Color4(1f, 1f, 1f, 1f);
			}
		}

		public void OnResize(EventArgs e, Size ClientSize)
		{
		}

		public void OnKeyDown(object sender, KeyboardKeyEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse)
		{
			if (e.Key == Key.Escape)
				StateManager.ClearStates();
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
			if (player.walkSource.State == OpenTK.Audio.OpenAL.ALSourceState.Playing)
				player.walkSource.Stop();
		}
	}
}
