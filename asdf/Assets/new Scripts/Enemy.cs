using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain = noname.Terrain;

public class Enemy : Thing
{

    bool isawaken;
    int move_speed;
    int action_per_turn;

    int[,] Plr_pos = new int[2,2];  //0�� �ε����� ���� �÷��̾� ��ġ, 1�� �ε����� ���������� �� �÷��̾� ��ġ

    public bool[,] FOV;

    public void Update() { //�ڸ��� �˰��� Ȯ�ο� �ӽ� �Լ�, ���߿� �� ������ ��
        if (Input.GetKeyDown(KeyCode.R)) {
            turn();
        }
    }

    public void turn() {
        Vision_research();

        if (GameManager.distance_cal(GameManager.Plr, this) <= 1 & Plr_pos[0,0] != -1) {
            Debug.Log("���Ͱ� ����� �����Ϸ��� �մϴ�. �ٵ� ���� ������ �� �ż� �� �ϳ׿�. ����!");
        }else if (route_pos.Count > 0)
        {
            transform.position = new Vector2(route_pos[0] % GameManager.cur_level.width, route_pos[0] / GameManager.cur_level.width);
            route_pos.RemoveAt(0);
        }
    }

    private void Vision_research() {
        FOV = new bool[GameManager.cur_level.width, GameManager.cur_level.height];
        Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);

        Plr_pos[0, 0] = -1; //Plr_pos[0,0]�� -1�� �־�ΰ� �÷��̾ �����ϸ� �� ��ǥ�� �����Ѵ�, ���� �÷��̾ �������� �� �ϸ� -1�� ä�� ���� ������ Ȯ���� �� �ִ�
        for (int i=0; i<GameManager.cur_level.width; i++)
        {
            for (int j = 0; j < GameManager.cur_level.height; j++)
            {
                //�þ߿� ���̴� ��ġ�ε�, �÷��̾�� ��ǥ���� ������ �ű⿡ �ִ� �� �÷��̾�ϱ� Plr_pos�� �����Ѵ�
                if (FOV[i,j] & i == GameManager.Plr.transform.position.x & j== GameManager.Plr.transform.position.y)
                {
                    Plr_pos[0, 0] = Plr_pos[1, 0] = (int)GameManager.Plr.transform.position.x;
                    Plr_pos[0, 1] = Plr_pos[1, 1] = (int)GameManager.Plr.transform.position.y;
                    break;
                }
            }
        }

        if (Plr_pos[0,0] != -1)
        {
            route_BFS(Plr_pos[0,0], Plr_pos[0,1], FOV);
        }

        //�ڸ����� �þ� ������ �Ķ�������, ������ cur_pos�� ������� ��Ÿ����, �翬�� ������ �þ� ������ ������ �ʿ䰡 �����Ƿ� ���߿� ������ ��
        for (int i = 0; i < GameManager.cur_level.width; i++)
        {
            for (int j = 0; j < GameManager.cur_level.height; j++)
            {
                if (FOV[i,j])
                {
                    GameManager.cur_level.temp_gameobjects[i, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1, 1);
                }
            }
        }
        GameManager.cur_level.temp_gameobjects[(int)transform.position.x, (int)transform.position.y].GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f, 1);
        //�ڸ��Ϳ��� �÷��̾�Է� ���� ��θ� �Ӱ� ǥ���Ѵ�, ���߿� ������ �ൿ�� �ڵ��Ǹ� ������ ��
        foreach (int ii in route_pos) {
            GameManager.cur_level.temp_gameobjects[ii%GameManager.cur_level.width, ii/GameManager.cur_level.width].GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
        }
    }
}
