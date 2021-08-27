using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;
using System;

namespace ArcanaDungeon.Object
{
    public class SlimeColony : Enemy
    {
        GameObject slime;
        Transform throw_coordinate;

        public void Awake()
        {
            this.maxhp = 300;
            this.maxstamina = 100;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.maxcooltime = 5;  //���ӽ� ��Ÿ��
            this.cooltime = 0;

            this.name = "������ ����";
            slime = Dungeon.dungeon.Mobs[Array.FindIndex(Dungeon.dungeon.Mobs, m => m.name == "Slime")];
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
                else if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)// �÷��̾ �ڽ��� ���� ���� ���� ������, �⺻ ������ ���� �켱���Ѵ�.
                {
                    Debug.Log(this.name + "��(��) ����� �����մϴ�.");
                    Dungeon.dungeon.Plr.HpChange(-10);  //��Floor�� ���� ����Ǵ� ���ݷ��� ������ ����־ �� ������ŭ�� ��ƾ� �Ѵ�
                    this.StaminaChange(-20);
                }
                else if(Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) > 1 & Plr_pos[0, 0] != -1 & this.cooltime <= 0) //�������� �ȵǰ�, �þ� ���� �ְ�, ��ٿ� ���� �ƴ� ��� ������ ��ȯ
                {
                    throw_coordinate = Dungeon.dungeon.Plr.transform;
                    ThrowSlime(throw_coordinate.position);
                    //Dungeon.dungeon.currentlevel.temp_gameobjects[(int)throw_coordinate.position.x, (int)throw_coordinate.position.y].GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1); �̷��� �÷��̾��Ͽ� �þ� ó���ϸ鼭 �ٽ� �ǵ��ƿ´�
                    this.cooltime = this.maxcooltime;
                    this.StaminaChange(-30);
                }
                else if (route_pos.Count > 0)// ������ ������ Ư���� ���� ������ ���� ���� ���� �־�� �ϹǷ� �������� �ʴ´�.
                {
                    //transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    //route_pos.RemoveAt(0);
                }
                this.cooltime -= 1;
                isTurn -= 1;
            }
        }


        public void ThrowSlime(Vector2 pos) // �÷��̾ ���� ���� ���� ���� �þ� ���� ���� ���� ��, ���� �÷��̾� ��ġ�� �׸��� ǥ��. ���� �Ͽ� �ش� ��ġ�� ���� ���� �� ������ ����.
                                 // �÷��̾� ��ġ�� �׸��� ����Ʈ ����. �׸��� ����Ʈ�� ���� �Ͽ� �ش� Ÿ�Ͽ� �÷��̾ ������ ���ظ� �ְ�, ��ó �� Ÿ�Ͽ� �������� �����Ѵ�.
                                 // ������ �ݷδϴ� �� �ൿ�� ��� �� ���� ü���� n%, Ȥ�� ���� ü�·��� �Ҹ��ϰ� �׸�ŭ�� ü���� ���� �������� ������.
        {
            GameObject thrown = Instantiate(slime, pos, Quaternion.identity);
            int health = (int)(this.hp * 0.1);
            if (health < 1)
                health = 1;

            thrown.GetComponent<Slime>().maxhp = (health);
            //Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor].Add(thrown);
        }

        public void HpChange(int val)
        {
            if (val > 0)
            {
                if (this.hp + val > this.maxhp)
                {
                    this.hp = this.maxhp;
                }
                else
                {
                    this.hp += val;
                }
            }
            else if (val != 0)
            {
                this.hp -= val;
                if (val > -15)//15 ������ ���ظ� ������, ���� ���ط��� ������ ü������ ������ �������� ��ȯ�Ѵ�.
                {
                    Debug.Log("������ �������� �Ϻΰ� ������ ���ɴϴ�.");
                    Vector2 pos = new Vector2(this.transform.position.x - 1, this.transform.position.y);
                    GameObject divided = Instantiate(slime, pos, Quaternion.identity);
                    divided.GetComponent<Slime>().maxhp = (int)(val / 2);
                    Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor].Add(divided);
                    //���⿡ ������ ��ȯ
                }
                else
                    Debug.Log("�������� ������ ��������, �̳� ������ ������ϴ�.");
                if (this.hp < 0)
                {
                    this.die();
                }

            }
        }
    }
}