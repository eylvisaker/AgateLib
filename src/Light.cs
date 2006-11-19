using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib
{
    public class Light
    {
        private bool mEnabled = true;
        private Color mAmbient = Color.Black;
        private Color mDiffuse = Color.White;
        private Color mSpecular = Color.Black;
        private Vector3 mPosition = Vector3.Empty;
        private float mConstantAttenuation = 0.01f;
        private float mLinearAttenuation = 0.0f;
        private float mQuadraticAttenuation = 0.00001f;
        private float mRange = 100000;

        public float AttenuationConstant
        {
            get { return mConstantAttenuation; }
            set { mConstantAttenuation = value; }
        }
        public float AttenuationLinear
        {
            get { return mLinearAttenuation; }
            set { mLinearAttenuation = value; }
        }
        public float AttenuationQuadratic
        {
            get { return mQuadraticAttenuation; }
            set { mQuadraticAttenuation = value; }
        }

        public float Range
        {
            get { return mRange; }
            set { mRange = value; }
        }

        public void SetAttenuationModel(float constant, float linear, float quadratic)
        {
            AttenuationConstant = constant;
            AttenuationLinear = linear;
            AttenuationQuadratic = quadratic;
        }
        public bool Enabled
        {
            get
            {
                return mEnabled;
            }
            set
            {
                mEnabled = value;
            }
        }

        public Color Ambient
        {
            get { return mAmbient; }
            set { mAmbient = value; }
        }
        public Color Diffuse
        {
            get { return mDiffuse; }
            set { mDiffuse = value; }
        }
        public Color Specular
        {
            get { return mSpecular; }
            set { mSpecular = value; }
        }
        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

    }
}