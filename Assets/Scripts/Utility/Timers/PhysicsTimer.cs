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
 * Filename: PhysicsTimer.cs
 * 
 * Description: Implements a Timer that updates in the Physics layer.
 * 
 *******************************************************************/
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utility.Timers
{
    /// <summary>
    /// A Timer that updates using the Physics interval time.
    /// </summary>
    public class PhysicsTimer : AbstractTimer
    {
        /// <summary>
        /// Constructor sets the Timeout.
        /// </summary>
        /// <param name="seconds">The timeout in seconds</param>
        public PhysicsTimer(float seconds) : base(seconds)
        {
        }

        /// <summary>
        /// Updates the Timer's internal count.
        /// This is performed using the FixedUpdate time.
        /// Must be called in the Objects FixedUpdate.
        /// </summary>
        protected override void InternalUpdate()
        {
            // Update the internal count with the time
            // from the last physics update.
            internalCount += Time.fixedDeltaTime;
        }
    }
}
