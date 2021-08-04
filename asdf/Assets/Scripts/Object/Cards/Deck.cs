using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.cards
{
    public class Deck
    {
        // �Ⱦ� ī��� �� ī�带 ���� ����
        private List<Cards> CardsDeck; // �� ����Ʈ
        private int CardCount; // ���� ī�� ��
        public Deck()
        {
            /*
            SettingFstDeck();
            CardCount = CardsDeck.Count;
            */
        }

        public void SettingFstDeck()// ���� �÷��̾� ���� ����� ������ �ʱ� ī�� ���� 
        {
            for(int i = 0; i < 5; i++)
            {
                AttackCard AtCd = new AttackCard();
                CardsDeck.Add(AtCd);
                CardCount = CardsDeck.Count;
            }
        }

        public Cards HandOverCards()
        {
            int TopOfDeck = CardsDeck.Count - 1; // �� ����Ʈ�� �� �� ī��
            Cards Tempcard = CardsDeck[TopOfDeck]; 
            CardsDeck.RemoveAt(TopOfDeck); // �� ����Ʈ�� �� �� ī�� ����
            return Tempcard;
        }

        public void ChangDeck(List<Cards> UsedDeck)
        {
            CardsDeck = UsedDeck;
        }

        public List<Cards> ShowDeckList()
        {
            return CardsDeck;
        }


    }
}