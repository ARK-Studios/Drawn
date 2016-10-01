using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player
{
    public class SimpleMove : MonoBehaviour
    {
        public int movementspeed = 100;

        // Use this for initialization
        void Start()
        {

        }

        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * movementspeed * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * movementspeed * Time.fixedDeltaTime);
            }
        }
    }
}
