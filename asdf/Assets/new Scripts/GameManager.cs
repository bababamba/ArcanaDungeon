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
        //a�� cur_pos�� cur_level.width�� ���� ���� y1, ���� �������� x1�� �����Ѵ�
        int y1 = a.cur_pos / cur_level.width;
        int x1 = a.cur_pos % cur_level.width;
        //b�� cur_pos�� curlevel.width�� ���� ���� y2, ���� �������� x2�� �����Ѵ�
        int y2 = b.cur_pos / cur_level.width;
        int x2 = b.cur_pos % cur_level.width;

        //y1���� y2�� �� ���밪�� y_gap�� �����Ѵ�
        int y_gap = Math.Abs(y1 - y2);
        //x1���� x2�� �� ���밪�� x_gap�� �����Ѵ�
        int x_gap = Math.Abs(x1 - x2);

        //���� ���� �� ū ���� ��ȯ�Ѵ�
        return (y_gap > x_gap ? y_gap : x_gap);
    }
}
