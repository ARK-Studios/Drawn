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
 * Filename: AbstractTimer.cs
 * 
 * Description: Describes the base behavior of a Timer and implements
 *  some generic functions.
 * 
 *******************************************************************/
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utility.Timers
{
    /// <summary>
    /// Provides the base Timer components.
    /// </summary>
    public abstract class AbstractTimer
    {
        /// <summary>
        /// The timeout has been triggered since
        /// the last reset.
        /// </summary>
        public bool triggered = false;

        /// <summary>
        /// Determines if the Timer is running or stalled.
        /// </summary>
        public bool runningState = false;

        /// <summary>
        /// Allows the timer to rollover on timeout
        /// if enabled.
        /// </summary>
        public bool enableRollover = false;

        /// <summary>
        /// The timeout of the Timer.
        /// </summary>
        public float timeout = 1.0f;

        /// <summary>
        /// The internal count of the Timer.
        /// </summary>
        protected float internalCount = 0f;

        /// <summary>
        /// Constructor to initialize the base
        /// timer components.
        /// </summary>
        /// <param name="seconds">Timeout in seconds</param>
        /// <param name="rollover">Allows the Timer to rollover the timeout. Default is false</param>
        protected AbstractTimer(float seconds, bool rollover = false)
        {
            // Set the values and state
            timeout = seconds;
            enableRollover = rollover;
            internalCount = 0;
            triggered = false;
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public virtual void Start()
        {
            // Set the Start Flag
            runningState = true;
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public virtual void Stop()
        {
            runningState = false;
        }

        /// <summary>
        /// Returns the triggered state of the Timer.
        /// If rollover is enabled, the state is returned.
        /// If the state was true, it is returned then set to false.
        /// </summary>
        /// <returns>The triggered state of the Timer</returns>
        public bool IsTriggered()
        {
            // If not rollover, simply return state
            if (!enableRollover)
            {
                return triggered;
            }
            else
            {
                // If rollover and is triggered, return
                // true and set to false
                if (triggered)
                {
                    triggered = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Sets a new Timeout value.
        /// </summary>
        /// <param name="time">Timeout in seconds</param>
        public void SetTimeout(float time)
        {
            timeout = time;
        }

        /// <summary>
        /// Resets the progress of the Timer.
        /// </summary>
        public virtual void Reset()
        {
            internalCount = 0;
            triggered = false;
        }

        /// <summary>
        /// Conveinance function for setting new timeout value.
        /// </summary>
        /// <param name="time">Timeout in seconds</param>
        public void SetTimeoutAndReset(float time)
        {
            SetTimeout(time);
            Reset();
        }

        /// <summary>
        /// Updates the Timer's internal count.
        /// </summary>
        public void Update()
        {
            // If the Timer is running, update the time
            if (runningState)
            {

                // If timer hasn't triggered, or rollover is enabled
                if (!triggered || enableRollover)
                {
                    // Update the count
                    InternalUpdate();

                    // If the count has now reached the timeout
                    if (internalCount >= timeout)
                    {
                        // If Rollover is enabled
                        if (enableRollover)
                        {
                            // Make the internal count the overrun
                            internalCount = internalCount % timeout;
                        }

                        // Set the timeout completion flag
                        triggered = true;
                    }
                }
            }
        }

        /// <summary>
        /// Convienence to Update the internal count and 
        /// return the triggered state.
        /// </summary>
        public bool UpdateAndCheck()
        {
            // Update the internal count
            Update();

            // Return the state
            return IsTriggered();
        }

        /// <summary>
        /// Performs the Timers internal count update
        /// </summary>
        protected abstract void InternalUpdate();
    }
}
