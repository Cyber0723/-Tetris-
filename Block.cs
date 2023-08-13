using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Tetris
{
    public abstract class Block
    {
        protected abstract Positions[][] Tiles { get; }

        protected abstract Positions StartOFFset { get; }

        public abstract int Id { get; }

        private int rotationState;
        private Positions offset;

        public Block()
        {
            offset = new Positions(StartOFFset.Row, StartOFFset.Column);
        }
        public IEnumerable<Positions> TilePositions()
        {
            foreach(Positions p in Tiles[rotationState])
            {
                yield return new Positions(p.Row + offset.Row, p.Column + offset.Column);
            }
        }
        public void RotateCW()
        {
            rotationState=(rotationState+1)% Tiles.Length;
        }
        public void RotateCCW()
        {
            if(rotationState == 0)
            {
                rotationState= Tiles.Length-1;
            }
            else
            {
                rotationState--;
            }
        }
        public void Move (int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }
        public void Reset()
        {
            rotationState = 0;
            offset.Row=StartOFFset.Row;
            offset.Column=StartOFFset.Column;
        }
    }
}
