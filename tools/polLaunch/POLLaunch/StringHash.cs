using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

// It´s a shame that hashtables are not xml serializable, and I´m too lazy
// to write a ReadXML / WriteXML for them... so here it is, a List<MyHashItem>.
// (Fernando Rozenblit)
namespace POLLaunch
{
    #region StrHashItem class (the name/value thing)

    //Here you define how the <Item name="..">value</Item> will show up
    [Serializable]
    public class MyHashItem
    {
        [XmlAttribute] // makes it an attribute to item element
        public string name;

        [XmlText]
        public string value;

        public MyHashItem()
        {
            name = "";
            value = "";
        }
        public MyHashItem(string key, string value)
        {
            this.name = key;
            this.value = value;
        }
    }
    #endregion

    [Serializable]
    public class MyHashtable : List<MyHashItem>
    {
        public MyHashtable() {}

        //We must make it convincing
        public void Add(string key, string value)
        {
            base.Add(new MyHashItem(key, value));
        }

        //Let´s just fool everyone and pretend we´re a hash
        public string this[string key]
        {
            get
            {
                string valueObject = null;
                MyHashItem pair = this.Find(
                    delegate(MyHashItem mhi)
                    {
                        return mhi.name == key;
                    });
                if (pair != null)
                    valueObject = pair.value;
                return valueObject;
            }
            set
            {
                MyHashItem pair = this.Find(
                    delegate(MyHashItem mhi)
                    {
                        return mhi.name == key;
                    });

                if (pair != null)
                    pair.value = value;
                else
                    this.Add(key, value);
            }
        }
    }
}
