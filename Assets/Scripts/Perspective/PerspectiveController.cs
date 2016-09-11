/*******************************************************************
 * 
 * Copyright (C) 2015 Frozen Metal Studios - All Rights Reserved
 * 
 * NOTICE:  All information contained herein is, and remains
 * the property of Frozen Metal Studios. The intellectual and 
 * technical concepts contained herein are proprietary to 
 * Frozen Metal Studios are protected by copyright law.
 * Dissemination of this information or reproduction of this material
 * is strictly forbidden unless prior written permission is obtained
 * from Frozen Metal Studios.
 * 
 * *****************************************************************
 * 
 * Filename: PerspectiveController.cs
 * 
 * Description: Allows for exchangable Camera behaviors.
 * 
 *******************************************************************/
using UnityEngine;
using Assets.Scripts.CustomEditor;
using Assets.Scripts.Perspective.Cameras;


namespace Assets.Scripts.Perspective
{
    /// <summary>
    /// Manages the Main Camera, allowing the behavior to be switched.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class PerspectiveController : MonoBehaviour
    {
        /// <summary>
        /// The position that that camera will be following.
        /// </summary>
        public Transform target;

        /// <summary>
        /// The List of possible Cameras.
        /// </summary>
        public AbstractCamera[] cameras;

        /// <summary>
        /// The Controllers camera object.
        /// </summary>
        public Camera controllerCamera;

        /// <summary>
        /// The Index of the Current Camera.
        /// </summary>
        public int currentCamera = 0;

        /// <summary>
        /// The Index of the Last Camera.
        /// If the value is -1, it means no Last Camera
        /// is specified.
        /// </summary>
        [ReadOnly]
        private int lastCamera = -1;

        /// <summary>
        /// Specifies if the Camera failed to complete a transition.
        /// True if a mid-transition event occured, false otherwise.
        /// </summary>
        private bool midTransitionFlag = false;

        /// <summary>
        /// If the Camera failed to complete a transition, stores
        /// the position of the failure.
        /// </summary>
        private Vector3 midTransitionPosition;

        /// <summary>
        /// The progress of the current Interpolation in time.
        /// </summary>
        private float interpolationTime = 0;

        /// <summary>
        /// The speed of the active transition.
        /// </summary>
        private float transitionSpeed = 0;

        /// <summary>
        /// The reported value of the Animation Curve.
        /// </summary>
        private float lastInterpol;

        /// <summary>
        /// An ease in, ease out animation curve (tangents are all flat)
        /// </summary>
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        /// <summary>
        /// Sets the current Cameras Target.
        /// </summary>
        /// <param name="target">The new Target Transform</param>
        public void SetTarget(Transform target)
        {
            this.target = transform;
            cameras[currentCamera].SetTarget(target);
        }

        /// <summary>
        /// Get the Target at Runtime.
        /// </summary>
        /// <returns>The Cameras Target Transform</returns>
        public Transform GetTarget()
        {
            return target;
        }
        
        /// <summary>
        /// Sets the Camera to use and sets a maximum transition time.
        /// </summary>
        /// <param name="newCamera">The Index of the new Camera</param>
        /// <param name="speed">Specifies the speed of the transition</param>
        public void SetCamera(int newCamera, float speed)
        {
            // Cannot change camera to self
            if (newCamera == currentCamera)
            {
                return;
            }

            // If the last Camera was free, set the new Camera to the
            // current one and start transitioning.
            if ((speed > 0) && (lastCamera == -1))
            {
                // Define the transition properties
                interpolationTime = 0;
                transitionSpeed = speed;

                // Set the Last Camera to the Current Camera
                lastCamera = currentCamera;
            }
            else if (speed > 0)
            {
                // Set the Mid-Transition Flag
                midTransitionFlag = true;

                // Reset the transition properties
                interpolationTime = 0;
                transitionSpeed = speed;
                midTransitionPosition = transform.position;

                // Set the Last Camera to the Current Camera
                lastCamera = currentCamera;
            }
            else
            {
                // If interpolation is already running, then do jumpcut
                lastCamera = -1;
            }

            // Set the current Camera to the new Camera and Initialize
            currentCamera = newCamera;
            cameras[currentCamera].SetTarget(target);
            cameras[currentCamera].InitCamera();
        }

        /// <summary>
        /// Gathers the Index of the current active Camera.
        /// </summary>
        /// <returns>The int index of the current active Camera</returns>
        public int GetCurrentCamera()
        {
            return currentCamera;
        }

        /// <summary>
        /// Sets the new Camera, with a transition time of 0.
        /// </summary>
        /// <param name="newCamera">The Index of the new Camera</param>
        public void SetCamera(int newCamera)
        {
            SetCamera(newCamera, 0f);
        }

        /// <summary>
        /// Set the Camera to the first Camera in the Array.
        /// </summary>
        void Start()
        {
            // All Cameras are disabled. This is acceptable because we are only 
            // using the Camera objects as motion and rotation behaviors. The
            // Perspective Controller simply copies their behaviors.
            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i] != null)
                {
                    cameras[i].enabled = false;
                    cameras[i].GetComponent<Camera>().enabled = false;
                }
            }

            // Ensure the PerspectiveController's Camera is active
            controllerCamera = this.GetComponent<Camera>();
            controllerCamera.enabled = true;

            // Initialize the Current Camera and copy the properties
            cameras[currentCamera].SetTarget(target);
            cameras[currentCamera].InitCamera();

            // Get the initial Position and Rotation if the current Camera
            this.transform.position = cameras[currentCamera].transform.position;
            this.transform.rotation = cameras[currentCamera].transform.rotation;

            // 
            controllerCamera.orthographicSize = cameras[currentCamera].GetComponent<Camera>().orthographicSize;
            controllerCamera.fieldOfView = cameras[currentCamera].GetComponent<Camera>().fieldOfView;
        }

        /// <summary>
        /// Controls the Current Camera.
        /// </summary>
        public void Update()
        {
            // Update the Cameras Position
            UpdatePosition();
        }

        /// <summary>
        /// Update the Current Camera Position.
        /// </summary>
        public void UpdatePosition()
        {
            // Update the Current Cameras Position
            cameras[currentCamera].UpdatePosition();

            // If the last camera hasn't been cleared, we are still in transition
            if (lastCamera != -1)
            {
                // Update our interpolation time
                interpolationTime += Time.deltaTime;

                // If in mid-transition, move from current position to new
                if (midTransitionFlag)
                {
                    // Use the Animation Curve to determine the progress of the Camera
                    float totalDistance = Vector3.Distance(midTransitionPosition,
                                                           cameras[currentCamera].transform.position);

                    // Ensure no division by zero, fixes jump on boundary when cameras are very similar.
                    if (totalDistance == 0)
                    {
                        lastCamera = -1;
                        this.transform.position = cameras[currentCamera].transform.position;
                        midTransitionFlag = false;
                        return;
                    }

                    // Determine Progress
                    lastInterpol = curve.Evaluate(interpolationTime * transitionSpeed / totalDistance);

                    // Update the position from the PerspectiveControllers current
                    this.transform.position = Vector3.Lerp(midTransitionPosition,
                                                           cameras[currentCamera].transform.position,
                                                           lastInterpol);
                }
                else
                {
                    // Update the last camera
                    cameras[lastCamera].UpdatePosition();

                    // Use the Animation Curve to determine the progress of the Camera
                    float totalDistance = Vector3.Distance(cameras[lastCamera].transform.position,
                                                           cameras[currentCamera].transform.position);

                    // Ensure no division by zero, fixes jump on boundary when cameras are very similar.
                    if (totalDistance == 0)
                    {
                        this.transform.position = cameras[currentCamera].transform.position;
                        lastCamera = -1;
                        midTransitionFlag = false;
                        return;
                    }

                    // Determine Progress
                    lastInterpol = curve.Evaluate(interpolationTime * transitionSpeed / totalDistance);

                    // Update the position from the last cameras position
                    this.transform.position = Vector3.Lerp(cameras[lastCamera].transform.position,
                                                           cameras[currentCamera].transform.position,
                                                           lastInterpol);
                }

                // If we have completed our Interpolation, disable the last camera Index
                if (lastInterpol >= 1)
                {
                    lastCamera = -1;
                    midTransitionFlag = false;
                }
            }
            else
            {
                // Set the Camera Manager to the current position
                this.transform.position = cameras[currentCamera].transform.position;
            }
        }
    }
}