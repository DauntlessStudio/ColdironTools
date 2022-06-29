using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using ColdironTools.Scriptables;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// A singleton game object responsible for interfacing with the save system.
    /// </summary>
    public class SaveJsonObject : MonoBehaviour
    {
        private string json;
        private Dictionary<string, object> SaveDataDictionary = new Dictionary<string, object>();

        public static SaveJsonObject Instance;

        /// <summary>
        /// Initializes the object, setting it as the singleton, and assigning Scriptable listeners.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            GetScriptables();
        }

        /// <summary>
        /// Iterates through all scriptable variables and sets any marked as ShouldSave to automatically update the saveDataDictionary when their value changes.
        /// </summary>
        private void GetScriptables()
        {
            foreach (IntScriptable item in Resources.LoadAll<IntScriptable>("Scriptables"))
            {
                if (item.ShouldSave) item.saveValueChange += SetValue;
            }

            foreach (FloatScriptable item in Resources.LoadAll<FloatScriptable>("Scriptables"))
            {
                if (item.ShouldSave) item.saveValueChange += SetValue;
            }

            foreach (BoolScriptable item in Resources.LoadAll<BoolScriptable>("Scriptables"))
            {
                if (item.ShouldSave) item.saveValueChange += SetValue;
            }

            foreach (StringScriptable item in Resources.LoadAll<StringScriptable>("Scriptables"))
            {
                if (item.ShouldSave) item.saveValueChange += SetValue;
            }
        }


        /// <summary>
        /// Iterates through all scriptable variables and sets the value of any marked as ShouldSave to the respecting value in the saveDataDictionary.
        /// </summary>
        private void SetScriptables()
        {
            foreach (IntScriptable item in Resources.LoadAll<IntScriptable>("Scriptables"))
            {
                if (item.ShouldSave && SaveDataDictionary.ContainsKey(item.name)) item.Value = (int)SaveDataDictionary[item.name];
            }

            foreach (FloatScriptable item in Resources.LoadAll<FloatScriptable>("Scriptables"))
            {
                if (item.ShouldSave && SaveDataDictionary.ContainsKey(item.name))
                {
                    if (SaveDataDictionary[item.name].ToString().Contains("."))
                    {
                        item.Value = (float)SaveDataDictionary[item.name];
                    }
                    else
                    {
                        item.Value = (int)SaveDataDictionary[item.name];
                    }
                }
            }

            foreach (BoolScriptable item in Resources.LoadAll<BoolScriptable>("Scriptables"))
            {
                if (item.ShouldSave && SaveDataDictionary.ContainsKey(item.name)) item.Value = (bool)SaveDataDictionary[item.name];
            }

            foreach (StringScriptable item in Resources.LoadAll<StringScriptable>("Scriptables"))
            {
                if (item.ShouldSave && SaveDataDictionary.ContainsKey(item.name)) item.Value = (string)SaveDataDictionary[item.name];
            }
        }

        /// <summary>
        /// Saves the key value pair into the saveDataDictionary.
        /// </summary>
        /// <param name="key">The name of the variable to write.</param>
        /// <param name="value">The value to be saved.</param>
        public void SetValue(string key, object value)
        {
            if (SaveDataDictionary.ContainsKey(key))
            {
                SaveDataDictionary[key] = value;
            }
            else
            {
                SaveDataDictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Gets a value from the saveDataDictionary.
        /// </summary>
        /// <param name="key">The variable name to read.</param>
        /// <returns>The value stored in the saveDataDictionary.</returns>
        public object GetValue(string key)
        {
            return SaveDataDictionary[key];
        }

        /// <summary>
        /// Saves to slot 0. For testing in the editor.
        /// </summary>
        [ContextMenu("Save Game")]
        private void TestSave()
        {
            SaveCurrentSlot();
        }

        public void SaveCurrentSlot()
        {
            SaveGame(PlayerPrefs.GetInt("CurrentSave"));
        }

        /// <summary>
        /// Serializes the saveDataDictionary into json, and sends it to the SaveSystem.
        /// </summary>
        /// <param name="saveSlot">The slot index to save into.</param>
        public void SaveGame(int saveSlot)
        {
            json = Serialize(SaveDataDictionary);
            print(json);
            SaveSystem.SaveData(json, saveSlot);
        }

        /// <summary>
        /// Loads from slot 0. For testing in the editor.
        /// </summary>
        [ContextMenu("Load Game")]
        private void TestLoad()
        {
            LoadGame(0);
        }

        /// <summary>
        /// Gets json from the SaveSystem and loads it into the SaveDataDictionary. Then SetsScriptables.
        /// </summary>
        /// <param name="saveSlot">The slot index to load.</param>
        public void LoadGame(int saveSlot)
        {
            json = SaveSystem.LoadData(saveSlot);
            print(json);
            SaveDataDictionary = Deserialize<Dictionary<string, object>>(json);

            SetScriptables();
        }

        /// <summary>
        /// Deletes a save file.
        /// </summary>
        /// <param name="saveSlot">The save slot index to be deleted.</param>
        public void DeleteGame(int saveSlot)
        {
            SaveSystem.DeleteData(saveSlot);
        }

        /// <summary>
        /// Deserializes a json string into a type T.
        /// </summary>
        /// <typeparam name="T">The type that should be deserialized to.</typeparam>
        /// <param name="body">The json string.</param>
        /// <returns>The deserialized T object.</returns>
        public static T Deserialize<T>(string body)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(body);
                writer.Flush();
                stream.Position = 0;
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
            }
        }

        /// <summary>
        /// Serializes a type T into a json string.
        /// </summary>
        /// <typeparam name="T">The type that should be serialized.</typeparam>
        /// <param name="item">The object of type T to be serialized.</param>
        /// <returns>The serialized json string.</returns>
        public static string Serialize<T>(T item)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                new DataContractJsonSerializer(typeof(T)).WriteObject(ms, item);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }
    }
}
