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
			StateManager.PushState(new GameState());
			using (var window = new EscapeGameWindow())
			{
				window.Run();
			}

		}

		#endregion

		private Stack<IState> states;

		public EscapeGameWindow()
			: base(1024, 768)
		{
			states = new Stack<IState>();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Resources.LoadAll();
			GL.ClearColor(Color4.CornflowerBlue);

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

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (states.Count > 0)
				states.Peek().OnKeyDown(this, e, Keyboard, Mouse);
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e)
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
