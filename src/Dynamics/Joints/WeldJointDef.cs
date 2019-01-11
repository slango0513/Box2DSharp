using System;
using System.Numerics;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics.Joints
{
    /// Weld joint definition. You need to specify local anchor points
    /// where they are attached and the relative body angle. The position
    /// of the anchor points is important for computing the reaction torque.
    public class WeldJointDef : JointDef
    {
        /// The damping ratio. 0 = no damping, 1 = critical damping.
        public Single DampingRatio;

        /// The mass-spring-damper frequency in Hertz. Rotation only.
        /// Disable softness with a value of 0.
        public Single FrequencyHz;

        /// The local anchor point relative to bodyA's origin.
        public Vector2 LocalAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public Vector2 LocalAnchorB;

        /// The bodyB angle minus bodyA angle in the reference state (radians).
        public Single ReferenceAngle;

        public WeldJointDef()
        {
            JointType = JointType.WeldJoint;
            LocalAnchorA.Set(0.0f, 0.0f);
            LocalAnchorB.Set(0.0f, 0.0f);
            ReferenceAngle = 0.0f;
            FrequencyHz = 0.0f;
            DampingRatio = 0.0f;
        }

        /// Initialize the bodies, anchors, and reference angle using a world
        /// anchor point.
        internal void Initialize(Body bA, Body bB, in Vector2 anchor)
        {
            BodyA = bA;
            BodyB = bB;
            LocalAnchorA = BodyA.GetLocalPoint(anchor);
            LocalAnchorB = BodyB.GetLocalPoint(anchor);
            ReferenceAngle = BodyB.GetAngle() - BodyA.GetAngle();
        }
    }
}