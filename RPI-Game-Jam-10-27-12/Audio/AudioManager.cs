using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;


namespace MineEscape.Audio
{
	public static class AudioManager
	{
		private static AudioContext context;
		private static List<Source> globalSources;

		static AudioManager()
		{
			try
			{
				context = new AudioContext();
			}
			catch
			{
				throw new Exception("OpenAL failed to initialize. Do you have it installed?");
			}

			globalSources = new List<Source>();
		}

		public static Vector3 ListenerPos
		{
			get
			{
				Vector3 pos;
				AL.GetListener(ALListener3f.Position, out pos);
				return pos;
			}
			set
			{
				AL.Listener(ALListener3f.Position, ref value);
			}
		}

		public static void Init()
		{
		}

		public static void PlayMusic(AudioBuffer buffer)
		{
			Source s = new Source();
			s.Buffer = buffer;
			s.Looping = true;
			s.Relative = true;
			s.Play();
			globalSources.Add(s);
		}
	}
}
