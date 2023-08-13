﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class IBlock : Block
    {
        private readonly Positions[][] tiles = new Positions[][]
        {
            new Positions[]{new(1,0), new (1,1), new (1,2), new(1,3) },
            new Positions[]{new(0,2), new (1,2), new (2,2), new(3,2) },
            new Positions[]{new(2,0), new (2,1), new (2,2), new(2,3) },
            new Positions[]{new(0,1), new (1,1), new (2,1), new(3,1) },
        };
        public override int Id => 1;

        protected override Positions StartOFFset => new Positions(-1, 3);

        protected override Positions[][] Tiles => tiles;
    }
}
