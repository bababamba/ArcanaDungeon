﻿using noname.util;
using System.Collections.Generic;
using System;
using noname.rooms;
using noname.painters;
using UnityEngine;
using Random = System.Random;
using Rect = noname.util.Rect;
using System.Linq;

namespace noname
{
    public enum LevelSize
    {
        SMALL = 4,
        NORMAL = 10,
        LARGE = 16
        //SMALL(4~8), NORMAL(10~14), LARGE(16~20)
    }

    public class Level 
    {
        public List<GameObject> temp_gameobjects;//★시야를 표현하기 위한 임시 게임오브젝트 배열, 나중에 그래픽 표현이나 좌표 체계를 정리할 필요가 있다
        public int width, height, length;
        public int[] map;

        public int entrance;
        public int exit;
        public List<Room> rooms;
        Painter painter;

        public LevelSize levelsize = LevelSize.SMALL;
        public int roomNum;
        public Rect levelr;
        public int exitnum;
        public static Random rand = new Random();

        public bool[] vision_blockings;
        

        public void Create()
        {
            Random random = new Random();
            roomNum = random.Next(0, 6) + (int)levelsize;
            InitRooms();

            foreach (Room r1 in rooms)
            {
                foreach (Room r2 in rooms)
                {
                    if (r1.GetHashCode() == r2.GetHashCode())
                        continue;
                    if (r1.IsNeighbour(r2))
                    {
                        r1.Connect(r2);
                        r2.Connect(r1);
                    }
                }
            }

            PaintRooms();
            foreach (Room r in rooms)
                Debug.Log(r.Info());


            //map[]에 있는 타일 중 시야를 가리는 친구들을 true로 나타내는 배열 vision_blocking을 완성한다, Visionchecker에서 사용한다



            temp_gameobjects = new List<GameObject>();
            vision_blockings = new bool[length];
            for (int i = 0; i < length; i++) {
                vision_blockings[i] = (Terrain.thing_tag[map[i]] & Terrain.vision_blocking) != 0;
            }

        }
        public void InitRooms()
        {
            rooms = new List<Room>();
            rooms.Add(new UpStairsRoom());
            exitnum = rand.Next(4, 5);
            for (int i = 0; i < exitnum; i++)
                rooms.Add(new DownStairsRoom());

            PlaceRooms();
            levelr = LevelRect();
            MoveRooms();
            map = new int[length];
            for (int i = 0; i < length; i++)
                map[i] = Terrain.WALL;
        }
        public void PlaceDoors(Room r)
        {
            foreach(Room room in r.connection.Keys.ToList())
            {
                var door = r.connection[room];
                if (door != null)
                    continue;
                var i = r.Intersect(r, room);
                if (i.Width() == 0)
                    door = new Room.Door(i.x - 1, rand.Next(i.y + 1, i.yMax - 1));
                else
                    door = new Room.Door(rand.Next(i.x + 1, i.xMax - 1), i.y - 1);

                if (r.connection.ContainsKey(room))
                    r.connection[room] = door;
                else
                    r.connection.Add(room, door);
                room.connection[r] = door;
            }
        }
        protected internal virtual void PaintDoors(Room r)
        {
            foreach (var n in r.connection.Keys)
            {
               
                var d = r.connection[n];
                var door = d.x + d.y * width;

                switch (d.type)
                {
                    case Room.Door.Type.EMPTY:
                        map[door] = Terrain.EMPTY;
                        break;
                    case Room.Door.Type.REGULAR:
                        map[door] = Terrain.DOOR;
                        break;
                }
            }
        }
        public void PlaceRooms()
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
            }// 출구방을 정해진 각도와 방향으로 배치한다.
            int n = 0;
            while (n < rooms.Count)
            {
                Room d = rooms[n];
                if (d.GetType() != typeof(DownStairsRoom))
                {
                    n++;
                    continue;
                }
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
                    }//이전 방과 같은 방이거나, 이미 배치된 방이면 넘긴다.

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
                        if (count > 50)//만약에 방 겹침이나 무한루프 이슈 발생시, count 값을 늘려볼 것.
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
                    //배치 후 방을 더 넣어야 하는지 검사
                    r.placed = true;
                    if (!r.IsNeighbour(d) && r.GetType() != typeof(DownStairsRoom))
                    {
                        //Room rm = new EmptyRoom();
                        //rooms.Add(rm);
                        num++;
                    }
                    ent = r;
                }


                n++;

            }

        }
        public bool MoveRoom(Room r, int xDir, int yDir, out int index)
        {
            r.MovePosition(xDir, yDir);
            int i = -1;
            if(CheckOverlap(r, out i))
            {
                r.MovePosition(-xDir, -yDir);
                index = i;
                return false;
            }
            index = -1;
            return true;
        }
        public bool CheckOverlap(Room r1, Room r2)
        {
            Rect rect = r1.Intersect(r1, r2);
            if (rect.Width() > 0 && rect.Height() > 0)
                return true;
            return false;
        }
        public bool CheckOverlap(Room r)
        {
            foreach (Room r1 in rooms)
            {
                if ((r.GetHashCode() == r1.GetHashCode() || !r1.placed) && r1.GetType() != typeof(DownStairsRoom))
                    continue;
                Rect rect = r.Intersect(r, r1);
                if (rect.Width() > 0 && rect.Height() > 0)
                {
                    //Debug.Log("Intersects with Room number " + rooms.IndexOf(r1) + ", Intersect Range : " + rect.Width() + ", " + rect.Height());
                    return true;
                }
            }
            return false;
        }
        public bool CheckOverlap(Room r, out int index)
        {
            foreach (Room r1 in rooms)
            {
                if (r.GetHashCode() == r1.GetHashCode() || !r1.placed)
                    continue;
                Rect rect = r.Intersect(r, r1);
                if (rect.Width() > 0 && rect.Height() > 0)
                {
                    //Debug.Log("Intersects with Room number " + rooms.IndexOf(r1) + ", Intersect Range : " + rect.Width() + ", " + rect.Height());
                    index = rooms.IndexOf(r1);
                    return true;
                }
            }
            index = -1;
            return false;
        }
        public void PaintRooms()
        {
            foreach(Room r in rooms)
            {
                PlaceDoors(r);
                r.Paint(this);
                PaintDoors(r);
            }
        }
        public Rect LevelRect()
        {
            Rect rect = new Rect();
            foreach(Room r in rooms)
            {
                if(r.x < rect.x)
                    rect.x = r.x;
                if (r.y < rect.y)
                    rect.y = r.y;
                if (r.xMax > rect.xMax)
                    rect.xMax = r.xMax;
                if (r.yMax > rect.yMax)
                    rect.yMax = r.yMax;
            }
            rect.x -= 1;
            rect.y -= 1;
            width = rect.Width();
            height = rect.Height();
            length = width * height;
            return rect;
        }
        public void MoveRooms()
        {
            int xDir = -levelr.x;
            int yDir = -levelr.y;

            levelr.SetPosition(-1, -1);
            foreach(Room r in rooms)
            {
                r.MovePosition(xDir, yDir);
            }
        }   
    }
   
}