using Box2DSharp.Common;
using System;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
#endif

namespace Box2DSharp.Collision
{
    /// Used to warm start b2Distance.
    /// Set count to zero on first call.
    public struct SimplexCache
    {
        /// length or area
        public Single Metric;

        public ushort Count;

        /// vertices on shape A
        public FixedArray3<byte> IndexA;

        /// vertices on shape B
        public FixedArray3<byte> IndexB;
    }
}