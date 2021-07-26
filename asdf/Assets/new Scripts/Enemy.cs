using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain = noname.Terrain;

public class Enemy : Thing
{

    bool isawaken;
    int move_speed;
    int action_per_turn;

    int[] Plr_pos = new int[2];  //0번 인덱스는 실제 플레이어 위치, 1번 인덱스는 마지막으로 본 플레이어 위치

    public bool[] FOV;

    public void Update() { //★몬스터 알고리즘 확인용 임시 함수, 나중에 꼭 삭제할 것
        if (Input.GetKeyDown(KeyCode.R)) {
            cur_pos = (int)Mathf.Round(GetComponent<Rigidbody2D>().position.y + 1);
            cur_pos *= -GameManager.cur_level.levelr.Width();
            cur_pos += (int)Mathf.Round(GetComponent<Rigidbody2D>().position.x + 1);
            Vision_research();
        }
    }

    private void Vision_research() {
        FOV = new bool[GameManager.cur_level.length];
        Visionchecker.vision_check(cur_pos % GameManager.cur_level.width, cur_pos / GameManager.cur_level.width, 6, FOV);

        for (int i=0; i<GameManager.cur_level.length; i++) {
            if (FOV[i] & i == GameManager.Plr.cur_pos) {
                Plr_pos[0] = Plr_pos[1] = GameManager.Plr.cur_pos;
                break;
            }

            //몬스터의 FOV에 플레이어가 잡히지 않으면 Plr[0]=-1로 변경해서 발견된 상태가 아님을 표시한다
            if (i == GameManager.cur_level.length - 1) {
                Plr_pos[0] = -1;
            }
        }

        if (Plr_pos[0] != -1)
        {
            route_BPS();
        }

        //★몬스터의 시야 범위를 파랑색으로, 몬스터의 cur_pos는 녹색으로 나타낸다, 당연히 몬스터의 시야 범위를 보여줄 필요가 없으므로 나중에 삭제할 것
        for (int i = 0; i < GameManager.cur_level.length; i++)
        {
            if (FOV[i])
            {
                GameManager.cur_level.temp_gameobjects[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1, 1);
            }
        }
        GameManager.cur_level.temp_gameobjects[cur_pos].GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f, 1);
        //★몬스터에서 플레이어에게로 가는 경로를 붉게 표시한다, 나중에 몬스터의 행동이 코딩되면 삭제할 것
        foreach (int ii in route_pos) {
            GameManager.cur_level.temp_gameobjects[ii].GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
        }
    }

    private void route_BPS() {
        List<int> checking = new List<int>();
        int[] prev = new int[GameManager.cur_level.length];
        int[] dir = new int[] { -1, 1, GameManager.cur_level.width, -GameManager.cur_level.width};

        checking.Add(cur_pos);
        int FOV_true = 0;
        foreach (bool b in FOV) {
            if (b) { FOV_true++; }
        }
        for (int i = 0; i<FOV_true; i++) {
            //동서남북 포함 시 확인해야 하는 것 : passable인가?, level의 length 범위 이내의 숫자인가, prev[i]==null인가, 몬스터의 cur_pos가 아닌가
            for (int ii = 0; ii < 4; ii++) {
                 int temp2 = checking[i] + dir[ii];
                if ((GameManager.cur_level.map[temp2] & Terrain.passable) != 0 & prev[temp2] == 0 & temp2 != cur_pos & temp2 > 0 & temp2 < GameManager.cur_level.length) {
                    checking.Add(temp2);
                    prev[temp2] = checking[i];
                }
            }

            //Plr_pos[0]이랑 같은 좌표인지 확인, 맞으면 prev 배열 쭉 타고올라가면서 route_pos에 저장
            if (checking[i] == Plr_pos[0])
            {
                int temp = checking[i];
                route_pos.Clear();
                while (prev[temp] != 0)
                {
                    route_pos.Add(temp);//Insert(0, temp);
                    temp = prev[temp];
                }
                break;
            }
        }

        return;
    }
}
