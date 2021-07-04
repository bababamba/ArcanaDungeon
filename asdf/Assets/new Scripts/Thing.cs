using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{
    private int hp;
    private int maxhp;
    private int block;
    private int vision_distance;
    private int cur_pos;    //�� ��ü�� ���� ��ġ, Level Ŭ������ map[]�� ��ǥó�� ����Ѵ�
    private int[] route_pos;
    private int[] FOV;  //Field Of Vision

    private string[] text;  //��ü�� �̸��� ����
    //Tag �������� ��� ����, ����� �� �� ����غ���� �ߴ�

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
            this.hp -= val - this.block;
        }
    }

    public void move() { } 
    private void pre_move() { 
        //��move()�� ��ü���� �޶������� pre_move�� �޶����� �����Ƿ� ���⼭ �ڵ��ž� �Ѵ�
        //1.�̵��ؼ� ������ ��ǥ�� �����´�
        //2.�� ��ü�� �������� ���� ���� ���ľ� �� ��θ� BPS �˰������� Ž���Ѵ�
        //3.�� ����� ��ǥ���� �迭�� ����� route_pos�� �����Ѵ�
        //4.move()�� route_pos�� 0�� �ε��� ��ǥ�θ� �̵��� �Ѵ�
    }

    public void die() { } //�ڳ��߿� �ڱ��ڽ��� map[]���� �����ϴ� ������ �־����
    private void turn() { } 
}
