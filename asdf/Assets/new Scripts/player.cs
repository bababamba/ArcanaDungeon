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

    public bool[] FOV;

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

    public int CheckLevel(float x, float y)
    {
        float a = y+1;
        a *= -GameManager.cur_level.levelr.Width();
        a += x+1;
  
        return GameManager.cur_level.map[(int)a];//jabassibo

    }
    public int whereishe()
    {
        int here=0;
        here = (int)Mathf.Round(rigid.position.y + 1);
        here *= -GameManager.cur_level.levelr.Width();
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
                //Debug.Log(PlayerPos);
                if (CheckLevel(PlayerPos.x, PlayerPos.y) != Terrain.WALL)
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
                if (CheckLevel(PlayerPos.x, PlayerPos.y) != Terrain.WALL)
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
                if (CheckLevel(PlayerPos.x, PlayerPos.y) != Terrain.WALL)
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
                if (CheckLevel(PlayerPos.x, PlayerPos.y) != Terrain.WALL)
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
                    vision_marker();//�ڳ��߿� �÷��̾��� Turn() �Լ��� �ϼ��Ǹ� ���ʿ��� 1���� ����ǰ� �ű� ��, �Ʒ��� ���� �Լ��鵵 ����
                    Debug.Log("2");
                }
                break;
            case 2:
                MoveVector = new Vector2(rigid.position.x + MovePower, rigid.position.y);
                rigid.position = MoveVector;
                if (rigid.position.x >= PlayerPos.x)
                {
                    rigid.position = PlayerPos;
                    vision_marker();//��
                    dir = 0;
                    anim.SetBool("iswalking", false);
                }
                break;
            case 3:
                MoveVector = new Vector2(rigid.position.x, rigid.position.y + MovePower);
                rigid.position = MoveVector;
                if (rigid.position.y >= PlayerPos.y) {
                    rigid.position = PlayerPos;
                    vision_marker();//��
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
        for(int i = 0; i < GameManager.cur_level.map.Length; i++)
        {
            if (GameManager.cur_level.map[i] == Terrain.STAIRS_UP)
            {
                int x = i % GameManager.cur_level.levelr.Width();
                int y = i / GameManager.cur_level.levelr.Width();
                PlayerPos = new Vector2(x - 1, -y - 1);
                me.rigid.position = PlayerPos;
                vision_marker();
                Debug.Log("Player spawned at" + PlayerPos);
                return;
            }
            else
                continue;
        }
        Debug.Log("Cannot find Upstairs.");
    }//�ʿ� ���� ��, ��� �ڸ��� ����
    //��visionchecker�� ���� ������ �þ߿� ���̴� �κ��� ǥ���ϰ�, Level�� �ִ� ���� �迭�� �����ͼ� ��ǥ�� ���� ������ ��ġ�� ǥ���ϴ� �Լ�
    private void vision_marker() {
        FOV = new bool[GameManager.cur_level.length];
        //cur_pos = -GameManager.cur_level.width *(int)this.transform.position.y  + (int)this.transform.position.x; //������ map[]�� player���� ����ϴ� ��ǥ�� ���� �ٸ���, ���߿� �����ϰ� ��ǥ ���� �Լ��� �����Ǹ� �� ���� ���� ��
        cur_pos = whereishe();
        Visionchecker.vision_check(cur_pos % GameManager.cur_level.width, cur_pos / GameManager.cur_level.width, 6, FOV);


        //�ڳ��߿� �ܼ��� �׸��ڸ� ����� ����� �� �̿ܿ� ������ ����� ����� �ٽ� ��Ÿ���� �ϴ� �ͱ��� �־���� �Ѵ�, �ƴϸ� �װ� ������ vision_searcher�� �ִ���
        //�������� RGB���� 0~1 ������ ��Ÿ���� �� �⺻������
        for (int i=0; i< GameManager.cur_level.length; i++){
            if(FOV[i]){
                GameManager.cur_level.temp_gameobjects[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }else{
                GameManager.cur_level.temp_gameobjects[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        } 
    }
}
