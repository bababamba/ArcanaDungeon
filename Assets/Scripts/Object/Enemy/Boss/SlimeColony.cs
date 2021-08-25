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

            this.maxcooltime = 20;  //★임시 쿨타임
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
                    Debug.Log(this.name + "이(가) 당신을 공격합니다.");
                    Dungeon.dungeon.Plr.HpChange(-10);  //★Floor에 따라 변경되는 공격력을 변수에 집어넣어서 그 변수만큼만 깎아야 한다
                }
                else if(Dungeon.distance_cal(Dungeon.dungeon.Plr.transform, this.transform) > 1 & Plr_pos[0, 0] != -1 & this.cooltime <= 0) //근접공격 안되고, 시야 내에 있고, 쿨다운 중이 아닐 경우 슬라임 소환
                {
                    throw_coordinate = Dungeon.dungeon.Plr.transform;
                    //Dungeon.dungeon.currentlevel.temp_gameobjects[(int)throw_coordinate.position.x, (int)throw_coordinate.position.y].GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1); 이러면 플레이어턴에 시야 처리하면서 다시 되돌아온다
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


        public void ThrowSlime() // 플레이어가 공격 범위 내에 없고 시야 범위 내에 있을 때, 현재 플레이어 위치에 그림자 표시. 다음 턴에 해당 위치에 공격 판정 및 슬라임 생성.
                                 // 플레이어 위치에 그림자 이펙트 생성. 그림자 이펙트는 다음 턴에 해당 타일에 플레이어가 있으면 피해를 주고, 근처 빈 타일에 슬라임을 스폰한다.
                                 // 슬라임 콜로니는 이 행동을 사용 시 현재 체력의 n%, 혹은 일정 체력량을 소모하고 그만큼의 체력을 가진 슬라임을 던진다.
        {

        }

        public void HpChange(int val) // 받은 피해량에 비례하는 체력을 가진 슬라임 소환. 일정 피해 이상을 받으면 발동하지 않는다.
        {
            base.HpChange(val);
            if(val >= 20 & GetHp() > 0)
            {
                Debug.Log("슬라임 일부가 떨어져 나오지만, 이내 힘없이 무너져 내립니다.");
                return;
            }
            GameObject divided = Instantiate(slime, new Vector2(0, 0), Quaternion.identity);
            //divided.GetComponent<Slime>().
            Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor].Add(divided);
        }
    }
}