using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IPlugin
    {
        string GetName { get; }
        string GetAutor { get; }
        Data CreateProduct(Dictionary<string, string> Parameter);
    }
}
