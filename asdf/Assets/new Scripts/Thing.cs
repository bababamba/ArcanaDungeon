using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain = noname.Terrain;

public class Thing : MonoBehaviour
{
    private int hp;
    private int maxhp;
    private int block;
    private int vision_distance;
    public int cur_pos;    //�� ��ü�� ���� ��ġ, Level Ŭ������ map[]�� ��ǥó�� ����Ѵ�
    public List<int> route_pos = new List<int>();  //������������ �̵� ���, �̵��� �׻� route_pos[0]���� �̵��ؼ� ����ȴ�

    private string[] text;  //��ü�� �̸��� ����

    private int condition;  //��ü�� ������ �����̻� �� ������ ��Ÿ��, ������ ȿ������ GameManager�� ����� �����Ǿ� ����

    public Thing() {
        condition = 0;
    }

    //hp ���� �Լ�
    public int gethp() {
        return hp;
    }

    public void hpdown(int val) {
        this.hp -= val;
        if (this.hp < 0) {
            this.die();
        }
    }

    public void hpup(int val) {
        if (this.hp + val > this.maxhp)
        {
            this.hp = this.maxhp;
        }
        else {
            this.hp += val;
        }
    }

    //block ���� �Լ�
    public int getblock() {
        return this.block;
    }

    public void blockup(int val) {
        this.block += val;
    }

    public void blockdown(int val) {
        //���� ������ ������ �ڵ����� ü���� ���ߵ��� �صд�
        if (this.block >= val)
        {
            this.block -= val;
        }
        else {
            hpdown(val - this.block);
        }
    }

    //�̵� ���� �Լ�
    public void move() { }

    public void route_BPS(int destination, bool[] fov)    //���� �켱 Ž������ ������������ ��θ� route_pos�� �������ִ� �Լ�
    {
        List<int> result = new List<int>();
        List<int> checking = new List<int>();
        int[] prev = new int[GameManager.cur_level.length];
        int[] dir = new int[] { -1, -1 + GameManager.cur_level.width, GameManager.cur_level.width, 1 + GameManager.cur_level.width, 1, 1 - GameManager.cur_level.width, -GameManager.cur_level.width, -1 - GameManager.cur_level.width };

        checking.Add(cur_pos);
        int FOV_true = 0;
        foreach (bool b in fov)
        {
            if (b) { FOV_true++; }
        }
        for (int i = 0; i < FOV_true; i++)
        {
            //�ֺ� ��ǥ ���� �� Ȯ���ؾ� �ϴ� �� : passable�ΰ�?, level�� length ���� �̳��� �����ΰ�, prev[i]==null�ΰ�, ������ cur_pos�� �ƴѰ�
            for (int ii = 0; ii < 8; ii++)
            {
                int temp2 = checking[i] + dir[ii];
                if ((GameManager.cur_level.map[temp2] & Terrain.passable) != 0 & prev[temp2] == 0 & temp2 != cur_pos & temp2 > 0 & temp2 < GameManager.cur_level.length)
                {
                    checking.Add(temp2);
                    prev[temp2] = checking[i];
                }
            }

            //Plr_pos[0]�̶� ���� ��ǥ���� Ȯ��, ������ prev �迭 �� Ÿ��ö󰡸鼭 route_pos�� ����
            if (checking[i] == destination)
            {
                int temp = checking[i];
                route_pos.Clear();
                while (prev[temp] != 0)
                {
                    route_pos.Insert(0,temp);
                    temp = prev[temp];
                }
                break;
            }
        }
        
        return;
    }

    //�����̻� ó�� ���� �Լ�
    public void condition_process() {
        if ((this.condition & GameManager.burnt) != 0) {
            burnt_process();
        }
        if ((this.condition & GameManager.stun) != 0) {
            stun_process();
        }
    }
    private void burnt_process() {
        hpdown(this.maxhp / 10);
    }
    private void stun_process() { 
        //���� ����
    }

    public void die() { } //�ڳ��߿� �ڱ��ڽ��� map[]���� �����ϴ� ������ �־����
    private void turn() { } 
}
