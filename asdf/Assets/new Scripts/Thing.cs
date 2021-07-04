using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{
    private int hp;
    private int maxhp;
    private int block;
    private int vision_distance;
    private int cur_pos;    //이 물체의 현재 위치, Level 클래스의 map[]을 좌표처럼 사용한다
    private int[] route_pos;
    private int[] FOV;  //Field Of Vision

    private string[] text;  //물체의 이름과 설명
    //Tag 열거형은 잠시 보류, 방법을 좀 더 고민해보기로 했다

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
            this.hp -= val - this.block;
        }
    }

    public void move() { } 
    private void pre_move() { 
        //★move()는 객체마다 달라지지만 pre_move는 달라지지 않으므로 여기서 코딩돼야 한다
        //1.이동해서 도착할 좌표를 가져온다
        //2.이 객체가 목적지에 가기 위해 거쳐야 할 경로를 BPS 알고리즘으로 탐색한다
        //3.그 경로의 좌표들을 배열로 만들어 route_pos에 저장한다
        //4.move()는 route_pos의 0번 인덱스 좌표로만 이동케 한다
    }

    public void die() { } //★나중에 자기자신을 map[]에서 삭제하는 정도는 넣어두자
    private void turn() { } 
}
