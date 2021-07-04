using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Thing
{
    /*★몬스터들의 스킬인 Action 클래스 제작후 주석 제거할 것
    Action[] actions;   //현재 행동 목록
    Action[] original_actions;  //원래의 행동 목록, 현재 행동 목록이 비면 이 배열을 복사해 리필해준다
     */

    bool isawaken;
    int move_speed;
    int action_per_turn;
    int[] Plr_pos;  //0번 인덱스는 실제 플레이어 위치, 1번 인덱스는 마지막으로 본 플레이어 위치

    private void Action_reset() { }//★
    private void Vision_research() { }//★Vision_check가 전해준 배열을 활용해 플레이어의 좌표 특정
}
