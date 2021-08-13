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

        int[,] Plr_pos = new int[2, 2];  //0번 인덱스는 실제 플레이어 위치, 1번 인덱스는 마지막으로 본 플레이어 위치

        public bool[,] FOV;

        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //여기에 필드 복사
            e.isEnemyturn = this.isEnemyturn;

            return e;
        }

        public void Update()
        { //★몬스터 알고리즘 확인용 임시 함수, 나중에 꼭 삭제할 것
            if (isEnemyturn == true)
            {
                Vision_research();

                if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                {
                    Debug.Log("몬스터가 당신을 공격하려고 합니다. 근데 아직 구현이 안 돼서 못 하네요. 저런!");
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

            Plr_pos[0, 0] = -1; //Plr_pos[0,0]에 -1을 넣어두고 플레이어를 포착하면 그 좌표로 변경한다, 만약 플레이어를 포착하지 못 하면 -1인 채로 남기 때문에 확인할 수 있다
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    //시야에 보이는 위치인데, 플레이어랑 좌표까지 같으면 거기에 있는 게 플레이어니까 Plr_pos에 저장한다
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

            //★몬스터의 시야 범위를 파랑색으로, 몬스터의 cur_pos는 녹색으로 나타낸다, 당연히 몬스터의 시야 범위를 보여줄 필요가 없으므로 나중에 삭제할 것
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
            //★몬스터에서 플레이어에게로 가는 경로를 붉게 표시한다, 나중에 몬스터의 행동이 코딩되면 삭제할 것
            foreach (int ii in route_pos)
            {
                Dungeon.dungeon.currentlevel.temp_gameobjects[ii % Dungeon.dungeon.currentlevel.width, ii / Dungeon.dungeon.currentlevel.width].GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
            }
        }
    }
}