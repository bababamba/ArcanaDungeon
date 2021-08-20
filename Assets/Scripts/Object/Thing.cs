using System;
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
        protected int maxhp = 300; // �ִ� ü�� ���Ƿ� �����߾��.jgh.
        private int stamina;
        protected int maxstamina;

        private int block;
        private int vision_distance;
        public int isTurn;  //1 �̻��� ��� �� ��ü�� ���̴�, 0�� ��� �ܼ��� �� ��ü�� ���� �ƴ� ���̸�, ������ ��� ���� ���� ������ ���� ������ ���̴�

        public List<int> route_pos = new List<int>();  //������������ �̵� ���, �̵��� �׻� route_pos[0]���� �̵��ؼ� ����ȴ�
        private string[] text;  //��ü�� �̸��� ����

        private Dictionary<int,int> condition;  //�����̻� �� ���� ǥ��, Ű���� �����̻� �����̸� ��ġ���� ���ӽð�, Ű���� ���� ȿ�� : 0=���� / 1=���� / 

        public string name;

        public Thing()
        {
            condition = new Dictionary<int, int>();
        }

        public abstract void Spawn();

        //hp ���� �Լ�
        public int GetHp()
        {
            return this.hp;
        }

        public void HpChange(int val)
        {
            if (val > 0) {
                if (this.hp + val > this.maxhp) {
                    this.hp = this.maxhp;
                }
                else {
                    this.hp += val;
                }
            } else {
                this.hp -= val;
                if (this.hp < 0)
                {
                    this.die();
                }
            }            
        }

        //stamina ���� �Լ�
        public int GetStamina() {
            return stamina;
        }

        public void StaminaChange(int val) {
            if (val > 0) {
                if (this.stamina + val > this.maxstamina) {
                    this.stamina = this.maxstamina;
                }
                else {
                    this.stamina += val;
                }
            } else {
                this.stamina -= val;
                if (this.stamina < 0) {
                    this.stamina = 0;
                }
            }
        }

        //block ���� �Լ�
        public int GetBlock() {
            return this.block;
        }

        public void BlockChange(int val)
        {
            if (val > 0)
            {
                this.block += val;
            }
            else {
                if (this.block - val < 0)
                {
                    this.block = 0;
                }
                else { 
                    this.block -= val;
                }
            }
            
        }

        //�̵� ���� �Լ�
        public void move() { }

        public void route_BFS(int dest_x, int dest_y)    //���� �켱 Ž������ ������������ ��θ� route_pos�� �������ִ� �Լ�
        {
            //route_BFS������ ��ǥ�� x+y*(�� �ʺ�)�� ��Ÿ����. BFS �˰����� �ִ��� �����ϰ� �����ϱ� ���� �ε����ϰ� ��ǥ�� int ���� 1���� ��Ÿ�� ���̴�
            int destination = dest_x + dest_y * Dungeon.dungeon.currentlevel.width;
            Queue<int> checking = new Queue<int>();
            int[] prev = new int[Dungeon.dungeon.currentlevel.length];
            int[] dir = new int[] { -1, -1 + Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.width, 1 + Dungeon.dungeon.currentlevel.width, 1, 1 - Dungeon.dungeon.currentlevel.width, -Dungeon.dungeon.currentlevel.width, -1 - Dungeon.dungeon.currentlevel.width };

            checking.Enqueue((int)(transform.position.x + transform.position.y * Dungeon.dungeon.currentlevel.width));
            int temp, temp2;
            while (checking.Count > 0)
            {
                //�ֺ� ��ǥ ���� �� Ȯ���ؾ� �ϴ� �� : ������ cur_pos�� �ƴѰ�, passable�ΰ�?, level�� length ���� �̳��� �����ΰ�, prev[i]==null�ΰ�
                for (int ii = 0; ii < 8; ii++)
                {
                    temp = checking.Peek() + dir[ii];
                    if ((transform.position.x + transform.position.y * Dungeon.dungeon.currentlevel.width != temp) & ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[temp % Dungeon.dungeon.currentlevel.width, temp / Dungeon.dungeon.currentlevel.width]] & Terrain.passable) != 0)
                        & (temp > 0 & temp < Dungeon.dungeon.currentlevel.length) & (prev[temp] == 0))
                    {
                        checking.Enqueue(temp);
                        prev[temp] = checking.Peek();
                    }
                }

                //Plr_pos[0]�̶� ���� ��ǥ���� Ȯ��, ������ prev �迭 �� Ÿ��ö󰡸鼭 route_pos�� ����
                if (checking.Peek() == destination)
                {
                    temp2 = checking.Peek();
                    route_pos.Clear();
                    while (prev[temp2] != 0)
                    {
                        route_pos.Insert(0, temp2);
                        temp2 = prev[temp2];
                    }
                    break;
                }
                checking.Dequeue();
            }

            return;
        }

        //�����̻� ó�� ���� �Լ�
        public void condition_process()
        {
            if (this.condition[0] > 0) { //����
                HpChange(-this.maxhp / 10);
                this.condition[0] -= 1;
            }
            if (this.condition[1] > 0) {    //����
                this.isTurn -= 1;
                this.condition[1] -= 1;
            }
            if (this.condition[2] > 0) {    //�޷�
                StaminaChange(15);
                this.condition[2] -= 1;
            }
            if (this.condition[3] > 0) {    //Ǯ����
                if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)transform.position.x, (int)transform.position.y]] & Terrain.water) != 0) { //�� Ÿ�� ���� ������ ���¹̳� ���ҷ� ����
                    StaminaChange(-30);
                } else {
                    StaminaChange(-15);
                }
                this.condition[3] -= 1;
            }
        }

        public void condition_add(int key, int val) {
            if (condition.ContainsKey(key)) {
                condition[key] += val;
            } else {
                condition.Add(key, val);
            }
        }

        public void die() { } //�ڳ��߿� �ڱ��ڽ��� map[]���� �����ϴ� ������ �־����
        public abstract void turn();
    }
}
