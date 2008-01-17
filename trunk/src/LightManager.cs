using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib
{
    /// <summary>
    /// The LightManager class keeps a list of Light objects which can be used
    /// to setup the lighting in the rendering API.
    /// </summary>
    public class LightManager : IList<Light>
    {
        List<Light> mLights = new List<Light>();
        bool mEnabled = true;
        Color mAmbient = Color.Black;

        internal static readonly LightManager Empty;

        static LightManager()
        {
            Empty = new LightManager();
            Empty.Enabled = false;
        }

        /// <summary>
        /// Adds a point light to the specified position with the given diffuse color.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="diffuse"></param>
        /// <returns></returns>
        public int AddPointLight(Vector3 position, Color diffuse)
        {
            return AddPointLight(position, diffuse, Color.Black);
        }
        /// <summary>
        /// Adds a point light to the specified position with the given diffuse and ambient colors.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="diffuse"></param>
        /// <param name="ambient"></param>
        /// <returns></returns>
        public int AddPointLight(Vector3 position, Color diffuse, Color ambient)
        {
            mLights.Add(new Light());

            int retval = mLights.Count - 1;

            mLights[retval].Position = position;
            mLights[retval].Diffuse = diffuse;
            mLights[retval].Ambient = ambient;

            return retval;
        }
        /// <summary>
        /// This tells the Display to start use the Lights in this LightManager structure for
        /// lighting.
        /// </summary>
        public void DoLighting()
        {
            Display.DoLighting(this);
        }
        /// <summary>
        /// Gets or sets a flag indicating whether or not lighting calculations should be performed.
        /// </summary>
        public bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
        }
        /// <summary>
        /// Gets or sets the global ambient light color.  This light color is applied to
        /// all objects rendered, regardless of their position.
        /// </summary>
        public Color Ambient
        {
            get { return mAmbient; }
            set { mAmbient = value; }
        }

        #region --- IList<Light> Members ---

        /// <summary>
        /// Gets or sets a Light in the list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Light this[int index]
        {
            get
            {
                return mLights[index];
            }
            set
            {
                mLights[index] = value;
            }
        }
        /// <summary>
        /// Removes a particular light.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            mLights.RemoveAt(index);
        }

        int IList<Light>.IndexOf(Light item)
        {
            return mLights.IndexOf(item);
        }
        void IList<Light>.Insert(int index, Light item)
        {
            mLights.Insert(index, item);
        }

        #endregion
        #region --- ICollection<Light> Members ---

        /// <summary>
        /// Adds a Light to the list.  If there are more lights than possible, an exception is thrown.
        /// </summary>
        /// <param name="item"></param>
        public void Add(Light item)
        {
            if (mLights.Count >= Display.Caps.MaxLights)
            {
                throw new InvalidOperationException("Too many lights!");
            }

            mLights.Add(item);
        }
        /// <summary>
        /// Removes all Lights from the list.
        /// </summary>
        public void Clear()
        {
            mLights.Clear();
        }
        /// <summary>
        /// Gets the number of Lights in the list.
        /// </summary>
        public int Count
        {
            get { return mLights.Count; }
        }

        bool ICollection<Light>.Contains(Light item)
        {
            return mLights.Contains(item);
        }
        void ICollection<Light>.CopyTo(Light[] array, int arrayIndex)
        {
            mLights.CopyTo(array, arrayIndex);
        }


        bool ICollection<Light>.IsReadOnly
        {
            get { return false; }
        }
        bool ICollection<Light>.Remove(Light item)
        {
            return mLights.Remove(item);
        }

        #endregion
        #region --- IEnumerable<Light> Members ---

        /// <summary>
        /// Enumerates the Lights in the list.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Light> GetEnumerator()
        {
            return mLights.GetEnumerator();
        }

        #endregion
        #region --- IEnumerable Members ---

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
