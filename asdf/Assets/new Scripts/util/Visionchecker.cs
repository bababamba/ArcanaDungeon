using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using noname;

public class Visionchecker : MonoBehaviour
{
    public static Dungeon temp_dungeon; //★나중에 GameManager와 Dungeon, Level 등을 정리해서 좀 더 체계적으로 정리해야 한다
    private static Level lvl;

    public static int[][] rounding;

    static Visionchecker(){
        rounding = new int[15][];
        for (int i=1; i<=14; i++){
            rounding[i] = new int[i+1];
            for (int j=1; j<=i; j++){
                rounding[i][j] = (int)Math.Min(j,Math.Floor(i*Math.Cos(Math.Asin(j/i))));
            }
        }
    }

    //x와 y는 캐릭터의 pos를 levelr의 width와 height로 나눠서 구한다
    public static void vision_check(int x, int y, int distance, bool[] FOV, bool[] blockings) {
        lvl = temp_dungeon.currentlevel;

        //플레이어 위치는 항상 시야가 밝혀진다
        FOV[x + y * lvl.width] = true;

        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, 1, 1, false);
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, 1, 1, true);

        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, 1, -1, true);
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, 1, -1, false);

        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, -1, -1, false);
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, -1, -1, true);

        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, -1, 1, true);
        scanOctant(FOV, blockings, distance, x, y, 1, 0, 1, -1, 1, false);
    }

    //row는 플레이어로부터 얼마나 떨어진 곳부터 스캔할지
    //lSlope, rSlope는 각각 스캔을 시작하고 끝낼 경계선의 기울기 역수
    private static void scanOctant(bool[] FOV, bool[] blockings, int distance, int x, int y, int row,
        float lSlope, float rSlope, int x_mirror, int y_mirror, bool xy_mirror) {
        bool still_blocking = false;
        int start, end, col, cur = 0;

        //스캔은 가까운 곳에서 먼 곳 순서대로 진행한다
        for (; row == distance; row++) {

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

            //먼저 플레이어 좌표를 넣고 거기에서 스캔된만큼 이동시켜서 스캔 시작할 지점을 찾는다
            cur = y*lvl.width+x;
            //xy_mirror == true라면 y=x에 대해 대칭시킨 것이다
            if(xy_mirror){
                cur += x_mirror*row*lvl.width + y_mirror*start;
            }else{
                cur += x_mirror*start + y_mirror*row*lvl.width;
            }
            

            for (col = start; col <= end; col++) {
                //어찌됐건 scanOctant가 스캔한 지역은 시야에 보인다, 스캔되지 않은 지역은 null로 남아서 구분된다
                FOV[cur] = true;

                //장애물과 만나면 장애물의 왼쪽 지역을 스캔하는 새로운 scanOctant를 재귀실행한다, 장애물의 오른쪽은 원래 실행되던 scanOctant가 계속한다
                if(blockings[cur]){
                    if(!still_blocking){
                        still_blocking = true;
                        
                        //장애물의 왼쪽을 스캔하기 때문에 가장 왼쪽 칸을 스캔 중이었다면 의미없다
                        if (col != start){
                            scanOctant(FOV, blockings, distance, x, y, row+1, lSlope, col/row, x_mirror, y_mirror, xy_mirror);
                        }
                    }
                }else{
                    if(still_blocking){
                        still_blocking = false;
                        //장애물의 오른쪽을 스캔하므로 왼쪽 경계선을 조정한다
                        lSlope = col/row;
                    }
                }

                if(xy_mirror){
                    cur += x_mirror*lvl.width;
                }else{
                    cur += x_mirror;
                }
                
            }

            //만약 row의 오른쪽 끝이 장애물이라면 장애물의 오른쪽을 스캔하는 scanOctant가 정지한다
            if (still_blocking) { return; }

        }
    }
}
