using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    internal interface IParserConfiguration : IConfiguration
    {
        bool HasVariable(string variable);
    }
}
