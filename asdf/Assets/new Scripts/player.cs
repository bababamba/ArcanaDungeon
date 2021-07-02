using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using noname;



public class player : MonoBehaviour
{
    public float Movepower;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Vector3 movement;
    
    Vector2 PlayerPos = new Vector2(0,0);
    Level l;
    int dir = 0;


    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (rigid.velocity.normalized.x == 0 && rigid.velocity.normalized.y == 0)
            anim.SetBool("iswalking", false);
        else
            anim.SetBool("iswalking", true);
    



    }
    public void SetLevel(Level Le)
    {
        this.l = Le;

    }
    int CheckLevel(float x, float y)
    {
        float a = y+1;
        a *= -l.levelr.Width();
        a += x+1;
  
        return l.map[(int)a];//jabassibo

    }

    private void FixedUpdate()
    {
        //left
        if (Input.GetAxisRaw("Horizontal") == -1 && !anim.GetBool("iswalking"))
        {
            PlayerPos = new Vector2((float)System.Math.Round(rigid.position.x - 1), (float)System.Math.Round(rigid.position.y));
            Debug.Log(PlayerPos.y);
            if (CheckLevel(PlayerPos.x, PlayerPos.y) != 2)
            {
                rigid.AddForce(Vector2.left * Movepower, ForceMode2D.Impulse);
                anim.SetBool("iswalking", true);

                
                dir = 1;
            }
        }
        //right
        else if (Input.GetAxisRaw("Horizontal") == 1 && !anim.GetBool("iswalking"))
        {

            rigid.AddForce(Vector2.right * Movepower, ForceMode2D.Impulse);
            anim.SetBool("iswalking", true);
            PlayerPos = new Vector2(rigid.position.x + 1, rigid.position.y);
            dir = 2;

        }
        //up
        else if (Input.GetAxisRaw("Vertical") == 1 && !anim.GetBool("iswalking"))
        {

            rigid.AddForce(Vector2.up * Movepower, ForceMode2D.Impulse);
            anim.SetBool("iswalking", true);
            PlayerPos = new Vector2(rigid.position.x , rigid.position.y + 1);
            dir = 3;

        }
        //down
        else if (Input.GetAxisRaw("Vertical") == -1 && !anim.GetBool("iswalking"))
        {

            rigid.AddForce(Vector2.down * Movepower, ForceMode2D.Impulse);
            anim.SetBool("iswalking", true);
            PlayerPos = new Vector2(rigid.position.x , rigid.position.y - 1);
            dir = 4;

        }

        if (rigid.position.x <= PlayerPos.x && anim.GetBool("iswalking")&& dir == 1)
                {   
                    rigid.velocity = new Vector2(0, 0);
                    anim.SetBool("iswalking", false);
                    rigid.position = PlayerPos;
                    //Debug.Log("2");
                }
        else if (rigid.position.x >= PlayerPos.x && anim.GetBool("iswalking") && dir == 2)
        {
            rigid.velocity = new Vector2(0, 0);
            anim.SetBool("iswalking", false);
        }
        else if (rigid.position.y >= PlayerPos.y && anim.GetBool("iswalking") && dir == 3)
        {
            rigid.velocity = new Vector2(0, 0);
            anim.SetBool("iswalking", false);
        }
        else if (rigid.position.y <= PlayerPos.y && anim.GetBool("iswalking") && dir == 4)
        {
            rigid.velocity = new Vector2(0, 0);
            anim.SetBool("iswalking", false);
        }



    }
}
