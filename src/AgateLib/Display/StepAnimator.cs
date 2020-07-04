using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Display
{
    /// <summary>
    /// Animates a transition between 0 and 1.
    /// </summary>
    public class StepAnimator
    {
        private int direction = 1;
        private float inputValue;

        /// <summary>
        /// The input value of the transition. This will track the advancement in wall clock time.
        /// </summary>
        public float InputValue
        {
            get => inputValue;
            set
            {
                if (value > 1) value = 1;
                if (value < 0) value = 0;

                inputValue = value;

                if (EasingFunction != null)
                {
                    T = EasingFunction(inputValue);
                }
                else
                {
                    T = value;
                }
            }
        }

        /// <summary>
        /// The output value of the transition. This is the current position in the animation.
        /// </summary>
        public float T { get; set; }

        /// <summary>
        /// Computes 4*T*(1-T). This is zero when T = 0 and 1 and 1 when T = 0.5.
        /// </summary>
        public float S => 4 * T * (1 - T);

        /// <summary>
        /// The rate at which the input value advances, in 1/seconds.
        /// </summary>
        public float Rate { get; set; } = 1;

        /// <summary>
        /// The duration in seconds for the animation.
        /// </summary>
        public float Duration 
        {
            get => 1 / Rate;
            set => Rate = 1 / value;
        }

        /// <summary>
        /// The easing function used to tranform the InputValue to T.
        /// </summary>
        public Func<float, float> EasingFunction { get; set; }
        
        public void Update(GameTime time)
        {
            InputValue += direction * time.ElapsedInSeconds() * Rate;
        }
    }

}
