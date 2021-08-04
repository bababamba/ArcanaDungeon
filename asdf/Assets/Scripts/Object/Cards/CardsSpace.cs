using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class Cards
    {
        public virtual void UseCard(Enemy enemy)
        {
            Debug.Log("이게 뜨면 안되는데?");
        }
        // 카드에 공통정인 요소
    }

    public class AttackCard : Cards
    {
        private int CardDamege = 10; // 기본 데미지

        public void IncreaseDMG(int DmgUp) // 공격력 강화
        {
            CardDamege += DmgUp; 
        }

        public override void UseCard(Enemy enemy)
        {
            enemy.hpdown(CardDamege);
        }
    }
}

