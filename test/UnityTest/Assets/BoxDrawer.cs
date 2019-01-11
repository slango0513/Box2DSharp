using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Box2DSharp.Inspection;
using UnityEngine;
using Color = System.Drawing.Color;
using Transform = Box2DSharp.Common.Transform;
#if USE_FIXED_POINT
using Single = FixedMath.Fix64;
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#else
using Vector2 = System.Numerics.Vector2;
#endif
using Vector3 = UnityEngine.Vector3;

namespace Box2DSharp
{
    public class BoxDrawer : IDrawer
    {
        public DebugDrawer Drawer;

        /// <inheritdoc />
        public DrawFlag Flags { get; set; }

        /// <inheritdoc />
        public void DrawPolygon(Vector2[] vertices, int vertexCount, in Color color)
        {
            var list = new List<(Vector3 begin, Vector3 end)>();

            for (var i = 0; i < vertexCount; i++)
            {
                if (i < vertexCount - 1)
                {
                    list.Add((vertices[i].ToUnityVector3(), vertices[i + 1].ToUnityVector3()));
                }
                else
                {
                    list.Add((vertices[i].ToUnityVector3(), vertices[0].ToUnityVector3()));
                }
            }

            Drawer.PostLines(list, color.ToUnityColor());
        }

        /// <inheritdoc />
        public void DrawSolidPolygon(Vector2[] vertices, int vertexCount, in Color color)
        {
            var list = new List<(Vector3 begin, Vector3 end)>();

            for (var i = 0; i < vertexCount; i++)
            {
                if (i < vertexCount - 1)
                {
                    list.Add((vertices[i].ToUnityVector3(), vertices[i + 1].ToUnityVector3()));
                }
                else
                {
                    list.Add((vertices[i].ToUnityVector3(), vertices[0].ToUnityVector3()));
                }
            }

            Drawer.PostLines(list, color.ToUnityColor());
        }

        /// <inheritdoc />
        public void DrawCircle(in Vector2 center, Single radius, in Color color)
        {
            var lines = new List<(Vector3, Vector3)>();
            const int lineCount = 200;
            for (var i = 0; i <= lineCount; ++i) //割圆术画圆
            {
                lines.Add(
                    (
                        new UnityEngine.Vector2(
                            (float)center.X + (float)radius * (float) Math.Cos(2 * Mathf.PI / lineCount * i),
                            (float)center.Y + (float)radius * (float) Math.Sin(2 * Mathf.PI / lineCount * i)),
                        new UnityEngine.Vector2(
                            (float)center.X + (float)radius * (float) Math.Cos(2 * Mathf.PI / lineCount * (i + 1)),
                            (float)center.Y + (float)radius * (float) Math.Sin(2 * Mathf.PI / lineCount * (i + 1)))
                    ));
            }

            Drawer.PostLines(lines, color.ToUnityColor());
        }

        /// <inheritdoc />
        public void DrawSolidCircle(in Vector2 center, Single radius, in Vector2 axis, in Color color)
        {
            var lines = new List<(Vector3, Vector3)>();
            const int lineCount = 200;

            //GL.Begin(GL.LINE_STRIP);
            for (var i = 0; i <= lineCount; ++i) //割圆术画圆
            {
                lines.Add(
                    (
                        new UnityEngine.Vector2(
                            (float)center.X + (float)radius * (float) Math.Cos(2 * Mathf.PI / lineCount * i),
                            (float)center.Y + (float)radius * (float) Math.Sin(2 * Mathf.PI / lineCount * i)),
                        new UnityEngine.Vector2(
                            (float)center.X + (float)radius * (float) Math.Cos(2 * Mathf.PI / lineCount * (i + 1)),
                            (float)center.Y + (float)radius * (float) Math.Sin(2 * Mathf.PI / lineCount * (i + 1)))
                    ));
            }

            Drawer.PostLines(lines, color.ToUnityColor());
        }

        /// <inheritdoc />
        public void DrawSegment(in Vector2 p1, in Vector2 p2, in Color color)
        {
            Drawer.PostLines(
                new List<(Vector3, Vector3)> {(p1.ToUnityVector2(), p2.ToUnityVector2())},
                color.ToUnityColor());
        }

        /// <inheritdoc />
        public void DrawTransform(in Transform xf)
        {
            const float axisScale = 0.4f;

            var p1 = xf.Position;
            var p2 = p1 + axisScale * xf.Rotation.GetXAxis();

            Drawer.PostLines(
                new List<(Vector3, Vector3)> {(p1.ToUnityVector2(), p2.ToUnityVector2())},
                UnityEngine.Color.red);

            p2 = p1 + axisScale * xf.Rotation.GetYAxis();
            Drawer.PostLines(
                new List<(Vector3 begin, Vector3 end)> {(p1.ToUnityVector2(), p2.ToUnityVector2())},
                UnityEngine.Color.green);
        }

        /// <inheritdoc />
        public void DrawPoint(in Vector2 p, Single size, in Color color)
        {
            Drawer.PostPoint((p.ToUnityVector3(), (float)size / 100, color.ToUnityColor()));
        }
    }
}