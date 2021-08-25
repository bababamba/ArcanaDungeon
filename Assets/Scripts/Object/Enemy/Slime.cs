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
                    Debug.Log(this.name+"이(가) 당신을 공격합니다.");
                    Dungeon.dungeon.Plr.HpChange(-10);  //★Floor에 따라 변경되는 공격력을 변수에 집어넣어서 그 변수만큼만 깎아야 한다
                    Dungeon.dungeon.Plr.condition_add(3, 1);    //중독 1 부여
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