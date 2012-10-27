using System;
using System.Collections.Generic;

using OpenTK;

using MineEscape.Graphics;
using MineEscape.Physics;

namespace MineEscape.Entities
{
	public class Player : Entity
	{
		public Player()
			: base(new Mesh(64, 64, Resources.Textures["TempPlayer.png"]), 0, new Vector2(2000, 1500), new Vector2(64, 64), 256)
		{
		}
	}
}
