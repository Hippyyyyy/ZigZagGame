using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace SCN.Audio
{
    [System.Serializable]
    public class Audio
    {
        [SerializeField] AudioSource source;
        [SerializeField] Coroutine corou;

        public string NameClip { get => source.clip.name; }
        public AudioSource Source { get => source; set => source = value; }
        public Coroutine Corou { get => corou; set => corou = value; }
    }

    public class AudioPlayer : MonoBehaviour
    {
        static AudioPlayer instance;
        public static AudioPlayer Instance
        {
            get
            {
                if (instance == null)
                    Setup();

                return instance;
            }
        }

        AudioSetting setting;

        static void Setup()
        {
            instance = Instantiate(LoadSource.LoadObject<GameObject>("Audio player")
                , DDOL.Instance.transform).GetComponent<AudioPlayer>();

            instance.setting = LoadSource.LoadObject<AudioSetting>(AudioSetting.AssetName);

            LoadSceneManager.BeforeExitScene += () =>
            {
				if (instance.setting.IsStopSoundOnLoadScene)
				{
                    instance.StopSoundList();
                }

				if (instance.setting.IsStopMusicOnLoadScene)
				{
                    instance.StopMusicList();
                }
            };
        }

        [SerializeField] List<Audio> listCurrentSound = new List<Audio>();
        [SerializeField] List<Audio> listCurrentMusic = new List<Audio>();

        [SerializeField] List<Audio> listAvailableAud = new List<Audio>();

        [SerializeField] AudioMixer[] audioMixer = new AudioMixer[2];
        [SerializeField] AudioMixerGroup[] audioMixerGroups = new AudioMixerGroup[2];

        Audio GetAvailableAudio(bool isSound)
        {
            if (listAvailableAud.Count > 0)
            {
                var audS = listAvailableAud[0];
                listAvailableAud.RemoveAt(0);

                audS.Source.outputAudioMixerGroup = audioMixerGroups[isSound ? 0 : 1];

                return audS;
            }
            else
            {
                var audS = new Audio
                {
                    Source = instance.gameObject.AddComponent<AudioSource>()
                };

                audS.Source.playOnAwake = false;
                listAvailableAud.Add(audS);

                return GetAvailableAudio(isSound);
            }
        }

        public void Preload()
		{

		}

        public void PlaySound(AudioClip audioClip, bool loop = false, bool stopAllSound = false
            , System.Action onDone = null, bool ignoreIfPlaying = false, bool canStackPlay = false)
        {
            if (stopAllSound)
            {
                StopSoundList();
            }
            if (ignoreIfPlaying && CheckSoundPlay(audioClip))
            {
                return;
            }

            var aud = GetAvailableAudio(true);
            aud.Source.clip = audioClip;

            if (!canStackPlay)
            {
                StopSound(aud.NameClip);
            }

            instance.listCurrentSound.Add(aud);
            PlayAud(aud, audioClip, loop, () =>
            {
                _ = instance.listCurrentSound.Remove(aud);
                RemoveAudio(aud);
                onDone?.Invoke();
            });
        }

        public void PlayMusic(AudioClip audioClip, bool loop = false, bool stopAllMusic = false
            , System.Action onDone = null, bool ignoreIfPlaying = false, bool canStackPlay = false)
        {
            if (stopAllMusic)
            {
                StopMusicList();
            }
            if (ignoreIfPlaying && CheckMusicPlay(audioClip))
            {
                return;
            }

            var aud = GetAvailableAudio(false);
            aud.Source.clip = audioClip;

            if (!canStackPlay)
            {
                StopMusic(aud.NameClip);
            }

            instance.listCurrentMusic.Add(aud);
            PlayAud(aud, audioClip, loop, () =>
            {
                instance.listCurrentMusic.Remove(aud);
                RemoveAudio(aud);
                onDone?.Invoke();
            });
        }

        void PlayAud(Audio aud, AudioClip audioClip, bool loop = false
        , System.Action onDone = null)
        {
            aud.Source.clip = audioClip;
            aud.Source.loop = loop;

            if (aud.Source.isPlaying) 
            {
                aud.Source.Stop();
            }
            aud.Source.Play();

            if (!loop)
                aud.Corou = instance.StartCoroutine(DelayCallMaster.WaitAndDoIE(aud.Source.clip.length, () =>
                {
                    onDone?.Invoke();
                }));
        }

        public void StopSoundList()
        {
            for (int i = 0; i < listCurrentSound.Count; i++)
            {
                RemoveAudio(listCurrentSound[i]);
            }
            listCurrentSound.Clear();
        }

        public void StopMusicList()
        {
            for (int i = 0; i < listCurrentMusic.Count; i++)
            {
                RemoveAudio(listCurrentMusic[i]);
            }
            listCurrentMusic.Clear();
        }

        public void StopSound(AudioClip aud)
        {
            StopSound(aud.name);
        }
        public void StopSound(string name)
        {
            var audios = instance.listCurrentSound.FindAll(sound => sound.NameClip == name);

            for (int i = 0; i < audios.Count; i++)
            {
                RemoveAudio(audios[i]);
                instance.listCurrentSound.Remove(audios[i]);
            }
        }

        public void StopMusic(AudioClip aud)
		{
            StopMusic(aud.name);
		}
        public void StopMusic(string name)
        {
            var audios = instance.listCurrentMusic.FindAll(music => music.NameClip == name);

            for (int i = 0; i < audios.Count; i++)
            {
                RemoveAudio(audios[i]);
                instance.listCurrentMusic.Remove(audios[i]);
            }
        }

        bool CheckSoundPlay(AudioClip aud)
		{
            return instance.listCurrentSound.Exists(sound => sound.NameClip == aud.name);
        }

        bool CheckMusicPlay(AudioClip aud)
        {
            return instance.listCurrentMusic.Exists(sound => sound.NameClip == aud.name);
        }

        void RemoveAudio(Audio audio)
        {
            audio.Source.Stop();
            audio.Source.clip = null;
            audio.Source.outputAudioMixerGroup = null;

            if (audio.Corou != null) instance.StopCoroutine(audio.Corou);

            listAvailableAud.Add(audio);
        }

        #region change volume
        public void ChangeSoundVol(float value)
        {
            _ = audioMixer[0].SetFloat("SoundVol", Mathf.Log10(value) * 20);
        }

        public void ChangeMusicVol(float value)
        {
            _ = audioMixer[1].SetFloat("MusicVol", Mathf.Log10(value) * 20);
        }
        #endregion
    }
}