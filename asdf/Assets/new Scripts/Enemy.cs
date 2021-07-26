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

            //������ FOV�� �÷��̾ ������ ������ Plr[0]=-1�� �����ؼ� �߰ߵ� ���°� �ƴ��� ǥ���Ѵ�
            if (i == GameManager.cur_level.length - 1) {
                Plr_pos[0] = -1;
            }
        }

        if (Plr_pos[0] != -1)
        {
            route_BPS();
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
            //�������� ���� �� Ȯ���ؾ� �ϴ� �� : passable�ΰ�?, level�� length ���� �̳��� �����ΰ�, prev[i]==null�ΰ�, ������ cur_pos�� �ƴѰ�
            for (int ii = 0; ii < 4; ii++) {
                 int temp2 = checking[i] + dir[ii];
                if ((GameManager.cur_level.map[temp2] & Terrain.passable) != 0 & prev[temp2] == 0 & temp2 != cur_pos & temp2 > 0 & temp2 < GameManager.cur_level.length) {
                    checking.Add(temp2);
                    prev[temp2] = checking[i];
                }
            }

            //Plr_pos[0]�̶� ���� ��ǥ���� Ȯ��, ������ prev �迭 �� Ÿ��ö󰡸鼭 route_pos�� ����
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
