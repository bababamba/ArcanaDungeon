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

    public float MovePower = 0.2f;
    public int MoveTimerLimit = 5;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public Rigidbody2D rigid;
    public Vector3 movement;
    
    public Vector2 PlayerPos = new Vector2(0,0);
    public Vector2 MoveVector = new Vector2(0, 0);
    public Level l;


    int MoveTimer = 0;
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

        
    



    }
    public void SetLevel(Level Le)
    {
        this.l = Le;

    }
    public int CheckLevel(float x, float y)
    {
        float a = y+1;
        a *= -l.levelr.Width();
        a += x+1;
  
        return l.map[(int)a];//jabassibo

    }
    public int whereishe()
    {
        int here=0;
        here = (int)Mathf.Round(rigid.position.y + 1);
        here *= -l.levelr.Width();
        here += (int)Mathf.Round(rigid.position.x + 1);
        return here;
    }

    private void FixedUpdate()
    {
        if (MoveTimer == 0)
        {

            //left
            if (Input.GetAxisRaw("Horizontal") == -1 && !anim.GetBool("iswalking"))
            {
                
                PlayerPos = new Vector2(Mathf.Round(rigid.position.x - 1), Mathf.Round(rigid.position.y));
                Debug.Log(PlayerPos);
                if (CheckLevel(PlayerPos.x, PlayerPos.y) != 2)
                {

                    anim.SetBool("iswalking", true);
                    MoveTimer = MoveTimerLimit;

                    dir = 1;
                }
            }
            //right
            else if (Input.GetAxisRaw("Horizontal") == 1 && !anim.GetBool("iswalking"))
            {
                


                PlayerPos = new Vector2(Mathf.Round(rigid.position.x + 1), Mathf.Round(rigid.position.y));
                if (CheckLevel(PlayerPos.x, PlayerPos.y) != 2)
                {

                    anim.SetBool("iswalking", true);
                    MoveTimer = MoveTimerLimit;

                    dir = 2;
                }

            }
            //up
            else if (Input.GetAxisRaw("Vertical") == 1 && !anim.GetBool("iswalking"))
            {
                


                PlayerPos = new Vector2(Mathf.Round(rigid.position.x), Mathf.Round(rigid.position.y + 1));
                if (CheckLevel(PlayerPos.x, PlayerPos.y) != 2)
                {

                    anim.SetBool("iswalking", true);
                    MoveTimer = MoveTimerLimit;

                    dir = 3;
                }

            }
            //down
            else if (Input.GetAxisRaw("Vertical") == -1 && !anim.GetBool("iswalking"))
            {
               


                PlayerPos = new Vector2(Mathf.Round(rigid.position.x), Mathf.Round(rigid.position.y - 1));
                if (CheckLevel(PlayerPos.x, PlayerPos.y) != 2)
                {

                    anim.SetBool("iswalking", true);
                    MoveTimer = MoveTimerLimit;

                    dir = 4;
                }

            }
        }
        if(MoveTimer > 0)
            MoveTimer--;
        //playermove
        switch (dir)
        {
            case 1:
                MoveVector = new Vector2(rigid.position.x - MovePower, rigid.position.y);
                rigid.position = MoveVector;
                if (rigid.position.x <= PlayerPos.x)
                {
                    dir = 0;
                    anim.SetBool("iswalking", false);
                    rigid.position = PlayerPos;
                    vision_marker();//★나중에 플레이어의 Turn() 함수가 완성되면 그쪽에서 1번만 실행되게 옮길 것, 아래의 같은 함수들도 동일
                    Debug.Log("2");
                }
                break;
            case 2:
                MoveVector = new Vector2(rigid.position.x + MovePower, rigid.position.y);
                rigid.position = MoveVector;
                if (rigid.position.x >= PlayerPos.x)
                {
                    rigid.position = PlayerPos;
                    vision_marker();//★
                    dir = 0;
                    anim.SetBool("iswalking", false);
                }
                break;
            case 3:
                MoveVector = new Vector2(rigid.position.x, rigid.position.y + MovePower);
                rigid.position = MoveVector;
                if (rigid.position.y >= PlayerPos.y) {
                    rigid.position = PlayerPos;
                    vision_marker();//★
                    dir = 0;
                    anim.SetBool("iswalking", false);
                }
                break;
            case 4:
                MoveVector = new Vector2(rigid.position.x, rigid.position.y - MovePower);
                rigid.position = MoveVector;
                if (rigid.position.y <= PlayerPos.y)
                {
                    rigid.position = PlayerPos;
                    vision_marker();//★
                    dir = 0;
                    anim.SetBool("iswalking", false);
                }
                break;
            default:
                break;

        }


    }

    //★visionchecker을 먼저 실행해 시야에 보이는 부분을 표시하고, Level에 있는 몬스터 배열을 가져와서 좌표를 비교해 몬스터의 위치도 표시하는 함수
    private void vision_marker() {
        FOV = new bool[this.l.length];
        //cur_pos = -l.width *(int)this.transform.position.y  + (int)this.transform.position.x; //★현재 map[]과 player에서 사용하는 좌표가 서로 다르다, 나중에 통합하고 좌표 갱신 함수가 정리되면 이 줄을 지울 것
        cur_pos = whereishe();
        Visionchecker.vision_check(cur_pos % l.width, cur_pos / l.width, 6, FOV, l.vision_blockings);


        //★나중에 단순히 그림자를 씌우고 벗기는 것 이외에 몬스터의 모습을 지우고 다시 나타나게 하는 것까지 넣어줘야 한다, 아니면 그건 몬스터의 vision_searcher에 넣던가
        //프리팹의 RGB값은 0~1 범위로 나타내는 게 기본값같다
        for (int i=0; i<l.length; i++){
            if(FOV[i]){
                l.temp_gameobjects[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }else{
                l.temp_gameobjects[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        } 
    }
}
