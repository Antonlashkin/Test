using System;
using System.Collections.Generic;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            List<char> letters = new List<char>();
            foreach (string word in words)
            {
                int lettersInWord;
                int lettersInList;
                for (int i = 0; i < word.Length; i++)
                {
                    lettersInWord = 0;
                    lettersInList = 0;
                    for (int j = i; j < word.Length; j++)
                    {
                        if (word[i] == word[j])
                        {
                            lettersInWord++;
                        }
                    }
                    for (int j = 0; j < letters.Count; j++)
                    {
                        if (word[i] == letters[j])
                        {
                            lettersInList++;
                        }
                    }
                    if (lettersInWord > lettersInList)
                    {
                        for (int k = 0; k < lettersInWord - lettersInList; k++)
                        {
                            letters.Add(word[i]);
                        }
                    }
                }
            }
            return letters;
        }
    }
}