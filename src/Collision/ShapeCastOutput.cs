using System;
using System.Numerics;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision
{
    /// Output results for b2ShapeCast
    public struct ShapeCastOutput
    {
        public Vector2 Point;

        public Vector2 Normal;

        public Single Lambda;

        public int Iterations;
    }
}