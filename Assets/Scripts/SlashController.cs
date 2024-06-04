using UnityEngine;
using UnityEngine.U2D;

public class SlashController : MonoBehaviour
{
    public Vector3 StartPoint { get; private set; }
    public Vector3 TargetPoint { get; private set; }
    public int Damage { get; private set; }
    private float time;
    private Vector3 p1, p2;

    private void Update()
    {
        if (StartPoint != null && time < 1f)
        {
            time += Time.deltaTime * 5f;
            time = Mathf.Clamp01(time);
            BezierCurves();
        }
        if (TargetPoint != null && this.transform.position == TargetPoint)
        {
            Destroy(gameObject, 0.2f);
        }
    }
    public void SetPoint(Vector3 _start, Vector3 _target, int _damage)
    {
        StartPoint = _start;
        TargetPoint = _target;
        Damage = _damage;
        float z = _target.z;
        float x = (_target.x - _start.x) / 2;
        float y = (_target.y - _start.y) / 2;
        x = _start.x + x + Random.Range(-0.5f, 0.5f);
        y = _start.y + y + Random.Range(-0.5f, 0.5f);
        p1 = new Vector3(x, y, z);
        p2 = p1;
        AudioManager.PlaySE(SEEnum.Slash);
    }
    public void BezierCurves()
    {
        Vector3 pt = BezierUtility.BezierPoint(p1, StartPoint, TargetPoint, p2, time);
        //this.transform.LookAt(pt); //3D物件 用z軸箭頭轉向 
        this.transform.position = pt;
    }
    public void OnDestroy()
    {
        if (Player.Instance != null)
            Player.Instance.Hurt(Damage);

        Vector3 vector = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
        TooltipManager.SpawnDamageHint(vector, Damage, gameObject.GetComponentInChildren<TrailRenderer>().startColor);
    }
}
