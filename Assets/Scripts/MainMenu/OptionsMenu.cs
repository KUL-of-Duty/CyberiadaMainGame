using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class OptionsMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Button applyButton;

    [Header("Behavior")]
    [SerializeField] private bool applyOnResolutionChange = true;

    private readonly List<Resolution> _resolutions = new();
    private int _pendingResolutionIndex = -1;

    private void Awake()
    {
        RefreshResolutions();

        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        }

        if (applyButton != null)
            applyButton.onClick.AddListener(ApplyPending);
    }

    private void OnDestroy()
    {
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.RemoveListener(OnResolutionDropdownChanged);

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggled);

        if (applyButton != null)
            applyButton.onClick.RemoveListener(ApplyPending);
    }

    public void RefreshResolutions()
    {
        if (resolutionDropdown == null)
        {
            Debug.LogWarning($"{nameof(OptionsMenu)}: resolutionDropdown is not assigned.");
            return;
        }

        _resolutions.Clear();
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        foreach (var r in Screen.resolutions)
        {
           
            if (_resolutions.Exists(x => x.width == r.width && x.height == r.height))
                continue;

            _resolutions.Add(r);
            options.Add($"{r.width} x {r.height}");
        }

        resolutionDropdown.AddOptions(options);

        var currentIndex = FindClosestCurrentResolutionIndex();
        _pendingResolutionIndex = currentIndex;

        resolutionDropdown.SetValueWithoutNotify(Mathf.Clamp(currentIndex, 0, _resolutions.Count - 1));
        resolutionDropdown.RefreshShownValue();
    }

    private int FindClosestCurrentResolutionIndex()
    {
        if (_resolutions.Count == 0)
            return 0;

        var w = Screen.width;
        var h = Screen.height;

        for (var i = 0; i < _resolutions.Count; i++)
        {
            if (_resolutions[i].width == w && _resolutions[i].height == h)
                return i;
        }

        
        var bestIndex = 0;
        var bestDiff = int.MaxValue;
        var currentArea = w * h;
        for (var i = 0; i < _resolutions.Count; i++)
        {
            var area = _resolutions[i].width * _resolutions[i].height;
            var diff = Math.Abs(area - currentArea);
            if (diff < bestDiff)
            {
                bestDiff = diff;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    private void OnResolutionDropdownChanged(int index)
    {
        _pendingResolutionIndex = index;

        if (applyOnResolutionChange)
            ApplyPending();
    }

    private void OnFullscreenToggled(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ApplyPending()
    {
        if (_pendingResolutionIndex < 0 || _pendingResolutionIndex >= _resolutions.Count)
            return;

        var r = _resolutions[_pendingResolutionIndex];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }
}

