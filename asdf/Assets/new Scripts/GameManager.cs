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

}
