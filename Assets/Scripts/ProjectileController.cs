using UnityEngine;
using UnityEngine.U2D;

public class ProjectileController : MonoBehaviour
{
    public GameObject Impact;
    public Vector3 StartPoint {  get; private set; }
    public Vector3 TargetPoint { get; private set; }
    public int Damage { get; private set; }
    private float time;
    private Vector3 p1,p2;

    private void Update()
    {
        if (StartPoint != null && time < 1f)
        {
            time += Time.deltaTime * 1.8f;
            time = Mathf.Clamp01(time);
            BezierCurves();
        }
        if (TargetPoint != null && this.transform.position == TargetPoint)
        {
            Destroy(gameObject,0.1f);
        }
    }
    public void SetPoint(Vector3 _start, Vector3 _target, int _damage)
    {
        StartPoint = _start;
        TargetPoint = _target;
        Damage = _damage;
        TargetPoint = new Vector3(_target.x + Random.Range(-1f,1f), _target.y + Random.Range(0f, 3f), _target.z);
        float z = 80;
        float x = _target.x - _start.x;
        float y = _target.y - _start.y;
        x = Random.Range(-5f, 5f);
        y = Random.Range(-2f, 2f);
        p1 = new Vector3(_start.x + x, y, z);
        p2 = new Vector3(_target.x - (Mathf.Abs(x * 2)), y, z);
        Debug.Log(TargetPoint);
    }
    public void BezierCurves()
    {
        Vector3 pt = BezierUtility.BezierPoint(p1, StartPoint, TargetPoint, p2, time);
        this.transform.LookAt(pt); // z軸箭頭 對向自己
        this.transform.position = pt;
    }
    public void OnDestroy()
    {
        if (Enemy.Instance != null)
            Enemy.Instance.Hurt(Damage);
        if (Impact != null)
        {
            GameObject obj = Instantiate(Impact,this.transform.position,Quaternion.identity);
            Destroy(obj, 0.5f);
        }
    }
}
