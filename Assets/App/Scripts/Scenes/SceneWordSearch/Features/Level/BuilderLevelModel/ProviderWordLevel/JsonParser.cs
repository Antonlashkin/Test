using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [System.Serializable]
    public class JsonParser
    {
        public List<string> words;

        public static JsonParser CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<JsonParser>(jsonString);
        }
    }
