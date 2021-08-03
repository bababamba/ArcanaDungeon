using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�����̻� A�� �ο��� ���� Thing.condition = Thing.condition | GameManager.A, �����̻� A�� ������ ���� Thing.condition = Thing.condition & !GameManager.A, �����̻� A�� �ɷȴ��� Ȯ���� ���� if (Thing.condition & GameManager.A != 0) {}
    public const int burnt = 0x01;
    public const int stun = 0x02;

    public static player Plr;
    public static GameObject Plr_object;
    public static noname.Level cur_level;


    public static int distance_cal(Thing a, Thing b) {
        //��ü�� x��ǥ�� ���̿� y��ǥ�� ���̸� ���ؼ� ������ �����.
        int x_gap = Math.Abs((int)(a.transform.position.x - b.transform.position.x));
        int y_gap = Math.Abs((int)(a.transform.position.x - b.transform.position.y));

        //���� ���� �� ū ���� ��ȯ�Ѵ�, �밢������ �̵��ϴ� ���� Ư�� �� �׳� �� ū ���� �Ÿ��� �ȴ�
        return (x_gap > y_gap ? x_gap : y_gap);
    }
}
