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

        public void Add(ushort id, short hue, sbyte z)
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

        public void Add(ushort id, sbyte z)
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
        public void Add(ushort id, sbyte z, sbyte flag)
        {
            if ((Count + 1) > m_Tiles.Length)
            {
                Tile[] old = m_Tiles;
                m_Tiles = new Tile[old.Length * 2];

                for (int i = 0; i < old.Length; ++i)
                    m_Tiles[i] = old[i];
            }

            m_Tiles[Count++].Set(id, z, flag);
        }

        public Tile[] ToArray()
        {
            Tile[] tiles = new Tile[Count];

            for (int i = 0; i < Count; ++i)
                tiles[i] = m_Tiles[i];

            Count = 0;

            return tiles;
        }

        public Tile Get(int i)
        {
            return m_Tiles[i];
        }

        public void Set(int i,ushort id, sbyte z)
        {
            if (i < Count)
                m_Tiles[i].Set(id,z);
        }

        public void Set(int i, ushort id, sbyte z, sbyte flag)
        {
            if (i < Count)
                m_Tiles[i].Set(id, z, flag);
        }
    }

    public sealed class MTileList
    {
        private MTile[] m_Tiles;

        public MTileList()
        {
            m_Tiles = new MTile[8];
            Count = 0;
        }

        public int Count { get; private set; }

        public void Add(ushort id, sbyte z)
        {
            if ((Count + 1) > m_Tiles.Length)
            {
                MTile[] old = m_Tiles;
                m_Tiles = new MTile[old.Length * 2];

                for (int i = 0; i < old.Length; ++i)
                    m_Tiles[i] = old[i];
            }

            m_Tiles[Count++].Set(id, z);
        }
        public void Add(ushort id, sbyte z, sbyte flag)
        {
            if ((Count + 1) > m_Tiles.Length)
            {
                MTile[] old = m_Tiles;
                m_Tiles = new MTile[old.Length * 2];

                for (int i = 0; i < old.Length; ++i)
                    m_Tiles[i] = old[i];
            }

            m_Tiles[Count++].Set(id, z, flag);
        }

        public MTile[] ToArray()
        {
            MTile[] tiles = new MTile[Count];

            for (int i = 0; i < Count; ++i)
                tiles[i] = m_Tiles[i];

            Count = 0;

            return tiles;
        }

        public MTile Get(int i)
        {
            return m_Tiles[i];
        }

        public void Set(int i, ushort id, sbyte z)
        {
            if (i < Count)
                m_Tiles[i].Set(id, z);
        }

        public void Set(int i, ushort id, sbyte z, sbyte flag)
        {
            if (i < Count)
                m_Tiles[i].Set(id, z, flag);
        }
    }
}