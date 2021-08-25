using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{
    public class Crab : Enemy
    {
        public void Awake()
        {
            this.maxhp = 60;
            this.maxstamina = 80;
            HpChange(this.maxhp);
            StaminaChange(this.maxstamina);

            this.name = "Crab";
        }

        public void HpChange(int val)   //�Դ� 20 �̻��� ���ظ� 1.5��� �޴� ���, �� �̸��� ���ظ� ���ݸ� ����
        {
            if (val > 0)
            {
                base.HpChange(this.maxhp);
            }
            else
            {
                if (val >= 20)
                {
                    base.HpChange((int)Math.Round(val * 1.5f));
                }
                else {
                    base.HpChange((int)Math.Round((float)val * 0.5f));
                }
            }
        }
    }
}
