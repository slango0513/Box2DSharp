using System;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
#endif

namespace Box2DSharp.Dynamics
{
    /// Profiling data. Times are in milliseconds.
    public class Profile
    {
        public Single Step;

        public Single Collide;

        public Single Solve;

        public Single SolveInit;

        public Single SolveVelocity;

        public Single SolvePosition;

        public Single Broadphase;

        public Single SolveTOI;
    }
}