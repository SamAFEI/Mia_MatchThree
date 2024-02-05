using System.Collections.Generic;

namespace Assets.Scripts
{
    public class TileCompare : IEqualityComparer<Tile>
    {
        public bool Equals(Tile x, Tile y)
        {
            return x.Index == y.Index;
        }

        public int GetHashCode(Tile obj)
        {
            return obj.Index.GetHashCode();
        }
    }
}
