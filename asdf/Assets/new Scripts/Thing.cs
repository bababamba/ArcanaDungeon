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
    public int cur_pos;    //이 물체의 현재 위치, Level 클래스의 map[]을 좌표처럼 사용한다
    public List<int> route_pos = new List<int>();  //목적지까지의 이동 경로, 이동은 항상 route_pos[0]으로 이동해서 진행된다

    private string[] text;  //물체의 이름과 설명

    private int condition;  //물체가 보유한 상태이상 및 버프를 나타냄, 각각의 효과들은 GameManager에 상수로 보관되어 있음

    public Thing() {
        condition = 0;
    }

    //hp 관련 함수
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

    //block 관련 함수
    public int getblock() {
        return this.block;
    }

    public void blockup(int val) {
        this.block += val;
    }

    public void blockdown(int val) {
        //방어도가 완전히 닳으면 자동으로 체력을 낮추도록 해둔다
        if (this.block >= val)
        {
            this.block -= val;
        }
        else {
            hpdown(val - this.block);
        }
    }

    //이동 관련 함수
    public void move() { }

    public void route_BPS(int destination, bool[] fov)    //넓이 우선 탐색으로 목적지까지의 경로를 route_pos에 저장해주는 함수
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
            //주변 좌표 포함 시 확인해야 하는 것 : passable인가?, level의 length 범위 이내의 숫자인가, prev[i]==null인가, 몬스터의 cur_pos가 아닌가
            for (int ii = 0; ii < 8; ii++)
            {
                int temp2 = checking[i] + dir[ii];
                if ((GameManager.cur_level.map[temp2] & Terrain.passable) != 0 & prev[temp2] == 0 & temp2 != cur_pos & temp2 > 0 & temp2 < GameManager.cur_level.length)
                {
                    checking.Add(temp2);
                    prev[temp2] = checking[i];
                }
            }

            //Plr_pos[0]이랑 같은 좌표인지 확인, 맞으면 prev 배열 쭉 타고올라가면서 route_pos에 저장
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

    //상태이상 처리 관련 함수
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
        //★턴 생략
    }

    public void die() { } //★나중에 자기자신을 map[]에서 삭제하는 정도는 넣어두자
    private void turn() { } 
}
