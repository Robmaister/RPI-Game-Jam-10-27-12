using System;

using OpenTK;

using MineEscape.Audio;
using MineEscape.Graphics;

namespace MineEscape.Entities
{
	public class Pickaxe : Entity
	{
		public int health;
		public bool hasGoal, active;
		public Vector2 goalPosition;

		private Source source;

		public Pickaxe(Vector2 position, float angle, bool active)
			: base(new Mesh(32, 32, Resources.Textures["pickaxe.png"]), angle, position, new Vector2(32, 32), 250)
		{
			health = 10;

			source = new Source();
			source.Buffer = Resources.Audio["pickaxe.wav"];
			source.Relative = true;

			this.active = active;
		}

		public override void Update(float time)
		{
			base.Update(time);

			if (active && hasGoal)
			{
				angle += MathHelper.TwoPi * time;

				Vector2 dir = goalPosition - position;

				if (dir.LengthFast < moveSpeed * time)
				{
					position = goalPosition;
					hasGoal = false;
				}

				Position += Vector2.Normalize(dir) * moveSpeed * time;
			}

			if (health <= 0)
				mesh.texture = Resources.Textures["pickaxe_broken.png"];
		}

		public void AIUpdate(Player player)
		{
			if (active && (player.Position - position).LengthFast < 500 && !hasGoal)
			{
				goalPosition = player.Position;
				hasGoal = true;
				source.Play();
			}
		}
	}
}
