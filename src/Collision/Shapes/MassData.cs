using System;
using System.Numerics;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision.Shapes
{
    public struct MassData
    {
        /// The mass of the shape, usually in kilograms.
        public Single Mass;

        /// The position of the shape's centroid relative to the shape's origin.
        public Vector2 Center;

        /// The rotational inertia of the shape about the local origin.
        public Single RotationInertia;
    }
}