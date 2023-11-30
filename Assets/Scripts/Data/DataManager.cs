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
        /// Load persistant data from json 
        /// </summary>
        public static void Load()
        {
            if (!data.isSaved)
            {
                Debug.LogWarning("DATA MANAGER: data isn't saved. data loading avorted");
                return;
            }
            
            SetPath();
            string json = File.ReadAllText(_persistantPath);
            data = JsonUtility.FromJson<PersistantData>(json);
        }
        
        /// <summary>
        /// Initialize data as persistant data
        /// </summary>
        public static void Reset()
        {
            data = new PersistantData();
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