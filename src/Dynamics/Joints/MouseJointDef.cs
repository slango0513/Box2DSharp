using System;
using System.Numerics;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics.Joints
{
    /// Mouse joint definition. This requires a world target point,
    /// tuning parameters, and the time step.
    public class MouseJointDef : JointDef
    {
        /// The damping ratio. 0 = no damping, 1 = critical damping.
        public Single DampingRatio;

        /// The response speed.
        public Single FrequencyHz;

        /// The maximum constraint force that can be exerted
        /// to move the candidate body. Usually you will express
        /// as some multiple of the weight (multiplier * mass * gravity).
        public Single MaxForce;

        /// The initial world target point. This is assumed
        /// to coincide with the body anchor initially.
        public Vector2 Target;

        public MouseJointDef()
        {
            JointType = JointType.MouseJoint;
            Target.Set(0.0f, 0.0f);
            MaxForce = 0.0f;
            FrequencyHz = 5.0f;
            DampingRatio = 0.7f;
        }
    }
}