using System;
using System.Drawing;

using OpenTK;
using OpenTK.Input;

namespace MineEscape
{
	public interface IState
	{
		void OnLoad(EventArgs e);
		void OnUpdateFrame(FrameEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse);
		void OnRenderFrame(FrameEventArgs e);
		void OnResize(EventArgs e, Size ClientSize);
		void OnKeyDown(object sender, KeyboardKeyEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse);
		void OnKeyUp(object sender, KeyboardKeyEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse);
		void OnMouseDown(object sender, MouseButtonEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse);
		void OnMouseUp(object sender, MouseButtonEventArgs e, KeyboardDevice Keyboard, MouseDevice Mouse);
		void OnUnload(EventArgs e);
	}
}
