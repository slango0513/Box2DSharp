using System;
using System.Numerics;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision.Collider
{
    /// This is used to compute the current state of a contact manifold.
    public struct WorldManifold
    {
        /// Evaluate the manifold with supplied transforms. This assumes
        /// modest motion from the original state. This does not change the
        /// point count, impulses, etc. The radii must come from the shapes
        /// that generated the manifold.
        public void Initialize(
            in Manifold manifold,
            in Transform xfA,
            Single radiusA,
            in Transform xfB,
            Single radiusB)
        {
            if (manifold.PointCount == 0)
            {
                return;
            }

            switch (manifold.Type)
            {
            case ManifoldType.Circles:
            {
                Normal.Set(1.0f, 0.0f);
                var pointA = MathUtils.Mul(xfA, manifold.LocalPoint);
                var pointB = MathUtils.Mul(xfB, manifold.Points[0].LocalPoint);
                if (MathUtils.DistanceSquared(pointA, pointB) > Settings.Epsilon * Settings.Epsilon)
                {
                    Normal = pointB - pointA;
                    Normal.Normalize();
                }

                var cA = pointA + radiusA * Normal;
                var cB = pointB - radiusB * Normal;
                Points[0] = 0.5f * (cA + cB);
                Separations[0] = MathUtils.Dot(cB - cA, Normal);
            }
                break;

            case ManifoldType.FaceA:
            {
                Normal = MathUtils.Mul(xfA.Rotation, manifold.LocalNormal);
                var planePoint = MathUtils.Mul(xfA, manifold.LocalPoint);

                for (var i = 0; i < manifold.PointCount; ++i)
                {
                    var clipPoint = MathUtils.Mul(xfB, manifold.Points[i].LocalPoint);
                    var cA = clipPoint + (radiusA - MathUtils.Dot(clipPoint - planePoint, Normal)) * Normal;
                    var cB = clipPoint - radiusB * Normal;
                    Points[i] = 0.5f * (cA + cB);
                    Separations[i] = MathUtils.Dot(cB - cA, Normal);
                }
            }
                break;

            case ManifoldType.FaceB:
            {
                Normal = MathUtils.Mul(xfB.Rotation, manifold.LocalNormal);
                var planePoint = MathUtils.Mul(xfB, manifold.LocalPoint);

                for (var i = 0; i < manifold.PointCount; ++i)
                {
                    var clipPoint = MathUtils.Mul(xfA, manifold.Points[i].LocalPoint);
                    var cB = clipPoint + (radiusB - MathUtils.Dot(clipPoint - planePoint, Normal)) * Normal;
                    var cA = clipPoint - radiusA * Normal;
                    Points[i] = 0.5f * (cA + cB);
                    Separations[i] = MathUtils.Dot(cA - cB, Normal);
                }

                // Ensure normal points from A to B.
                Normal = -Normal;
            }
                break;
            }
        }

        /// world vector pointing from A to B
        public Vector2 Normal;

        /// <summary>
        /// world contact point (point of intersection), size Settings.MaxManifoldPoints
        /// </summary>
        public FixedArray2<Vector2> Points;

        /// <summary>
        /// a negative value indicates overlap, in meters, size Settings.MaxManifoldPoints
        /// </summary>
        public FixedArray2<Single> Separations;
    }
}