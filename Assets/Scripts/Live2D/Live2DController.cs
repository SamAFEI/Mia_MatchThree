using Live2D.Cubism.Core;
using UnityEngine;

public class Live2DController : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public CubismModel Model { get; private set; }
    public bool IsBreak1 { get; private set; }
    public bool IsBreak2 { get; private set; }
    public int Damage { get; private set; }
    private void Start()
    {
        Anim = GetComponent<Animator>();
        Model = this.FindCubismModel();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            PlayAnim("Hurt");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayAnim("Attack");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayAnim("Break1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayAnim("Break2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayAnim("Idle");
        }
    }
    private void LateUpdate()
    {
        if (IsBreak1)
        {
            Model.Parameters.FindById("Break1").Value = -30;
            if (!Model.Parameters.FindById("Break1"))
            {
                Model.Parameters.FindById("Hair1").Value = 30;
            }
        }
        if (IsBreak2)
        {
            Model.Parameters.FindById("Break2").Value = -30;
        }
    }
    public void PlayAnim(string _anim)
    {
        Anim.Play(_anim);
    }
    public virtual void PlayIdle()
    {
        Anim.Play("Idle");
    }
    public void SetBreak1() { IsBreak1 = true; }
    public void SetBreak2() { IsBreak2 = true; }
    public void SetDamage(int _damage) { Damage = _damage; }
    public void DoAttack() 
    { 
        Player.Instance.SlashHurt(Damage); 
        Debug.Log("DoAttack"); 
    }
}
