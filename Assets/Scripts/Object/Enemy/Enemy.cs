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

        int[,] Plr_pos = new int[2, 2];  //0�� �ε����� ���� �÷��̾� ��ġ, 1�� �ε����� ���������� �� �÷��̾� ��ġ

        public bool[,] FOV;

        public Enemy Copy()
        {
            Enemy e = new Enemy();
            //���⿡ �ʵ� ����
            e.isTurn = this.isTurn;

            return e;
        }

        public void Update()
        { //�ڸ��� �˰��� Ȯ�ο� �ӽ� �Լ�, ���߿� �� ������ ��
            if (isTurn > 0)
            {
                Vision_research();

                if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                {
                    Debug.Log("���Ͱ� ����� �����Ϸ��� �մϴ�. �ٵ� ���� ������ �� �ż� �� �ϳ׿�. ����!");
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

        private void range_attack(int dest_x, int dest_y, int val, bool pierce) { //���Ÿ� ����, ��ų�̳� �⺻ ���� ��� ����� ��, pierce�� ���� ���� ���� ��� ���� �����ϴ� ���� ������ ��� true�� �ȴ�
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
                //�ڴ��� �Ͼ� �������� ���������� �������� 1�� ���� ��ħ, ���� �����̱� ������ ����� ��� ���� �Ƶ� �߻��Ѵ�
            }else{
                //Dungeon.dungeon.currentlevel.mobs�� Plr �� �� ��ǥ���� ���Ͽ� ���� ����� ����� ã�ƿ�
                Thing closest = null;
                int closest_distance = 999;
                foreach (Thing t in Dungeon.dungeon.currentlevel.enemies){
                    if (result.Contains(new float[2] { t.transform.position.x, t.transform.position.y}) & Dungeon.distance_cal(transform, t.transform)<closest_distance){
                        closest = t;
                        closest_distance = Dungeon.distance_cal(transform, t.transform);
                    }
                }
                if (result.Contains(new float[2] { Dungeon.dungeon.Plr.transform.position.x, Dungeon.dungeon.Plr.transform.position.y}) & Dungeon.distance_cal(transform, Dungeon.dungeon.Plr.transform)<closest_distance){
                    closest = Dungeon.dungeon.Plr;  //closest_distance�� closest�� ������ ������ �ʿ��ϹǷ� �������� �˻��ϴ� �÷��̾� ������ �� ������ �������� �ʴ´�
                }
                if(closest == Dungeon.dungeon.Plr){
                    Dungeon.dungeon.Plr.HpChange(-val);
                    //�ڴ��� �Ͼ�� �������� ���������� �������� 1�� ���� ��ħ
                }else{
                    //��route_pos[0] ��ǥ�� �̵�, move()�� �̵� ���� �κ��� �� ������� ����ϴ� �� ���� �� ����
                }
            }
        }
    }
}