using System;
using System.Collections;
using System.Collections.Generic;
using System.Media;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sfxSounds;
    public Sound[] bgmSounds;

    [HideInInspector]
    public string nowBgm;

    public static AudioManager instance;

    public AudioMixer audioMixer;
    [HideInInspector]
    public AudioMixerGroup materAudioMixer, bgmAudioMixer, sfxAudioMixer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        materAudioMixer = audioMixer.FindMatchingGroups("Master")[0];
        bgmAudioMixer = audioMixer.FindMatchingGroups("BGM")[0];
        sfxAudioMixer = audioMixer.FindMatchingGroups("SFX")[0];

        foreach (Sound s in sfxSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = sfxAudioMixer;
        }

        foreach (Sound s in bgmSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = bgmAudioMixer;
        }

    }

    public void SFXPlay(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);

        //사운드를 찾을 수 없을때
        if (s == null)
        {
            return;
        }
        float vol;
        s.source.outputAudioMixerGroup.audioMixer.GetFloat("SFXVolume", out vol);
        
        s.source.Play();
    }

    public void BGMPlay(string name)
    {
        Sound s = Array.Find(bgmSounds, sound => sound.name == name);
        
        if(nowBgm != "")
        {
            Sound stopBgm = Array.Find(bgmSounds, sound => sound.name == nowBgm);
            if (stopBgm != null)
            {
                stopBgm.source.Stop();
            }
        }
        //사운드를 찾을 수 없을때
        if (s == null)
        {
            return;
        }

        float vol;
        s.source.outputAudioMixerGroup.audioMixer.GetFloat("BGMVolume", out vol);
        s.source.Play();

        nowBgm = name;
    }

    public void SFXStop(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);

        //사운드를 찾을 수 없을때
        if (s == null)
        {
            return;
        }

        s.source.Stop();
    }
    public void BGMStop(string name)
    {
        Sound s = Array.Find(bgmSounds, sound => sound.name == name);

        //사운드를 찾을 수 없을때
        if (s == null)
        {
            return;
        }

        s.source.Stop();
    }
}
