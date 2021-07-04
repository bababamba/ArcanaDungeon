using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace noname
{
    public class Dungeon : MonoBehaviour
    {
        public GameObject wallTile, floorTile, upStairsTile, downStairsTile;
        public GameObject doorTile;
        public List<Level> levels = new List<Level>();
        public Level currentlevel;
        public bool changed = false;

        public player Plr;  //★player 클래스에서 Level을 활용하기 위한 임시 변수, 나중에 GameManager와 계층 구조를 정리하면 아마 없애야 할 것이다

        private void Start() {
            currentlevel = new Level();
            currentlevel.Create();
            levels.Add(currentlevel);
            PrintLevel();
            Plr.SetLevel(currentlevel);//★
            Visionchecker.temp_dungeon = this;//★visionchecker에서 현재 level을 원활하게 가져오기 위해 넣었다, 나중에 더 나은 방법을 발견하면 그걸 사용하자
    }
        public void Nextlevel()
        {
            Level l = new Level();
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
        }//여기 판 갈아주세요 (판 치우고 새로 깔아야 한다.)
        public void PrintLevel()
        {
            int pos = 0;
            for (int i = 0; i < currentlevel.height; i++, pos += currentlevel.width)
            {
                for (int j = pos; j < pos + currentlevel.width; j++)
                {
                    GameObject tileObject;
                    int tile = currentlevel.map[j];
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
                    GameObject newTile = Instantiate(tileObject, new Vector2(j - pos - 1, -i - 1), Quaternion.identity) as GameObject;
                    newTile.transform.SetParent(this.transform, false);
                    try
                    {
                        currentlevel.temp_gameobjects.Add(newTile);//★나중에 그래픽 표현 방법이랑 좌표 체계 정리해야 한다
                    }
                    catch (Exception e) {
                        Debug.Log(e);
                    }
                }
            }
        }//현재 레벨의 맵 화면상에 출력

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && changed == false)
            {
                Nextlevel();
                changed = true;
            }
        }
    }
}