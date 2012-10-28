using System;
using System.Collections.Generic;

using OpenTK;

using MineEscape.Audio;
using MineEscape.Graphics;

namespace MineEscape.Entities
{
	public class Generator : Entity
	{
		private bool used;

		private Source source;

		public Generator(Vector2 position)
			: base(new Mesh(128, 103, Resources.Textures["generatoroff.png"]), 0, position, new Vector2(128, 64), 0)
		{
			helpText = "Press E to active the generator.";

			source = new Source();
			source.Buffer = Resources.Audio["generator.wav"];
			source.Relative = true;
			//source.Position = new Vector3(Position);
		}

		public bool Used
		{
			get { return used; }
			set
			{
				used = value;
				if (used)
				{
					helpText = "The generator has been activated!";
					mesh.texture = Resources.Textures["generatoron.png"];
					source.Play();
				}
			}
		}
	}
}
