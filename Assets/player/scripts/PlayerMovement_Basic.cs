using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement_Basic : MonoBehaviour
{
    private Rigidbody2D body;
    private float key_input;
    public float speed = 16f;
    public float acceleration_rate = 6f;
    public float stop_rate = 8f;
    public Transform foot;
    public float jump_power = 6.0f;
    public float fall_rate = 0.3f;
    public bool active_doublejump = false;
    private bool candoublejump = true;
    public LayerMask groundlayer;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

    }

    public bool OnGround()
    {
        return (Physics2D.OverlapBox(foot.position, new Vector2(1.6f, 0.2f), 0f, groundlayer) != null) ? true : false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //좌우 이동
        key_input = Input.GetAxisRaw("Horizontal");
        float targetspeed = key_input * speed;

        if (targetspeed - body.linearVelocityX > 0)
        {
            if (body.linearVelocityX >= 0) { body.AddForce(Vector2.right * (targetspeed - body.linearVelocityX) * acceleration_rate, ForceMode2D.Force); }
            else { body.AddForce(Vector2.right * (targetspeed - body.linearVelocityX) * stop_rate, ForceMode2D.Force); }
        }
        else if (targetspeed - body.linearVelocityX < 0)
        {
            if (body.linearVelocityX <= 0) { body.AddForce(Vector2.right * (targetspeed - body.linearVelocityX) * acceleration_rate, ForceMode2D.Force); }
            else { body.AddForce(Vector2.right * (targetspeed - body.linearVelocityX) * stop_rate, ForceMode2D.Force); }
        }
        //여기까지

    }

    private void Update()
    {
        //점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnGround())
            {
                body.linearVelocityY = jump_power;
                candoublejump = true;
            }
            else if (active_doublejump && candoublejump)
            {
                body.linearVelocityY = jump_power;
                candoublejump = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocityY > 0)
        {

            body.linearVelocityY *= fall_rate;
        }
        //여기까지
    }
}
