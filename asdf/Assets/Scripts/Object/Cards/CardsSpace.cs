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
            Debug.Log("�̰� �߸� �ȵǴµ�?");
        }
        // ī�忡 �������� ���
    }

    public class AttackCard : Cards
    {
        private int CardDamege = 10; // �⺻ ������

        public void IncreaseDMG(int DmgUp) // ���ݷ� ��ȭ
        {
            CardDamege += DmgUp; 
        }

        public override void UseCard(Enemy enemy)
        {
            enemy.hpdown(CardDamege);
        }
    }
}

