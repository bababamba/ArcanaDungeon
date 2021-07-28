using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain = noname.Terrain;

public class Enemy : Thing
{

    bool isawaken;
    int move_speed;
    int action_per_turn;

    int[] Plr_pos = new int[2];  //0�� �ε����� ���� �÷��̾� ��ġ, 1�� �ε����� ���������� �� �÷��̾� ��ġ

    public bool[] FOV;

    public void Update() { //�ڸ��� �˰��� Ȯ�ο� �ӽ� �Լ�, ���߿� �� ������ ��
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
            Debug.Log("���Ͱ� ����� �����Ϸ��� �մϴ�. �ٵ� ���� ������ �� �ż� �� �ϳ׿�. ����!");
        }else if (route_pos.Count > 0)
        {
            Debug.Log("���� ��ġ / ����� 0�� : "+cur_pos+" / "+route_pos[0]);
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

            //������ FOV�� �÷��̾ ������ ������ Plr[0]=-1�� �����ؼ� �߰ߵ� ���°� �ƴ��� ǥ���Ѵ�
            if (i == GameManager.cur_level.length - 1) {
                Plr_pos[0] = -1;
            }
        }

        if (Plr_pos[0] != -1)
        {
            route_BPS(Plr_pos[0], FOV);
        }

        //�ڸ����� �þ� ������ �Ķ�������, ������ cur_pos�� ������� ��Ÿ����, �翬�� ������ �þ� ������ ������ �ʿ䰡 �����Ƿ� ���߿� ������ ��
        for (int i = 0; i < GameManager.cur_level.length; i++)
        {
            if (FOV[i])
            {
                GameManager.cur_level.temp_gameobjects[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1, 1);
            }
        }
        GameManager.cur_level.temp_gameobjects[cur_pos].GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f, 1);
        //�ڸ��Ϳ��� �÷��̾�Է� ���� ��θ� �Ӱ� ǥ���Ѵ�, ���߿� ������ �ൿ�� �ڵ��Ǹ� ������ ��
        foreach (int ii in route_pos) {
            GameManager.cur_level.temp_gameobjects[ii].GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
        }
    }
}
