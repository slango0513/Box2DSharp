using System;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
#endif

namespace Box2DSharp.Dynamics
{
    /// This is an internal structure.
    public struct TimeStep
    {
        public Single Dt; // time step

        public Single InvDt; // inverse time step (0 if dt == 0).

        public Single DtRatio; // dt * inv_dt0

        public int VelocityIterations;

        public int PositionIterations;

        public bool WarmStarting;
    }
}