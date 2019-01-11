using System;
using System.Numerics;
using Box2DSharp.Common;
#if USE_FIXED_POINT
using Math = FixedMath.MathFix;
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Dynamics.Joints
{
    /// A wheel joint. This joint provides two degrees of freedom: translation
    /// along an axis fixed in bodyA and rotation in the plane. In other words, it is a point to
    /// line constraint with a rotational motor and a linear spring/damper.
    /// This joint is designed for vehicle suspensions.
    internal class WheelJoint : Joint
    {
        // Solver shared
        private readonly Vector2 _localAnchorA;

        private readonly Vector2 _localAnchorB;

        private readonly Vector2 _localXAxisA;

        private readonly Vector2 _localYAxisA;

        private Vector2 _ax, _ay;

        private Single _bias;

        private Single _dampingRatio;

        private bool _enableMotor;

        private Single _frequencyHz;

        private Single _gamma;

        private Single _impulse;

        // Solver temp
        private int _indexA;

        private int _indexB;

        private Single _invIa;

        private Single _invIb;

        private Single _invMassA;

        private Single _invMassB;

        private Vector2 _localCenterA;

        private Vector2 _localCenterB;

        private Single _mass;

        private Single _maxMotorTorque;

        private Single _motorImpulse;

        private Single _motorMass;

        private Single _motorSpeed;

        private Single _sAx, _sBx;

        private Single _sAy, _sBy;

        private Single _springImpulse;

        private Single _springMass;

        internal WheelJoint(WheelJointDef def) : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;
            _localXAxisA = def.LocalAxisA;
            _localYAxisA = MathUtils.Cross(1.0f, _localXAxisA);

            _mass = 0.0f;
            _impulse = 0.0f;
            _motorMass = 0.0f;
            _motorImpulse = 0.0f;
            _springMass = 0.0f;
            _springImpulse = 0.0f;

            _maxMotorTorque = def.MaxMotorTorque;
            _motorSpeed = def.MotorSpeed;
            _enableMotor = def.EnableMotor;

            _frequencyHz = def.FrequencyHz;
            _dampingRatio = def.DampingRatio;

            _bias = 0.0f;
            _gamma = 0.0f;

            _ax.SetZero();
            _ay.SetZero();
        }

        /// The local anchor point relative to bodyA's origin.
        private Vector2 GetLocalAnchorA()
        {
            return _localAnchorA;
        }

        /// The local anchor point relative to bodyB's origin.
        private Vector2 GetLocalAnchorB()
        {
            return _localAnchorB;
        }

        /// The local joint axis relative to bodyA.
        private Vector2 GetLocalAxisA()
        {
            return _localXAxisA;
        }

        /// Get the current joint translation, usually in meters.
        private Single GetJointTranslation()
        {
            var bA = BodyA;
            var bB = BodyB;

            var pA = bA.GetWorldPoint(_localAnchorA);
            var pB = bB.GetWorldPoint(_localAnchorB);
            var d = pB - pA;
            var axis = bA.GetWorldVector(_localXAxisA);

            var translation = MathUtils.Dot(d, axis);
            return translation;
        }

        /// Get the current joint linear speed, usually in meters per second.
        private Single GetJointLinearSpeed()
        {
            var bA = BodyA;
            var bB = BodyB;

            var rA = MathUtils.Mul(bA.Transform.Rotation, _localAnchorA - bA.Sweep.LocalCenter);
            var rB = MathUtils.Mul(bB.Transform.Rotation, _localAnchorB - bB.Sweep.LocalCenter);
            var p1 = bA.Sweep.C + rA;
            var p2 = bB.Sweep.C + rB;
            var d = p2 - p1;
            var axis = MathUtils.Mul(bA.Transform.Rotation, _localXAxisA);

            var vA = bA.LinearVelocity;
            var vB = bB.LinearVelocity;
            var wA = bA.AngularVelocity;
            var wB = bB.AngularVelocity;

            var speed = MathUtils.Dot(d, MathUtils.Cross(wA, axis))
                      + MathUtils.Dot(axis, vB + MathUtils.Cross(wB, rB) - vA - MathUtils.Cross(wA, rA));
            return speed;
        }

        /// Get the current joint angle in radians.
        private Single GetJointAngle()
        {
            var bA = BodyA;
            var bB = BodyB;
            return bB.Sweep.A - bA.Sweep.A;
        }

        /// Get the current joint angular speed in radians per second.
        private Single GetJointAngularSpeed()
        {
            var wA = BodyA.AngularVelocity;
            var wB = BodyB.AngularVelocity;
            return wB - wA;
        }

        /// Is the joint motor enabled?
        private bool IsMotorEnabled()
        {
            return _enableMotor;
        }

        /// Enable/disable the joint motor.
        private void EnableMotor(bool flag)
        {
            if (flag != _enableMotor)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _enableMotor = flag;
            }
        }

        /// Set the motor speed, usually in radians per second.
        private void SetMotorSpeed(Single speed)
        {
            if (!speed.Equals(_motorSpeed))
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _motorSpeed = speed;
            }
        }

        /// Get the motor speed, usually in radians per second.
        private Single GetMotorSpeed()
        {
            return _motorSpeed;
        }

        /// Set/Get the maximum motor force, usually in N-m.
        private void SetMaxMotorTorque(Single torque)
        {
            if (torque != _maxMotorTorque)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _maxMotorTorque = torque;
            }
        }

        private Single GetMaxMotorTorque()
        {
            return _maxMotorTorque;
        }

        /// Get the current motor torque given the inverse time step, usually in N-m.
        private Single GetMotorTorque(Single inv_dt)
        {
            return inv_dt * _motorImpulse;
        }

        /// Set/Get the spring frequency in hertz. Setting the frequency to zero disables the spring.
        private void SetSpringFrequencyHz(Single hz)
        {
            _frequencyHz = hz;
        }

        private Single GetSpringFrequencyHz()
        {
            return _frequencyHz;
        }

        /// Set/Get the spring damping ratio
        private void SetSpringDampingRatio(Single ratio)
        {
            _dampingRatio = ratio;
        }

        private Single GetSpringDampingRatio()
        {
            return _dampingRatio;
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorA()
        {
            return BodyA.GetWorldPoint(_localAnchorA);
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorB()
        {
            return BodyB.GetWorldPoint(_localAnchorB);
        }

        /// <inheritdoc />
        public override Vector2 GetReactionForce(Single inv_dt)
        {
            return inv_dt * (_impulse * _ay + _springImpulse * _ax);
        }

        /// <inheritdoc />
        public override Single GetReactionTorque(Single inv_dt)
        {
            return inv_dt * _motorImpulse;
        }

        /// Dump to Logger.Log
        public override void Dump()
        {
            var indexA = BodyA.IslandIndex;
            var indexB = BodyB.IslandIndex;

            Logger.Log("  b2WheelJointDef jd;");
            Logger.Log($"  jd.bodyA = bodies[{indexA}];");
            Logger.Log($"  jd.bodyB = bodies[{indexB}];");
            Logger.Log($"  jd.collideConnected = bool({CollideConnected});");
            Logger.Log($"  jd.localAnchorA.Set({_localAnchorA.X}, {_localAnchorA.Y});");
            Logger.Log($"  jd.localAnchorB.Set({_localAnchorB.X}, {_localAnchorB.Y});");
            Logger.Log($"  jd.localAxisA.Set({_localXAxisA.X}, {_localXAxisA.Y});");
            Logger.Log($"  jd.enableMotor = bool({_enableMotor});");
            Logger.Log($"  jd.motorSpeed = {_motorSpeed};");
            Logger.Log($"  jd.maxMotorTorque = {_maxMotorTorque};");
            Logger.Log($"  jd.frequencyHz = {_frequencyHz};");
            Logger.Log($"  jd.dampingRatio = {_dampingRatio};");
            Logger.Log($"  joints[{Index}] = m_world.CreateJoint(&jd);");
        }

        /// <inheritdoc />
        internal override void InitVelocityConstraints(in SolverData data)
        {
            _indexA = BodyA.IslandIndex;
            _indexB = BodyB.IslandIndex;
            _localCenterA = BodyA.Sweep.LocalCenter;
            _localCenterB = BodyB.Sweep.LocalCenter;
            _invMassA = BodyA.InvMass;
            _invMassB = BodyB.InvMass;
            _invIa = BodyA.InverseInertia;
            _invIb = BodyB.InverseInertia;

            Single mA = _invMassA, mB = _invMassB;
            Single iA = _invIa, iB = _invIb;

            var cA = data.Positions[_indexA].Center;
            var aA = data.Positions[_indexA].Angle;
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;

            var cB = data.Positions[_indexB].Center;
            var aB = data.Positions[_indexB].Angle;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            var qA = new Rotation(aA);
            var qB = new Rotation(aB);

            // Compute the effective masses.
            var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
            var d = cB + rB - cA - rA;

            // Point to line constraint
            {
                _ay = MathUtils.Mul(qA, _localYAxisA);
                _sAy = MathUtils.Cross(d + rA, _ay);
                _sBy = MathUtils.Cross(rB, _ay);

                _mass = mA + mB + iA * _sAy * _sAy + iB * _sBy * _sBy;

                if (_mass > 0.0f)
                {
                    _mass = 1.0f / _mass;
                }
            }

            // Spring constraint
            _springMass = 0.0f;
            _bias = 0.0f;
            _gamma = 0.0f;
            if (_frequencyHz > 0.0f)
            {
                _ax = MathUtils.Mul(qA, _localXAxisA);
                _sAx = MathUtils.Cross(d + rA, _ax);
                _sBx = MathUtils.Cross(rB, _ax);

                var invMass = mA + mB + iA * _sAx * _sAx + iB * _sBx * _sBx;

                if (invMass > 0.0f)
                {
                    _springMass = 1.0f / invMass;

                    var C = MathUtils.Dot(d, _ax);

                    // Frequency
                    var omega = 2.0f * Settings.Pi * _frequencyHz;

                    // Damping coefficient
                    var damp = 2.0f * _springMass * _dampingRatio * omega;

                    // Spring stiffness
                    var k = _springMass * omega * omega;

                    // magic formulas
                    var h = data.Step.Dt;
                    _gamma = h * (damp + h * k);
                    if (_gamma > 0.0f)
                    {
                        _gamma = 1.0f / _gamma;
                    }

                    _bias = C * h * k * _gamma;

                    _springMass = invMass + _gamma;
                    if (_springMass > 0.0f)
                    {
                        _springMass = 1.0f / _springMass;
                    }
                }
            }
            else
            {
                _springImpulse = 0.0f;
            }

            // Rotational motor
            if (_enableMotor)
            {
                _motorMass = iA + iB;
                if (_motorMass > 0.0f)
                {
                    _motorMass = 1.0f / _motorMass;
                }
            }
            else
            {
                _motorMass = 0.0f;
                _motorImpulse = 0.0f;
            }

            if (data.Step.WarmStarting)
            {
                // Account for variable time step.
                _impulse *= data.Step.DtRatio;
                _springImpulse *= data.Step.DtRatio;
                _motorImpulse *= data.Step.DtRatio;

                var P = _impulse * _ay + _springImpulse * _ax;
                var LA = _impulse * _sAy + _springImpulse * _sAx + _motorImpulse;
                var LB = _impulse * _sBy + _springImpulse * _sBx + _motorImpulse;

                vA -= _invMassA * P;
                wA -= _invIa * LA;

                vB += _invMassB * P;
                wB += _invIb * LB;
            }
            else
            {
                _impulse = 0.0f;
                _springImpulse = 0.0f;
                _motorImpulse = 0.0f;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        /// <inheritdoc />
        internal override void SolveVelocityConstraints(in SolverData data)
        {
            Single mA = _invMassA, mB = _invMassB;
            Single iA = _invIa, iB = _invIb;

            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            // Solve spring constraint
            {
                var Cdot = MathUtils.Dot(_ax, vB - vA) + _sBx * wB - _sAx * wA;
                var impulse = -_springMass * (Cdot + _bias + _gamma * _springImpulse);
                _springImpulse += impulse;

                var P = impulse * _ax;
                var LA = impulse * _sAx;
                var LB = impulse * _sBx;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }

            // Solve rotational motor constraint
            {
                var Cdot = wB - wA - _motorSpeed;
                var impulse = -_motorMass * Cdot;

                var oldImpulse = _motorImpulse;
                var maxImpulse = data.Step.Dt * _maxMotorTorque;
                _motorImpulse = MathUtils.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _motorImpulse - oldImpulse;

                wA -= iA * impulse;
                wB += iB * impulse;
            }

            // Solve point to line constraint
            {
                var Cdot = MathUtils.Dot(_ay, vB - vA) + _sBy * wB - _sAy * wA;
                var impulse = -_mass * Cdot;
                _impulse += impulse;

                var P = impulse * _ay;
                var LA = impulse * _sAy;
                var LB = impulse * _sBy;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        /// <inheritdoc />
        internal override bool SolvePositionConstraints(in SolverData data)
        {
            var cA = data.Positions[_indexA].Center;
            var aA = data.Positions[_indexA].Angle;
            var cB = data.Positions[_indexB].Center;
            var aB = data.Positions[_indexB].Angle;

            var qA = new Rotation(aA);
            var qB = new Rotation(aB);

            var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
            var d = cB - cA + rB - rA;

            var ay = MathUtils.Mul(qA, _localYAxisA);

            var sAy = MathUtils.Cross(d + rA, ay);
            var sBy = MathUtils.Cross(rB, ay);

            var C = MathUtils.Dot(d, ay);

            var k = _invMassA + _invMassB + _invIa * _sAy * _sAy + _invIb * _sBy * _sBy;

            Single impulse;
            if (!k.Equals(0.0f))
            {
                impulse = -C / k;
            }
            else
            {
                impulse = 0.0f;
            }

            var P = impulse * ay;
            var LA = impulse * sAy;
            var LB = impulse * sBy;

            cA -= _invMassA * P;
            aA -= _invIa * LA;
            cB += _invMassB * P;
            aB += _invIb * LB;

            data.Positions[_indexA].Center = cA;
            data.Positions[_indexA].Angle = aA;
            data.Positions[_indexB].Center = cB;
            data.Positions[_indexB].Angle = aB;

            return Math.Abs(C) <= Settings.LinearSlop;
        }
    }
}