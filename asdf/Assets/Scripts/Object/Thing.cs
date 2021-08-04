using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon;
using Terrain = ArcanaDungeon.Terrain;

namespace ArcanaDungeon.Object
{
    public abstract class Thing : MonoBehaviour
    {
        private int hp;
        private int maxhp;
        private int block;
        private int vision_distance;
        //public int[] cur_pos;    //�� ��ü�� ���� ��ġ, Level Ŭ������ map[]�� ��ǥó�� ����Ѵ�     �ڼ����ؾ� �Ѵ�, ���� ��� ��ǥ�� unity�� transform ��ǥ�� ����� ���̴�, �� �ٲ�� �Ѵ� �㿣��
        public List<int> route_pos = new List<int>();  //������������ �̵� ���, �̵��� �׻� route_pos[0]���� �̵��ؼ� ����ȴ�
        private string[] text;  //��ü�� �̸��� ����

        private int condition;  //��ü�� ������ �����̻� �� ������ ��Ÿ��, ������ ȿ������ d�� ����� �����Ǿ� ����

        public Thing()
        {
            condition = 0;
        }

        public abstract void Spawn();

        //hp ���� �Լ�
        public int gethp()
        {
            return hp;
        }

        public void hpdown(int val)
        {
            this.hp -= val;
            if (this.hp < 0)
            {
                this.die();
            }
        }

        public void hpup(int val)
        {
            if (this.hp + val > this.maxhp)
            {
                this.hp = this.maxhp;
            }
            else
            {
                this.hp += val;
            }
        }

        //block ���� �Լ�
        public int getblock()
        {
            return this.block;
        }

        public void blockup(int val)
        {
            this.block += val;
        }

        public void blockdown(int val)
        {
            //���� ������ ������ �ڵ����� ü���� ���ߵ��� �صд�
            if (this.block >= val)
            {
                this.block -= val;
            }
            else
            {
                hpdown(val - this.block);
            }
        }

        //�̵� ���� �Լ�
        public void move() { }

        public void route_BFS(int dest_x, int dest_y, bool[,] fov)    //���� �켱 Ž������ ������������ ��θ� route_pos�� �������ִ� �Լ�
        {
            //route_BFS������ ��ǥ�� x+y*(�� �ʺ�)�� ��Ÿ����. BFS �˰����� �ִ��� �����ϰ� �����ϱ� ���� �ε����ϰ� ��ǥ�� int ���� 1���� ��Ÿ�� ���̴�
            int destination = dest_x + dest_y * Dungeon.dungeon.currentlevel.width;
            List<int> checking = new List<int>();
            int[] prev = new int[Dungeon.dungeon.currentlevel.length];
            int[] dir = new int[] { -1, -1 + Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.width, 1 + Dungeon.dungeon.currentlevel.width, 1, 1 - Dungeon.dungeon.currentlevel.width, -Dungeon.dungeon.currentlevel.width, -1 - Dungeon.dungeon.currentlevel.width };


            int FOV_true = 0; foreach (bool b in fov) { if (b) { FOV_true++; } }

            checking.Add((int)(transform.position.x + transform.position.y * Dungeon.dungeon.currentlevel.width));
            int temp, temp2;
            for (int i = 0; i < FOV_true - 1; i++)
            {
                //�ֺ� ��ǥ ���� �� Ȯ���ؾ� �ϴ� �� : ������ cur_pos�� �ƴѰ�, passable�ΰ�?, level�� length ���� �̳��� �����ΰ�, prev[i]==null�ΰ�
                for (int ii = 0; ii < 8; ii++)
                {
                    temp = checking[i] + dir[ii];
                    if ((transform.position.x + transform.position.y * Dungeon.dungeon.currentlevel.width != temp) & ((Dungeon.dungeon.currentlevel.map[temp % Dungeon.dungeon.currentlevel.width, temp / Dungeon.dungeon.currentlevel.width] & Terrain.passable) != 0)
                        & (temp > 0 & temp < Dungeon.dungeon.currentlevel.length) & (prev[temp] == 0))
                    {
                        checking.Add(temp);
                        prev[temp] = checking[i];
                    }
                }

                //Plr_pos[0]�̶� ���� ��ǥ���� Ȯ��, ������ prev �迭 �� Ÿ��ö󰡸鼭 route_pos�� ����
                if (checking[i] == destination)
                {
                    temp2 = checking[i];
                    route_pos.Clear();
                    while (prev[temp2] != 0)
                    {
                        route_pos.Insert(0, temp2);
                        temp2 = prev[temp2];
                    }
                    break;
                }
            }

            return;
        }

        //�����̻� ó�� ���� �Լ�
        public void condition_process()
        {
            if ((this.condition & Dungeon.burnt) != 0)
            {
                burnt_process();
            }
            if ((this.condition & Dungeon.stun) != 0)
            {
                stun_process();
            }
        }
        private void burnt_process()
        {
            hpdown(this.maxhp / 10);
        }
        private void stun_process()
        {
            //���� ����
        }

        public void die() { } //�ڳ��߿� �ڱ��ڽ��� map[]���� �����ϴ� ������ �־����
        public void turn() { }
    }
}
