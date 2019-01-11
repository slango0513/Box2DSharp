using Box2DSharp.Common;
using System;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
#endif

namespace Box2DSharp.Dynamics.Contacts
{
    /// Contact impulses for reporting. Impulses are used instead of forces because
    /// sub-step forces may approach infinity for rigid body collisions. These
    /// match up one-to-one with the contact points in b2Manifold.
    public struct ContactImpulse
    {
        public Single[] NormalImpulses;

        public Single[] TangentImpulses;

        public int Count;

        public static ContactImpulse Create()
        {
            return new ContactImpulse
            {
                Count = 0,
                NormalImpulses = new Single[Settings.MaxManifoldPoints],
                TangentImpulses = new Single[Settings.MaxManifoldPoints]
            };
        }
    }
}