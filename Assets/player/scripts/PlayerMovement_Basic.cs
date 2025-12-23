using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement_Basic : MonoBehaviour
{
    public PlayerConfig config;
    private Rigidbody2D body;
    private float key_input;
    public BoxCollider2D foot;
    public PlayerMovementSkill skill;

    public bool channeling = false;
    public bool canmove = true;


    public bool active_doublejump = false;
    private bool candoublejump = true;
    public LayerMask groundlayer;
    private float last_onground_time;

    private List<RaycastHit2D> trash = new List<RaycastHit2D>();
    private ContactFilter2D filter;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        

    }

    private void Start()
    {

        body.gravityScale = config.gravity;
        filter = new ContactFilter2D();

        
        filter.SetLayerMask(groundlayer); 
        filter.useLayerMask = true;
    }

    public bool OnGround()
    {
        int hitCount = foot.Cast(Vector2.down, filter, trash, 0.02f);

        if (foot.Cast(Vector2.down, filter, trash, 0.02f) > 0)
        {
            foreach (RaycastHit2D hit in trash)
            {
                if (hit.normal.y > 0.8f) { return true; }
            }
        }
        return false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //좌우 이동
        key_input = Input.GetAxisRaw("Horizontal");
        float targetspeed = key_input * config.movespeed;

        if (targetspeed - body.linearVelocityX > 0 && canmove)
        {
            if (body.linearVelocityX >= 0) { body.AddForce(Vector2.right * (targetspeed - body.linearVelocityX) * config.accel_rate, ForceMode2D.Force); }
            else if ( ( !skill.flying_b) || (Input.GetAxisRaw("Horizontal") > 0)) { body.AddForce(Vector2.right * (targetspeed - body.linearVelocityX) * config.stop_rate, ForceMode2D.Force); }
        }
        else if (targetspeed - body.linearVelocityX < 0 && canmove )
        {
            if (body.linearVelocityX <= 0) { body.AddForce(Vector2.right * (targetspeed - body.linearVelocityX) * config.accel_rate, ForceMode2D.Force); }
            else if ((!skill.flying_f) || (Input.GetAxisRaw("Horizontal") < 0)) { body.AddForce(Vector2.right * (targetspeed - body.linearVelocityX) * config.stop_rate, ForceMode2D.Force); }
        }
        //여기까지

    }

    private void Update()
    {
        if (OnGround()) { last_onground_time = Time.time ;}
        
        
        if (OnGround()) 
        { 
            candoublejump = true;

        }
        
        //점프
        if (Input.GetKeyDown(KeyCode.Space) && !channeling)
        {
            
            //print(OnGround());
            if (Time.time - last_onground_time < 0.1f && canmove)
            {
                
                body.linearVelocityY = config.jump_pow;
                
                
                
            }
            else if (active_doublejump && candoublejump && canmove)
            {
                body.linearVelocityY = config.jump_pow;
                
                candoublejump = false;
            }
        }

        /*
        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocityY > 0)
        {

            body.linearVelocityY *= fall_rate;
        }
        //여기까지 */
        
        
    }
}
