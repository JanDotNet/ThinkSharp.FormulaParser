using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public interface IConfigureConstants
    {
        void Add(string name, double value);

        void RemoveAll();

        void Remove(string name);
    }
}
