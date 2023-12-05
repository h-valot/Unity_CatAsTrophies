using System.IO;
using UnityEngine;

namespace Data
{
    public static class DataManager
    {
        public static PersistantData data;

        private static string _persistantPath;
        
        /// <summary>
        /// Save persistant data to json 
        /// </summary>
        public static void Save()
        {
            SetPath();
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_persistantPath, json);
        }

        /// <summary>
        /// Load persistant data from json, if file exists
        /// Otherwise, create a new data
        /// </summary>
        public static void Load()
        {
            SetPath();

            if (File.Exists(_persistantPath))
            {
                string json = File.ReadAllText(_persistantPath);
                data = new PersistantData(JsonUtility.FromJson<PersistantData>(json));
            }
            else
            {
                data ??= new PersistantData(null);
            }
        }
        
        /// <summary>
        /// Set persistant path if it doesn't already exists
        /// </summary>
        private static void SetPath()
        {
            if (string.IsNullOrEmpty(_persistantPath)) _persistantPath = Path.Combine(Application.persistentDataPath, "data.json");
        }
    }
}