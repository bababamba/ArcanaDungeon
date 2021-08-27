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
        protected int cooltime;
        public int maxcooltime;

        bool isawaken;  //���� �Լ��� ���� �ʿ������� ���� �����غ���

        protected int[,] Plr_pos = new int[2, 2];  //0�� �ε����� ���� �÷��̾� ��ġ, 1�� �ε����� ���������� �� �÷��̾� ��ġ

        public bool[,] FOV;

        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //���⿡ �ʵ� ����
            e.isTurn = this.isTurn;

            return e;
        }

        public void FixedUpdate()
        {
            if (isTurn > 0)
            {
                if (this.GetStamina() < 20 && this.exhausted == false)
                    this.exhausted = true;
                else if (this.GetStamina() >= 60 && this.exhausted == true)
                    this.exhausted = false;

                Vision_research();
                if (this.exhausted == true)// ���¹̳� ȸ�� ���. �Ϲ������δ� Ư�� ���� ���� �� Ż���� �ɸ���, ���� ��ġ �̻��� ���¹̳����� �޽ĸ� �Ѵ�.
                                           // �׷��� ���� ��ġ���� ȸ���� ����, Ż�� �����̻��� ���ŵǰ�, ������ �ൿ �켱����� �ൿ�� �簳�Ѵ�.
                {
                    this.StaminaChange(20);
                }
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// ���� �Ÿ� ���� �÷��̾ ���� ��, �⺻ ������ �켱���Ѵ�.
                {
                    //Debug.Log(this.name+"��(��) ����� �����մϴ�.");
                    //���⿡ ���ݳ��� �Է�
                    //this.StaminaChange(-20);
                }
                else if (route_pos.Count > 0) // �÷��̾� ����
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                }
                isTurn -= 1;
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

        protected void Vision_research()
        {
            FOV = new bool[Dungeon.dungeon.currentlevel.width, Dungeon.dungeon.currentlevel.height];
            Visionchecker.vision_check((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), 6, FOV);

            Plr_pos[0, 0] = -1; //Plr_pos[0,0]�� -1�� �־�ΰ� �÷��̾ �����ϸ� �� ��ǥ�� �����Ѵ�, ���� �÷��̾ �������� �� �ϸ� -1�� ä�� ���� ������ Ȯ���� �� �ִ�
            for (int i = 0; i < Dungeon.dungeon.currentlevel.width; i++)
            {
                for (int j = 0; j < Dungeon.dungeon.currentlevel.height; j++)
                {
                    //�þ߿� ���̴� ��ġ�ε�, �÷��̾�� ��ǥ���� ������ �ű⿡ �ִ� �� �÷��̾�ϱ� Plr_pos�� �����Ѵ�
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

            //�ڸ����� �þ� ������ �Ķ�������, ������ cur_pos�� ������� ��Ÿ����, �翬�� ������ �þ� ������ ������ �ʿ䰡 �����Ƿ� ���߿� ������ ��
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
            //�ڸ��Ϳ��� �÷��̾�Է� ���� ��θ� �Ӱ� ǥ���Ѵ�, ���߿� ������ �ൿ�� �ڵ��Ǹ� ������ ��
            foreach (int ii in route_pos)
            {
                Dungeon.dungeon.currentlevel.temp_gameobjects[ii % Dungeon.dungeon.currentlevel.width, ii / Dungeon.dungeon.currentlevel.width].GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
            }
        }

        protected void range_attack(int dest_x, int dest_y, int val, bool pierce, bool friendly_fire) { //���Ÿ� ����, pierce�� ���� ���� ���� ��� ���� �����ϴ� ���� ������ ��� true, friendly_fire�� �� �������� �Ʊ��� ���� ������ ��� true(���� 1�� ���� �� ������ ��ġ�� �߻��ϴ� ��ų�� ���� ����)
            //�� ������ ���� ��ǥ���� (dest_x,dest_y)���� �´�� �簢�� ��ǥ���� ���ؿ�
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
                    if (mirrored) {
                        for (float i=more_slope; more_slope < i + (slope / 2); more_slope++) { result.Add(new float[2] { more_slope, less_slope - 0.5f }); }
                        while (less_slope + 1 <= dest_y | more_slope + slope <= dest_x )
                        {
                            for (float i=more_slope; more_slope < i + slope; more_slope++) { result.Add(new float[2] { more_slope, less_slope - 0.5f } ); }
                            less_slope += 1f;
                        }
                    } else {
                        for (float i=more_slope; more_slope < i + (slope / 2); more_slope++) { result.Add(new float[2] { less_slope - 0.5f, more_slope }); }
                        while (less_slope + 1 <= dest_x | more_slope + slope <= dest_y)
                        {
                            for (float i=more_slope; more_slope < i + slope; more_slope++) { result.Add(new float[2] { less_slope - 0.5f, more_slope }); }
                            less_slope += 1f;
                        }
                    }
                }
                
            if (pierce){
                foreach (GameObject t in Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor-1]){
                    if (result.Contains(new float[2] { t.transform.position.x, t.transform.position.y})){
                        t.GetComponent<Enemy>().HpChange(-val);
                    }
                }
                if (result.Contains(new float[2] { Dungeon.dungeon.Plr.transform.position.x, Dungeon.dungeon.Plr.transform.position.y})){
                    Dungeon.dungeon.Plr.HpChange(-val);
                }
                //LineRenderer�� �� �κ��� ���� �׷��� �����̶�� ���Ƶ� �����ϴ�
                Dungeon.dungeon.GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, transform.position.y,-1));
                Dungeon.dungeon.GetComponent<LineRenderer>().SetPosition(1, new Vector3(dest_x, dest_y,-1));
                Dungeon.dungeon.GetComponent<LineRenderer>().SetColors(new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 1f));
            }
            else{
                //Dungeon.dungeon.currentlevel.mobs�� Plr �� �� ��ǥ���� ���Ͽ� ���� ����� ����� ã�ƿ�
                Thing closest = null;
                int closest_distance = 999;
                foreach (GameObject t in Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1])
                {
                    if (result.Contains(new float[2] { t.transform.position.x, t.transform.position.y}) & Dungeon.distance_cal(transform, t.transform)<closest_distance){
                        closest = t.GetComponent<Enemy>();
                        closest_distance = Dungeon.distance_cal(transform, t.transform);
                    }
                }
                if (result.Contains(new float[2] { Dungeon.dungeon.Plr.transform.position.x, Dungeon.dungeon.Plr.transform.position.y}) & Dungeon.distance_cal(transform, Dungeon.dungeon.Plr.transform)<closest_distance){
                    closest = Dungeon.dungeon.Plr;  //closest_distance�� closest�� ������ ������ �ʿ��ϹǷ� �������� �˻��ϴ� �÷��̾� ������ �� ������ �������� �ʴ´�
                }
                if(closest == Dungeon.dungeon.Plr | friendly_fire){
                    closest.HpChange(-val);
                    Dungeon.dungeon.GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
                    Dungeon.dungeon.GetComponent<LineRenderer>().SetPosition(1, new Vector3(dest_x, dest_y, -1));
                    Dungeon.dungeon.GetComponent<LineRenderer>().SetColors(new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 1f));
                }
                else{
                    //��route_pos[0] ��ǥ�� �̵�, move()�� �̵� ���� �κ��� �� ������� ����ϴ� �� ���� �� ����
                }
            }
                
        }

        public override void die()
        {

        }
    }
}