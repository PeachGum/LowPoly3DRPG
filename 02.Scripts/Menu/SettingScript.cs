using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{

    [HideInInspector]
    public bool isMasterMute = false, isBGMMute = false, isSFXMute = false, isFullScreen = false;
    float nowMasterVolume = 0f, nowBGMVolume = 0f, nowSFXVolume = 0f;
    private const string masterVolume = "MasterVolume", bgmVolume = "BGMVolume", sfxVolume = "SFXVolume";
    enum VolumeType
    {
        masterVolume = 0,
        bgmVolume = 1,
        sfxVolume = 2
    }
    VolumeType volumeType;

    private Toggle fullScreenToggle, masterVolumeToggle, bgmVolumeToggle, sfxVolumeToggle;
    private Slider masterVolumeSlider, bgmVolumeSlider, sfxVolumeSlider;
    private Dropdown resolutionDropdown, qualityDropdown;
    public GameObject settingPanel;

    //Resolution[] resolutions;
    List<Resolution> resolutions;

    public List<GameObject> settingForHide;
    private int currentResolutionIndex;

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown = settingPanel.transform.GetChild(2).GetComponent<Dropdown>();
        fullScreenToggle = settingPanel.transform.GetChild(3).GetComponent<Toggle>();
        qualityDropdown = settingPanel.transform.GetChild(5).GetComponent<Dropdown>();
        masterVolumeSlider = settingPanel.transform.GetChild(8).GetComponent<Slider>();
        masterVolumeToggle = settingPanel.transform.GetChild(9).GetComponent<Toggle>();
        bgmVolumeSlider = settingPanel.transform.GetChild(11).GetComponent<Slider>();
        bgmVolumeToggle = settingPanel.transform.GetChild(12).GetComponent<Toggle>();
        sfxVolumeSlider = settingPanel.transform.GetChild(14).GetComponent<Slider>();
        sfxVolumeToggle = settingPanel.transform.GetChild(15).GetComponent<Toggle>();
        //resolutions = Screen.resolutions;
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToList();
        //resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60);

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);


            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }

        }


        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        if (SceneManager.GetActiveScene().name == "Village")
        {
            if (FindObjectOfType<PushSettingScript>() != null)
            {
                PushSettingScript p = FindObjectOfType<PushSettingScript>();
                Screen.fullScreen = p.isFullScreen;
                SettingReceive(p.isMasterMute, p.isBGMMute, p.isSFXMute, p.nowMasterVolume, p.nowBGMVolume, p.nowSFXVolume, p.resolutionIndex, p.isFullScreen, p.qualityIndex);
            }

        }
    }
    public void PullSettingValues()
    {
        if (FindObjectOfType<PushSettingScript>() != null)
        {
            PushSettingScript p = FindObjectOfType<PushSettingScript>();

            if(p != null)
            p.PushSettineValues(isMasterMute, isBGMMute, isSFXMute, nowMasterVolume, nowBGMVolume, nowSFXVolume, resolutionDropdown.value, fullScreenToggle.isOn, qualityDropdown.value);
        }
    }
    public void SettingReceive(bool isMasterMute, bool isBGMMute, bool isSFXMute, float nowMasterVolume, float nowBGMVolume, float nowSFXVolume, int resolutionIndex, bool isFullScreen, int qualityIndex)
    {
        SetVolumeType(0);
        SetMute(isMasterMute);
        SetVolume(nowMasterVolume);

        SetVolumeType(1);
        SetMute(isBGMMute);
        SetVolume(nowBGMVolume);

        SetVolumeType(2);
        SetMute(isSFXMute);
        SetVolume(nowSFXVolume);

        SetResolution(resolutionIndex);

        SetFullScreen(isFullScreen);

        SetQuality(qualityIndex);
        
    }
    public void OnSetting(bool val)
    {
        
        if (settingForHide != null)
        {
            foreach (var obj in settingForHide)
            {
                obj.gameObject.SetActive(!val);
            }
            AudioManager.instance.SFXPlay("ButtonClick");

            settingPanel.SetActive(val);
            
            fullScreenToggle.isOn = Screen.fullScreen;
        }
    }

    //해상도 드롭다운
    public void SetResolution(int resolutionIndex)
    { 
        if(resolutions != null)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            Application.targetFrameRate = resolution.refreshRate;
            resolutionDropdown.value = resolutionIndex;

        }
    }
    //그래픽 품질 드롭다운
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropdown.value = qualityIndex;

    }
    //전체 화면 토글
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        this.isFullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
    }

    
    public void SetVolumeType(int type)
    {
        if (type == 0) volumeType = VolumeType.masterVolume;
        else if(type == 1) volumeType = VolumeType.bgmVolume;
        else if(type == 2) volumeType = VolumeType.sfxVolume;
    }

    //볼륨 슬라이더
    public void SetVolume(float val)
    {
        
        if(volumeType == VolumeType.masterVolume)
        {
            nowMasterVolume = val;
            masterVolumeSlider.value = nowMasterVolume;

            if (!isMasterMute)
            {
                AudioManager.instance.materAudioMixer.audioMixer.SetFloat(masterVolume, nowMasterVolume);
            }
        }
        else if(volumeType == VolumeType.bgmVolume)
        {
            nowBGMVolume = val;
            bgmVolumeSlider.value = nowBGMVolume;

            if (!isBGMMute)
            {
                AudioManager.instance.bgmAudioMixer.audioMixer.SetFloat(bgmVolume, nowBGMVolume);
            }
        }
        else if(volumeType == VolumeType.sfxVolume)
        {
            nowSFXVolume = val;   
            sfxVolumeSlider.value = nowSFXVolume;

            if(!isSFXMute)
            {
                AudioManager.instance.sfxAudioMixer.audioMixer.SetFloat(sfxVolume, nowSFXVolume);
            }
        }

    }
    public void SetMute(bool mute)
    {
        if (volumeType == VolumeType.masterVolume)
        {
            isMasterMute = mute;
            masterVolumeToggle.isOn = mute;
            if (isMasterMute)
            {
                AudioManager.instance.materAudioMixer.audioMixer.SetFloat(masterVolume, -80f);
            }
            else
            {
                AudioManager.instance.materAudioMixer.audioMixer.SetFloat(masterVolume, nowMasterVolume);
            }
        }
        else if (volumeType == VolumeType.bgmVolume)
        {
            isBGMMute = mute;
            bgmVolumeToggle.isOn = mute;
            if (isBGMMute)
            {
                AudioManager.instance.bgmAudioMixer.audioMixer.SetFloat(bgmVolume, -80f);
            }
            else
            {
                AudioManager.instance.bgmAudioMixer.audioMixer.SetFloat(bgmVolume, nowBGMVolume);
            }
        }
        else if (volumeType == VolumeType.sfxVolume)
        {
            isSFXMute = mute;
            sfxVolumeToggle.isOn = mute;
            if (isSFXMute)
            {
                AudioManager.instance.sfxAudioMixer.audioMixer.SetFloat(sfxVolume, -80f);
            }
            else
            {
                AudioManager.instance.sfxAudioMixer.audioMixer.SetFloat(sfxVolume, nowSFXVolume);
            }
        }
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
