using System;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics.Contacts
{
    public class ContactPositionConstraint
    {
        public readonly Vector2[] LocalPoints = new Vector2[Settings.MaxManifoldPoints];

        public int IndexA;

        public int IndexB;

        public Single InvIa, InvIb;

        public Single InvMassA, InvMassB;

        public Vector2 LocalCenterA, LocalCenterB;

        public Vector2 LocalNormal;

        public Vector2 LocalPoint;

        public int PointCount;

        public Single RadiusA, RadiusB;

        public ManifoldType Type;
    }
}