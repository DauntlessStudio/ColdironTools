// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System.Collections.Generic;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// A helper class for Lists.
    /// </summary>
    public static class ListUtilities
    {
        #region Methods
        /// <summary>
        /// Gets the next element in the list, or the first element if current element is the last element.
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to get an element from</param>
        /// <param name="currentElement">The element to search from</param>
        /// <returns>The next, or first element in the list</returns>
        public static T GetNextListElementLooping<T>(List<T> list, T currentElement)
        {
            if (list.IndexOf(currentElement) + 1 < list.Count)
            {
                return list[list.IndexOf(currentElement) + 1];
            }
            else return list[0];
        }

        /// <summary>
        /// Gets the next element in the list.
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to get an element from</param>
        /// <param name="currentElement">The element to search from</param>
        /// <returns>The next element in the list. Can be null</returns>
        public static T GetNextListElement<T>(List<T> list, T currentElement)
        {
            if (list.IndexOf(currentElement) + 1 < list.Count)
            {
                return list[list.IndexOf(currentElement) + 1];
            }
            else return list[list.Count -1];
        }

        /// <summary>
        /// Gets the prior element in the list, or the last element if current element is the first element.
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to get an element from</param>
        /// <param name="currentElement">The element to search from</param>
        /// <returns>The prior, or last element in the list</returns>
        public static T GetPriorListElementLooping<T>(List<T> list, T currentElement)
        {
            if (list.IndexOf(currentElement) - 1 > -1)
            {
                return list[list.IndexOf(currentElement) - 1];
            }
            else return list[list.Count - 1];
        }

        /// <summary>
        /// Gets the prior element in the list.
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to get an element from</param>
        /// <param name="currentElement">The element to search from</param>
        /// <returns>The next element in the list. Can be null</returns>
        public static T GetPriorListElement<T>(List<T> list, T currentElement)
        {
            if (list.IndexOf(currentElement) - 1 > -1)
            {
                return list[list.IndexOf(currentElement) - 1];
            }
            else return list[0];
        }
        #endregion
    }
}