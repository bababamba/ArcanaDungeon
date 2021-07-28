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
            turn();
        }
    }

    public void turn() {
        Vision_research();

        if (GameManager.distance_cal(GameManager.Plr, this) <= 1 & Plr_pos[0] != -1) {
            Debug.Log("몬스터가 당신을 공격하려고 합니다. 근데 아직 구현이 안 돼서 못 하네요. 저런!");
        }else if (route_pos.Count > 0)
        {
            Debug.Log("몬스터 위치 / 경로의 0번 : "+cur_pos+" / "+route_pos[0]);
            GetComponent<Rigidbody2D>().position = new Vector2(GetComponent<Rigidbody2D>().position.x + route_pos[0] % GameManager.cur_level.width - cur_pos % GameManager.cur_level.width , GetComponent<Rigidbody2D>().position.y - route_pos[0] / GameManager.cur_level.width + cur_pos / GameManager.cur_level.width );
            route_pos.RemoveAt(0);
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
            route_BPS(Plr_pos[0], FOV);
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
}
