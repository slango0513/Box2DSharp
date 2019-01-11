using System;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Math = FixedMath.MathFix;
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Collision.Shapes
{
    public class CircleShape : Shape
    {
        /// Position
        public Vector2 Position;

        public CircleShape()
        {
            ShapeType = ShapeType.Circle;
            Radius = 0.0f;
            Position.SetZero();
        }

        /// Implement b2Shape.
        public override Shape Clone()
        {
            var clone = new CircleShape {Position = Position, Radius = Radius};
            return clone;
        }

        /// @see b2Shape::GetChildCount
        public override int GetChildCount()
        {
            return 1;
        }

        /// Implement b2Shape.
        public override bool TestPoint(in Transform transform, in Vector2 p)
        {
            var center = transform.Position + MathUtils.Mul(transform.Rotation, Position);
            var d = p - center;
            return MathUtils.Dot(d, d) <= Radius * Radius;
        }

        /// Implement b2Shape.
        public override bool RayCast(
            out RayCastOutput output,
            in RayCastInput input,
            in Transform transform,
            int childIndex)
        {
            output = default;
            var position = transform.Position + MathUtils.Mul(transform.Rotation, Position);
            var s = input.P1 - position;
            var b = MathUtils.Dot(s, s) - Radius * Radius;

            // Solve quadratic equation.
            var r = input.P2 - input.P1;
            var c = MathUtils.Dot(s, r);
            var rr = MathUtils.Dot(r, r);
            var sigma = c * c - rr * b;

            // Check for negative discriminant and short segment.
            if (sigma < 0.0f || rr < Settings.Epsilon)
            {
                return false;
            }

            // Find the point of intersection of the line with the circle.
            var a = -(c + (Single) Math.Sqrt(sigma));

            // Is the intersection point on the segment?
            if (0.0f <= a && a <= input.MaxFraction * rr)
            {
                a /= rr;
                output = new RayCastOutput {Fraction = a, Normal = s + a * r};
                output.Normal.Normalize();
                return true;
            }

            return false;
        }

        /// @see b2Shape::ComputeAABB
        public override void ComputeAABB(
            out AABB aabb,
            in Transform transform,
            int
                childIndex)
        {
            var p = transform.Position + MathUtils.Mul(transform.Rotation, Position);
            aabb = new AABB();
            aabb.LowerBound.Set(p.X - Radius, p.Y - Radius);
            aabb.UpperBound.Set(p.X + Radius, p.Y + Radius);
        }

        /// @see b2Shape::ComputeMass
        public override void ComputeMass(out MassData massData, Single density)
        {
            massData = new MassData();
            massData.Mass = density * Settings.Pi * Radius * Radius;
            massData.Center = Position;

            // inertia about the local origin
            massData.RotationInertia = massData.Mass * (0.5f * Radius * Radius + MathUtils.Dot(Position, Position));
        }
    }
}