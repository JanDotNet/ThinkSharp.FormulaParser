using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public interface IConfigureFunctions
    {        
        void Add(string name, Func<double> function);

        void Add(string name, Func<double, double> function);

        void Add(string name, Func<double, double, double> function);

        void Add(string name, Func<double, double, double, double> function);

        void Add(string name, Func<double, double, double, double, double> function);

        void Add(string name, Func<double, double, double, double, double, double> function);

        void Add(string name, Func<double[], double> function);

        void RemoveAll();

        void Remove(string name);
    }
}
