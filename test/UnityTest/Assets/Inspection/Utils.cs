using System;
using System.Numerics;
#if USE_FIXED_POINT
using Vector2 = FixedMath.Numerics.Fix64Vector2;
#endif

namespace Box2DSharp.Inspection
{
    public static class Utils
    {
        public static UnityEngine.Vector3 ToUnityVector3(this Vector3 vector3)
        {
            return new UnityEngine.Vector3(vector3.X, vector3.Y, vector3.Z);
        }

        public static UnityEngine.Vector2 ToUnityVector2(this Vector3 vector3)
        {
            return new UnityEngine.Vector2(vector3.X, vector3.Y);
        }

        public static UnityEngine.Vector2 ToUnityVector2(this Vector2 vector2)
        {
            return new UnityEngine.Vector2((float)vector2.X, (float)vector2.Y);
        }

        public static UnityEngine.Vector3 ToUnityVector3(this Vector2 vector2)
        {
            return new UnityEngine.Vector3((float)vector2.X, (float)vector2.Y, 0);
        }

        public static UnityEngine.Color ToUnityColor(this System.Drawing.Color color)
        {
            return new UnityEngine.Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        public static Vector2 ToVector2(this UnityEngine.Vector2 vector2)
        {
            return new Vector2(vector2.x, vector2.y);
        }

        public static Vector3 ToVector3(this UnityEngine.Vector3 vector3)
        {
            return new Vector3(vector3.x, vector3.y, vector3.z);
        }

        public static Vector2 ToVector2(this UnityEngine.Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }

        public static Vector3 ToVector3(this UnityEngine.Vector2 vector3)
        {
            return new Vector3(vector3.x, vector3.y, 0);
        }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class ShowOnlyAttribute : UnityEngine.PropertyAttribute
    { }

    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class ShowVectorAttribute : UnityEngine.PropertyAttribute
    { }
#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
    public class ShowOnlyAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(UnityEngine.Rect rect, UnityEditor.SerializedProperty prop, UnityEngine.GUIContent label)
        {
            bool wasEnabled = UnityEngine.GUI.enabled;
            UnityEngine.GUI.enabled = false;
            UnityEditor.EditorGUI.PropertyField(rect, prop);
            UnityEngine.GUI.enabled = wasEnabled;
        }
    }

    [UnityEditor.CustomPropertyDrawer(typeof(ShowVectorAttribute))]
    public class ShowVectorAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(UnityEngine.Rect rect, UnityEditor.SerializedProperty prop, UnityEngine.GUIContent label)
        {
            UnityEditor.EditorGUI.PropertyField(rect, prop);
        }
    }
#endif
}