using System;
using System.Numerics;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics.Contacts
{
    public class ContactVelocityConstraint
    {
        public readonly VelocityConstraintPoint[] Points = new VelocityConstraintPoint[Settings.MaxManifoldPoints]
        {
            new VelocityConstraintPoint(),
            new VelocityConstraintPoint()
        };

        public int ContactIndex;

        public Single Friction;

        public int IndexA;

        public int IndexB;

        public Single InvIa, InvIb;

        public Single InvMassA, InvMassB;

        public Matrix2x2 K;

        public Vector2 Normal;

        public Matrix2x2 NormalMass;

        public int PointCount;

        public Single Restitution;

        public Single TangentSpeed;
    }
}