using System;

namespace MineEscape.Physics
{
	public class AABB
	{
		private readonly float left, right, top, bottom;

		public float Left { get { return left; } }
		public float Right { get { return right; } }
		public float Top { get { return top; } }
		public float Bottom { get { return bottom; } }

		public AABB(float left, float right, float top, float bottom)
		{
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}
	}
}
