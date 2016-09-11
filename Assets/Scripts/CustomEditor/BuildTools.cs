#if UNITY_EDITOR
/*******************************************************************
 * 
 * Filename: BuildTools.cs
 * 
 * Description: Support Calls for Build Management and Automation.
 * 
 *******************************************************************/
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class BuildTools
{
    //-------------------------------------------------------------------------------------------------------------------------
    public static void BuildSetDebug()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "DEBUG");
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static void BuildSetProfile()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "PROFILE");
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static void BuildSetFinal()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "FINAL");
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static String ConfigureBuildFromCommandLine()
    {
        String[] arguments = Environment.GetCommandLineArgs();
        String buildConfig = "Debug"; // Default Debug value
        for (int i=0; i < arguments.Length; i++)
        {
            if (("-buildConfig" == arguments[i]) && ((i + 1) < arguments.Length))
            {
                buildConfig = arguments[i + 1];
            }
        }
        switch (buildConfig)
        {
            case "Release":
                BuildSetFinal();
                break;

            case "Profile":
                BuildSetProfile();
                break;

            case "Debug":
            default:
                BuildSetDebug();
                break;
        }
        return buildConfig;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static void BuildWin32()
    {
        String config = BuildTools.ConfigureBuildFromCommandLine();
        String output = String.Format("..\\AutoBuild\\Platforms\\Win32\\{0}\\Application.exe", config);
        String log = String.Format("..\\AutoBuild\\Logs\\Summary_Win32_{0}.log", config);
        Build(config, log, BuildTarget.StandaloneWindows, output);
    }
    
    //-------------------------------------------------------------------------------------------------------------------------
    public static void Build(string config, string log_filename, BuildTarget target, string output)
    {
        BuildLogger logger = new BuildLogger(log_filename, false);

        logger.Log("Building Platform: " + target.ToString());
        logger.Log("Building Config:   " + config);
        logger.Log("");

        string[] level_list = FindScenes();
        logger.Log("Scenes to be processed: " + level_list.Length);

        foreach (string s in level_list)
        {
            string cutdown_level_name = s.Remove(s.IndexOf(".unity"));
            logger.Log("   " + cutdown_level_name);
        }

        // Make sure the paths exist before building.
        try
        {
            Directory.CreateDirectory(output);
        }
        catch
        {
            logger.Log("Failed to create directories: " + output);
        }

        string results = "";

        try
        {
            results = BuildPipeline.BuildPlayer(level_list, output, target, BuildOptions.None);
        }
        catch
        {
            logger.Log("Errors Found.");
            logger.Log(results);
            logger.Close();
        }

        logger.Log("");

        if (results.Length == 0)
            logger.Log("No Build Errors");
        else
        {
            logger.Log("Errors Found.");
            logger.Log(results);
        }

        logger.Close();
    }
    
    //-------------------------------------------------------------------------------------------------------------------------
    public static string[] FindScenes()
    {
        int num_scenes = 0;
        
        // Count active scenes.
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
                num_scenes++;
        }
        
        // Build the list of scenes.
        string[] scenes = new string[num_scenes];
        
        int x = 0;
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
                scenes[x++] = scene.path;
        }
        
        return ( scenes );
    }
}
#endif
