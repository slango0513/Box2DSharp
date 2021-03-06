using System.Numerics;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision
{
    /// Input parameters for b2ShapeCast
    public struct ShapeCastInput
    {
        public DistanceProxy ProxyA;

        public DistanceProxy ProxyB;

        public Transform TransformA;

        public Transform TransformB;

        public Vector2 TranslationB;
    }
}