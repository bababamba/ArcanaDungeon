using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon;
using ArcanaDungeon.cards;
using ArcanaDungeon.rooms;
using Terrain = ArcanaDungeon.Terrain;



namespace ArcanaDungeon.Object
{
    public class player : Thing
    {
        /*��Card Ŭ���� ���� �� �ּ� ������ ��
        Card[] deck;
        Card[] discarded;
        Card[] hand;
         */
        AttackCard atcd = new AttackCard();

        public player me = null;
        private bool isturn;    //�÷��̾��� ���� true;

        private int hand_limit; //�� ��� ����

        public float MovePower = 0.2f;
        public int MoveTimerLimit = 5;
        public bool isPlayerTurn = true;
        public SpriteRenderer spriteRenderer;
        public Animator anim;
        public Rigidbody2D rigid;
        public Transform tr;
        public Vector3 movement;

        public Vector2 PlayerPos = new Vector2(0, 0);
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
        {//�Է¹޴°�
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Q������");

                atcd.UseCard(Dungeon.dungeon.Ene);
                Debug.Log("�÷��̾��� ü��" + Dungeon.dungeon.Ene.GetHp());
            }// �ӽ� Ű �Է� 
            if (isPlayerTurn == true)
            {
                
                if (MoveTimer == 0)
                {

                    //left
                    if (Input.GetAxisRaw("Horizontal") == -1 && !anim.GetBool("iswalking"))
                    {

                        PlayerPos = new Vector2(Mathf.Round(transform.position.x - 1), Mathf.Round(transform.position.y));
                        //Debug.Log(PlayerPos);
                        if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x - 1), (int)Mathf.Round(transform.position.y)]] & Terrain.passable) != 0)
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
                        if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x + 1), (int)Mathf.Round(transform.position.y)]] & Terrain.passable) != 0)
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
                        if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y + 1)]] & Terrain.passable) != 0)
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
                        Debug.Log(Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y - 1)] + " , " + Terrain.passable);
                        if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y - 1)]] & Terrain.passable) != 0)
                        {

                            anim.SetBool("iswalking", true);
                            MoveTimer = MoveTimerLimit;

                            dir = 4;
                        }


                    }
                }
                
            }


            //�Է¹޴°� ��
            if (MoveTimer > 0)
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
                        isPlayerTurn = false;            //Debug.Log("2");
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
                        isPlayerTurn = false;
                    }
                    break;
                case 3:
                    MoveVector = new Vector2(transform.position.x, transform.position.y + MovePower);
                    transform.position = MoveVector;
                    if (transform.position.y >= PlayerPos.y)
                    {
                        transform.position = PlayerPos;
                        vision_marker();//��
                        dir = 0;
                        anim.SetBool("iswalking", false);
                        isPlayerTurn = false;
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
                        isPlayerTurn = false;
                    }
                    break;
                default:
                    break;

            }


        }
        public override void Spawn()
        {
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    if (Dungeon.dungeon.currentlevel.map[i, j] == Terrain.STAIRS_UP)
                    {
                        PlayerPos = new Vector2(i + 1, j);
                        transform.position = new Vector2(i + 1, j);
                        vision_marker();
                        return;
                    }
                    else
                        continue;
                }
            }
            Debug.Log("Cannot find Upstairs.");
        }//�ʿ� ���� ��, ��� �ڸ��� ����
        public void Spawn(Vector2 pos)
        {
            PlayerPos = pos;
            transform.position = pos;
            vision_marker();
        }// Ư�� ��ǥ�� ��ȯ
         //��visionchecker�� ���� ������ �þ߿� ���̴� �κ��� ǥ���ϰ�, Level�� �ִ� ���� �迭�� �����ͼ� ��ǥ�� ���� ������ ��ġ�� ǥ���ϴ� �Լ�
        public override void turn()
        {
            
        }

        private void vision_marker()
        {
            FOV = new bool[Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.height];
            util.Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);


            //�ڳ��߿� �ܼ��� �׸��ڸ� ����� ����� �� �̿ܿ� ������ ����� ����� �ٽ� ��Ÿ���� �ϴ� �ͱ��� �־���� �Ѵ�, �ƴϸ� �װ� ������ vision_searcher�� �ִ���
            //�������� RGB���� 0~1 ������ ��Ÿ���� �� �⺻������
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    if (FOV[i, j])
                    {
                        Dungeon.dungeon.currentlevel.temp_gameobjects[i, j].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        Dungeon.dungeon.currentlevel.temp_gameobjects[i, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                    }
                }
            }
        }
    }
}