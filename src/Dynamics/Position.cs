using System;
using System.Numerics;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics
{
    /// This is an internal structure.
    public struct Position
    {
        public Vector2 Center;

        public Single Angle;
    }
}