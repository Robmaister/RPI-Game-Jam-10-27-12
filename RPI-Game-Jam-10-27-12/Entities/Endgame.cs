using System;

using OpenTK;

using MineEscape.Graphics;

namespace MineEscape.Entities
{
	public class Endgame : Entity
	{
		public Endgame(Vector2 position)
			: base(new Mesh(0, 0, Texture.Zero), 0, position, new Vector2(512, 512), 0)
		{
		}
	}
}
