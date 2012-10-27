using System;
using System.Collections.Generic;
using System.IO;

using OpenTK.Audio.OpenAL;

namespace MineEscape.Audio
{
	public class AudioBuffer
	{
		public int ID { get; private set; }
		public int SourceID { get; private set; }

		public ALSourceState State { get; private set; }
		public ALFormat Format { get; private set; }

		public bool Looping { get; set; }

		private double updateTime;

		public AudioBuffer(string path)
		{
			ID = AL.GenBuffer();
			SourceID = AL.GenSource();

			int channels, bps, rate;
			byte[] soundData = LoadWav(new FileStream(path, FileMode.Open), out channels, out bps, out rate);

			Format = GetSoundFormat(channels, bps);
			State = ALSourceState.Initial;

			AL.BufferData(ID, Format, soundData, soundData.Length, rate);
			AL.Source(SourceID, ALSourcei.Buffer, ID);
		}

		public void Play()
		{
			AL.SourcePlay(SourceID);
		}

		public void Pause()
		{
			AL.SourcePause(SourceID);
		}

		public void Stop()
		{
			AL.SourceStop(SourceID);
		}

		public void Update(double time)
		{
			updateTime += time;

			if (updateTime >= .25)
			{
				State = AL.GetSourceState(SourceID);
			}

			if (Looping && State == ALSourceState.Stopped)
			{
				Stop();
				Play();
			}
		}

		public void Unload()
		{
			AL.SourceStop(SourceID);
			AL.DeleteSource(SourceID);
			AL.DeleteBuffer(ID);
		}

		/// <summary>
		/// Loads a .wav file. Taken from the OpenTK example.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="channels"></param>
		/// <param name="bps"></param>
		/// <param name="rate"></param>
		/// <returns></returns>
		private static byte[] LoadWav(Stream stream, out int channels, out int bps, out int rate)
		{
			if (stream == null)
				throw new ArgumentException("No stream!", "stream");

			using (BinaryReader reader = new BinaryReader(stream))
			{

				// RIFF header
				string signature = new string(reader.ReadChars(4));
				if (signature != "RIFF")
					throw new NotSupportedException("Specified stream is not a wave file.");

				int riff_chunck_size = reader.ReadInt32();

				string format = new string(reader.ReadChars(4));
				if (format != "WAVE")
					throw new NotSupportedException("Specified stream is not a wave file.");

				// WAVE header
				string format_signature = new string(reader.ReadChars(4));
				if (format_signature != "fmt ")
					throw new NotSupportedException("Specified wave file is not supported.");

				int format_chunk_size = reader.ReadInt32();
				int audio_format = reader.ReadInt16();
				int num_channels = reader.ReadInt16();
				int sample_rate = reader.ReadInt32();
				int byte_rate = reader.ReadInt32();
				int block_align = reader.ReadInt16();
				int bits_per_sample = reader.ReadInt16();

				string data_signature = new string(reader.ReadChars(4));
				if (data_signature != "data")
					throw new NotSupportedException("Specified wave file is not supported.");

				int data_chunk_size = reader.ReadInt32();

				channels = num_channels;
				bps = bits_per_sample;
				rate = sample_rate;


				return reader.ReadBytes((int)reader.BaseStream.Length);
			}
		}

		/// <summary>
		/// Gets the audio format. Taken from the OpenTK sample.
		/// </summary>
		/// <param name="channels"></param>
		/// <param name="bps"></param>
		/// <returns></returns>
		private static ALFormat GetSoundFormat(int channels, int bps)
		{
			switch (channels)
			{
				case 1:
					return bps == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
				case 2:
					return bps == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
				default:
					throw new NotSupportedException("The specified sound format is not supported.");
			}
		}
	}
}
