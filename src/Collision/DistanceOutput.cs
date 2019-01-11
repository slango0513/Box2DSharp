using System;
using System.Numerics;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision
{
    /// Output for b2Distance.
    public struct DistanceOutput
    {
        /// closest point on shapeA
        public Vector2 PointA;

        /// closest point on shapeB
        public Vector2 PointB;

        public Single Distance;

        /// number of GJK iterations used
        public int Iterations;
    }
}