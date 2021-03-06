﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ByeByeBones
{
	public class WAV
	{
		private static float bytesToFloat(byte firstByte, byte secondByte)
		{
			short num = (short)((int)secondByte << 8 | (int)firstByte);
			return (float)num / 32768f;
		}

		private static int bytesToInt(byte[] bytes, int offset = 0)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				num |= (int)bytes[offset + i] << i * 8;
			}
			return num;
		}

		private static byte[] GetBytes(string filename)
		{
			return File.ReadAllBytes(filename);
		}
		public float[] LeftChannel { get; internal set; }

		public float[] RightChannel { get; internal set; }

		public int ChannelCount { get; internal set; }

		public int SampleCount { get; internal set; }

		public int Frequency { get; internal set; }

		public bool failedContructor { get; internal set; }

		public WAV(string filename) : this(WAV.GetBytes(filename))
		{
		}

		public WAV(byte[] wav)
		{
			try
			{
				this.ChannelCount = (int)wav[22];
				this.Frequency = WAV.bytesToInt(wav, 24);
				int i = 12;
				while (wav[i] != 100 || wav[i + 1] != 97 || wav[i + 2] != 116 || wav[i + 3] != 97)
				{
					i += 4;
					int num = (int)wav[i] + (int)wav[i + 1] * 256 + (int)wav[i + 2] * 65536 + (int)wav[i + 3] * 16777216;
					i += 4 + num;
				}
				i += 8;
				this.SampleCount = (wav.Length - i) / 2;
				bool flag = this.ChannelCount == 2;
				if (flag)
				{
					this.SampleCount /= 2;
				}
				this.LeftChannel = new float[this.SampleCount];
				bool flag2 = this.ChannelCount == 2;
				if (flag2)
				{
					this.RightChannel = new float[this.SampleCount];
				}
				else
				{
					this.RightChannel = null;
				}
				int num2 = 0;
				while (i < wav.Length)
				{
					this.LeftChannel[num2] = WAV.bytesToFloat(wav[i], wav[i + 1]);
					i += 2;
					bool flag3 = this.ChannelCount == 2;
					if (flag3)
					{
						this.RightChannel[num2] = WAV.bytesToFloat(wav[i], wav[i + 1]);
						i += 2;
					}
					num2++;
				}
			}
			catch
			{
				this.failedContructor = true;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022C4 File Offset: 0x000004C4
		public override string ToString()
		{
			return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", new object[]
			{
			this.LeftChannel,
			this.RightChannel,
			this.ChannelCount,
			this.SampleCount,
			this.Frequency
			});
		}
	}
}
