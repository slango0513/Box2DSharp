#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
#endif

using System;

namespace Box2DSharp.Common
{
    public static class Settings
    {
        public static readonly Single MaxFloat = Single.MaxValue;

        public static readonly Single Epsilon = Single.Epsilon;

        public static readonly Single Pi = 3.14159265359f;

        // @file
        // Global tuning constants based on meters-kilograms-seconds (MKS) units.

        // Collision

        /// The maximum number of contact points between two convex shapes. Do
        /// not change this value.
        public const int MaxManifoldPoints = 2;

        /// The maximum number of vertices on a convex polygon. You cannot increase
        /// this too much because b2BlockAllocator has a maximum object size.
        public const int MaxPolygonVertices = 8;

        /// This is used to fatten AABBs in the dynamic tree. This allows proxies
        /// to move by a small amount without triggering a tree adjustment.
        /// This is in meters.
        public static readonly Single AABBExtension = 0.1f;

        /// This is used to fatten AABBs in the dynamic tree. This is used to predict
        /// the future position based on the current displacement.
        /// This is a dimensionless multiplier.
        public static readonly Single AABBMultiplier = 2.0f;

        /// A small length used as a collision and constraint tolerance. Usually it is
        /// chosen to be numerically significant, but visually insignificant.
        public static readonly Single LinearSlop = 0.005f;

        /// A small angle used as a collision and constraint tolerance. Usually it is
        /// chosen to be numerically significant, but visually insignificant.
        public static readonly Single AngularSlop = 2.0f / 180.0f * Pi;

        /// The radius of the polygon/edge shape skin. This should not be modified. Making
        /// this smaller means polygons will have an insufficient buffer for continuous collision.
        /// Making it larger may create artifacts for vertex collision.
        public static readonly Single PolygonRadius = 2.0f * LinearSlop;

        /// Maximum number of sub-steps per contact in continuous physics simulation.
        public const int MaxSubSteps = 8;

        // Dynamics

        /// Maximum number of contacts to be handled to solve a TOI impact.
        public const int MaxToiContacts = 32;

        /// A velocity threshold for elastic collisions. Any collision with a relative linear
        /// velocity below this threshold will be treated as inelastic.
        public static readonly Single VelocityThreshold = 1.0f;

        /// The maximum linear position correction used when solving constraints. This helps to
        /// prevent overshoot.
        public static readonly Single MaxLinearCorrection = 0.2f;

        /// The maximum angular position correction used when solving constraints. This helps to
        /// prevent overshoot.
        public static readonly Single MaxAngularCorrection = 8.0f / 180.0f * Pi;

        /// The maximum linear velocity of a body. This limit is very large and is used
        /// to prevent numerical problems. You shouldn't need to adjust this.
        public static readonly Single MaxTranslation = 2.0f;

        public static readonly Single MaxTranslationSquared = MaxTranslation * MaxTranslation;

        /// The maximum angular velocity of a body. This limit is very large and is used
        /// to prevent numerical problems. You shouldn't need to adjust this.
        public static readonly Single MaxRotation = 0.5f * Pi;

        public static readonly Single MaxRotationSquared = MaxRotation * MaxRotation;

        /// This scale factor controls how fast overlap is resolved. Ideally this would be 1 so
        /// that overlap is removed in one time step. However using values close to 1 often lead
        /// to overshoot.
        public static readonly Single Baumgarte = 0.2f;

        public static readonly Single ToiBaugarte = 0.75f;

        // Sleep

        /// The time that a body must be still before it will go to sleep.
        public static readonly Single TimeToSleep = 0.5f;

        /// A body cannot sleep if its linear velocity is above this tolerance.
        public static readonly Single LinearSleepTolerance = 0.01f;

        /// A body cannot sleep if its angular velocity is above this tolerance.
        public static readonly Single AngularSleepTolerance = 2.0f / 180.0f * Pi;
    }
}