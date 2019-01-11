using System;
using System.Numerics;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision.Collider
{
    /// A manifold point is a contact point belonging to a contact
    /// manifold. It holds details related to the geometry and dynamics
    /// of the contact points.
    /// The local point usage depends on the manifold type:
    /// -e_circles: the local center of circleB
    /// -e_faceA: the local center of cirlceB or the clip point of polygonB
    /// -e_faceB: the clip point of polygonA
    /// This structure is stored across time steps, so we keep it small.
    /// Note: the impulses are used for internal caching and may not
    /// provide reliable contact forces, especially for high speed collisions.
    public struct ManifoldPoint
    {
        /// usage depends on manifold type
        public Vector2 LocalPoint;

        /// the non-penetration impulse
        public Single NormalImpulse;

        /// /// the friction impulse
        public Single TangentImpulse;

        /// uniquely identifies a contact point between two shapes
        public ContactId Id;
    }
}