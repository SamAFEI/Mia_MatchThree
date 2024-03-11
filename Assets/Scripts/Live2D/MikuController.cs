using UnityEngine;

public class MikuController : MonoBehaviour
{
    [SerializeField] public Animator animator {  get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            animator.Play("miku_idle");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.Play("miku_01");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.Play("miku_02");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.Play("miku_03");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animator.Play("miku_04");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            animator.Play("miku_05");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            animator.Play("miku_06");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            animator.Play("miku_07");
        }
    }
}
