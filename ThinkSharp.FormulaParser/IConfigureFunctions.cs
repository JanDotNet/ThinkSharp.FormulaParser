using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public interface IConfigureFunctions
    {        
        /// <summary>
        /// Adds a function with zero paramteter.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double> function);

        /// <summary>
        /// Adds a function with one paramteter.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double> function);

        /// <summary>
        /// Adds a function with two paramteters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double, double> function);

        /// <summary>
        /// Adds a function with three paramteters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double, double, double> function);

        /// <summary>
        /// Adds a function with four paramteters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double, double, double, double> function);

        /// <summary>
        /// Adds a function with five paramteters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double, double, double, double, double> function);

        /// <summary>
        /// Adds a function with 2 to n paramteters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double[], double> function);

        /// <summary>
        /// Removes all configured functions.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Removed the function with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the function to remove.
        /// </param>
        void Remove(string name);
    }
}
