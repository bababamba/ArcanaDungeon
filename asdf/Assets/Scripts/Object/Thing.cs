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
        private int maxhp = 300; // 최대 체력 임의로 설정했어요.jgh.
        private int block;
        private int vision_distance;
        //public int[] cur_pos;    //이 물체의 현재 위치, Level 클래스의 map[]을 좌표처럼 사용한다     ★수정해야 한다, 이제 모든 좌표는 unity의 transform 좌표로 사용할 것이다, 다 바꿔야 한다 쥐엔장
        public List<int> route_pos = new List<int>();  //목적지까지의 이동 경로, 이동은 항상 route_pos[0]으로 이동해서 진행된다
        private string[] text;  //물체의 이름과 설명

        private int condition;  //물체가 보유한 상태이상 및 버프를 나타냄, 각각의 효과들은 d에 상수로 보관되어 있음

        public string name;

        public Thing()
        {
            condition = 0;
        }

        public abstract void Spawn();

        //hp 관련 함수
        public int GetHp()
        {
            return this.hp;
        }

        public void HpChange(int val)
        {
            if (val > 0)
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
            else {
                this.hp += val;
                if (this.hp < 0)
                {
                    this.die();
                }
            }            
        }

        //block 관련 함수
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

        //이동 관련 함수
        public void move() { }

        public void route_BFS(int dest_x, int dest_y, bool[,] fov)    //넓이 우선 탐색으로 목적지까지의 경로를 route_pos에 저장해주는 함수
        {
            //route_BFS에서의 좌표는 x+y*(맵 너비)로 나타낸다. BFS 알고리즘을 최대한 간략하게 구현하기 위해 부득이하게 좌표를 int 변수 1개로 나타낸 것이다
            int destination = dest_x + dest_y * Dungeon.dungeon.currentlevel.width;
            List<int> checking = new List<int>();
            int[] prev = new int[Dungeon.dungeon.currentlevel.length];
            int[] dir = new int[] { -1, -1 + Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.width, 1 + Dungeon.dungeon.currentlevel.width, 1, 1 - Dungeon.dungeon.currentlevel.width, -Dungeon.dungeon.currentlevel.width, -1 - Dungeon.dungeon.currentlevel.width };


            int FOV_true = 0; foreach (bool b in fov) { if (b) { FOV_true++; } }

            checking.Add((int)(transform.position.x + transform.position.y * Dungeon.dungeon.currentlevel.width));
            int temp, temp2;
            for (int i = 0; i < FOV_true - 1; i++)
            {
                //주변 좌표 포함 시 확인해야 하는 것 : 몬스터의 cur_pos가 아닌가, passable인가?, level의 length 범위 이내의 숫자인가, prev[i]==null인가
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

                //Plr_pos[0]이랑 같은 좌표인지 확인, 맞으면 prev 배열 쭉 타고올라가면서 route_pos에 저장
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

        //상태이상 처리 관련 함수
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
            HpChange(-this.maxhp / 10);
        }
        private void stun_process()
        {
            //★턴 생략
        }

        public void die() { } //★나중에 자기자신을 map[]에서 삭제하는 정도는 넣어두자
        public abstract void turn();
    }
}
