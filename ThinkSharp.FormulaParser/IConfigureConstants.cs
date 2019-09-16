using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    /// <summary>
    /// Interface encapsulating the API for configuring constants.
    /// </summary>
    public interface IConfigureConstants
    {
        /// <summary>
        /// Adds a constant.
        /// </summary>
        /// <param name="name">
        /// The name of the constant to add.
        /// </param>
        /// <param name="value">
        /// The value of the constant.
        /// </param>
        void Add(string name, double value);

        /// <summary>
        /// Removes all configured contants.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Removes the constant with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the constant to remove.
        /// </param>
        void Remove(string name);
    }
}
