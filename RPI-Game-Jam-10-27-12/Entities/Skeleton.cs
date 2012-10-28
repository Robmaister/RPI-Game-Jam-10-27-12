using System;

using OpenTK;

using MineEscape.Graphics;

namespace MineEscape.Entities
{
	public class Skeleton : Entity
	{
		public Skeleton(int type, Vector2 position)
			: base(new Mesh(64, 64, Resources.Textures["skeleton" + (type + 1) + ".png"]), 0, position, new Vector2(64, 64), 0)
		{
			switch (type)
			{
				case 0:
					helpText = "His name tag says \"Robert Paulson\"";
					break;
				case 1:
					helpText = "His driver's license says he's an organ donor from Hawaii.";
					break;
				case 2:
					helpText = "He was kind of a big deal...";
					break;
			}
		}
	}
}
