using System;

using OpenTK;

using MineEscape.Audio;
using MineEscape.Graphics;

namespace MineEscape.Entities
{
	public class Lift : Entity
	{
		public bool enabled;
		public Source source;

		public Lift(Vector2 position)
			: base(new Mesh(64, 64, Resources.Textures["shaft1.png"]), 0, position, new Vector2(64, 64), 0)
		{
			helpText = "The lift is not powered. Find the generator first!";

			source = new Source();
			source.Buffer = Resources.Audio["lift.wav"];
			source.Relative = true;
		}

		public void Enable()
		{
			enabled = true;
			mesh.texture = Resources.Textures["shaft2.png"];
			helpText = "Using the lift...";
		}
	}
}
