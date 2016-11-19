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
 * Filename: AbstractCamera.cs
 * 
 * Description: Implements the basic requirements for a stand-alone 
 *  Camera behaviour.
 * 
 *******************************************************************/
using UnityEngine;
using System.Collections.Generic;


namespace Assets.Scripts.Perspective.Cameras
{
    public enum eAxisRestriction
    {
        AxisRestrictionNone,
        AxisRestrictionNegative,
        AxisRestrictionPositive,
        AxisRestrictionBoth
    }

    public class CameraMovementRestrictions
    {

        public eAxisRestriction xAxis;
        public eAxisRestriction yAxis;
        public eAxisRestriction zAxis;

        public CameraMovementRestrictions()
        {
            xAxis = eAxisRestriction.AxisRestrictionNone;
            yAxis = eAxisRestriction.AxisRestrictionNone;
            zAxis = eAxisRestriction.AxisRestrictionNone;
        }

        public static CameraMovementRestrictions operator +(CameraMovementRestrictions r1, CameraMovementRestrictions r2)
        {
            CameraMovementRestrictions result = new CameraMovementRestrictions();

            if (r1.xAxis == eAxisRestriction.AxisRestrictionNone)
            {
                result.xAxis = r2.xAxis;
            }
            else if (((r1.xAxis == eAxisRestriction.AxisRestrictionNegative) && (r2.xAxis == eAxisRestriction.AxisRestrictionPositive)) ||
                     ((r1.xAxis == eAxisRestriction.AxisRestrictionPositive) && (r2.xAxis == eAxisRestriction.AxisRestrictionNegative)) ||
                      (r2.xAxis == eAxisRestriction.AxisRestrictionBoth))
            {
                result.xAxis = eAxisRestriction.AxisRestrictionBoth;
            }

            if (r1.yAxis == eAxisRestriction.AxisRestrictionNone)
            {
                result.yAxis = r2.yAxis;
            }
            else if (((r1.yAxis == eAxisRestriction.AxisRestrictionNegative) && (r2.yAxis == eAxisRestriction.AxisRestrictionPositive)) ||
                     ((r1.yAxis == eAxisRestriction.AxisRestrictionPositive) && (r2.yAxis == eAxisRestriction.AxisRestrictionNegative)) ||
                      (r2.yAxis == eAxisRestriction.AxisRestrictionBoth))
            {
                result.yAxis = eAxisRestriction.AxisRestrictionBoth;
            }

            if (r1.zAxis == eAxisRestriction.AxisRestrictionNone)
            {
                result.zAxis = r2.zAxis;
            }
            else if (((r1.zAxis == eAxisRestriction.AxisRestrictionNegative) && (r2.zAxis == eAxisRestriction.AxisRestrictionPositive)) ||
                     ((r1.zAxis == eAxisRestriction.AxisRestrictionPositive) && (r2.zAxis == eAxisRestriction.AxisRestrictionNegative)) ||
                      (r2.zAxis == eAxisRestriction.AxisRestrictionBoth))
            {
                result.zAxis = eAxisRestriction.AxisRestrictionBoth;
            }

            return result;
        }
    }

    /// <summary>
    /// Default Behavior and Requirements for a Camera.
    /// </summary>
    public abstract class AbstractCamera : MonoBehaviour
    {
        /// <summary>
        /// The position that that camera will be following.
        /// </summary>
        public Transform target;

        public Dictionary<int, CameraMovementRestrictions> restrictionList = new Dictionary<int, CameraMovementRestrictions>();

        /// <summary>
        /// Any movment axis that are restricted.
        /// </summary>
        public CameraMovementRestrictions restrictions = new CameraMovementRestrictions();

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnEnable()
        {
            PerspectiveManager.Instance.RegisterCamera(this);
        }

        public virtual void OnDisable()
        {
            PerspectiveManager.Instance.DeregisterCamera(this);
        }

        /// <summary>
        /// Sets the target runtime.
        /// </summary>
        public virtual void SetTarget(Transform transform)
        {
            this.target = transform;
        }

        /// <summary>
        /// Get the Target at Runtime.
        /// </summary>
        /// <returns>The Cameras Target Transform</returns>
        public Transform GetTarget()
        {
            return this.target;
        }

        /// <summary>
        /// Sets the Initial State of the Camera.
        /// </summary>
        public virtual void InitCamera()
        {
            // Perform any Initialization steps required.
        }

        /// <summary>
        /// Controls the Camera actions every Frame.
        /// </summary>
        public void Update()
        {
            // Update the Cameras Position
            UpdatePosition();
        }

        /// <summary>
        /// Ensures the Cameras position is updated every Frame if required.
        /// </summary>
        public virtual void UpdatePosition()
        {
            // Abstract Implementation does not move the Camera.
        }

        public virtual void UpdateMovementRestrictions()
        {
            CameraMovementRestrictions newRestriction = new CameraMovementRestrictions();

            if (0 == restrictionList.Count)
            {
                restrictions.xAxis = eAxisRestriction.AxisRestrictionNone;
                restrictions.yAxis = eAxisRestriction.AxisRestrictionNone;
                restrictions.zAxis = eAxisRestriction.AxisRestrictionNone;
            }
            else
            {
                foreach (KeyValuePair<int, CameraMovementRestrictions> entry in restrictionList)
                {
                    newRestriction += entry.Value;
                }
            }
            restrictions = newRestriction;
        }

        public void SetPosition(Vector3 pos)
        {
            float tempX, tempY, tempZ;
            
            // Calculate Lock status
            UpdateMovementRestrictions();
            
            if ( (restrictions.xAxis == eAxisRestriction.AxisRestrictionBoth) || 
                (((pos.x - this.transform.position.x) < 0) && (restrictions.xAxis == eAxisRestriction.AxisRestrictionNegative)) ||
                (((pos.x - this.transform.position.x) > 0) && (restrictions.xAxis == eAxisRestriction.AxisRestrictionPositive)))
            {
                tempX = this.transform.position.x;
            }
            else
            {
                tempX = pos.x;
            }
            
            if ((restrictions.yAxis == eAxisRestriction.AxisRestrictionBoth) ||
                (((pos.y - this.transform.position.y) < 0) && (restrictions.yAxis == eAxisRestriction.AxisRestrictionNegative)) ||
                (((pos.y - this.transform.position.y) > 0) && (restrictions.yAxis == eAxisRestriction.AxisRestrictionPositive)))
            {
                tempY = this.transform.position.y;
            }
            else
            {
                tempY = pos.y;
            }
            
            if ((restrictions.zAxis == eAxisRestriction.AxisRestrictionBoth) ||
                (((pos.z - this.transform.position.z) < 0) && (restrictions.zAxis == eAxisRestriction.AxisRestrictionNegative)) ||
                (((pos.z - this.transform.position.z) > 0) && (restrictions.zAxis == eAxisRestriction.AxisRestrictionPositive)))
            {
                tempZ = this.transform.position.z;
            }
            else
            {
                tempZ = pos.z;
            }
            this.transform.position = new Vector3(tempX, tempY, tempZ);
        }

        /// <summary>
        /// Axis the movement should be constrained.
        /// </summary>
        public virtual void RegisterBound(int id, CameraMovementRestrictions restriction)
        {
            CameraMovementRestrictions dummy;

            if ( false == restrictionList.TryGetValue(id, out dummy))
            {
                restrictionList.Add(id, restriction);
                Debug.Log("Restriction Added");
            }
            else
            {
                restrictionList.Remove(id);
                restrictionList.Add(id, restriction);
                Debug.Log("Restriction Replaced");
            }
        }

        /// <summary>
        /// Axis the movement should be unconstrained.
        /// </summary>
        public virtual void DeregisterBound(int id)
        {
            restrictionList.Remove(id);
            Debug.Log("Restriction Removed");
        }
    }
}