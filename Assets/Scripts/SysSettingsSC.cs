using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SysSettingsSC : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown screenModeDropdown;
    private List<Resolution> _availableResolutions;
    
    private int _LanguageOld;
    private int _Vol_MasterOld;
    private int _Vol_BGMOld;
    private int _Vol_SFXOld;

    void OnEnable()
    {
        Resolution[] allResolutions = Screen.resolutions;
        _availableResolutions = new();
        List<int> widthInList = new();
        int selected = 0;
        List<string> availableResString = new();
        for (int i = 0; i < allResolutions.Length; i++)
        {
            Debug.Log(allResolutions[i]);
            if (allResolutions[i].width / 16 * 9 == allResolutions[i].height && !widthInList.Contains(allResolutions[i].width))
            {
                widthInList.Add(allResolutions[i].width);
                availableResString.Add($"{allResolutions[i].width} × {allResolutions[i].height}");
                _availableResolutions.Add(allResolutions[i]);
                if (allResolutions[i].width == Screen.width && allResolutions[i].height == Screen.height)
                {
                    selected = availableResString.Count - 1;
                }
            }
        }
        resolutionDropdown.AddOptions(availableResString);
        resolutionDropdown.value = selected;
        
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                screenModeDropdown.value = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                screenModeDropdown.value = 1;
                break;
            case FullScreenMode.Windowed:
                screenModeDropdown.value = 2;
                break;
            default:
                screenModeDropdown.value = 0;
                break;
        }
    }

    public void ApplyScreen()
    {
        FullScreenMode mode = FullScreenMode.Windowed;
        switch (screenModeDropdown.value)
        {
            case 0:
                mode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                mode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                mode = FullScreenMode.Windowed;
                break;
            default:
                mode = FullScreenMode.Windowed;
                break;
        }
        Resolution resolution = _availableResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, mode);
    }
    
    public void _ChangeLangSet()
    {
        if (SysSaveSC._Language == 0)
        {
            SysSaveSC._Language = 1;
        }
        else if (SysSaveSC._Language == 1)
        {
            SysSaveSC._Language = 0;
        }
    }
    public void ConfirmSet()
    {
        SysSaveSC._SysSave();
    }
}
