using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain = ArcanaDungeon.Terrain;
using ArcanaDungeon;
using ArcanaDungeon.util;

namespace ArcanaDungeon.Object
{
    public class Enemy : Thing
    {

        bool isawaken;
        int move_speed;
        int action_per_turn;
        public bool isEnemyturn = false;

        int[,] Plr_pos = new int[2, 2];  //0�� �ε����� ���� �÷��̾� ��ġ, 1�� �ε����� ���������� �� �÷��̾� ��ġ

        public bool[,] FOV;

        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //���⿡ �ʵ� ����
            e.isEnemyturn = this.isEnemyturn;

            return e;
        }

        public void Update()
        { //�ڸ��� �˰��� Ȯ�ο� �ӽ� �Լ�, ���߿� �� ������ ��
            if (isEnemyturn == true)
            {
                Vision_research();

                if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                {
                    Debug.Log("���Ͱ� ����� �����Ϸ��� �մϴ�. �ٵ� ���� ������ �� �ż� �� �ϳ׿�. ����!");
                }
                else if (route_pos.Count > 0)
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                }
                this.isEnemyturn = false;
            }
        }

        public override void Spawn()
        {

        }
        public void Spawn(Vector2 pos)
        {
            transform.position = pos;
            Vision_research();
        }

        public override void turn()
        {
            isEnemyturn = true;
        }

        private void Vision_research()
        {
            FOV = new bool[Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.height];
            Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);

            Plr_pos[0, 0] = -1; //Plr_pos[0,0]�� -1�� �־�ΰ� �÷��̾ �����ϸ� �� ��ǥ�� �����Ѵ�, ���� �÷��̾ �������� �� �ϸ� -1�� ä�� ���� ������ Ȯ���� �� �ִ�
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    //�þ߿� ���̴� ��ġ�ε�, �÷��̾�� ��ǥ���� ������ �ű⿡ �ִ� �� �÷��̾�ϱ� Plr_pos�� �����Ѵ�
                    if (FOV[i, j] & i == Dungeon.dungeon.Plr.transform.position.x & j == Dungeon.dungeon.Plr.transform.position.y)
                    {
                        Plr_pos[0, 0] = Plr_pos[1, 0] = (int)Dungeon.dungeon.Plr.transform.position.x;
                        Plr_pos[0, 1] = Plr_pos[1, 1] = (int)Dungeon.dungeon.Plr.transform.position.y;
                        break;
                    }
                }
            }

            if (Plr_pos[0, 0] != -1)
            {
                route_BFS(Plr_pos[0, 0], Plr_pos[0, 1]);
            }

            //�ڸ����� �þ� ������ �Ķ�������, ������ cur_pos�� ������� ��Ÿ����, �翬�� ������ �þ� ������ ������ �ʿ䰡 �����Ƿ� ���߿� ������ ��
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    if (FOV[i, j])
                    {
                        Dungeon.dungeon.currentlevel.temp_gameobjects[i, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1, 1);
                    }
                }
            }
            Dungeon.dungeon.currentlevel.temp_gameobjects[(int)transform.position.x, (int)transform.position.y].GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f, 1);
            //�ڸ��Ϳ��� �÷��̾�Է� ���� ��θ� �Ӱ� ǥ���Ѵ�, ���߿� ������ �ൿ�� �ڵ��Ǹ� ������ ��
            foreach (int ii in route_pos)
            {
                Dungeon.dungeon.currentlevel.temp_gameobjects[ii % Dungeon.dungeon.currentlevel.width, ii / Dungeon.dungeon.currentlevel.width].GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
            }
        }
    }
}