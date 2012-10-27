using System;

using OpenTK;

using MineEscape.Graphics;

namespace MineEscape.Entities
{
	public class Pickaxe : Entity
	{
		public Pickaxe(Vector2 position, float angle)
			: base(new Mesh(32, 32, Resources.Textures["TempPlayer.png"]), angle, position, new Vector2(32, 32), 0)
		{
		}
	}
}
