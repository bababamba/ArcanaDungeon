using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using noname;
using noname.rooms;
using Terrain = noname.Terrain;




public class player : Thing
{
    /*★Card 클래스 제작 후 주석 제거할 것
    Card[] deck;
    Card[] discarded;
    Card[] hand;
     */

    public player me = null;
    private bool isturn;    //플레이어턴 동안 true;

    private int hand_limit; //패 장수 제한

    public float MovePower = 0.2f;
    public int MoveTimerLimit = 5;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public Rigidbody2D rigid;
    public Transform tr;
    public Vector3 movement;
    
    public Vector2 PlayerPos = new Vector2(0,0);
    public Vector2 MoveVector = new Vector2(0, 0);

    int MoveTimer = 0;
    int dir = 0;

    public bool[,] FOV;

    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        if (me == null)
            me = this;
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        if (me == null)
            me = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        
    



    }

    private void FixedUpdate()
    {
        if (MoveTimer == 0)
        {

            //left
            if (Input.GetAxisRaw("Horizontal") == -1 && !anim.GetBool("iswalking"))
            {
                
                PlayerPos = new Vector2(Mathf.Round(transform.position.x - 1), Mathf.Round(transform.position.y));
                //Debug.Log(PlayerPos);
                if ((GameManager.cur_level.map[(int)Mathf.Round(transform.position.x-1), (int)Mathf.Round(transform.position.y)] & Terrain.passable) != 0)
                {

                    anim.SetBool("iswalking", true);
                    MoveTimer = MoveTimerLimit;

                    dir = 1;
                }
            }
            //right
            else if (Input.GetAxisRaw("Horizontal") == 1 && !anim.GetBool("iswalking"))
            {
                


                PlayerPos = new Vector2(Mathf.Round(transform.position.x + 1), Mathf.Round(transform.position.y));
                if ((GameManager.cur_level.map[(int)Mathf.Round(transform.position.x + 1), (int)Mathf.Round(transform.position.y)] & Terrain.passable) != 0)
                {

                    anim.SetBool("iswalking", true);
                    MoveTimer = MoveTimerLimit;

                    dir = 2;
                }

            }
            //up
            else if (Input.GetAxisRaw("Vertical") == 1 && !anim.GetBool("iswalking"))
            {
                


                PlayerPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y + 1));
                if ((GameManager.cur_level.map[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y + 1)] & Terrain.passable) != 0)
                {

                    anim.SetBool("iswalking", true);
                    MoveTimer = MoveTimerLimit;

                    dir = 3;
                }

            }
            //down
            else if (Input.GetAxisRaw("Vertical") == -1 && !anim.GetBool("iswalking"))
            {
               


                PlayerPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 1));
                if ((GameManager.cur_level.map[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y - 1)] & Terrain.passable) != 0)
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
                MoveVector = new Vector2(transform.position.x - MovePower, transform.position.y);
                transform.position = MoveVector;
                if (transform.position.x <= PlayerPos.x)
                {
                    dir = 0;
                    anim.SetBool("iswalking", false);
                    transform.position = PlayerPos;
                    vision_marker();//★나중에 플레이어의 Turn() 함수가 완성되면 그쪽에서 1번만 실행되게 옮길 것, 아래의 같은 함수들도 동일
                    Debug.Log("2");
                }
                break;
            case 2:
                MoveVector = new Vector2(transform.position.x + MovePower, transform.position.y);
                transform.position = MoveVector;
                if (transform.position.x >= PlayerPos.x)
                {
                    transform.position = PlayerPos;
                    vision_marker();//★
                    dir = 0;
                    anim.SetBool("iswalking", false);
                }
                break;
            case 3:
                MoveVector = new Vector2(transform.position.x, transform.position.y + MovePower);
                transform.position = MoveVector;
                if (transform.position.y >= PlayerPos.y) {
                    transform.position = PlayerPos;
                    vision_marker();//★
                    dir = 0;
                    anim.SetBool("iswalking", false);
                }
                break;
            case 4:
                MoveVector = new Vector2(transform.position.x, transform.position.y - MovePower);
                transform.position = MoveVector;
                if (transform.position.y <= PlayerPos.y)
                {
                    transform.position = PlayerPos;
                    vision_marker();//★
                    dir = 0;
                    anim.SetBool("iswalking", false);
                }
                break;
            default:
                break;

        }
        

    }
    public void Spawn()
    {
        for(int i = 0; i < GameManager.cur_level.width; i++){
            for (int j = 0; j < GameManager.cur_level.height; j++)
            {
                if (GameManager.cur_level.map[i, j] == Terrain.STAIRS_UP)
                {
                    transform.position = new Vector2(i, j);
                    vision_marker();
                    return;
                }
                else
                    continue;
            }
        }
        Debug.Log("Cannot find Upstairs.");
    }//맵에 입장 시, 계단 자리에 스폰
    //★visionchecker을 먼저 실행해 시야에 보이는 부분을 표시하고, Level에 있는 몬스터 배열을 가져와서 좌표를 비교해 몬스터의 위치도 표시하는 함수
    private void vision_marker() {
        FOV = new bool[GameManager.cur_level.width, GameManager.cur_level.height];
        Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);


        //★나중에 단순히 그림자를 씌우고 벗기는 것 이외에 몬스터의 모습을 지우고 다시 나타나게 하는 것까지 넣어줘야 한다, 아니면 그건 몬스터의 vision_searcher에 넣던가
        //프리팹의 RGB값은 0~1 범위로 나타내는 게 기본값같다
        for (int i=0; i< GameManager.cur_level.width; i++){
            for (int j = 0; j < GameManager.cur_level.height; j++)
            {
                if (FOV[i,j])
                {
                    GameManager.cur_level.temp_gameobjects[i,j].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
                else
                {
                    GameManager.cur_level.temp_gameobjects[i,j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                }
            }
        } 
    }
}
