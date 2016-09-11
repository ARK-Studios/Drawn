using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Utility;


public class ISingleton : MonoBehaviour
{
    virtual public void Init()
    {
    }
}

public class Singleton<T> : ISingleton
{
    private static object _Singleton = null;

    // --------------------------------------------------------------------
    public static T Instance
    {
        get { return (T) _Singleton; }
    }

    override public void Init()
    {
        // Ensure only 1 singleton
        if (null != _Singleton)
        {
            ARKLogger.LogMessage(eLogCategory.Programmer,
                                 eLogLevel.System,
                                 "{0}: Multiple Singletons violates pattern",
                                 this.GetType());
        }
        else
        {
            ARKLogger.LogMessage(eLogCategory.Programmer,
                                 eLogLevel.System,
                                 "{0}: Creating Singleton",
                                 this.GetType());

            _Singleton = this;
        }
    }
}
