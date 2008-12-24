using System;
using System.Collections.Generic;
using System.Text;

using POLUtils.UOConvert;

namespace POLUtils.UOConvert.UOCRealms
{
    public class Realms
    {
        protected string _RealmName;
        protected int _MapID;
        protected bool _UseDiff;
        protected int _Width;
        protected int _Height;

        public string GetRealmname()
        {
            return _RealmName;
        }
        public int GetMapID()
        {
            return _MapID;
        }
        public bool UseDif
        {
            get
            {
                return _UseDiff;
            }
            set
            {
                _UseDiff = value;
            }
        }
        public int GetWidth()
        {
            return _Width;
        }
        public int GetHeight()
        {
            return _Height;
        }
        public string GetUOCMapCommand()
        {
            string result = "map realm=" + _RealmName + " mapid=" + _MapID + " usedif=" + _UseDiff + " width=" + _Width + " height=" + _Height;
            return result;
        }
        public string GetUOCStaticCommand()
        {
            string result = "statics realm=" + _RealmName;
            return result;
        }
        public string GetUOCMapTileCommand()
        {
            string result = "maptile realm=" + _RealmName;
            return result;
        }

    }

    public class BritanniaT2A : Realms
    {
        public BritanniaT2A()
        {
            _RealmName = "britannia";
            _MapID = 0;
            _UseDiff = true;
            _Width = 6144;
            _Height = 4096;
        }
    }

    public class BritanniaML : Realms
    {
        public BritanniaML()
        {
            _RealmName = "britannia";
            _MapID = 0;
            _UseDiff = true;
            _Width = 7168;
            _Height = 4096;
        }
    }

    public class Britannia_Alt : Realms
    {
        public Britannia_Alt()
        {
            _RealmName = "britannia_alt";
            _MapID = 1;
            _UseDiff = true;
            _Width = 6144;
            _Height = 4096;
        }
    }

    public class Ilshenar : Realms
    {
        public Ilshenar()
        {
            _RealmName = "ilshenar";
            _MapID = 2;
            _UseDiff = true;
            _Width = 2304;
            _Height = 1600;
        }
    }

    public class Malas : Realms
    {
        public Malas()
        {
            _RealmName = "malas";
            _MapID = 3;
            _UseDiff = true;
            _Width = 2560;
            _Height = 2048;
        }
    }

    public class Tokuno : Realms
    {
        public Tokuno()
        {
            _RealmName = "tokuno";
            _MapID = 4;
            _UseDiff = true;
            _Width = 1448;
            _Height = 1448;
        }
    }
}
