using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using noname;



public class player : Thing
{
    /*★Card 클래스 제작 후 주석 제거할 것
    Card[] deck;
    Card[] discarded;
    Card[] hand;
     */
    private bool isturn;    //플레이어턴 동안 true;

    private int hand_limit; //패 장수 제한

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

    //★visionchecker을 먼저 실행해 시야에 보이는 부분을 표시하고, Level에 있는 몬스터 배열을 가져와서 좌표를 비교해 몬스터의 위치도 표시하는 함수
    private void vision_marker() {
        FOV = new bool[l.length];
        cur_pos = -l.width *(int)this.transform.position.y  + (int)this.transform.position.x; //★현재 map[]과 player에서 사용하는 좌표가 서로 다르다, 나중에 통합하고 좌표 갱신 함수가 정리되면 이 줄을 지울 것
        Visionchecker.vision_check(cur_pos % l.width, cur_pos / l.width, 6, FOV, l.vision_blockings);

        /*★
        for (int i=0; i<l.length; i++){
            if(FOV[i]){
                
            }
        }
         */

    }
}
