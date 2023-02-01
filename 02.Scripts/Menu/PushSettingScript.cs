using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushSettingScript : MonoBehaviour
{
    [HideInInspector]
    public bool isMasterMute, isBGMMute, isSFXMute, isFullScreen;

    [HideInInspector]
    public float nowMasterVolume, nowBGMVolume, nowSFXVolume;

    [HideInInspector]
    public int resolutionIndex, qualityIndex;
    public void PushSettineValues(bool isMasterMute, bool isBGMMute, bool isSFXMute, float nowMasterVolume, float nowBGMVolume, float nowSFXVolume, int resolutionIndex, bool isFullScreen, int qualityIndex)
    {

        this.isMasterMute = isMasterMute;
        this.isBGMMute = isBGMMute;
        this.isSFXMute = isSFXMute;
        this.nowMasterVolume = nowMasterVolume;
        this.nowBGMVolume = nowBGMVolume;
        this.nowSFXVolume = nowSFXVolume;
        this.resolutionIndex = resolutionIndex;
        this.isFullScreen = isFullScreen;
        this.qualityIndex = qualityIndex;

    }
}
