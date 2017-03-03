using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour {
    public Transform[] pathPoints;
    public int currentPath = 0;
    public float pathPointRadius = 5.0f;
	public float speed = 5f;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ObjectMovement();

    }

    void ObjectMovement()
    {
        Vector3 dir = pathPoints[currentPath].position - transform.position;
        Vector3 dirNorm = dir.normalized;

        transform.Translate(dirNorm * speed *Time.fixedDeltaTime);
        if(dir.magnitude <= pathPointRadius)
        {
            currentPath++;
            if (currentPath >= pathPoints.Length)
            {
                currentPath = 0;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(pathPoints == null)
        {
            return;
        }
        foreach (Transform pathPoint in pathPoints)
        {
            Gizmos.DrawSphere(pathPoint.position, pathPointRadius);
        }
    }
}
