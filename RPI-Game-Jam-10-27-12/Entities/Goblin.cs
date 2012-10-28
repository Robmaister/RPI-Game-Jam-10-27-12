using System;

using OpenTK;

using MineEscape.Audio;
using MineEscape.Graphics;

namespace MineEscape.Entities
{
	public class Goblin : Entity
	{
		public bool hasGoal;
		public Vector2 goalPos;

		private Source source;

		public Goblin(Vector2 position)
			: base(new Mesh(64, 64, Resources.Textures["goblin.png"]), 0, position, new Vector2(64, 64), 200)
		{
			source = new Source();
			source.Buffer = Resources.Audio["goblin.wav"];
			//source.Looping = true;
			source.Relative = true;
		}

		public override void Update(float time)
		{
			base.Update(time);

			Position += Vector2.Normalize(goalPos - position) * moveSpeed * time;
		}

		public void AIUpdate(Player player)
		{
			if ((player.Position - position).LengthFast < 300)
			{
				goalPos = player.Position;
				hasGoal = true;

				if (source.State != OpenTK.Audio.OpenAL.ALSourceState.Playing)
					source.Play();
			}
			else
			{
				if (source.State == OpenTK.Audio.OpenAL.ALSourceState.Playing)
					source.Stop();

				hasGoal = false;
			}
		}
	}
}
