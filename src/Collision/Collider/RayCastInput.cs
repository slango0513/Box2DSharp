using System;
using System.Numerics;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision.Collider
{
    /// Ray-cast input data. The ray extends from p1 to p1 + maxFraction * (p2 - p1).
    public struct RayCastInput
    {
        public Vector2 P1;

        public Vector2 P2;

        public Single MaxFraction;
    }
}