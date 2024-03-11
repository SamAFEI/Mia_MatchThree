using UnityEngine;

public class HiyoriController : MonoBehaviour
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
            animator.Play("hiyori_m01");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.Play("hiyori_m01");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.Play("hiyori_m02");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.Play("hiyori_m03");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animator.Play("hiyori_m04");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            animator.Play("hiyori_m05");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            animator.Play("hiyori_m06");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            animator.Play("hiyori_m07");
        }
    }
}
