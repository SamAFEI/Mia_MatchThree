using Live2D.Cubism.Core;
using UnityEngine;

public class Live2DController : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public CubismModel Model { get; private set; }
    public bool IsBreak1 { get; private set; }
    public bool IsBreak2 { get; private set; }
    private void Start()
    {
        Anim = GetComponent<Animator>();
        Model = this.FindCubismModel();
    }
    private void LateUpdate()
    {
        if (IsBreak1)
        {
            Model.Parameters.FindById("Break1").Value = -10;
        }
        if (IsBreak2)
        {
            Model.Parameters.FindById("Break2").Value = -10;
        }
    }
    public void PlayAnim(string _anim)
    {
        Anim.Play(_anim);
        Debug.Log(_anim);
    }
    public virtual void PlayIdle()
    {
        Anim.Play("Idle");
    }
    public void SetBreak1() { IsBreak1 = true; }
    public void SetBreak2() { IsBreak2 = true; }
}
