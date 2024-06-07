using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    public SerializableDictionary<SoundType, AudioSource> soundDic = new SerializableDictionary<SoundType, AudioSource>();
    public SerializableDictionary<string, AudioClip> soundClipDic = new SerializableDictionary<string, AudioClip>();
    //public SerializableDictionary<SettingType, GameObject> settingDic = new SerializableDictionary<SettingType, GameObject>();

    public SoundTable soundTable;
    public SoundData soundData;

    public AudioMixer audioMixer;
    AudioClip clip;

    string getPath;
    string bgmPath = "Sound/BGM/";
    string voicePath = "Sound/Voice/";
    string sfxPath = "Sound/SFX/";

    private void Awake()
    {
        soundTable = SoundData.Table;

        for (int index = 0; index < soundTable.Count; index++)
        {
            soundData = soundTable.TryGet(index);

            string directoryPath = string.Empty;
            switch (soundData.Type)
            {
                case (int)SoundType.BGM:
                    directoryPath = bgmPath;
                    break;
                case (int)SoundType.Voice:
                    directoryPath = voicePath;
                    break;
                case (int)SoundType.SFX:
                    directoryPath = sfxPath;
                    break;
            }

            clip = Resources.Load<AudioClip>(directoryPath + soundData.SoundFileName);

            soundClipDic.Add(soundData.SoundType, clip);
        }

        soundDic.Add(SoundType.Master, transform.Find("Master").GetComponent<AudioSource>());
        soundDic.Add(SoundType.BGM, transform.Find("BGM").GetComponent<AudioSource>());
        soundDic.Add(SoundType.Voice, transform.Find("Voice").GetComponent<AudioSource>());
        soundDic.Add(SoundType.SFX, transform.Find("SFX").GetComponent<AudioSource>());
    }

    // Start is called before the first frame update
    void Start()
    {
        //soundTable = SoundData.Table;

        //for (int index = 0; index < soundTable.Count; index++)
        //{
        //    soundData = soundTable.TryGet(index);

        //    string directoryPath = string.Empty;
        //    switch (soundData.Type)
        //    {
        //        case (int)SoundType.BGM:
        //            directoryPath = bgmPath;
        //            break;
        //        case (int)SoundType.Voice:
        //            directoryPath = voicePath;
        //            break;
        //        case (int)SoundType.SFX:
        //            directoryPath = sfxPath;
        //            break;
        //    }

        //    clip = Resources.Load<AudioClip>(directoryPath + soundData.SoundFileName);

        //    soundClipDic.Add(soundData.SoundType, clip);
        //}

        //soundDic.Add(SoundType.Master, transform.Find("Master").GetComponent<AudioSource>());
        //soundDic.Add(SoundType.BGM, transform.Find("BGM").GetComponent<AudioSource>());
        //soundDic.Add(SoundType.Voice, transform.Find("Voice").GetComponent<AudioSource>());
        //soundDic.Add(SoundType.SFX, transform.Find("SFX").GetComponent<AudioSource>());
    }

    public void VolumeControl(SoundType sourceType, float volume)
    {
        audioMixer.SetFloat($"{sourceType.ToString()}", Mathf.Log10(volume) * 20);
        Debug.Log($"{audioMixer} / {sourceType.ToString()} / {volume} / {Mathf.Log10(volume) * 20}");
    }


    void PlaySound(SoundType sourceType, string str)
    {
        AudioSource source = soundDic[sourceType];

        Debug.Log($"{source.name} / {soundClipDic[str]}");

        if (source.isPlaying)
            source.Stop();

        source.clip = soundClipDic[str];

        Debug.Log($"{source.clip}");

        source.Play();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaySound(SoundType.SFX, "Click_0");
        }
    }
}
