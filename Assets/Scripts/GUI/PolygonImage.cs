using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Experimental.AI;


#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Assets.Scripts.GUI
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonImage : Image
    {
        private PolygonCollider2D _polygonCollider; 
        private PolygonCollider2D polygonCollider
        {
            get
            {
                if (_polygonCollider == null)
                    _polygonCollider = GetComponent<PolygonCollider2D>();
                return _polygonCollider;
            }
        }
        protected PolygonImage()
        {
            useLegacyMeshGeneration = true;
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return polygonCollider.OverlapPoint(eventCamera.ScreenToWorldPoint(screenPoint));
        }
        #if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            transform.localPosition = Vector3.zero;
            float w = (rectTransform.sizeDelta.x * 0.5f) + 0.1f;
            float h = (rectTransform.sizeDelta.y * 0.5f) + 0.1f;
            polygonCollider.points = new Vector2[]
            {
                new Vector2(-w,-h),
                new Vector2(w,-h),
                new Vector2(w,h),
                new Vector2(-w,h)
            };
        }
        #endif
    }
    #if UNITY_EDITOR
    [CustomEditor(typeof(PolygonImage), true)]
    public class PolygonImageInspector : Editor
    {
        public override void OnInspectorGUI()
        {
        }
    }
    #endif
}