using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    /// <summary>
    /// Interface encapsulating the API for configuring functions.
    /// </summary>
    public interface IConfigureFunctions
    {        
        /// <summary>
        /// Adds a function with zero parameters.
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
        /// Adds a function with two parameters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double, double> function);

        /// <summary>
        /// Adds a function with three parameters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double, double, double> function);

        /// <summary>
        /// Adds a function with four parameters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double, double, double, double> function);

        /// <summary>
        /// Adds a function with five parameters.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        void Add(string name, Func<double, double, double, double, double, double> function);

        /// <summary>
        /// Adds a function with 2 to n parameters.
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
        /// Removes the function with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the function to remove.
        /// </param>
        void Remove(string name);
    }
}
