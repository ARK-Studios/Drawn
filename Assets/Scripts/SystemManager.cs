/*******************************************************************
 * 
 * Copyright (C) 2015 Frozen Metal Studios - All Rights Reserved
 * 
 * NOTICE:  All information contained herein is, and remains
 * the property of Frozen Metal Studios. The intellectual and 
 * technical concepts contained herein are proprietary to 
 * Frozen Metal Studios are protected by copyright law.
 * Dissemination of this information or reproduction of this material
 * is strictly forbidden unless prior written permission is obtained
 * from Frozen Metal Studios.
 * 
 * *****************************************************************
 * 
 * Filename: SceneLoader.cs
 * 
 * Description: Controls Initial Game Entry and Scene Management.
 * 
 *******************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics;
using Assets.Scripts.Utility;
using Assets.Scripts.CustomEditor;


class SystemManager : Singleton<SystemManager>
{
    private ARKLogger _logger = new ARKLogger();

    private Component[] _managers;

    // --------------------------------------------------------------------
    public string GameTitle = "Default Game Title";

    // --------------------------------------------------------------------
    private string _CurrentScene;
    private string _TransitionTo;

    private string __TransitionScene = "Transition";

    // --------------------------------------------------------------------
#if !FINAL
    private float _FPS;
    private float _FPSTime;
    private int _FPSFrames;
#endif

    // --------------------------------------------------------------------
    public void Awake()
    {
        // Initialize the Logger
        _logger.Initialize();

        // Initialize self
        base.Init();

        // Init the FPS Tracker
        InitFPS();

        // Initialize managers
        _managers = GetComponentsInChildren<ISingleton>();
        foreach (ISingleton manager in _managers)
        {
            if (manager != this)
            {
                manager.Init();
            }
        }

        // Make sure this object persists between scene loads.
        DontDestroyOnLoad(gameObject);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public bool TransitionToScene(string sceneName)
    {
        this._TransitionTo = sceneName;
        return this.LoadScene(this.__TransitionScene);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public string GetSceneToLoad()
    {
        return _TransitionTo;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public bool LoadScene(string sceneName)
    {
        this._CurrentScene = sceneName;
        SceneManager.LoadScene(this._CurrentScene);
        return true;
    }
    
    //-------------------------------------------------------------------------------------------------------------------------
    public void LoadLevelSync(string level)
    {
        ARKLogger.LogMessage(eLogCategory.Control,
                             eLogLevel.Info,
                             "SceneLoader: Loading Level ( " + level + " )");

        _CurrentScene = level;
        SceneManager.LoadScene(this._CurrentScene);
    }
    
    //-------------------------------------------------------------------------------------------------------------------------
    public void ResetCurrentLevel()
    {
        ARKLogger.LogMessage(eLogCategory.Control,
                             eLogLevel.Info,
                             "SceneLoader: Restarting Level (" + _CurrentScene + " )");

        SceneManager.LoadScene(this._CurrentScene);
    }

    //-------------------------------------------------------------------------------------------------------------------------
#if !FINAL
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }

        UpdateFPS();
    }
#endif

    //-------------------------------------------------------------------------------------------------------------------------
    public void Quit()
    {
        ARKLogger.LogMessage(eLogCategory.Control,
                             eLogLevel.Info,
                             "SceneLoader: Terminating.");
        Application.Quit();
    }

    //-------------------------------------------------------------------------------------------------------------------------
#if !FINAL
    void OnGUI()
    {
        RenderFPS();
    }
#endif

    //-------------------------------------------------------------------------------------------------------------------------
    [Conditional("DEBUG"), Conditional("PROFILE")]
    void InitFPS()
    {
#if !FINAL
        // Clear the fps counters.
        _FPS = 0.0f;
        _FPSTime = 0.0f;
        _FPSFrames = 0;
#endif
    }

    //-------------------------------------------------------------------------------------------------------------------------
    [Conditional("DEBUG"), Conditional("PROFILE")]
    void UpdateFPS()
    {
#if !FINAL
        // Increase the number of frames.
        _FPSFrames++;

        // Accumulate time.
        _FPSTime += Time.deltaTime;

        // Have we reached 1 second.
        if (_FPSTime > 1.0f)
        {
            // Store number of frames per second.
            _FPS = _FPSFrames;

            // Reset for the next frame.
            _FPSTime -= 1.0f;
            _FPSFrames = 0;
        }
#endif
    }

    //-------------------------------------------------------------------------------------------------------------------------
    [Conditional("DEBUG"), Conditional("PROFILE")]
    void RenderFPS()
    {
#if !FINAL
        // Display on screen the current Frames Per Second.
        string fps_string = String.Format("FPS: {0:d2}", (int) _FPS);
        GUI.Label(new Rect(10, 10, 600, 30), fps_string);
#endif
    }

    public void OnDestroy()
    {
        // Cleanup the logger
        _logger.Cleanup();
    }
}
