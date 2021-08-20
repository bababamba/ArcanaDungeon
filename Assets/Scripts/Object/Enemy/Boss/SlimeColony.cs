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
        public void Awake()
        {
            this.name = "SlimeColony";
            slime = Dungeon.dungeon.Mobs[Array.FindIndex(Dungeon.dungeon.Mobs, m => m.name == "Slime")];
        }

        public void FixedUpdate()
        { //�ڸ��� �˰��� Ȯ�ο� �ӽ� �Լ�, ���߿� �� ������ ��
            if (isTurn > 0)
            {
                Vision_research();

                if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                {
                    Debug.Log("���Ͱ� ����� �����Ϸ��� �մϴ�. �ٵ� ���� ������ �� �ż� �� �ϳ׿�. ����!");
                }
                /*else if(Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                {
                    ThrowSlime();
                    //�������� �ȵǰ�, �þ� ���� �ְ�, ��ٿ� ���� �ƴ� ��쿡 ����.
                }*/
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

        public void DivideSelf(int dmg) // ���� ���ط��� ����ϴ� ü���� ���� ������ ��ȯ. ���� ���� �̻��� ������ �ߵ����� �ʴ´�.
        {
            if(dmg > 20)
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