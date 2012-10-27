using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MineEscape
{
	public static class StateManager
	{
		private static Queue<StateAction> actions = new Queue<StateAction>();

		public static void UpdateStateStack(Stack<IState> stack)
		{
			foreach (var action in actions)
			{
				if (action.adding)
				{
					var state = action.state;
					state.OnLoad(new EventArgs());
					stack.Push(action.state);
				}
				else
				{
					stack.Pop().OnUnload(new EventArgs());
				}
			}

			actions.Clear();
		}

		public static void PushState(IState state)
		{
			actions.Enqueue(new StateAction(state, true));
		}

		public static void PopState()
		{
			actions.Enqueue(new StateAction(null, false));
		}

		private class StateAction
		{
			public IState state;
			public bool adding;

			public StateAction()
			{
			}

			public StateAction(IState state, bool adding)
			{
				this.state = state;
				this.adding = adding;
			}
		}
	}
}
