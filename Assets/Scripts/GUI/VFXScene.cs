using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class VFXScene : MonoBehaviour
{
    private float t;
    public GameObject prefab;
    public GameObject Obj;

    private void Start()
    {
        //Obj = Instantiate(prefab, Vector3.right * 10f, Quaternion.identity);
        Obj.transform.LookAt(Vector3.right);
        Obj.GetComponent<ProjectileController>().enabled = false;
    }
    private void Update()
    {
        if (t < 1f)
        {
            t += Time.deltaTime * 0.2f;
            t = Mathf.Clamp01(t);
            BezierCurves();
        }
        if (Obj.transform.position == new Vector3(10, 0, 0))
        { Destroy(Obj); }
    }
    public void BezierCurves()
    {
        /*Vector3 p0 = Vector3.left * 10f;
        Vector3 p1 = Vector3.up * 3f + Vector3.forward * 1;
        Vector3 p2 = Vector3.up * 3f + Vector3.forward * 1;
        Vector3 p3 = Vector3.right * 10f;*/
        Vector3 p0 = new Vector3(-10, 0, 0);
        Vector3 p1 = new Vector3(-5, 5, 0);
        Vector3 p2 = new Vector3(5, -5, 0);
        Vector3 p3 = new Vector3(10, 0, 0);

        Vector3 pt = BezierUtility.BezierPoint(p1, p0, p3, p2, t);
        Obj.transform.LookAt(pt); // z軸箭頭 對向自己
        Obj.transform.position = pt;
        prefab.transform.position = pt;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Vector3 p0 = new Vector3(-10, 0, 0);
        Vector3 p1 = new Vector3(-5, 5, 0);
        Vector3 p2 = new Vector3(5, -5, 0);
        Vector3 p3 = new Vector3(10, 0, 0);

        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Handles.Label(p0, "P0");
        Handles.Label(p1, "P1");
        Handles.Label(p2, "P2");
        Handles.Label(p3, "P3");
        Handles.SphereHandleCap(0, p0, Quaternion.identity, 0.1f, EventType.Repaint);
        Handles.SphereHandleCap(0, p1, Quaternion.identity, 0.1f, EventType.Repaint);
        Handles.SphereHandleCap(0, p2, Quaternion.identity, 0.1f, EventType.Repaint);
        Handles.SphereHandleCap(0, p3, Quaternion.identity, 0.1f, EventType.Repaint);

        Gizmos.color = Color.green;
        for (int i = 0; i < 100; i++)
        {
            Vector3 curr = BezierUtility.BezierPoint(p1, p0, p3, p2,  i / 100f);
            Vector3 next = BezierUtility.BezierPoint(p1, p0, p3, p2, (i + 1) / 100f);
            Gizmos.color = t > (i / 100f) ? Color.red : Color.green;
            Gizmos.DrawLine(curr, next);
        }
        Vector3 pt = BezierUtility.BezierPoint(p1, p0, p3, p2, t);
        //Handles.Label(pt, string.Format("Pt(t={0})", t));
        //Handles.SphereHandleCap(0, pt, Quaternion.identity, t, EventType.Repaint);
    }
#endif
}
