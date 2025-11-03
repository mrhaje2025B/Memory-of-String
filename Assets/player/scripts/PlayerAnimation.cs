using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private float key_input;
    public bool heading = true;
    public PlayerMovement_Basic playermove;
    private Animator animator;
    private Transform trans;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        key_input = Input.GetAxisRaw("Horizontal");
        //좌우 애니메이션
        if (key_input > 0)
        {
            if (!heading && playermove.OnGround())
            {
                trans.localScale = new Vector3(-trans.localScale.x, trans.localScale.y, trans.localScale.z);
                heading = true;
            }
            animator.SetBool("running", true);
        }
        else if (key_input < 0)
        {
            if (heading && playermove.OnGround())
            {
                trans.localScale = new Vector3(-trans.localScale.x, trans.localScale.y, trans.localScale.z);
                heading = false;
            }
            animator.SetBool("running", true);
        }
        else { animator.SetBool("running", false); }
        //끝

        //점프 애니메이션
        if (!playermove.OnGround()) { animator.SetBool("jumping", true); }
        else { animator.SetBool("jumping", false); }

    }
}
