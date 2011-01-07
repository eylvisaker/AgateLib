using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.Geometry;

namespace ShootTheTraps
{
    public abstract class GameObject
    {
        
        /// <summary>
        /// Position of the object.  This is automatically updated from
        /// acceleration and velocity during the call to Update(). 
        /// </summary>
        public Vector3d Position;
        /// <summary>
        /// Velocity of the object.  This is automatically updated from
        /// acceleration during the call to Update(). 
        /// </summary>
        public Vector3d Velocity;
        /// <summary>
        /// Acceleration of the object based on its position at the start
        /// of this frame.  The object or application is responsible for
        /// updating this if it changes. */
        /// </summary>
        public Vector3d Acceleration;

        public bool mDoDeleteObjects = false;

        private Vector3d mOldAcceleration;

        /// <summary>
        /// The value for gravity, in pixels / second / second. 
        /// </summary>
        public const float GRAVITY = 300;

        /// Creates a new instance of GameObject 
        /// </summary>
        public GameObject()
        {
            Position = new Vector3d();
            Velocity = new Vector3d();
            Acceleration = new Vector3d();
            mOldAcceleration = new Vector3d();
        }

        /// <summary>
        /// Integrates motion equations to calculate new position.
        /// UpdateDisplay Acceleration before calling this.
        /// </summary>
        public void Update(double milliseconds)
        {
            double seconds = milliseconds / 1000.0;

            // here we do a velocity verlet scheme
            Position +=
                    Velocity * seconds +
                    mOldAcceleration * (0.5 * seconds * seconds);

            Vector3d vel2 = Velocity + mOldAcceleration * (0.5 * seconds);
            Velocity = vel2 + Acceleration * (0.5 * seconds);

            mOldAcceleration = Acceleration;
        }

        /// <summary>
        /// Draws the object to the specified graphics context.
        ///  Must be overriden. 
        /// </summary>
        public abstract void Draw();
        /// <summary>
        /// Returns true if the object should be deleted.  Should be overriden
        /// so objects can notify the controller when they no longer need to be
        /// considered. */
        /// </summary>
        public virtual bool DeleteMe
        {
            get { return false; }
        }
        /// <summary>
        /// Returns an array of objects to be added when this object is deleted.
        /// This allows for objects to create "particle effects" or the like
        /// when destroyed. 
        /// Returns null to add nothing.     
        /// </summary>
        public List<GameObject> DeleteObjects()
        {
            if (mDoDeleteObjects == false)
                return null;
            else
                return DeleteObjectsInternal();
        }

        /// <summary>
        /// Actual implementation of DeleteObjects is done here.
        /// Override this in derived classes. 
        /// </summary>
        protected virtual List<GameObject> DeleteObjectsInternal()
        {
            return null;
        }
    }

}