using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //상태이상 A를 부여할 때는 Thing.condition = Thing.condition | GameManager.A, 상태이상 A를 해제할 때는 Thing.condition = Thing.condition & !GameManager.A, 상태이상 A에 걸렸는지 확인할 때는 if (Thing.condition & GameManager.A != 0) {}
    public const int burnt = 0x01;
    public const int stun = 0x02;

    public static player Plr;
    public static GameObject Plr_object;
    public static noname.Level cur_level;


    public static int distance_cal(Thing a, Thing b) {
        //a의 cur_pos를 cur_level.width로 나눈 몫을 y1, 나눈 나머지를 x1에 저장한다
        int y1 = a.cur_pos / cur_level.width;
        int x1 = a.cur_pos % cur_level.width;
        //b의 cur_pos를 curlevel.width로 나눈 몫을 y2, 나눈 나머지를 x2에 저장한다
        int y2 = b.cur_pos / cur_level.width;
        int x2 = b.cur_pos % cur_level.width;

        //y1에서 y2를 뺀 절대값을 y_gap에 저장한다
        int y_gap = Math.Abs(y1 - y2);
        //x1에서 x2를 뺀 절대값을 x_gap에 저장한다
        int x_gap = Math.Abs(x1 - x2);

        //둘을 비교해 더 큰 값을 반환한다
        return (y_gap > x_gap ? y_gap : x_gap);
    }
}
