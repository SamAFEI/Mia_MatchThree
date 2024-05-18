using UnityEngine;

public class HaruController : MonoBehaviour
{
    [SerializeField] public Animator animator { get; private set; }
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
            animator.Play("haru_g_idle");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.Play("haru_g_m01");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.Play("haru_g_m02");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.Play("haru_g_m03");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animator.Play("haru_g_m04");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            animator.Play("haru_g_m05");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            animator.Play("haru_g_m06");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            animator.Play("haru_g_m07");
        }
    }
}
