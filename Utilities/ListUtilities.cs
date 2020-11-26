using System.Collections.Generic;

namespace ColdironTools.Utilities
{
    public static class ListUtilities
    {
        #region Methods
        public static T GetNextListElement<T>(List<T> list, T currentElement)
        {
            if (list.IndexOf(currentElement) + 1 < list.Count)
            {
                return list[list.IndexOf(currentElement) + 1];
            }
            else return list[0];
        }

        public static T GetPriorListElement<T>(List<T> list, T currentElement)
        {
            if (list.IndexOf(currentElement) - 1 > -1)
            {
                return list[list.IndexOf(currentElement) - 1];
            }
            else return list[list.Count - 1];
        }
        #endregion
    }
}