using UnityEngine;


namespace Assets.Scripts.Perspective
{
    public enum eCameraBoundEntryAxis
    {
        CameraBoundEntryNone,
        CameraBoundEntryNegative,
        CameraBoundEntryPositive,
        CameraBoundEntryBoth
    }

    [RequireComponent(typeof(Renderer))]
    public class CameraBound : MonoBehaviour
    {
        public eCameraBoundEntryAxis onXEntry;
        public eCameraBoundEntryAxis onYEntry;

        void OnBecameVisible()
        {
            PerspectiveManager.Instance.OnCameraBoundEntry(this);
        }
        
        void OnBecameInvisible()
        {
            PerspectiveManager.Instance.OnCameraBoundExit(this);
        }
    }
}
