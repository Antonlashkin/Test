using System;
using System.IO;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            string json = File.ReadAllText(@$"Assets/App/Resources/WordSearch/Levels/{levelIndex}.json");

            LevelInfo levelInfo = new LevelInfo();
            JsonParser jsonParse = JsonParser.CreateFromJSON(json);
            levelInfo.words = jsonParse.words;

            return levelInfo;
        }
    }
}