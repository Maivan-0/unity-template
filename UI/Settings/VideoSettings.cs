using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettings : MonoBehaviour
{
    [Header("Resolution")]
    [SerializeField] private TMP_Text resolutionDropdown;
    [SerializeField] private Button previousResolution;
    [SerializeField] private Button nextResolution;
    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;

    [Header("Graphics")]
    [SerializeField] private TMP_Text graphicsDropdown;
    [SerializeField] private Button previousGraphics;
    [SerializeField] private Button nextGraphics;
    private string[] graphics = new string[] { "Low", "Medium", "High" };
    private int currentGraphicsIndex = 0;

    [Header("Video")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;

    private void Start()
    {
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        fullscreenToggle.isOn = Screen.fullScreen;
        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;

        previousResolution.onClick.AddListener(PreviousResolutionOption);
        nextResolution.onClick.AddListener(NextResolutionOption);

        previousGraphics.onClick.AddListener(PreviousGraphicsOption);
        nextGraphics.onClick.AddListener(NextGraphicsOption);

        UpdateText();
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isOn)
    {
        Screen.fullScreen = isOn;
    }

    public void SetVsync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }

    public void UpdateText()
    {
        resolutionDropdown.text = resolutions[currentResolutionIndex].width + "x" + resolutions[currentResolutionIndex].height;
        graphicsDropdown.text = graphics[currentGraphicsIndex];
    }

    public void PreviousResolutionOption()
    {
        currentResolutionIndex--;
        if(currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutions.Length - 1;
        }
        UpdateText();
    }

    public void NextResolutionOption()
    {
        currentResolutionIndex++;
        if (currentResolutionIndex >= resolutions.Length)
        {
            currentResolutionIndex = 0;
        }
        UpdateText();
    }

    public void PreviousGraphicsOption()
    {
        currentGraphicsIndex--;
        if (currentGraphicsIndex < 0)
        {
            currentGraphicsIndex = graphics.Length - 1;
        }
        UpdateText();
    }

    public void NextGraphicsOption()
    {
        currentGraphicsIndex++;
        if (currentGraphicsIndex >= graphics.Length)
        {
            currentGraphicsIndex = 0;
        }
        UpdateText();
    }
}
