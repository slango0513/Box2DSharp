using System.Numerics;
#if USE_FIXED_POINT
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision.Collider
{
    /// Used for computing contact manifolds.
    public struct ClipVertex
    {
        public Vector2 Vector;

        public ContactId Id;
    }
}