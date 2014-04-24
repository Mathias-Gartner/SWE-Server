using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin.Data.Definitions
{
    public interface IDefinitionFactory
    {
        IDefinition CreateDefinitionForType<T>();
    }

    public class DefinitionFactory : IDefinitionFactory
    {
        public IDefinition CreateDefinitionForType<T>()
        {
            var erpAssembly = Assembly.GetAssembly(GetType());
            var dalType = erpAssembly.GetTypes().Single(
                t => t.Name == typeof(T).Name + "Definition"
                && typeof(IDefinition).IsAssignableFrom(t)
                );

            return (IDefinition)Activator.CreateInstance(dalType);
        }
    }
}
