using System;
using System.Numerics;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics.Joints
{
    /// Rope joint definition. This requires two body anchor points and
    /// a maximum lengths.
    /// Note: by default the connected objects will not collide.
    /// see collideConnected in b2JointDef.
    public class RopeJointDef : JointDef
    {
        /// The local anchor point relative to bodyA's origin.
        public Vector2 LocalAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public Vector2 LocalAnchorB;

        /// The maximum length of the rope.
        /// Warning: this must be larger than b2_linearSlop or
        /// the joint will have no effect.
        public Single MaxLength;

        private RopeJointDef()
        {
            JointType = JointType.RopeJoint;
            LocalAnchorA.Set(-1.0f, 0.0f);
            LocalAnchorB.Set(1.0f, 0.0f);
            MaxLength = 0.0f;
        }
    }
}