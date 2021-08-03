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
        //물체의 x좌표값 차이와 y좌표값 차이를 구해서 절댓값을 씌운다.
        int x_gap = Math.Abs((int)(a.transform.position.x - b.transform.position.x));
        int y_gap = Math.Abs((int)(a.transform.position.x - b.transform.position.y));

        //둘을 비교해 더 큰 값을 반환한다, 대각선으로 이동하는 게임 특성 상 그냥 더 큰 쪽이 거리가 된다
        return (x_gap > y_gap ? x_gap : y_gap);
    }
}
