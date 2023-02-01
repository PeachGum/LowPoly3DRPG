using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResolutionDropdownScript : MonoBehaviour, IPointerClickHandler
{
    Dropdown resolutionDropdown;
    void Start()
    {
        resolutionDropdown = GetComponent<Dropdown>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.SFXPlay("ButtonClick");
        RectTransform content = transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        if (resolutionDropdown.value >= resolutionDropdown.options.Count - 7)
        {
            content.anchoredPosition = new Vector2(0, content.rect.height - 150);
        }
        else
        {
            content.anchoredPosition = new Vector2(0, resolutionDropdown.value * 20 + 2);
        }
    }

    public void OptionClickPlaySFX()
    {
        try
        {
            if(transform.GetChild(3) != null)
            {
                AudioManager.instance.SFXPlay("ButtonClick");
            }
        }
        catch(UnityException ex)
        {
            Debug.Log(ex);
            return;
        }
    }
}
