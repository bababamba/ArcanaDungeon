using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using noname;



public class player : Thing
{
    /*��Card Ŭ���� ���� �� �ּ� ������ ��
    Card[] deck;
    Card[] discarded;
    Card[] hand;
     */
    private bool isturn;    //�÷��̾��� ���� true;

    private int hand_limit; //�� ��� ����

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

    //��visionchecker�� ���� ������ �þ߿� ���̴� �κ��� ǥ���ϰ�, Level�� �ִ� ���� �迭�� �����ͼ� ��ǥ�� ���� ������ ��ġ�� ǥ���ϴ� �Լ�
    private void vision_marker() {
        FOV = new bool[this.l.length];
        //cur_pos = -l.width *(int)this.transform.position.y  + (int)this.transform.position.x; //������ map[]�� player���� ����ϴ� ��ǥ�� ���� �ٸ���, ���߿� �����ϰ� ��ǥ ���� �Լ��� �����Ǹ� �� ���� ���� ��
        cur_pos = whereishe();
        Visionchecker.vision_check(cur_pos % l.width, cur_pos / l.width, 6, FOV, l.vision_blockings);


        //�ڳ��߿� �ܼ��� �׸��ڸ� ����� ����� �� �̿ܿ� ������ ����� ����� �ٽ� ��Ÿ���� �ϴ� �ͱ��� �־���� �Ѵ�, �ƴϸ� �װ� ������ vision_searcher�� �ִ���
        //�������� RGB���� 0~1 ������ ��Ÿ���� �� �⺻������
        for (int i=0; i<l.length; i++){
            if(FOV[i]){
                l.temp_gameobjects[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }else{
                l.temp_gameobjects[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        } 
    }
}
