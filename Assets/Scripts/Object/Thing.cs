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
        protected int maxhp = 300; // 최대 체력 임의로 설정했어요.jgh.
        private int stamina;
        protected int maxstamina;

        private int block;
        private int vision_distance;
        public int isTurn;  //1 이상일 경우 이 객체의 턴이다, 0일 경우 단순히 이 객체의 턴이 아닌 것이며, 음수일 경우 기절 등의 이유로 턴이 생략될 것이다

        public List<int> route_pos = new List<int>();  //목적지까지의 이동 경로, 이동은 항상 route_pos[0]으로 이동해서 진행된다
        private string[] text;  //물체의 이름과 설명

        private Dictionary<int,int> condition;  //상태이상 및 버프 표시, 키값은 상태이상 종류이며 가치값은 지속시간, 키값에 따른 효과 : 0=연소 / 1=기절 / 

        public string name;

        public Thing()
        {
            condition = new Dictionary<int, int>();
        }

        public abstract void Spawn();

        //hp 관련 함수
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

        //stamina 관련 함수
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

        public void route_BFS(int dest_x, int dest_y)    //넓이 우선 탐색으로 목적지까지의 경로를 route_pos에 저장해주는 함수
        {
            //route_BFS에서의 좌표는 x+y*(맵 너비)로 나타낸다. BFS 알고리즘을 최대한 간략하게 구현하기 위해 부득이하게 좌표를 int 변수 1개로 나타낸 것이다
            int destination = dest_x + dest_y * Dungeon.dungeon.currentlevel.width;
            Queue<int> checking = new Queue<int>();
            int[] prev = new int[Dungeon.dungeon.currentlevel.length];
            int[] dir = new int[] { -1, -1 + Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.width, 1 + Dungeon.dungeon.currentlevel.width, 1, 1 - Dungeon.dungeon.currentlevel.width, -Dungeon.dungeon.currentlevel.width, -1 - Dungeon.dungeon.currentlevel.width };

            checking.Enqueue((int)(transform.position.x + transform.position.y * Dungeon.dungeon.currentlevel.width));
            int temp, temp2;
            while (checking.Count > 0)
            {
                //주변 좌표 포함 시 확인해야 하는 것 : 몬스터의 cur_pos가 아닌가, passable인가?, level의 length 범위 이내의 숫자인가, prev[i]==null인가
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

                //Plr_pos[0]이랑 같은 좌표인지 확인, 맞으면 prev 배열 쭉 타고올라가면서 route_pos에 저장
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

        //상태이상 처리 관련 함수
        public void condition_process()
        {
            if (this.condition[0] > 0) { //연소
                HpChange(-this.maxhp / 10);
                this.condition[0] -= 1;
            }
            if (this.condition[1] > 0) {    //기절
                this.isTurn -= 1;
                this.condition[1] -= 1;
            }
            if (this.condition[2] > 0) {    //급류
                StaminaChange(15);
                this.condition[2] -= 1;
            }
            if (this.condition[3] > 0) {    //풀묶기
                if ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[(int)transform.position.x, (int)transform.position.y]] & Terrain.water) != 0) { //물 타일 위에 있으면 스태미나 감소량 증가
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

        public void die() { } //★나중에 자기자신을 map[]에서 삭제하는 정도는 넣어두자
        public abstract void turn();
    }
}
