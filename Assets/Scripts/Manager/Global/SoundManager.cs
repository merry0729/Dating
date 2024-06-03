using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public SerializableDictionary<SoundType, AudioSource> soundDic = new SerializableDictionary<SoundType, AudioSource>();
    //public SerializableDictionary<SettingType, GameObject> settingDic = new SerializableDictionary<SettingType, GameObject>();

    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        soundDic.Add(SoundType.Master, GetComponentInChildren<AudioSource>());
        soundDic.Add(SoundType.BGM, GetComponentInChildren<AudioSource>());
        soundDic.Add(SoundType.Voice, GetComponentInChildren<AudioSource>());
        soundDic.Add(SoundType.SFX, GetComponentInChildren<AudioSource>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
