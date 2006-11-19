using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib
{
    public class LightManager : IList<Light>
    {
        List<Light> mLights = new List<Light>();
        bool mEnabled = true;
        Color mAmbient = Color.Black;

        public int AddPointLight(Vector3 position, Color diffuse)
        {
            mLights.Add(new Light());

            int retval = mLights.Count - 1;

            mLights[retval].Position = position;
            mLights[retval].Diffuse = diffuse;

            return retval;
        }
        public void SetLights()
        {
            Display.DoLighting(this);
        }
        public bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
        }
        public Color Ambient
        {
            get { return mAmbient; }
            set { mAmbient = value; }
        }

        #region --- IList<Light> Members ---

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

        public void Add(Light item)
        {
            if (mLights.Count >= Display.Caps.MaxLights)
            {
                throw new InvalidOperationException("Too many lights!");
            }

            mLights.Add(item);
        }
        public void Clear()
        {
            mLights.Clear();
        }
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
