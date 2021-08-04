using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon
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
        public int changed = 0;

        public static Dungeon dungeon;

        private void Awake() 
        {
            if (dungeon == null)
            {
                dungeon = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if (dungeon != this)
                    Destroy(this.gameObject);
            }// 오브젝트 싱글턴화. 시작시 할당되는 자기 자신(씬에 있는 그거)만 유일한 dungeon이다.
            currentlevel = new RegularLevel();
            currentlevel.Create();
            levels.Add(currentlevel);
            PrintLevel();
            
            Player = Instantiate(Player, new Vector2(0, 0), Quaternion.identity)as GameObject;
            Plr = Player.GetComponent<player>();
            Plr.Spawn();
            
            
        }
        public void NextLevel()
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

            //내려간 계단 자리 확인
            currentlevel.laststair = new Vector2((int)Plr.transform.position.x, (int)Plr.transform.position.y);

            //새로 판 깔기
            if (levels.IndexOf(currentlevel) == levels.Count - 1)//현재가 마지막층이면, 새 층을 만들어 깐다.
            {
                l.Create();
                levels.Add(l);
                currentlevel = l;
            }
            else//마지막층이 아니면, 이미 있는 다음층을 깐다.
            {
                currentlevel = levels[levels.IndexOf(currentlevel) + 1];
            }
            PrintLevel();

            Plr.Spawn();
            Player.transform.position = Plr.PlayerPos;
        }//여기 판 갈아주세요 (판 치우고 새로 깔아야 한다.)

        public void PrevLevel()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.name == transform.name)
                    continue;
                Destroy(child.gameObject);
            }

            currentlevel = levels[levels.IndexOf(currentlevel) - 1];
            PrintLevel();
            Plr.Spawn(new Vector2(currentlevel.laststair.x + 1, currentlevel.laststair.y));
            Player.transform.position = Plr.PlayerPos;
        }//이전 층으로 갈 때는, 내려갈 때 저장된 계단 위치로 이동한다.
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
            if (currentlevel.map[(int)Plr.transform.position.x, (int)Plr.transform.position.y] == Terrain.STAIRS_DOWN && levels.IndexOf(currentlevel) < 2)
            {
                NextLevel();
            }
            if (currentlevel.map[(int)Plr.transform.position.x, (int)Plr.transform.position.y] == Terrain.STAIRS_UP && levels.IndexOf(currentlevel) > 0)
            {
                PrevLevel();
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