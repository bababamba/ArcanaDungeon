using System.Collections;
using noname.util;
using System.Collections.Generic;
using noname.painters;

namespace noname.rooms
{
    public class DownStairsRoom : Room
    {
        public DownStairsRoom()
        {
            SizePicker sp = new SizePicker();
            width = 4; // sp.Pick(MINROOMSIZE + 1, MAXROOMSIZE - 2);
            height = 4; // sp.Pick(MINROOMSIZE + 1, MAXROOMSIZE - 2);
            DefaultSet();
        }
        public override void Paint(Level l)
        {
            DownStairRoomPainter erg = new DownStairRoomPainter();
            erg.Paint(l, this);
        }
    }
}