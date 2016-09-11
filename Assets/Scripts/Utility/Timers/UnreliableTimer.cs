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
 * Filename: UnreliableTimer.cs
 * 
 * Description: Implements a timer that can be updated spuratically.
 * 
 *******************************************************************/
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utility.Timers
{
    /// <summary>
    /// Used for Loose timeout on Objects who update 
    /// irregularly. 
    /// </summary>
    public class UnreliableTimer : AbstractTimer
    {
        /// <summary>
        /// Determines if the Timer is running or stalled.
        /// </summary>
        public bool runningState = false;

        /// <summary>
        /// Used to determine the amount of time to add
        /// to the timer count.
        /// </summary>
        private float timeOfLastUpdate;

        /// <summary>
        /// Constructor sets the timeout.
        /// </summary>
        /// <param name="seconds">The timeout in seconds</param>
        public UnreliableTimer(float seconds) : base(seconds)
        {
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Start()
        {
            // Set the time of initialization
            timeOfLastUpdate = Time.time;

            // Set the Start Flag
            runningState = true;
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void Stop()
        {
            runningState = false;
        }

        /// <summary>
        /// Updates the Timer's internal count.
        /// This is performed using the difference in time
        /// from the last Update.
        /// Can be called anywhere in an Object.
        /// </summary>
        protected override void InternalUpdate()
        {
            // If the Timer is running, update the time
            if (runningState)
            {
                // Find the difference in time since the last update
                float difference = Time.time - timeOfLastUpdate;

                // Add the difference to the internal count
                internalCount += difference;
            }
        }
    }
}
