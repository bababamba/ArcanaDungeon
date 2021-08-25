using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;


namespace ArcanaDungeon.Object
{
    public class Slime : Enemy
    {
        public void Awake()
        {
            this.maxhp = 115;
            this.maxstamina = 100;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "Slime";
        }

        public void FixedUpdate()
        {
            if (isTurn > 0)
            {
                Vision_research();

                if (Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) <= 1 & Plr_pos[0, 0] != -1)
                {
                    Debug.Log(this.name+"��(��) ����� �����մϴ�.");
                    Dungeon.dungeon.Plr.HpChange(-10);  //��Floor�� ���� ����Ǵ� ���ݷ��� ������ ����־ �� ������ŭ�� ��ƾ� �Ѵ�
                    Dungeon.dungeon.Plr.condition_add(3, 1);    //�ߵ� 1 �ο�
                }
                else if (route_pos.Count > 0)
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                }
                isTurn -= 1;
            }
        }
    }
}