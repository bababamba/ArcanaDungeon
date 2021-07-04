using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using noname;

//http://www.roguebasin.com/index.php?title=FOV_using_recursive_shadowcasting <<< ���� �߿��ϴ� �ʵ� �ʵ� ���ʵ�

public class Visionchecker : MonoBehaviour
{
    public static Dungeon temp_dungeon; //�ڳ��߿� GameManager�� Dungeon, Level ���� �����ؼ� �� �� ü�������� �����ؾ� �Ѵ�
    private static Level lvl;

    public static int[][] rounding;

    private static void temp_Visionchecker(){   //���� �Լ��� �� 1���� ����Ǿ� �ϴµ� static���θ� ����� Ŭ������ ���� �����ڸ� ����� ���� ����, ���߿� ����� ã��
        rounding = new int[15][];
        for (int i=1; i<=14; i++){
            rounding[i] = new int[i+1];
            for (int j=1; j<=i; j++){
                rounding[i][j] = (int)Math.Min(j,Math.Floor(i*Math.Cos(Math.Asin((double)j/(double)i))));
            }
        }
    }

    //x�� y�� ĳ������ pos�� levelr�� width�� ������ ���Ѵ�
    public static void vision_check(int x, int y, int distance, bool[] FOV, bool[] blockings) {
        temp_Visionchecker();//�ڻ�� �̰� �Ź� ����Ǵ� �� �ƴ϶� 1���� ����Ǿ� �Ѵ�

        lvl = temp_dungeon.currentlevel;

        //�÷��̾� ��ġ�� �׻� �þ߰� ��������
        FOV[x + y * lvl.width] = true;

        //scanOctant�� 8��е� ����ŭ �þ߸� Ž���Ѵ�
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, 1, 1, false); //������ �Ʒ�, ����
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, 1, 1, true);    //������ �Ʒ�, ������

        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, 1, -1, true);   //������ ��, ������
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, 1, -1, false);    //������ ��, ����

        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, -1, -1, false);   //���� ��, ������
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, -1, -1, true);  //���� ��, ����

        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, -1, 1, true);   //���� �Ʒ�, ���� 
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, -1, 1, false);    //���� �Ʒ�, ������
    }

    //row�� �÷��̾�κ��� �󸶳� ������ ������ ��ĵ����
    //lSlope, rSlope�� ���� ��ĵ�� �����ϰ� ���� ��輱�� ���� ����
    //x_mirro�� y_mirror�� ���� x��ǥ�� y��ǥ�� ��ȣ�� �ٲ��� �ǹ��ϸ�, xy_mirror�� y=x Ȥ�� y=-x�� ���� ��Ī�� �ǹ��Ѵ�
    private static void scanOctant(bool[] FOV, bool[] blockings, int distance, int x, int y, int row,
        double lSlope, double rSlope, int x_mirror, int y_mirror, bool xy_mirror) {
        bool still_blocking = false;
        int start, end, col, cur = 0;

        //��ĵ�� ����� ������ �� �� ������� �����Ѵ�
        for (; row <= distance; row++) {

            if (lSlope == 0)
            {
                start = 0;
            }
            else {
                start = (int)Math.Ceiling(row * lSlope);
            }

            if (rSlope == 1)
            {
                end = rounding[distance][row];
            }
            else {
                end = Math.Min(rounding[distance][row], (int)Math.Floor(row * rSlope));
            }

            //���� �÷��̾� ��ǥ�� �ְ� �ű⿡�� ��ĵ�� �Ϸ�� row�� �ǳʶپ �̹� ��ĵ�� ������ ������ ã�´�
            cur = y*lvl.width+x;

            //xy_mirror == true��� y=x Ȥ�� y=-x�� ���� ��Ī��Ų ���̴�
            if(xy_mirror){
                cur += x_mirror*row + y_mirror*start * lvl.width;
            }else{
                cur += x_mirror*start + y_mirror*row*lvl.width;
            }

            for (col = start; col <= end; col++) {
                //����ư� scanOctant�� ��ĵ�� ������ �þ߿� ���δ�, ��ĵ���� ���� ������ null�� ���Ƽ� ���еȴ�
                FOV[cur] = true;

                //��ֹ��� ������ ��ֹ��� ���� ������ ��ĵ�ϴ� ���ο� scanOctant�� ��ͽ����Ѵ�, ��ֹ��� �������� ���� ����Ǵ� scanOctant�� ����Ѵ�
                if(blockings[cur]){
                    if(!still_blocking){
                        still_blocking = true;
                        
                        //��ֹ��� ������ ��ĵ�ϱ� ������ ���� ���� ĭ�� ��ĵ ���̾��ٸ� �ǹ̾���
                        if (col != start){
                            //��ֹ��� ������ ��ĵ�ҷ��� ��ֹ��� 4�� ������ �߿� �÷��̾�Լ� row�� �ְ� col�� ����� ���� ���ο� ��輱���� ���� �Ѵ�, ���� rSlope ��꿡 row�� +1�� �Ѵ�
                            scanOctant(FOV, blockings, distance, x, y, row+1, lSlope, (double)col/(double)(row+1), x_mirror, y_mirror, xy_mirror);
                        }
                    }
                }else{
                    if(still_blocking){
                        still_blocking = false;
                        //��ֹ��� �������� ��ĵ�ϹǷ� ���� ��輱�� �����Ѵ�
                        lSlope = col/row;
                    }
                }

                if(xy_mirror){
                    cur += y_mirror*lvl.width;
                }else{
                    cur += x_mirror;
                }
                
            }

            //���� row�� ������ ���� ��ֹ��̶�� ��ֹ��� �������� ��ĵ�ϴ� scanOctant�� �����Ѵ�
            if (still_blocking) { break ; }

        }
    }
}
