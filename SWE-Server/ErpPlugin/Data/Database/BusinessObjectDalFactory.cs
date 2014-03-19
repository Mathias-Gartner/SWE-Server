using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Database
{
    public interface IBusinessObjectDalFactory
    {
        IBusinessObjectDal<T> CreateDalForType<T>() where T : BusinessObject;
    }

    public class BusinessObjectDalFactory : IBusinessObjectDalFactory
    {
        public IBusinessObjectDal<T> CreateDalForType<T>() where T : BusinessObject
        {
            var erpAssembly = Assembly.GetAssembly(GetType());
            var dalType = erpAssembly.GetTypes().Single(
                t=>t.Name == typeof(T).Name + "Dal"
                && t.GetInterfaces().Any(i=>i.IsGenericType && i.Name.StartsWith("IBusinessObjectDal")));

            return (IBusinessObjectDal<T>)Activator.CreateInstance(dalType);
        }
    }
}
