using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using noname;
using noname.rooms;
using Terrain = noname.Terrain;




public class player : Thing
{
    /*��Card Ŭ���� ���� �� �ּ� ������ ��
    Card[] deck;
    Card[] discarded;
    Card[] hand;
     */

    public player me = null;
    private bool isturn;    //�÷��̾��� ���� true;

    private int hand_limit; //�� ��� ����

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
                    vision_marker();//�ڳ��߿� �÷��̾��� Turn() �Լ��� �ϼ��Ǹ� ���ʿ��� 1���� ����ǰ� �ű� ��, �Ʒ��� ���� �Լ��鵵 ����
                    Debug.Log("2");
                }
                break;
            case 2:
                MoveVector = new Vector2(transform.position.x + MovePower, transform.position.y);
                transform.position = MoveVector;
                if (transform.position.x >= PlayerPos.x)
                {
                    transform.position = PlayerPos;
                    vision_marker();//��
                    dir = 0;
                    anim.SetBool("iswalking", false);
                }
                break;
            case 3:
                MoveVector = new Vector2(transform.position.x, transform.position.y + MovePower);
                transform.position = MoveVector;
                if (transform.position.y >= PlayerPos.y) {
                    transform.position = PlayerPos;
                    vision_marker();//��
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
                    vision_marker();//��
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
    }//�ʿ� ���� ��, ��� �ڸ��� ����
    //��visionchecker�� ���� ������ �þ߿� ���̴� �κ��� ǥ���ϰ�, Level�� �ִ� ���� �迭�� �����ͼ� ��ǥ�� ���� ������ ��ġ�� ǥ���ϴ� �Լ�
    private void vision_marker() {
        FOV = new bool[GameManager.cur_level.width, GameManager.cur_level.height];
        Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);


        //�ڳ��߿� �ܼ��� �׸��ڸ� ����� ����� �� �̿ܿ� ������ ����� ����� �ٽ� ��Ÿ���� �ϴ� �ͱ��� �־���� �Ѵ�, �ƴϸ� �װ� ������ vision_searcher�� �ִ���
        //�������� RGB���� 0~1 ������ ��Ÿ���� �� �⺻������
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
