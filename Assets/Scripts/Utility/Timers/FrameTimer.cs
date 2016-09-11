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
 * Filename: FrameTimer.cs
 * 
 * Description: Implements a Timer that updates during the Graphics 
 *  layer.
 * 
 *******************************************************************/
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utility.Timers
{
    /// <summary>
    /// A timer that updates using the Frame interval time.
    /// </summary>
    public class FrameTimer : AbstractTimer
    {
        /// <summary>
        /// Constructor sets the Timeout.
        /// </summary>
        /// <param name="seconds">The timeout in seconds</param>
        public FrameTimer(float seconds) : base(seconds)
        {
        }

        /// <summary>
        /// Updates the Timer's internal count.
        /// This is performed using the DeltaTime for the frame.
        /// Must be called in the Objects Update or LateUpdate.
        /// </summary>
        protected override void InternalUpdate()
        {
            // Update the internal count with the time
            // from the last Frame.
            internalCount += Time.deltaTime;
        }
    }
}
