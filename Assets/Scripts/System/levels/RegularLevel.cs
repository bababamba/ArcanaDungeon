using ArcanaDungeon.util;
using System.Collections.Generic;
using System;
using ArcanaDungeon.rooms;
using ArcanaDungeon.painters;
using ArcanaDungeon.Object;
using UnityEngine;
using Random = System.Random;
using Rect = ArcanaDungeon.util.Rect;
using System.Linq;

namespace ArcanaDungeon
{
    public class RegularLevel : Level
    {
        public override void InitRooms()
        {
            levelsize = LevelSize.SMALL;
            rooms = new List<Room>();
            rooms.Add(new UpStairsRoom());
            exitnum = rand.Next(4, 5);
            for (int i = 0; i < exitnum; i++)
                rooms.Add(new DownStairsRoom());

            PlaceRooms();
            levelr = LevelRect();
            MoveRooms();
            map = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j] = Terrain.WALL;
                }
            }
        }

        public override void PlaceRooms()
        {
            rooms[0].SetPosition(0, 0);
            rooms[0].placed = true;
            int radius = (int)levelsize;
            double startangle = rand.Next(0, 360 / exitnum);
            int i = 0;
            foreach (Room d in rooms)
            {
                if (d.GetType() != typeof(DownStairsRoom))
                    continue;

                int x = (int)(radius * 7 * Math.Sin((2 * Math.PI * i / exitnum) + startangle));
                int y = (int)(radius * 7 * Math.Cos((2 * Math.PI * i / exitnum) + startangle));
                d.angle = (2 * Math.PI * i / exitnum) + startangle;
                d.SetPosition(x, y);
                d.placed = true;
                i++;
            }// �ⱸ���� ������ ������ �������� ��ġ�Ѵ�.
            int n = 0;
            while (n < rooms.Count)//���ⱸ�� ������ ����� ��ġ�Ѵ�.
            {
                Room d = rooms[n];
                if (d.GetType() != typeof(DownStairsRoom))
                {
                    n++;
                    continue;
                }//�� ����� ���� �� �ⱸ�� ã�� �д�. �ش� �������� ����� ��ġ ��, �ٽ� �ٸ� �ⱸ�� ã�� ��ġ�ϴ� ���.

                Room ent = rooms[0];
                int num = 0;
                Room room = new EmptyRoom();
                rooms.Add(room);

                while (num < rooms.Count)
                {
                    Room r = rooms[num];

                    if (ent.GetHashCode() == r.GetHashCode() || r.placed == true)
                    {
                        num++;
                        continue;
                    }//���� ��� ���� ���̰ų�, �̹� ��ġ�� ���̸� �ѱ��.

                    r.SetPosition(ent.x, ent.y);
                    int xOrigin = r.x;
                    int yOrigin = r.y;
                    int count = 1;



                    while (CheckOverlap(r))
                    {
                        if (CheckOverlap(r, d))
                        {
                            d.MovePosition(r.x, r.y);
                            rooms.Remove(r);
                            r = d;
                            break;
                        }
                        r.SetPosition(xOrigin + (int)(count * Math.Sin(d.angle)), yOrigin + (int)(count * Math.Cos(d.angle)));
                        count++;
                        if (count > 200)//���࿡ �� ��ħ�̳� ���ѷ��� �̽� �߻���, count ���� �÷��� ��.
                        {
                            Debug.Log("ERR");
                            break;
                        }
                    }

                    count = 0;
                    int max = 0;
                    while (!r.IsNeighbour(ent))
                    {
                        Rect rect = r.Intersect(r, ent);
                        int xDir = 0, yDir = 0;

                        if (rect.Width() < 0)
                            xDir = -rect.Width();
                        else if (rect.Width() > 0 && rect.Width() < 3)
                            xDir = 3 - rect.Width();

                        if (rect.Height() < 0)
                            yDir = -rect.Height();
                        else if (rect.Height() > 0 && rect.Height() < 3)
                            yDir = 3 - rect.Height();

                        if (rect.Width() == 0 && rect.Height() == 0)
                        {
                            //Debug.Log(".");
                            bool hor = rand.Next(2) == 0 ? true : false;
                            if (hor)
                                xDir = 3;
                            else
                                yDir = 3;
                        }
                        if (r.y > ent.y)
                            yDir *= -1;
                        if (r.x > ent.x)
                            xDir *= -1;

                        int index = -1;
                        if (MoveRoom(r, xDir, yDir, out index) && r.IsNeighbour(ent))
                        {
                            //rect = r.Intersect(r, ent);
                            break;
                        }
                        if (max++ > 2)
                        {
                            if (count++ > rooms.Count)
                            {
                                Debug.Log("failed to attach room " + rooms.IndexOf(r) + ", " + rect.Width() + " " + rect.Height());
                                break;
                            }
                            if (index != -1)
                                ent = rooms[index];
                            max = 0;
                            continue;
                        }

                    }
                    //��ġ �� ���� �� �־�� �ϴ��� �˻�
                    r.placed = true;
                    if (!r.IsNeighbour(d) && r.GetType() != typeof(DownStairsRoom))
                    {
                        Room rm = new EmptyRoom();
                        rooms.Add(rm);
                        num++;
                    }
                    ent = r;
                }


                n++;

            }

        }

        public override Vector2 SpawnPoint()
        {
            return new Vector2();
        }
        public override void SpawnMobs()
        {
            

        }
    }
}