using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{
    public class Gnole : Enemy
    {

        public void Update()
        { //�ڸ��� �˰��� Ȯ�ο� �ӽ� �Լ�, ���߿� �� ������ ��
            if (isTurn > 0)
            {
                Vision_research();

                if (Plr_pos[0, 0] != -1)
                {
                    this.range_attack(Plr_pos[0, 0], Plr_pos[0, 1], 10, true);  //���׽�Ʈ�� ���� �ӽ� �ڵ�, ���� �ʹݺ��� 10�� ���Ÿ� ���� �����̸� �÷��̾� �׾���� ���;ƾ�
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
