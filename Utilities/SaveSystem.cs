using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// A static class that handles reading and writing json based save data to a binary file.
    /// </summary>
    public static class SaveSystem
    {
        private static string path = Application.persistentDataPath;
        private static BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Saves json data to a binary file.
        /// </summary>
        /// <param name="saveData">The json data to save.</param>
        /// <param name="saveSlot">The index of the save slot.</param>
        public static void SaveData(string saveData, int saveSlot)
        {
            FileStream stream = new FileStream(path + saveSlot + ".dat", FileMode.Create);

            formatter.Serialize(stream, saveData);
            stream.Close();
        }

        /// <summary>
        /// Loads json data from a binary file.
        /// </summary>
        /// <param name="saveSlot">The save slot index to load.</param>
        /// <returns>The json save data.</returns>
        public static string LoadData(int saveSlot)
        {
            if (IsSlotValid(saveSlot))
            {
                FileStream stream = new FileStream(path + saveSlot + ".dat", FileMode.Open);

                string saveData = formatter.Deserialize(stream) as string;
                stream.Close();

                return saveData;
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }

        /// <summary>
        /// Deletes a save file permanently.
        /// </summary>
        /// <param name="saveSlot">The save slot index to delete.</param>
        public static void DeleteData(int saveSlot)
        {
            File.Delete(path + saveSlot + ".dat");
        }

        /// <summary>
        /// Checks if a given save slot exists.
        /// </summary>
        /// <param name="saveSlot">The save slot index to check.</param>
        /// <returns>True if the file exists.</returns>
        public static bool IsSlotValid(int saveSlot)
        {
            return File.Exists(path + saveSlot + ".dat");
        }
    }
}
