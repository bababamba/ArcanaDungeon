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


        public void HpChange(int val) //�Դ� 20 �̻��� ���ظ� 1.5��� �޴� ���, �� �̸��� ���ظ� ���ݸ� ����
        {
            if (val > 0)
            {
                if (this.hp + val > this.maxhp)
                {
                    this.hp = this.maxhp;
                }
                else
                {
                    this.hp += val;
                }
            }
            else
            {
                if (val <= -20)
                {
                    this.hp -= (int)(val * 1.5);
                    if (this.hp < 0)
                    {
                        this.die();
                    }
                }
                else
                {
                    this.hp -= (int)(val * 0.5);
                    if (this.hp < 0)
                    {
                        this.die();
                    }
                }
            }
        }
    }
}
