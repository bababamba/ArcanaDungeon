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

            this.maxcooltime = 20;  //���ӽ� ��Ÿ��
            this.cooltime = 0;

            this.name = "SlimeColony";
            slime = Dungeon.dungeon.Mobs[Array.FindIndex(Dungeon.dungeon.Mobs, m => m.name == "Slime")];
        }

        public void FixedUpdate()
        {
            if (isTurn > 0)
            {
                Vision_research();

                if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                {
                    Debug.Log(this.name + "��(��) ����� �����մϴ�.");
                    Dungeon.dungeon.Plr.HpChange(-10);  //��Floor�� ���� ����Ǵ� ���ݷ��� ������ ����־ �� ������ŭ�� ��ƾ� �Ѵ�
                }
                else if(Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) > 1 & Plr_pos[0, 0] != -1 & this.cooltime <= 0) //�������� �ȵǰ�, �þ� ���� �ְ�, ��ٿ� ���� �ƴ� ��� ������ ��ȯ
                {
                    throw_coordinate = Dungeon.dungeon.Plr.transform;
                    //Dungeon.dungeon.currentlevel.temp_gameobjects[(int)throw_coordinate.position.x, (int)throw_coordinate.position.y].GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1); �̷��� �÷��̾��Ͽ� �þ� ó���ϸ鼭 �ٽ� �ǵ��ƿ´�
                    this.cooltime = this.maxcooltime;
                }
                else if (route_pos.Count > 0)
                {
                    //transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    //route_pos.RemoveAt(0);
                }
                isTurn -= 1;
            }
        }


        public void ThrowSlime() // �÷��̾ ���� ���� ���� ���� �þ� ���� ���� ���� ��, ���� �÷��̾� ��ġ�� �׸��� ǥ��. ���� �Ͽ� �ش� ��ġ�� ���� ���� �� ������ ����.
                                 // �÷��̾� ��ġ�� �׸��� ����Ʈ ����. �׸��� ����Ʈ�� ���� �Ͽ� �ش� Ÿ�Ͽ� �÷��̾ ������ ���ظ� �ְ�, ��ó �� Ÿ�Ͽ� �������� �����Ѵ�.
                                 // ������ �ݷδϴ� �� �ൿ�� ��� �� ���� ü���� n%, Ȥ�� ���� ü�·��� �Ҹ��ϰ� �׸�ŭ�� ü���� ���� �������� ������.
        {

        }

        public void HpChange(int val) // ���� ���ط��� ����ϴ� ü���� ���� ������ ��ȯ. ���� ���� �̻��� ������ �ߵ����� �ʴ´�.
        {
            base.HpChange(val);
            if(val >= 20 & GetHp() > 0)
            {
                Debug.Log("������ �Ϻΰ� ������ ��������, �̳� ������ ������ �����ϴ�.");
                return;
            }
            GameObject divided = Instantiate(slime, new Vector2(0, 0), Quaternion.identity);
            //divided.GetComponent<Slime>().
            Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor].Add(divided);
        }
    }
}