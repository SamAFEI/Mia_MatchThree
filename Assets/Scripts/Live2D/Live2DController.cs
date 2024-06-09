using Live2D.Cubism.Core;
using UnityEngine;

public class Live2DController : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public CubismModel Model { get; private set; }
    public bool IsBreak1 { get; private set; }
    public bool IsBreak2 { get; private set; }
    public bool IsDisplay { get; private set; }
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
        if (Model.Parameters.FindById("Display")) //Stella
        {
            if (IsDisplay)
            {
                Model.Parameters.FindById("Display").Value = 10;
            }
            else
                Model.Parameters.FindById("Display").Value = 0;
        }
        if (IsBreak1)
        {
            Model.Parameters.FindById("Break1").Value = -30;
            if (Model.Parameters.FindById("Hair1")) //Elise
            {
                Model.Parameters.FindById("Hair1").Value = 30;
            }
            if (Model.Parameters.FindById("RED")) //Stella
            {
                Model.Parameters.FindById("Break1").Value = 10;
                Model.Parameters.FindById("RED").Value = 10;
            }
        }
        else
        {
            if (Model.Parameters.FindById("Hair1")) //Elise
            {
                Model.Parameters.FindById("Hair1").Value = -30;
            }
            
        }
        if (IsBreak2)
        {
            Model.Parameters.FindById("Break2").Value = -30;
            if (Model.Parameters.FindById("Eyes")) //Stella
            {
                Model.Parameters.FindById("Break2").Value = 10;
                Model.Parameters.FindById("Eyes").Value = 0;
            }
        }
    }
    public void PlayAnim(string _anim)
    {
        if (_anim == "Break1")
        {
            IsDisplay = true;
        }
        Anim.Play(_anim);
    }
    public virtual void PlayIdle()
    {
        Anim.Play("Idle");
        if (Model.Parameters.FindById("Hair1"))
        {
            Model.Parameters.FindById("Param17").Value = 0;
        }
    }
    public void SetBreak1()
    {
        IsBreak1 = true;
        IsBreak2 = false;
    }
    public void SetBreak2()
    {
        IsBreak1 = true;
        IsBreak2 = true;
    }
    public void SetDamage(int _damage) { Damage = _damage; }
    public void DoAttack()
    {
        if (Player.Instance)
        {
            Player.Instance.SlashHurt(Damage);
        }
    }
    public void PlayAttackVoice()
    {
        AudioManager.PlayVoice(VoiceEnum.Attack);
    }
}
