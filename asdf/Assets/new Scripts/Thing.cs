using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void pre_move() { 
        //��move()�� ��ü���� �޶������� pre_move�� �޶����� �����Ƿ� ���⼭ �ڵ��ž� �Ѵ�
        //1.�̵��ؼ� ������ ��ǥ�� �����´�
        //2.�� ��ü�� �������� ���� ���� ���ľ� �� ��θ� BPS �˰������� Ž���Ѵ�
        //3.�� ����� ��ǥ���� �迭�� ����� route_pos�� �����Ѵ�
        //4.move()�� route_pos�� 0�� �ε��� ��ǥ�� �̵��� �ϰ� �Ѵ�
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
