using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Animation_Engine.Engine.Core.Scene.Scenegraph.FX.WaterFoil
{
    /// <summary>
    /// Used struct because:
    /// * We dont need to pass it to a function anyways
    /// * The program doesnt have to figure out the ref object
    /// </summary>
    public struct WaterFoilPatch
    {
        private Vector2 _directionalWaveForce;
        private float _verticalPreImpuls; // This stores the impuls for the next frame
        private float _verticalImpuls; // Used to evaluate the current impuls
        private float _currentHeight;
        private float _minimalHeight;
        private bool _isSolid;
        

        public float MinimalHeight
        {
            get { return _minimalHeight; }
            set { _minimalHeight = value; }
        }

        public bool IsSolid
        {
            get { return _isSolid; }
            set { _isSolid = value; }
        }

        public float CurrentHeight
        {
            get { return _currentHeight; }
            set { _currentHeight = value; }
        }

        public float VerticalImpuls
        {
            get { return _verticalImpuls; }
            set { _verticalImpuls = value; }
        }

        public Vector2 DirectionalWaveForce
        {
            get { return _directionalWaveForce; }
            set { _directionalWaveForce = value; }
        }

        public float VerticalPreImpuls
        {
            get { return _verticalPreImpuls; }
            set { _verticalPreImpuls = value; }
        }
    }
}
