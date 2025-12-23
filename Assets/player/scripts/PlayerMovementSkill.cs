using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;

public class PlayerMovementSkill : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerMovement_Basic Basic;
    public PlayerAnimation anim;
    private Rigidbody2D body;
    private Transform trans;
    private SpriteRenderer sprite;
    public LayerMask ground;
    private Collider2D coli;

    public PlayerConfig config;

    private Coroutine spin;
    private Coroutine blink;
    public bool spinning = false;

    public GameObject vanish_effect;
    public GameObject blink_effect;
    public GameObject arrow;
    public GameObject zansang_pre;
    public GameObject bomb_pre;
    private GameObject bomb;
    public Coroutine rope;
    private GameObject ankor;
    private LineRenderer rope_renderer;

    private bool ankorcontact;
    public bool flying_f = false;
    public bool flying_b = false;
    private Coroutine boom;

    private bool xpressed = false;
    private bool cpressed = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        coli = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rope_renderer = GetComponent<LineRenderer>();
        rope_renderer.positionCount = 2;

    }
    IEnumerator Spin()
    {   
        spinning = true;
        StartCoroutine(anim.Spin());
        if (!Basic.OnGround()) { body.linearVelocityY = config.spin_pow; }
        yield return new WaitForSeconds(0.3f);
        spinning = false;
        yield return new WaitForSeconds(config.spin_cooldown);
        spin = null;
    }

    IEnumerator Dash()
    {

        if (anim.heading) 
        {
            /*
            RaycastHit2D laser = Physics2D.BoxCast(trans.position,new Vector2(1.5f,2.8f),0 , new Vector2(1,0), config.dash_length+0.8f, ground);
            trans.position += new Vector3((laser.collider != null) ? laser.distance : config.dash_length, 0, 0);
            body.linearVelocityY /= 4; */

            body.linearVelocityX = config.dash_speed;
            body.linearVelocityY = 0;
            body.gravityScale = 0;  
            Basic.canmove = false;

            GameObject zansang1 = Instantiate(zansang_pre, trans.position, trans.rotation);
            zansang1.GetComponent<ZansangScript>().SetSprite(sprite.sprite, trans.localScale);
            for (int i = 0; i<3; i++)
            {
                for (int j = 0; j < (int)(1 / Time.timeScale); j++) { yield return new WaitForFixedUpdate(); }
                GameObject zansang = Instantiate(zansang_pre, trans.position, trans.rotation);
                zansang.GetComponent<ZansangScript>().SetSprite(sprite.sprite, trans.localScale);
            }

            Basic.canmove = true;
            body.gravityScale = config.gravity;
            body.linearVelocityX = config.movespeed;

            yield return new WaitForSeconds(config.dash_cooldown);

        }
        else  
        {
            body.linearVelocityX = -config.dash_speed;
            body.linearVelocityY = 0;
            body.gravityScale = 0;
            Basic.canmove = false;

            GameObject zansang1 = Instantiate(zansang_pre, trans.position, trans.rotation);
            zansang1.GetComponent<ZansangScript>().SetSprite(sprite.sprite, trans.localScale);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < (int)(1 / Time.timeScale); j++) { yield return new WaitForFixedUpdate(); }
                GameObject zansang = Instantiate(zansang_pre, trans.position, trans.rotation);
                zansang.GetComponent<ZansangScript>().SetSprite(sprite.sprite, trans.localScale);
            }

            Basic.canmove = true;
            body.gravityScale = config.gravity;
            body.linearVelocityX = -config.movespeed;

            yield return new WaitForSeconds(config.dash_cooldown);

        }
            
        blink = null;
    }

    public void Ankor_move()
    {
        ankorcontact = true;
    }

    IEnumerator Rope()
    {
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.004f;
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow));

        if (Input.GetKey(KeyCode.RightArrow)) 
        { 
            ankor = Instantiate(arrow, trans.position, Quaternion.Euler(0, 0, 0));
            ankor.GetComponent<ArrowScript>().player = this.gameObject;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        else if (Input.GetKey(KeyCode.UpArrow)) 
        {
            ankor = Instantiate(arrow, trans.position, Quaternion.Euler(0, 0, 90));
            ankor.GetComponent<ArrowScript>().player = this.gameObject;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            ankor = Instantiate(arrow, trans.position, Quaternion.Euler(0, 180, 0));
            ankor.GetComponent<ArrowScript>().player = this.gameObject;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
        else 
        { 
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            rope = null;
            yield break;
        }

        yield return new WaitUntil(() => ankorcontact || xpressed);
        if (!ankorcontact && Input.GetKey(KeyCode.X))
        {
            Destroy(ankor);
            xpressed = false;
            rope = null;
            yield break;

        }
        else
        {
            body.gravityScale = 0;
            cpressed = false;
            
            while (!xpressed /*&& !coli.IsTouchingLayers(ground)*/)
            {
                if (((Vector2)ankor.transform.position - (Vector2)body.transform.position).magnitude > 0.32f)
                {
                    body.linearVelocity = ((Vector2)ankor.transform.position - (Vector2)body.transform.position).normalized * config.rope_move_speed;
                }
                else { body.linearVelocity = Vector2.zero; }

                if (cpressed)
                {
                    cpressed = false;
                    if (spin == null) 
                    {
                        spin = StartCoroutine(Spin());
                        break;
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }
        xpressed = false;
        body.gravityScale = config.gravity;
        ankorcontact = false;
        Destroy(ankor);
        ankor = null;

        yield return new WaitForSeconds(0.2f);
        rope = null;
    }

    IEnumerator Bomb()
    {
        if (anim.heading)
        {
            bomb = Instantiate(bomb_pre, trans.position + new Vector3(1.2f, -1.4f, 0), Quaternion.Euler(0, 0, 0));
            flying_b = true;
            flying_f = false;
            body.linearVelocityX = -config.bomb_speed_x;
            body.linearVelocityY = config.bomb_speed_y;
        }
        else
        {
            bomb = Instantiate(bomb_pre, trans.position + new Vector3(-1.2f, -1.4f, 0), Quaternion.Euler(0, 180, 0));
            flying_f = true;
            flying_b= false;
            body.linearVelocityX = config.bomb_speed_x;
            body.linearVelocityY = config.bomb_speed_y;
        }
        yield return new WaitForSeconds(0.2f);
        Destroy(bomb);  
        yield return new WaitForSeconds(config.bomb_cooddown-0.2f);
        flying_f = false;
        flying_b = false;
        boom = null;
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X)) { xpressed = true; }

        if (Input.GetKeyDown(KeyCode.C)) { cpressed = true; }

        if (Input.GetKeyDown(KeyCode.C) && spin == null && !Basic.channeling && !ankorcontact) 
        {

            spin = StartCoroutine(Spin());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && blink == null)
        {
            blink = StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.V) && boom == null)
        {
            boom = StartCoroutine(Bomb());
        }
        if (body.linearVelocityX < config.movespeed && flying_f) { flying_f = false; }
        if (body.linearVelocityX > -config.movespeed && flying_b) { flying_b = false;  }


            if (xpressed && rope == null)   
        {
            xpressed = false;   
            rope = StartCoroutine(Rope()); 
        }

        if (ankor != null)
        {
            rope_renderer.enabled = true;
            rope_renderer.SetPosition(0, trans.position);
            rope_renderer.SetPosition(1, ankor.GetComponent<Transform>().position);
        }
        else { rope_renderer.enabled = false; }

    }
}
