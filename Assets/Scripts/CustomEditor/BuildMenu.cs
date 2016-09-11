#if UNITY_EDITOR
/*******************************************************************
 * 
 * Filename: BuildMenu.cs
 * 
 * Description: Gives Editor Access to Build Configurations.
 * 
 *******************************************************************/
using UnityEditor;
using UnityEngine;
using System;

public class BuildMenu : ScriptableObject
{
    //-------------------------------------------------------------------------------------------------------------------------
    [MenuItem("Build/Build Win32", false, 1000)]
    public static void BuildPCPlayer()
    {
        BuildTools.BuildWin32();
    }
    
    //-------------------------------------------------------------------------------------------------------------------------
    [MenuItem("Build/Config Debug", false, 2000)]
    public static void BuildSetDebug()
    {
        BuildTools.BuildSetDebug();
        Debug.Log("Setting build to DEBUG.");
    }
    
    //-------------------------------------------------------------------------------------------------------------------------
    [MenuItem("Build/Config Profile", false, 2000)]
    public static void BuildSetProfile()
    {
        BuildTools.BuildSetProfile();
        Debug.Log("Setting build to Profile.");
    }
    
    //-------------------------------------------------------------------------------------------------------------------------
    [MenuItem("Build/Config Final", false, 2000)]
    public static void BuildSetFinal()
    {
        BuildTools.BuildSetFinal();
        Debug.Log("Setting build to FINAL.");
    }

}
#endif