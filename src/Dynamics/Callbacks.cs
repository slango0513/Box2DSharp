using System;
using System.Numerics;
using Box2DSharp.Collision.Collider;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics
{
    public delegate bool QueryCallback(Fixture fixture);

    /// Callback for ray casts.
    /// See b2World::RayCast
    /// Called for each fixture found in the query. You control how the ray cast
    /// proceeds by returning a Single:
    /// return -1: ignore this fixture and continue
    /// return 0: terminate the ray cast
    /// return fraction: clip the ray to this point
    /// return 1: don't clip the ray and continue
    /// @param fixture the fixture hit by the ray
    /// @param point the point of initial intersection
    /// @param normal the normal vector at the point of intersection
    /// @return -1 to filter, 0 to terminate, fraction to clip the ray for
    /// closest hit, 1 to continue
    public delegate Single RayCastCallback(Fixture fixture, Vector2 point, Vector2 normal, Single fraction);
}

namespace Box2DSharp.Dynamics.Internal
{
    public delegate void AddPairCallback(FixtureProxy proxyA, FixtureProxy proxyB);

    public delegate bool InternalQueryCallback(int proxyId);

    public delegate Single InternalRayCastCallback(in RayCastInput input, int proxyId);
}