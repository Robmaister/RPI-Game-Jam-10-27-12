using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using MineEscape.States;

namespace MineEscape
{
	public class EscapeGameWindow : GameWindow
	{
		#region Main

		static void Main(string[] args)
		{
			MenuState startMenu = new MenuState("MineEscape", "\n\nRobert Rouhani, Craig Carlson, Domenic Cristaldi\n\nRPI Game Dev Club Gamejam 10/27/2012\n\n\n\nPress any key to continue...");
			MenuState objectiveMenu = new MenuState("Objective", "\nA miner has accidentally unleashed an evil force deep in the mines.\nYou are the lone survivor who must escape the cave.\nFind the generator to activate power for the floor.\nEscape through the lift.\n\n\n\nPress any key to continue...");
			MenuState controlsMenu = new MenuState("Controls", "\nWASD to move\n\nE to interact\n\nEsc to quit\n\n\nPress any key to continue...");
			startMenu.OnPopped +=
				(s, e) =>
				{
					startMenu.Reset();
					StateManager.PushState(startMenu); //reset and loop
					StateManager.PushState(objectiveMenu);
				};
			objectiveMenu.OnPopped +=
				(s, e) =>
				{
					StateManager.PushState(controlsMenu);
				};
			controlsMenu.OnPopped += (s, e) => StateManager.PushState(new GameState(1));
			StateManager.PushState(startMenu);
			using (var window = new EscapeGameWindow())
			{
				window.Run();
			}

		}

		#endregion

		private Stack<IState> states;

		public EscapeGameWindow()
			: base(1024, 768, new GraphicsMode(new ColorFormat(24), 0, 8))
		{
			this.WindowBorder = OpenTK.WindowBorder.Fixed;
			states = new Stack<IState>();
			Title = "MineEscape";
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Audio.AudioManager.Init();

			Resources.LoadAll();

			Audio.AudioManager.PlayMusic(Resources.Audio["background.wav"]);

			GL.ClearColor(Color4.Black);

			Keyboard.KeyDown += OnKeyDown;
			Keyboard.KeyDown += OnKeyUp;
			Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonUp);
			Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonDown);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			this.Title = "FPS - " + 1f / (float)e.Time;

			StateManager.UpdateStateStack(states);

			if (states.Count > 0)
				states.Peek().OnUpdateFrame(e, Keyboard, Mouse);
			else
				Exit();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Clear(ClearBufferMask.ColorBufferBit);

			if (states.Count > 0)
				states.Peek().OnRenderFrame(e);

			SwapBuffers();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (states.Count > 0)
				states.Peek().OnResize(e, ClientSize);
		}

		protected void OnKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (states.Count > 0)
				states.Peek().OnKeyDown(this, e, Keyboard, Mouse);
		}

		protected void OnKeyUp(object sender, KeyboardKeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (states.Count > 0)
				states.Peek().OnKeyUp(this, e, Keyboard, Mouse);
		}

		void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (states.Count > 0)
				states.Peek().OnMouseDown(this, e, Keyboard, Mouse);
		}

		void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (states.Count > 0)
				states.Peek().OnMouseUp(this, e, Keyboard, Mouse);
		}

		protected override void OnUnload(EventArgs e)
		{
			base.OnUnload(e);

			foreach (var state in states)
				state.OnUnload(e);

			states.Clear();
		}
	}
}
