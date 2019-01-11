using System;
using System.Numerics;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics.Contacts
{
    public class VelocityConstraintPoint
    {
        public Single NormalImpulse;

        public Single NormalMass;

        public Vector2 Ra;

        public Vector2 Rb;

        public Single TangentImpulse;

        public Single TangentMass;

        public Single VelocityBias;
    }
}