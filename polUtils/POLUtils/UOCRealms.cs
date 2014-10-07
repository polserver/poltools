/***************************************************************************
 *
 * $Author: MuadDib
 *
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/


namespace POLUtils.UOConvert.UOCRealms
{
    /// <summary>
    ///     Inheritable Public and Private Entities for UOConvert Realm Information.
    ///     No default constructor as this Class is for Inheritable Purposes only.
    /// </summary>
    public class Realms
    {
        /// <summary>
        ///     Protected Realm Name Storage
        /// </summary>
        protected string _RealmName;
        /// <summary>
        ///     Protected Realm Map ID Storage
        /// </summary>
        protected int _MapID;
        /// <summary>
        ///     Protected Realm Use Dif Storage
        /// </summary>
        protected bool _UseDiff;
        /// <summary>
        ///     Protected Realm Width Storage
        /// </summary>
        protected int _Width;
        /// <summary>
        ///     Protected Realm Height Storage
        /// </summary>
        protected int _Height;

        /// <summary>
        ///     Gets/Sets the Realm Name for a Realm Object
        /// </summary>
        public string Realmname
        {
            get
            {
                return _RealmName;
            }
            set
            {
                _RealmName = value;
            }
        }
        /// <summary>
        ///     Gets/Sets the Realm Map ID for a Realm Object
        /// </summary>
        public int MapID
        {
            get
            {
                return _MapID;
            }
            set
            {
                _MapID = value;
            }
        }
        /// <summary>
        ///     Gets/Sets the Realm UseDif for a Realm Object
        /// </summary>
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
        /// <summary>
        ///     Gets/Sets the Realm Width for a Realm Object
        /// </summary>
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }
        /// <summary>
        ///     Gets/Sets the Realm Height for a Realm Object
        /// </summary>
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }
        /// <summary>
        ///     Retrieves the Realm UOConvert Map Conversion Command for a Realm Object
        /// </summary>
        public string GetUOCMapCommand()
        {
            string result = "map realm=" + _RealmName + " mapid=" + _MapID + " usedif=" + _UseDiff + " width=" + _Width + " height=" + _Height;
            return result;
        }
        /// <summary>
        ///     Retrieves the Realm UOConvert Statics Conversion Command for a Realm Object
        /// </summary>
        public string GetUOCStaticCommand()
        {
            string result = "statics realm=" + _RealmName;
            return result;
        }
        /// <summary>
        ///     Retrieves the Realm UOConvert Maptile Conversion Command for a Realm Object
        /// </summary>
        public string GetUOCMapTileCommand()
        {
            string result = "maptile realm=" + _RealmName;
            return result;
        }

    }

    /// <summary>
    ///     Custom Realm information for UOConvert. Inherits from Realms Class.
    ///     Mainly useable for custom size realms, or otherwise non-default realms from
    ///     Ultima Online.
    /// </summary>
    public class NewRealm : Realms
    {
        /// <summary>
        ///     Default Constructor to access Britannia (The Second Age) Realm Information
        /// </summary>
        public NewRealm()
        {
            _RealmName = "this_realm";
            _MapID = 0;
            _UseDiff = true;
            _Width = 6144;
            _Height = 4096;
        }
    }

    /// <summary>
    ///     Britannia T2A Realm information for UOConvert. Inherits from Realms Class
    /// </summary>
    public class BritanniaT2A : Realms
    {
        /// <summary>
        ///     Default Constructor to access Britannia (The Second Age) Realm Information
        /// </summary>
        public BritanniaT2A()
        {
            _RealmName = "britannia";
            _MapID = 0;
            _UseDiff = true;
            _Width = 6144;
            _Height = 4096;
        }
    }

    /// <summary>
    ///     Britannia ML Realm information for UOConvert. Inherits from Realms Class
    /// </summary>
    public class BritanniaML : Realms
    {
        /// <summary>
        ///     Default Constructor to access Britannia (Mondain's Legacy) Realm Information
        /// </summary>
        public BritanniaML()
        {
            _RealmName = "britannia";
            _MapID = 0;
            _UseDiff = true;
            _Width = 7168;
            _Height = 4096;
        }
    }

    /// <summary>
    ///     Britannia_Alt Realm information for UOConvert. Inherits from Realms Class
    /// </summary>
    public class Britannia_Alt : Realms
    {
        /// <summary>
        ///     Default Constructor to access Trammel Realm Information
        /// </summary>
        public Britannia_Alt()
        {
            _RealmName = "britannia_alt";
            _MapID = 1;
            _UseDiff = true;
            _Width = 6144;
            _Height = 4096;
        }
    }

    /// <summary>
    ///     Ilshenar Realm information for UOConvert. Inherits from Realms Class
    /// </summary>
    public class Ilshenar : Realms
    {
        /// <summary>
        ///     Default Constructor to access Ilshenar Realm Information
        /// </summary>
        public Ilshenar()
        {
            _RealmName = "ilshenar";
            _MapID = 2;
            _UseDiff = true;
            _Width = 2304;
            _Height = 1600;
        }
    }

    /// <summary>
    ///     Malas Realm information for UOConvert. Inherits from Realms Class
    /// </summary>
    public class Malas : Realms
    {
        /// <summary>
        ///     Default Constructor to access Malas Realm Information
        /// </summary>
        public Malas()
        {
            _RealmName = "malas";
            _MapID = 3;
            _UseDiff = true;
            _Width = 2560;
            _Height = 2048;
        }
    }

    /// <summary>
    ///     Tokuno Realm information for UOConvert. Inherits from Realms Class
    /// </summary>
    public class Tokuno : Realms
    {
        /// <summary>
        ///     Default Constructor to access Tokuno Realm Information
        /// </summary>
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
