using UnityEngine;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Common
{
    public class ARKDebug : MonoBehaviour
    {
        public GameObject managers;

        // Use this for initialization
        void Awake()
        {
            if (null == SystemManager.Instance)
            {
                Instantiate(managers);
                ARKLogger.LogMessage(eLogCategory.General,
                                     eLogLevel.Info,
                                     "Instantiated Debug Managers.");
            }
        }
    }
}
