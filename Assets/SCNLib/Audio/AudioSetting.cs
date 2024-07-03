using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SCN.Audio
{
	[CreateAssetMenu(fileName = AssetName, menuName = "SCN/Scriptable Objects/Audio settings")]
	public class AudioSetting : ScriptableObject
	{
        public const string AssetName = "Audio setting";

        [SerializeField] bool isStopSoundOnLoadScene = true;
		[SerializeField] bool isStopMusicOnLoadScene = true;

		public bool IsStopSoundOnLoadScene => isStopSoundOnLoadScene;
		public bool IsStopMusicOnLoadScene => isStopMusicOnLoadScene;

        public void Preload()
		{
            AudioPlayer.Instance.Preload();
		}

#if UNITY_EDITOR
        [MenuItem("SCN/Audio/Settings")]
        static void SelectSettingAudioConfig()
        {
            Master.CreateAndSelectAssetInResource<AudioSetting>(AssetName);
        }
#endif
    }
}