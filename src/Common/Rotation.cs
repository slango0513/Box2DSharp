using System;
using System.Numerics;
using System.Runtime.CompilerServices;
#if USE_FIXED_POINT
using Math = FixedMath.MathFix;
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Common
{
    /// Rotation
    public struct Rotation
    {
        /// Sine and cosine
        public Single Sin;

        public Single Cos;

        /// Initialize from an angle in radians
        public Rotation(Single angle)
        {
            // TODO_ERIN optimize
            Sin = (Single) Math.Sin(angle);
            Cos = (Single) Math.Cos(angle);
        }

        /// Set using an angle in radians.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(Single angle)
        {
            // TODO_ERIN optimize
            Sin = (Single) Math.Sin(angle);
            Cos = (Single) Math.Cos(angle);
        }

        /// Set to the identity rotation
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetIdentity()
        {
            Sin = 0.0f;
            Cos = 1.0f;
        }

        /// Get the angle in radians
        public Single Angle => (Single) Math.Atan2(Sin, Cos);

        /// Get the x-axis
        public Vector2 GetXAxis()
        {
            return new Vector2(Cos, Sin);
        }

        /// Get the u-axis
        public Vector2 GetYAxis()
        {
            return new Vector2(-Sin, Cos);
        }
    }
}