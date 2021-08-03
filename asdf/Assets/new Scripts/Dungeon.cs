﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace noname
{
    public class Dungeon : MonoBehaviour
    {

        public const int burnt = 0x01;
        public const int stun = 0x02;

        public GameObject wallTile, floorTile, upStairsTile, downStairsTile;
        public GameObject doorTile;
        public GameObject Player;
        public player Plr;
        public List<Level> levels = new List<Level>();
        public Level currentlevel;
        public bool changed = false;

        private void Start() 
        {
            currentlevel = new RegularLevel();
            currentlevel.Create();
            levels.Add(currentlevel);
            PrintLevel();
            
            Player = Instantiate(Player, new Vector2(0, 0), Quaternion.identity)as GameObject;
            Visionchecker.temp_dungeon = this;//★visionchecker에서 현재 level을 원활하게 가져오기 위해 넣었다, 나중에 더 나은 방법을 발견하면 그걸 사용하자
            Plr = Player.GetComponent<player>();
            Plr.Spawn(this);
            //★Player.transform.position = GameManager.Plr.PlayerPos;
            
        }
        public void Nextlevel()
        {
            Level l = new RegularLevel();
            //기존에 깔린 판 치우기(어차피 레벨 자체에 맵 정보는 저장되어 있으니 상관없음)
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.name == transform.name)
                    continue;
                Destroy(child.gameObject);
            }

            //새로 판 깔기
            l.Create();
            levels.Add(l);
            currentlevel = l;
            PrintLevel();

            Plr.Spawn(this);
            Player.transform.position = Plr.PlayerPos;
        }//여기 판 갈아주세요 (판 치우고 새로 깔아야 한다.)
        public void PrintLevel()
        {
            for (int i = 0; i < currentlevel.width; i++){
                for (int j = 0; j < currentlevel.height; j++){
                    GameObject tileObject;
                    int tile = currentlevel.map[i,j];
                    switch (tile)
                    {
                        case Terrain.EMPTY:
                            continue;
                        case Terrain.GROUND:
                            tileObject = floorTile;
                            break;
                        case Terrain.WALL:
                            tileObject = wallTile;
                            break;
                        case Terrain.STAIRS_UP:
                            tileObject = upStairsTile;
                            break;
                        case Terrain.STAIRS_DOWN:
                            tileObject = downStairsTile;
                            break;
                        case Terrain.DOOR:
                            tileObject = doorTile;
                            break;
                        default:
                            continue;
                    }
                    GameObject newTile = Instantiate(tileObject, new Vector2(i, j), Quaternion.identity) as GameObject;
                    newTile.transform.SetParent(this.transform, false);
                    try
                    {
                        currentlevel.temp_gameobjects[i,j]=newTile;//★나중에 그래픽 표현 방법이랑 좌표 체계 정리해야 한다
                    }
                    catch (Exception e) {
                        Debug.Log(e);
                    }
                }
            }
        }//현재 레벨의 맵 화면상에 출력

        private void Update()
        {
            if (currentlevel.map[(int)Plr.transform.position.x, (int)Plr.transform.position.y] == Terrain.STAIRS_DOWN && changed == false)
            {
                //Nextlevel();
                changed = true;
            }
        }

        public static int distance_cal(Thing a, Thing b)
        {
            //물체의 x좌표값 차이와 y좌표값 차이를 구해서 절댓값을 씌운다.
            int x_gap = Math.Abs((int)(a.transform.position.x - b.transform.position.x));
            int y_gap = Math.Abs((int)(a.transform.position.x - b.transform.position.y));

            //둘을 비교해 더 큰 값을 반환한다, 대각선으로 이동하는 게임 특성 상 그냥 더 큰 쪽이 거리가 된다
            return (x_gap > y_gap ? x_gap : y_gap);
        }


    }
}