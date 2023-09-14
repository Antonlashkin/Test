using System;
using UnityEngine;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using System.IO;
using System.Text;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        public GridFillWords LoadModel(int index)
        {
            string[] splitLevels = File.ReadAllText(@"Assets/App/Resources/Fillwords/pack_0.txt").Split('\n');
            string[] splitWords = File.ReadAllText(@"Assets/App/Resources/Fillwords/words_list.txt").Split('\n');

            GridFillWords grid = new GridFillWords(new Vector2Int(2,2)); //изменить
            CharGridModel letter = new CharGridModel('\0');

            Debug.Log(splitLevels[index - 1]);
            for (int i = 0; i < 4; i++)
            {
                int letterIndex = int.Parse(Convert.ToString(splitLevels[int.Parse(Convert.ToString(splitLevels[index - 1][0]))][2 * (i + 1)]));
                letter = new CharGridModel(splitWords[int.Parse(Convert.ToString(splitLevels[index][0]))][letterIndex]);
                grid.Set(i % 2, i / 2, letter);
            }
            return grid;
        }
    }
}