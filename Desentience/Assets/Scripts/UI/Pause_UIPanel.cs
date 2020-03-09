﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class Pause_UIPanel : Base_UIPanel
{
    public Button resumeButton;
    public Button menuButton;
    public override void OpenBehavior()
    {
        base.OpenBehavior();

        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() => { ResumeButtonPressed(); });

        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(() => { MenuButtonPressed(); });
    }

    private void Awake()
    {
        
    }

    void ResumeButtonPressed()
    {
        GameManager.Instance.TogglePause();
    }

    void MenuButtonPressed()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}