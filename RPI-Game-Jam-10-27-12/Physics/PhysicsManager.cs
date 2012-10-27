using System;

using OpenTK;

using MineEscape.Entities;

namespace MineEscape.Physics
{
	public static class PhysicsManager
	{
		public static bool IsColliding(Entity ent1, Entity ent2)
		{
			return IsColliding(ent1.BoundingBox, ent2.BoundingBox);
		}

		public static bool IsColliding(AABB box1, AABB box2)
		{
			if (box1.Left > box2.Right) return false;
			if (box1.Right < box2.Left) return false;
			if (box1.Top < box2.Bottom) return false;
			if (box1.Bottom > box2.Top) return false;

			return true;
		}

		/// <summary>
		/// Minimum vector for ent1 to leave ent2
		/// </summary>
		/// <param name="ent1"></param>
		/// <param name="ent2"></param>
		/// <returns></returns>
		public static Vector2 ReactCollision(Entity ent1, Entity ent2)
		{
			Vector2 sizes = ent1.Size + ent2.Size;
			Vector2 colDist = ent1.Position - ent2.Position;

			//abs
			if (colDist.X < 0) colDist.X = -colDist.X;
			if (colDist.Y < 0) colDist.Y = -colDist.Y;

			colDist += (sizes / 2);

			//(combined sizes) - (combined world sizes from one corner to the other) = minimum distance to exit
			Vector2 minMoveDist = sizes - colDist;

			//set the proper direction for movement
			if (ent1.Position.X <= ent2.Position.X) minMoveDist.X = -minMoveDist.X;
			if (ent1.Position.Y <= ent2.Position.Y) minMoveDist.Y = -minMoveDist.Y;

			//use only the smaller value (vertex-vertex collisions snap to one axis or the other, left/right axes if equal)
			if (Math.Abs(minMoveDist.X) > Math.Abs(minMoveDist.Y))
				minMoveDist.X = 0;
			else
				minMoveDist.Y = 0;

			return minMoveDist;
		}
	}
}
