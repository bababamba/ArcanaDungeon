using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{
    public class Gnole : Enemy
    {

        public void Update()
        { //★몬스터 알고리즘 확인용 임시 함수, 나중에 꼭 삭제할 것
            if (isTurn > 0)
            {
                Vision_research();

                if (Plr_pos[0, 0] != -1)
                {
                    this.range_attack(Plr_pos[0, 0], Plr_pos[0, 1], 10, true);  //★테스트를 위한 임시 코드, 던전 초반부터 10딜 원거리 관통 공격이면 플레이어 죽어나간다 구와아악
                }
                else if (route_pos.Count > 0)
                {
                    transform.position = new Vector2(route_pos[0] % Dungeon.dungeon.currentlevel.width, route_pos[0] / Dungeon.dungeon.currentlevel.width);
                    route_pos.RemoveAt(0);
                }
                this.isTurn -= 1;
            }
        }
    }
}
