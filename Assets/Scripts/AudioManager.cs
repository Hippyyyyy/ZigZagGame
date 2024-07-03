using SCN.Audio;
using SCN.BinaryData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(fileName = "Audio Manager", menuName = "Audio Manager")]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip drop;
    [SerializeField] AudioClip collect;
    [SerializeField] AudioClip backgroundMusic;

    public static AudioManager Ins;

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
        }
    }

    public void Stop_Music()
    {
        AudioPlayer.Instance.StopMusicList();
        AudioPlayer.Instance.StopSoundList();
    }
    public void Play_Drop()
    {
        var data = DataManagerSample.Instance.LocalData;
        if (data.isAudio)
        {
            AudioPlayer.Instance.PlaySound(drop, false);
        }
    }
    public void Play_Background_Music()
    {
        var data = DataManagerSample.Instance.LocalData;
        if (data.isAudio)
        {
            AudioPlayer.Instance.PlayMusic(backgroundMusic, true);
        }
    }
    public void Play_Collect_Diamond()
    {
        var data = DataManagerSample.Instance.LocalData;
        if (data.isAudio)
        {
            AudioPlayer.Instance.PlaySound(collect, false);
        }
    }
}
