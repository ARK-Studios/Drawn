using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class LinearMovementPlatform : MonoBehaviour {

    public Vector2 StartPoint;
    public Vector2 EndPoint;

    public bool Repeat;
    public bool TransformIsBound;

    public float Speed;

    private bool inverseDirection;
    private float progress;
    private float distance;

    private Vector2 firstPosition;
    private float firstPassDistance;
    private bool firstPass;

    // Use this for initialization
    public void Start () {
		if (TransformIsBound)
        {
            StartPoint = new Vector2(transform.position.x, transform.position.y);
        }

        inverseDirection = false;
        progress = 0;
        distance = Vector2.Distance(StartPoint, EndPoint);

        firstPosition = new Vector2(transform.position.x, transform.position.y);
        firstPassDistance = Vector2.Distance(transform.position, StartPoint);
        firstPass = true;
    }

	
	// Update is called once per frame
	public void Update () {
        if (distance != 0 || firstPassDistance != 0)
        {
            // Always goes toward First Point first
            if (firstPass)
            {
                // Update position
                progress += Speed * Time.deltaTime;
                progress = Mathf.Clamp(progress, 0, firstPassDistance);
                transform.position = Vector2.Lerp(firstPosition, StartPoint, progress / firstPassDistance);

                if ((transform.position.x == StartPoint.x) &&
                    (transform.position.y == StartPoint.y))
                {
                    firstPass = false;
                    progress = 0;
                    inverseDirection = false;
                }
            }
            else
            {
                // Determine Direction and Lerp Values
                int direction = inverseDirection ? -1 : 1;

                // Update position
                progress += direction * Speed * Time.deltaTime;
                progress = Mathf.Clamp(progress, 0, distance);
                transform.position = Vector2.Lerp(StartPoint, EndPoint, progress / distance);

                // Inverse Direction if required
                if (Repeat)
                {
                    if ((transform.position.x == EndPoint.x) &&
                        (transform.position.y == EndPoint.y))
                    {
                        inverseDirection = true;
                    }
                    else if ((transform.position.x == StartPoint.x) &&
                             (transform.position.y == StartPoint.y))
                    {
                        inverseDirection = false;
                    }
                }
            }
        }
    }
}
