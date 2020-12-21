using System;
using System.IO;
using MelonLoader;
using PuppetMasta;
using StressLevelZero;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerRuntimeLib;
using UnhollowerBaseLib;
using StressLevelZero.Props;
using StressLevelZero.AI;
using StressLevelZero.Combat;
using StressLevelZero.Props.Weapons;
using Random = UnityEngine.Random;

using BoneworksModdingToolkit;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Harmony;

namespace ByeByeBones
{
    public static class BuildInfo
    {
        public const string Name = "ByeByeBones ALPHA"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "L4rs"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "0.0.1"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class ByeByeBones : MelonMod
    {
		List<AudioClip> LoadedSnapClips;

        public override void OnApplicationStart()
        {
            ClassInjector.RegisterTypeInIl2Cpp<ByeByeBonesBehavior>();
            //ClassInjector.RegisterTypeInIl2Cpp<Eatingbehaviour>();

            MelonPrefs.RegisterCategory("ByeByeBones", "ByeByeBones");
            MelonPrefs.RegisterFloat("ByeByeBones", "NeckBreakForce", 100.0f);

			LoadedSnapClips = LoadClipsFromDir("UserData\\ByeByeBones\\");

			harmonyInstance.Patch(typeof(AIBrain).GetMethod("Awake"), null, new HarmonyMethod(typeof(ByeByeBones).GetMethod("BrainPatch")));
		}

		public void BrainPatch(AIBrain __instance)
		{
			if (__instance.gameObject.GetComponent<ByeByeBonesBehavior>() == null && !__instance.isDead)
			{
				ByeByeBonesBehavior b = __instance.gameObject.AddComponent<ByeByeBonesBehavior>();
				b.SnapSounds = LoadedSnapClips.ToArray();
			}
		}

        List<AudioClip> LoadClipsFromDir(string soundsPath)
        {
			List<AudioClip> audioClips = new List<AudioClip>();

			string path = Directory.GetCurrentDirectory() + "\\" + soundsPath;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			List<string> list = Directory.GetFiles(path).ToList<string>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].Contains(".wav"))
				{
					list[i] = Path.GetFileName(list[i]);
				}
				else
				{
					list.RemoveAt(i);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				string wavText = Path.GetFileNameWithoutExtension(list[j]);
				wavText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(wavText);
				byte[] wav = File.ReadAllBytes(soundsPath + list[j]);
				WAV wav2 = new WAV(wav);
				bool failedContructor = wav2.failedContructor;
				if (failedContructor)
				{
					MelonLogger.LogError("Could not read " + list[j]);
				}
				else
				{
					AudioClip audioClip = AudioClip.Create(wavText, wav2.SampleCount, 1, wav2.Frequency, false);
					audioClip.SetData(wav2.LeftChannel, 0);
					if (audioClip != null)
					{
						audioClips.Add(audioClip);
						MelonLogger.Log("Added " + list[j]);
					}
					else
					{
						MelonLogger.LogError("Failed To Read " + list[j]);
					}
				}
			}

			return audioClips;
		}
    }
}