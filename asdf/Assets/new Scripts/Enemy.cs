using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Thing
{
    /*�ڸ��͵��� ��ų�� Action Ŭ���� ������ �ּ� ������ ��
    Action[] actions;   //���� �ൿ ���
    Action[] original_actions;  //������ �ൿ ���, ���� �ൿ ����� ��� �� �迭�� ������ �������ش�
     */

    bool isawaken;
    int move_speed;
    int action_per_turn;
    int[] Plr_pos;  //0�� �ε����� ���� �÷��̾� ��ġ, 1�� �ε����� ���������� �� �÷��̾� ��ġ

    private void Action_reset() { }//��
    private void Vision_research() { }//��Vision_check�� ������ �迭�� Ȱ���� �÷��̾��� ��ǥ Ư��
}
