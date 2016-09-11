#if UNITY_EDITOR
/*******************************************************************
 * 
 * Filename: BuildLogger.cs
 * 
 * Description: Provides a Simple Independant Logger for Builds.
 * 
 *******************************************************************/
using UnityEngine;
using System.Collections;
using System.IO;

public class BuildLogger
{
    //-------------------------------------------------------------------------------------------------------------------------
    // A simple log to file system.
    //-------------------------------------------------------------------------------------------------------------------------
    private StreamWriter    _OutputStream;
    private bool            _Echo;

    //-------------------------------------------------------------------------------------------------------------------------
    public BuildLogger(string filename, bool echo_to_debug)
    {
        _OutputStream = new StreamWriter(filename);

        _Echo = echo_to_debug;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public void Log(string msg)
    {
        if (_OutputStream != null)
        {
            _OutputStream.WriteLine(msg);

            if (_Echo)
                Debug.Log(msg);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public void Close()
    {
        if (_OutputStream != null)
        {
            _OutputStream.Close();
            _OutputStream = null;
        }
    }
}
#endif