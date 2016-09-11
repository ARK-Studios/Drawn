using UnityEngine;


namespace Assets.Scripts.UI
{
    public class LoadingSpinner : MonoBehaviour
    {
        public float speed;

        void Update()
        {
            transform.Rotate(Vector3.back, speed * Time.deltaTime);
        }

    }
}
