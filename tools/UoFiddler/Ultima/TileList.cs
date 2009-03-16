namespace Ultima
{
    public sealed class HuedTileList
    {
        private HuedTile[] m_Tiles;

        public HuedTileList()
        {
            m_Tiles = new HuedTile[8];
            Count = 0;
        }

        public int Count { get; private set; }

        public void Add(short id, short hue, sbyte z)
        {
            if ((Count + 1) > m_Tiles.Length)
            {
                HuedTile[] old = m_Tiles;
                m_Tiles = new HuedTile[old.Length * 2];

                for (int i = 0; i < old.Length; ++i)
                    m_Tiles[i] = old[i];
            }

            m_Tiles[Count++].Set(id, hue, z);
        }

        public HuedTile[] ToArray()
        {
            HuedTile[] tiles = new HuedTile[Count];

            for (int i = 0; i < Count; ++i)
                tiles[i] = m_Tiles[i];

            Count = 0;

            return tiles;
        }
    }

    public sealed class TileList
    {
        private Tile[] m_Tiles;

        public TileList()
        {
            m_Tiles = new Tile[8];
            Count = 0;
        }

        public int Count { get; private set; }

        public void Add(short id, sbyte z)
        {
            if ((Count + 1) > m_Tiles.Length)
            {
                Tile[] old = m_Tiles;
                m_Tiles = new Tile[old.Length * 2];

                for (int i = 0; i < old.Length; ++i)
                    m_Tiles[i] = old[i];
            }

            m_Tiles[Count++].Set(id, z);
        }

        public Tile[] ToArray()
        {
            Tile[] tiles = new Tile[Count];

            for (int i = 0; i < Count; ++i)
                tiles[i] = m_Tiles[i];

            Count = 0;

            return tiles;
        }
    }
}