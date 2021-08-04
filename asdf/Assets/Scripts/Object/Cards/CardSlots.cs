using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

namespace ArcanaDungeon.cards
{
    public class CardSlots
    {
        private List<Cards> CardSlot;
        private int LimitCardsNum = 10; // ���� �� �ִ� �ִ� ī�� ��  
        private int StartTurnHands = 4;
        public Enemy DetectedEnemy = new Enemy();// �þ� �ڵ� �ϼ��Ǹ� �ޱ�
        public void DrawCards(Deck deck_to_draw, int CardsNum_to_draw) // ���� �ִ� �� ������ ī�� ������ �� ��ŭ ��������
        {
            for(int CardsNum = 0; CardsNum < CardsNum_to_draw; CardsNum++)
            {
                if (CardSlot.Count < LimitCardsNum)
                    CardSlot.Add(deck_to_draw.HandOverCards());
                else
                    break;
            }
        }
        public void UsingCard(int SlotNum)
        {
            CardSlot[SlotNum].UseCard(DetectedEnemy);
        }

        public int StartTurnCardsNum()
        {
            return StartTurnHands;
        }
        public void UpStarHandsNum(int AddedDraw)
        {
            StartTurnHands += AddedDraw;
            if (StartTurnHands > LimitCardsNum)
                StartTurnHands = LimitCardsNum;
        }
    }
}

