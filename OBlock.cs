using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class OBlock : Block
    {
        private readonly Positions[][] tiles = new Positions[][]
        {
            new Positions[] {new(0,0), new(0,1), new(1,0), new(1,1)}

        };

        public override int Id => 4;

        protected override Positions StartOFFset => new Positions(0, 4);

        protected override Positions[][] Tiles => tiles;
        
    }
}
