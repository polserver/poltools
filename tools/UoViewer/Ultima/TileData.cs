using System;
using System.IO;
using System.Text;

namespace Ultima
{
	/// <summary>
	/// Represents land tile data.
	/// <seealso cref="ItemData" />
	/// <seealso cref="LandData" />
	/// </summary>
	public struct LandData
	{
		private string m_Name;
        private short m_TexID;
		private TileFlag m_Flags;

		public LandData( string name, int TexID, TileFlag flags )
		{
			m_Name = name;
            m_TexID = (short)TexID;
			m_Flags = flags;
		}

		/// <summary>
		/// Gets the name of this land tile.
		/// </summary>
		public string Name
		{
            get { return m_Name; }
            set { m_Name = value; }
		}

        /// <summary>
        /// Gets the Texture ID of this land tile.
        /// </summary>
        public short TextureID
        {
            get { return m_TexID; }
            set { m_TexID = value; }
        }

		/// <summary>
		/// Gets a bitfield representing the 32 individual flags of this land tile.
		/// </summary>
		public TileFlag Flags
		{
            get { return m_Flags; }
            set { m_Flags = value; }
		}
	}

	/// <summary>
	/// Represents item tile data.
	/// <seealso cref="TileData" />
	/// <seealso cref="LandData" />
	/// </summary>
	public struct ItemData
	{
		internal string m_Name;
		internal TileFlag m_Flags;
		internal byte m_Weight;
		internal byte m_Quality;
		internal byte m_Quantity;
		internal byte m_Value;
		internal byte m_Height;
		internal short m_Animation;
        internal byte m_Hue;
        internal byte m_StackOffset;
        internal short m_Unk1;
        internal byte m_Unk2;
        internal byte m_Unk3;

		public ItemData( string name, TileFlag flags, int weight, int quality, int quantity, int value, int height, int anim, int hue, int stackingoffset, int unk1,int unk2,int unk3 )
		{
			m_Name = name;
			m_Flags = flags;
			m_Weight = (byte)weight;
			m_Quality = (byte)quality;
			m_Quantity = (byte)quantity;
			m_Value = (byte)value;
			m_Height = (byte)height;
			m_Animation = (short)anim;
            m_Hue = (byte)hue;
            m_StackOffset = (byte)stackingoffset;
            m_Unk1 = (short)unk1;
            m_Unk2 = (byte)unk2;
            m_Unk3 = (byte)unk3;
		}

		/// <summary>
		/// Gets the name of this item.
		/// </summary>
		public string Name
		{
            get { return m_Name; }
            set { m_Name = value; }
		}

		/// <summary>
		/// Gets the animation body index of this item.
		/// <seealso cref="Animations" />
		/// </summary>
		public short Animation
		{
            get { return m_Animation; }
            set { m_Animation = value; }
		}

		/// <summary>
		/// Gets a bitfield representing the 32 individual flags of this item.
		/// <seealso cref="TileFlag" />
		/// </summary>
		public TileFlag Flags
		{
            get { return m_Flags; }
            set { m_Flags = value; }
		}

		/// <summary>
		/// Whether or not this item is flagged as '<see cref="TileFlag.Background" />'.
		/// <seealso cref="TileFlag" />
		/// </summary>
		public bool Background
		{
			get{ return ( (m_Flags & TileFlag.Background) != 0 ); }
		}

		/// <summary>
		/// Whether or not this item is flagged as '<see cref="TileFlag.Bridge" />'.
		/// <seealso cref="TileFlag" />
		/// </summary>
		public bool Bridge
		{
			get{ return ( (m_Flags & TileFlag.Bridge) != 0 ); }
		}

		/// <summary>
		/// Whether or not this item is flagged as '<see cref="TileFlag.Impassable" />'.
		/// <seealso cref="TileFlag" />
		/// </summary>
		public bool Impassable
		{
			get{ return ( (m_Flags & TileFlag.Impassable) != 0 ); }
		}

		/// <summary>
		/// Whether or not this item is flagged as '<see cref="TileFlag.Surface" />'.
		/// <seealso cref="TileFlag" />
		/// </summary>
		public bool Surface
		{
			get{ return ( (m_Flags & TileFlag.Surface) != 0 ); }
		}

		/// <summary>
		/// Gets the weight of this item.
		/// </summary>
		public byte Weight
		{
            get { return m_Weight; }
            set { m_Weight = value; }
		}

		/// <summary>
		/// Gets the 'quality' of this item. For wearable items, this will be the layer.
		/// </summary>
		public byte Quality
		{
            get { return m_Quality; }
            set { m_Quality = value; }
		}

		/// <summary>
		/// Gets the 'quantity' of this item.
		/// </summary>
        public byte Quantity
		{
            get { return m_Quantity; }
            set { m_Quantity = value; }
		}

		/// <summary>
		/// Gets the 'value' of this item.
		/// </summary>
        public byte Value
		{
            get { return m_Value; }
            set { m_Value = value; }
		}

		/// <summary>
		/// Gets the Hue of this item.
		/// </summary>
        public byte Hue
		{
            get { return m_Hue; }
            set { m_Hue = value; }
		}

        /// <summary>
        /// Gets the stackingoffset of this item. (If flag Generic)
        /// </summary>
        public byte StackingOffset
        {
            get { return m_StackOffset; }
            set { m_StackOffset = value; }
        }

        /// <summary>
        /// Gets the height of this item.
        /// </summary>
        public byte Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        /// <summary>
        /// Gets the unk1 of this item.
        /// </summary>
        public short Unk1
        {
            get { return m_Unk1; }
            set { m_Unk1 = value; }
        }

        /// <summary>
        /// Gets the unk2 of this item.
        /// </summary>
        public byte Unk2
        {
            get { return m_Unk2; }
            set { m_Unk2 = value; }
        }

        /// <summary>
        /// Gets the unk3 of this item.
        /// </summary>
        public byte Unk3
        {
            get { return m_Unk3; }
            set { m_Unk3 = value; }
        }

		/// <summary>
		/// Gets the 'calculated height' of this item. For <see cref="Bridge">bridges</see>, this will be: <c>(<see cref="Height" /> / 2)</c>.
		/// </summary>
		public int CalcHeight
		{
			get
			{
				if ( (m_Flags & TileFlag.Bridge) != 0 )
					return m_Height / 2;
				else
					return m_Height;
			}
		}

        /// <summary>
        /// Whether or not this item is wearable as '<see cref="TileFlag.Wearable" />'.
        /// <seealso cref="TileFlag" />
        /// </summary>
        public bool Wearable
        {
            get { return ((m_Flags & TileFlag.Wearable) != 0); }
        }
	}

	/// <summary>
	/// An enumeration of 32 different tile flags.
	/// <seealso cref="ItemData" />
	/// <seealso cref="LandData" />
	/// </summary>
	[Flags]
	public enum TileFlag
	{
		/// <summary>
		/// Nothing is flagged.
		/// </summary>
		None			= 0x00000000,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		Background		= 0x00000001,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		Weapon			= 0x00000002,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		Transparent		= 0x00000004,
		/// <summary>
		/// The tile is rendered with partial alpha-transparency.
		/// </summary>
		Translucent		= 0x00000008,
		/// <summary>
		/// The tile is a wall.
		/// </summary>
		Wall			= 0x00000010,
		/// <summary>
		/// The tile can cause damage when moved over.
		/// </summary>
		Damaging		= 0x00000020,
		/// <summary>
		/// The tile may not be moved over or through.
		/// </summary>
		Impassable		= 0x00000040,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		Wet				= 0x00000080,
		/// <summary>
		/// Unknown.
		/// </summary>
		Unknown1		= 0x00000100,
		/// <summary>
		/// The tile is a surface. It may be moved over, but not through.
		/// </summary>
		Surface			= 0x00000200,
		/// <summary>
		/// The tile is a stair, ramp, or ladder.
		/// </summary>
		Bridge			= 0x00000400,
		/// <summary>
		/// The tile is stackable
		/// </summary>
		Generic			= 0x00000800,
		/// <summary>
		/// The tile is a window. Like <see cref="TileFlag.NoShoot" />, tiles with this flag block line of sight.
		/// </summary>
		Window			= 0x00001000,
		/// <summary>
		/// The tile blocks line of sight.
		/// </summary>
		NoShoot			= 0x00002000,
		/// <summary>
		/// For single-amount tiles, the string "a " should be prepended to the tile name.
		/// </summary>
		ArticleA		= 0x00004000,
		/// <summary>
		/// For single-amount tiles, the string "an " should be prepended to the tile name.
		/// </summary>
		ArticleAn		= 0x00008000,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		Internal		= 0x00010000,
		/// <summary>
		/// The tile becomes translucent when walked behind. Boat masts also have this flag.
		/// </summary>
		Foliage			= 0x00020000,
		/// <summary>
		/// Only gray pixels will be hued
		/// </summary>
		PartialHue		= 0x00040000,
		/// <summary>
		/// Unknown.
		/// </summary>
		Unknown2		= 0x00080000,
		/// <summary>
		/// The tile is a map--in the cartography sense. Unknown usage.
		/// </summary>
		Map				= 0x00100000,
		/// <summary>
		/// The tile is a container.
		/// </summary>
		Container		= 0x00200000,
		/// <summary>
		/// The tile may be equiped.
		/// </summary>
		Wearable		= 0x00400000,
		/// <summary>
		/// The tile gives off light.
		/// </summary>
		LightSource		= 0x00800000,
		/// <summary>
		/// The tile is animated.
		/// </summary>
		Animation		= 0x01000000,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		NoDiagonal		= 0x02000000,
		/// <summary>
		/// Unknown.
		/// </summary>
		Unknown3		= 0x04000000,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		Armor			= 0x08000000,
		/// <summary>
		/// The tile is a slanted roof.
		/// </summary>
		Roof			= 0x10000000,
		/// <summary>
		/// The tile is a door. Tiles with this flag can be moved through by ghosts and GMs.
		/// </summary>
		Door			= 0x20000000,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		StairBack		= 0x40000000,
		/// <summary>
		/// Not yet documented.
		/// </summary>
		StairRight		= unchecked( (int)0x80000000 )
	}

	/// <summary>
	/// Contains lists of <see cref="LandData">land</see> and <see cref="ItemData">item</see> tile data.
	/// <seealso cref="LandData" />
	/// <seealso cref="ItemData" />
	/// </summary>
	public sealed class TileData
	{
		private static LandData[] m_LandData;
		private static ItemData[] m_ItemData;
		private static int[] m_HeightTable;

		/// <summary>
		/// Gets the list of <see cref="LandData">land tile data</see>.
		/// </summary>
		public static LandData[] LandTable
		{
            get { return m_LandData; }
            set { m_LandData = value; }
		}

		/// <summary>
		/// Gets the list of <see cref="ItemData">item tile data</see>.
		/// </summary>
		public static ItemData[] ItemTable
		{
            get { return m_ItemData; }
            set { m_ItemData = value; }
		}

		public static int[] HeightTable
		{
			get{ return m_HeightTable; }
		}

        private static byte[] m_StringBuffer = new byte[20];
		private static string ReadNameString( BinaryReader bin )
		{
            bin.Read(m_StringBuffer, 0, 20);

            int count;

            for (count = 0; count < 20 && m_StringBuffer[count] != 0; ++count) ;

            return Encoding.Default.GetString(m_StringBuffer, 0, count);
		}

		private TileData()
		{
		}

        private static int[] landheader;
        private static int[] itemheader;

		static TileData()
		{
            Initialize();
		}

        public static void Initialize()
        {
            string filePath = Files.GetFilePath("tiledata.mul");

            if (filePath != null)
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    landheader = new int[512];
                    int j = 0;
                    BinaryReader bin = new BinaryReader(fs);

                    m_LandData = new LandData[0x4000];

                    for (int i = 0; i < 0x4000; ++i)
                    {
                        if ((i & 0x1F) == 0)
                        {
                            landheader[j] = bin.ReadInt32(); // header
                            ++j;
                        }
                        TileFlag flags = (TileFlag)bin.ReadInt32();
                        int texID = bin.ReadInt16();

                        m_LandData[i] = new LandData(ReadNameString(bin), texID, flags);
                    }

                    m_ItemData = new ItemData[0x4000];
                    m_HeightTable = new int[0x4000];
                    itemheader = new int[512];
                    j = 0;

                    for (int i = 0; i < 0x4000; ++i)
                    {
                        if ((i & 0x1F) == 0)
                        {
                            itemheader[j] = bin.ReadInt32(); // header
                            j++;
                        }

                        TileFlag flags = (TileFlag)bin.ReadInt32();
                        int weight = bin.ReadByte();
                        int quality = bin.ReadByte();
                        int unk1 = bin.ReadInt16();
                        int unk2 = bin.ReadByte();
                        int quantity = bin.ReadByte();
                        int anim = bin.ReadInt16();
                        int unk3 = bin.ReadByte();
                        int hue = bin.ReadByte();
                        int stackingoffset = bin.ReadByte(); //unk4
                        int value = bin.ReadByte(); //unk5
                        int height = bin.ReadByte();

                        m_ItemData[i] = new ItemData(ReadNameString(bin), flags, weight, quality, quantity, value, height, anim, hue, stackingoffset, unk1, unk2, unk3);
                        m_HeightTable[i] = height;
                    }
                }
            }
            else
            {
                
            }
        }

        /// <summary>
        /// Saves <see cref="LandData"/> and <see cref="ItemData"/> to tiledata.mul
        /// </summary>
        /// <param name="FileName"></param>
        public static void SaveTileData(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryWriter bin = new BinaryWriter(fs);
                int j = 0;
                for (int i = 0; i < 0x4000; ++i)
                {
                    if ((i & 0x1F) == 0)
                    {
                        bin.Write(landheader[j]); //header
                        ++j;
                    }
                    bin.Write((int)m_LandData[i].Flags);
                    bin.Write(m_LandData[i].TextureID);
                    byte[] b = new byte[20];
                    if (m_LandData[i].Name != null)
                    {
                        byte[] bb = Encoding.Default.GetBytes(m_LandData[i].Name);
                        if (bb.Length > 20)
                            Array.Resize(ref bb, 20);
                        bb.CopyTo(b, 0);
                    }
                    bin.Write(b);
                }
                j = 0;
                for (int i = 0; i < 0x4000; ++i)
                {
                    if ((i & 0x1F) == 0)
                    {
                        bin.Write(itemheader[j]); // header
                        j++;
                    }
                    bin.Write((int)m_ItemData[i].Flags);
                    bin.Write(m_ItemData[i].Weight);
                    bin.Write(m_ItemData[i].Quality);
                    bin.Write(m_ItemData[i].Unk1);
                    bin.Write(m_ItemData[i].Unk2);
                    bin.Write(m_ItemData[i].Quantity);
                    bin.Write(m_ItemData[i].Animation);
                    bin.Write(m_ItemData[i].Unk3);
                    bin.Write(m_ItemData[i].Hue);
                    bin.Write(m_ItemData[i].StackingOffset); //unk4
                    bin.Write(m_ItemData[i].Value); //unk5
                    bin.Write(m_ItemData[i].Height);
                    byte[] b = new byte[20];
                    if (m_ItemData[i].Name != null)
                    {
                        byte[] bb =Encoding.Default.GetBytes(m_ItemData[i].Name);
                        if (bb.Length > 20)
                            Array.Resize(ref bb, 20);
                        bb.CopyTo(b, 0);
                    }
                    bin.Write(b);
                }
            }
        }

        /// <summary>
        /// Exports <see cref="ItemData"/> to csv file
        /// </summary>
        /// <param name="FileName"></param>
        public static void ExportItemDataToCSV(string FileName)
        {
            using (StreamWriter Tex = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite), System.Text.Encoding.GetEncoding(1252)))
            {
                Tex.Write("ID;Name;Weight/Quantity;Layer/Quality;Gump/AnimID;Height;Hue;Class/Quantity;StackingOffset;Unkown1;Unkown2;Unkown3");
                Tex.Write(";Background;Weapon;Transparent;Translucent;Wall;Damage;Impassible;Wet;Unknow1");
                Tex.Write(";Surface;Bridge;Generic;Window;NoShoot;PrefixA;PrefixAn;Internal;Foliage;PartialHue");
                Tex.Write(";Unknow2;Map;Container/Height;Wearable;Lightsource;Animation;NoDiagonal");
                Tex.WriteLine(";Unknow3;Armor;Roof;Door;StairBack;StairRight" );

                for (int i = 0; i < m_ItemData.Length; i++)
                {
                    ItemData tile = m_ItemData[i];
                    Tex.Write(String.Format("0x{0:X4}", i));
                    Tex.Write(";" + tile.Name);
                    Tex.Write(";" + tile.Weight);
                    Tex.Write(";" + tile.Quality);
                    Tex.Write(";" + String.Format("0x{0:X4}", tile.Animation));
                    Tex.Write(";" + tile.Height);
                    Tex.Write(";" + tile.Hue);
                    Tex.Write(";" + tile.Quantity);
                    Tex.Write(";" + tile.StackingOffset);
                    Tex.Write(";" + tile.Unk1);
                    Tex.Write(";" + tile.Unk2);
                    Tex.Write(";" + tile.Unk3);

                    Tex.Write(";" + (((tile.Flags & TileFlag.Background) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Weapon) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Transparent) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Translucent) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Wall) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Damaging) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Impassable) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Wet) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Unknown1) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Surface) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Bridge) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Generic) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Window) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.NoShoot) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.ArticleA) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.ArticleAn) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Internal) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Foliage) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.PartialHue) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Unknown2) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Map) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Container) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Wearable) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.LightSource) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Animation) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.NoDiagonal) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Unknown3) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Armor) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Roof) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Door) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.StairBack) != 0) ? "1" : "0"));
                    Tex.WriteLine(";" + (((tile.Flags & TileFlag.StairRight) != 0) ? "1" : "0"));
                }
            }
        }

        /// <summary>
        /// Exports <see cref="LandData"/> to csv file
        /// </summary>
        /// <param name="FileName"></param>
        public static void ExportLandDataToCSV(string FileName)
        {
            using (StreamWriter Tex = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite)))
            {
                Tex.Write("ID;Name;TextureID");
                Tex.Write(";Background;Weapon;Transparent;Translucent;Wall;Damage;Impassible;Wet;Unknow1");
                Tex.Write(";Surface;Bridge;Generic;Window;NoShoot;PrefixA;PrefixAn;Internal;Foliage;PartialHue");
                Tex.Write(";Unknow2;Map;Container/Height;Wearable;Lightsource;Animation;NoDiagonal");
                Tex.WriteLine(";Unknow3;Armor;Roof;Door;StairBack;StairRight");

                for (int i = 0; i < m_LandData.Length; i++)
                {
                    LandData tile = m_LandData[i];
                    Tex.Write(String.Format("0x{0:X4}", i));
                    Tex.Write(";" + tile.Name);
                    Tex.Write(";" + String.Format("0x{0:X4}", tile.TextureID));

                    Tex.Write(";" + (((tile.Flags & TileFlag.Background) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Weapon) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Transparent) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Translucent) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Wall) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Damaging) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Impassable) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Wet) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Unknown1) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Surface) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Bridge) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Generic) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Window) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.NoShoot) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.ArticleA) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.ArticleAn) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Internal) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Foliage) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.PartialHue) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Unknown2) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Map) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Container) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Wearable) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.LightSource) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Animation) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.NoDiagonal) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Unknown3) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Armor) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Roof) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.Door) != 0) ? "1" : "0"));
                    Tex.Write(";" + (((tile.Flags & TileFlag.StairBack) != 0) ? "1" : "0"));
                    Tex.WriteLine(";" + (((tile.Flags & TileFlag.StairRight) != 0) ? "1" : "0"));
                }
            }
        }
	}
}