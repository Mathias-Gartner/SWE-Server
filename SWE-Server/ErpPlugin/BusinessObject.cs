using ErpPlugin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin
{
    public abstract class BusinessObject
    {
        protected BusinessObject()
        {
            State = BusinessObjectState.New;
            ID = -1;
        }

        public enum BusinessObjectState
        {
            New,
            Modified,
            Unmodified,
            Deleted,
            SearchObject
        }

        public BusinessObjectState State { get; /*protected */set; }

        public int ID { get; set; }

        public static T CreateSearchObject<T>() where T : BusinessObject, new()
        {
            return new T() { State = BusinessObjectState.SearchObject };
        }

        /*public static T LoadObject<T>(int id) where T : BusinessObject, new()
        {
            var t = new T();
            t.Load(id);
            return t;
        }*/

        public bool Save()
        {
            return CurrentDalFactory.Instance.CreateDal().SaveBusinessObject(this);
        }
    }
}
