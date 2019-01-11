using System;
using System.Numerics;
using System.Runtime.CompilerServices;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
using Vector3 = FixedMath.Numerics.Fix64Vector3;
#endif

namespace Box2DSharp.Common
{
    public static class MathExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(in this Vector2 vector2)
        {
            return !Single.IsInfinity(vector2.X) && !Single.IsInfinity(vector2.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this Single x)
        {
            return !Single.IsInfinity(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetZero(ref this Vector2 vector2)
        {
            vector2.X = 0.0f;
            vector2.Y = 0.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set(ref this Vector2 vector2, Single x, Single y)
        {
            vector2.X = x;
            vector2.Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetZero(ref this Vector3 vector3)
        {
            vector3.X = 0.0f;
            vector3.Y = 0.0f;
            vector3.Z = 0.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set(ref this Vector3 vector3, Single x, Single y, Single z)
        {
            vector3.X = x;
            vector3.Y = y;
            vector3.Z = z;
        }

        /// Convert this vector into a unit vector. Returns the length.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Normalize(ref this Vector2 vector2)
        {
            var length = vector2.Length();
            if (length < Settings.Epsilon)
            {
                return 0.0f;
            }

            var invLength = 1.0f / length;
            vector2.X *= invLength;
            vector2.Y *= invLength;

            return length;
        }
    }
}