using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector3 StartPoint {  get; private set; }
    public Vector3 TargetPoint { get; private set; }
    private float time;

    private void Update()
    {
        if (time < 1f)
        {
            time += Time.deltaTime * 1f;
            time = Mathf.Clamp01(time);
            //BezierCurves();
        }
    }
    public void SetPath()
    {

    }
}
