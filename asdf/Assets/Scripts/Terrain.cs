﻿
namespace ArcanaDungeon
{
	public class Terrain
	{
		public const int EMPTY = 0;
		public const int GROUND = 1;
		public const int WALL = 2;

		public const int DOOR = 3;
		public const int DOOR_OPEN = 4;
		public const int DOOR_LOCKED = 5;
		public const int DOOR_HIDDEN = 6;

		public const int STAIRS_DOWN = 7;
		public const int STAIRS_UP = 8;

		public const int WATER = 9;


		public const int passable = 0x01;	//지나갈 수 있는가
		public const int vision_blocking = 0x02;    //시야를 가리는가
		public const int water = 0x04;	//물웅덩이인가

		public static int[] thing_tag = new int[256];
		//아래의 블록은 클래스가 로딩될 때 1번만 실행된다, 생성자의 클래스 버전
		static Terrain(){
			thing_tag[EMPTY] = passable;
			thing_tag[GROUND] = passable;
			thing_tag[WALL] = vision_blocking;

			thing_tag[DOOR] = passable | vision_blocking;
			thing_tag[DOOR_OPEN] = passable;
			thing_tag[DOOR_LOCKED] = vision_blocking;
			thing_tag[DOOR_HIDDEN] = thing_tag[WALL];

			thing_tag[STAIRS_DOWN] = passable;
			thing_tag[STAIRS_UP] = passable;

			thing_tag[WATER] = passable | water;
			
		}

		public int discover(int terr)
		{
			switch (terr)
			{
				case DOOR_HIDDEN:
					return DOOR;
				default:
					return terr;
			}
		}

	}
}