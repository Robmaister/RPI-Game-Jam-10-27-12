using System;

using OpenTK;
using OpenTK.Audio.OpenAL;

namespace MineEscape.Audio
{
	public class Source
	{
		private int id;

		public Source()
		{
			id = AL.GenSource();
		}

		public Vector3 Position
		{
			get
			{
				Vector3 position;
				AL.GetSource(id, ALSource3f.Position, out position);
				return position;
			}
			set
			{
				AL.Source(id, ALSource3f.Position, ref value);
			}
		}

		public float Gain
		{
			get
			{
				float value;
				AL.GetSource(id, ALSourcef.Gain, out value);
				return value;
			}

			set
			{
				AL.Source(id, ALSourcef.Gain, value);
			}
		}

		public bool Looping
		{
			get
			{
				bool value;
				AL.GetSource(id, ALSourceb.Looping, out value);
				return value;
			}

			set
			{
				AL.Source(id, ALSourceb.Looping, value);
			}
		}

		public bool Relative
		{
			get
			{
				bool value;
				AL.GetSource(id, ALSourceb.SourceRelative, out value);
				return value;
			}

			set
			{
				AL.Source(id, ALSourceb.SourceRelative, value);
			}
		}

		public ALSourceState State
		{
			get
			{
				return AL.GetSourceState(id);
			}
			set
			{
				AL.Source(id, (ALSourcei)ALGetSourcei.SourceState, (int)value);
			}
		}

		public AudioBuffer Buffer
		{
			get
			{
				return null; //HACK need a getter to work, just return null.
			}
			set
			{
				AL.Source(id, ALSourcei.Buffer, value.ID);
			}
		}

		public void Play()
		{
			AL.SourcePlay(id);
		}

		public void Pause()
		{
			AL.SourcePause(id);
		}

		public void Stop()
		{
			AL.SourceStop(id);
		}

		public void Unload()
		{
			AL.SourceStop(id);
			AL.DeleteSource(id);
		}
	}
}
