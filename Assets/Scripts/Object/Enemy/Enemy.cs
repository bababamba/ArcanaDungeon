using System;
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

        int[,] Plr_pos = new int[2, 2];  //0번 인덱스는 실제 플레이어 위치, 1번 인덱스는 마지막으로 본 플레이어 위치

        public bool[,] FOV;

        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //여기에 필드 복사
            e.isTurn = this.isTurn;

            return e;
        }

        public void Update()
        { //★몬스터 알고리즘 확인용 임시 함수, 나중에 꼭 삭제할 것
            if (isTurn > 0)
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
                this.isTurn -= 1;
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

        private void range_attack(int dest_x, int dest_y, int val, bool pierce) { //원거리 공격, 스킬이나 기본 공격 등에서 사용할 것, pierce는 공격 범위 안의 모든 적을 공격하는 관통 공격일 경우 true가 된다
            //이 몬스터의 현재 좌표부터 (dest_x,dest_y)까지 맞닿는 사각형 좌표들을 구해옴   
                List<float[]> result = new List<float[]>();
                if (dest_y-transform.position.y == 0){
                    for (float i=transform.position.x; i<=dest_x; i++){
                    result.Add(new float[2] { i, transform.position.y });
                    }
                }else if (dest_x-transform.position.x == 0){             
                    for (float i=transform.position.y; i<=dest_y; i++){
                    result.Add(new float[2] { transform.position.x, i });
                    }
                }else{         
                    bool mirrored = false;
                    float more_slope, less_slope, slope;
                    if (Math.Abs(dest_x-transform.position.x) > Math.Abs(dest_y-transform.position.y)){
                        more_slope = transform.position.x;
                        less_slope = transform.position.y;
                        slope = Math.Abs(dest_x-transform.position.x) / Math.Abs(dest_y-transform.position.y);
                        mirrored = true;
                    }else{
                        more_slope = transform.position.y;
                        less_slope = transform.position.x;
                        slope = Math.Abs(dest_y-transform.position.y)/ Math.Abs(dest_x-transform.position.x);
                    }
                    less_slope += 0.5f;
                    for (; more_slope<more_slope+(slope/2); more_slope++){ result.Add(mirrored ? new float[2] { more_slope, less_slope-0.5f} : new float[2] { less_slope-0.5f, more_slope}); }
                    while (less_slope+1<=(mirrored ? dest_y : dest_x) & more_slope+slope<=(mirrored ? dest_x : dest_y)){
                        for (; more_slope<more_slope+slope; more_slope++){ result.Add(mirrored ? new float[2] { more_slope, less_slope-0.5f} : new float[2] { less_slope-0.5f, more_slope}); }
                    }
                }
                    
            if (pierce){
                foreach (Thing t in Dungeon.dungeon.currentlevel.enemies){
                    if (result.Contains(new float[2] { t.transform.position.x, t.transform.position.y})){
                        t.HpChange(-val);
                    }
                }
                if (result.Contains(new float[2] { Dungeon.dungeon.Plr.transform.position.x, Dungeon.dungeon.Plr.transform.position.y})){
                    Dungeon.dungeon.Plr.HpChange(-val);
                }
                //★대충 하얀 레이저가 시작점에서 도착점에 1초 정도 걸침, 관통 공격이기 때문에 대상이 없어도 어찌 됐든 발사한다
            }else{
                //Dungeon.dungeon.currentlevel.mobs와 Plr 과 그 좌표들을 비교하여 가장 가까운 대상을 찾아옴
                Thing closest = null;
                int closest_distance = 999;
                foreach (Thing t in Dungeon.dungeon.currentlevel.enemies){
                    if (result.Contains(new float[2] { t.transform.position.x, t.transform.position.y}) & Dungeon.distance_cal(transform, t.transform)<closest_distance){
                        closest = t;
                        closest_distance = Dungeon.distance_cal(transform, t.transform);
                    }
                }
                if (result.Contains(new float[2] { Dungeon.dungeon.Plr.transform.position.x, Dungeon.dungeon.Plr.transform.position.y}) & Dungeon.distance_cal(transform, Dungeon.dungeon.Plr.transform)<closest_distance){
                    closest = Dungeon.dungeon.Plr;  //closest_distance는 closest를 갱신할 때에만 필요하므로 마지막에 검사하는 플레이어 때에는 그 변수를 변경하지 않는다
                }
                if(closest == Dungeon.dungeon.Plr){
                    Dungeon.dungeon.Plr.HpChange(-val);
                    //★대충 하얀색 레이저가 시작점에서 도착점에 1초 정도 걸침
                }else{
                    //★route_pos[0] 좌표로 이동, move()로 이동 관련 부분을 똑 떼어놓고 사용하는 게 나을 것 같다
                }
            }
        }
    }
}