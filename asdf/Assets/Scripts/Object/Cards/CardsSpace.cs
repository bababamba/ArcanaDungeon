using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class Cards
    {
        public int CardTape = 0; // ī�� ����(ex ����,ȸ��,��ο�)���� �ٸ� ��.
        /*
         * 0 ����Ʈ �� 
         *  X( 1, 2, 3...)  ����ī�� ,�����̻� ���� ī��, ����Ÿ�� ī��� enemy ���� ���ڷ� �޾ƾ��ϴ� ī��
         * 1X(11,12,13...)  ȸ��, ��,����� ������ ����� �ִ� �÷��̾� ��� ī��
         */
        public virtual void UseCard(Enemy enemy)
        {
            
        }
    }

    public class AttackCard : Cards
    {
        private int CardDamege = 10; // �⺻ ������.

        public AttackCard()
        {
            this.CardTape = 1;
        }
        public void IncreaseDMG(int DmgUp) // ���ݷ� ����.
        {
            CardDamege += DmgUp; 
        }

        public override void UseCard(Enemy enemy)
        {
            if (enemy != null)
            {
                Debug.Log("���� �� ü��"+enemy.GetHp());
                enemy.HpChange(-CardDamege);
                Debug.Log("���� �� ü��"+enemy.GetHp());
            }
            else
                Debug.Log("���� ã�� �� �����ϴ�.");

        }
    }
}

