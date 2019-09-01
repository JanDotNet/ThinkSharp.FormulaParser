using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    internal interface IConfigurationEvaluator
    {
        double EvaluateFunction(string name, double[] args);

        IEnumerable<KeyValuePair<string, double>> EnumerateConstantes();
    }
}
