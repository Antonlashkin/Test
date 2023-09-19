using System;
using UnityEngine;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Xml.Linq;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private int maxLevel = 10;
        private int level = -1;
        private int laterIndex = 0;
        private bool isNextLevel = true;
        private string[] splitLevels = File.ReadAllText(@"Assets/App/Resources/Fillwords/pack_0.txt").Split('\n');
        private string[] splitWords = File.ReadAllText(@"Assets/App/Resources/Fillwords/words_list.txt").Split('\n');
        private List<GridFillWords> levels = new List<GridFillWords>();


        public GridFillWords LoadModel(int index)
        {
            if (((laterIndex < index) || (laterIndex > index && laterIndex == 3 && index == 1)) && !(laterIndex < index && laterIndex == 1 && index == 3))
            {
                isNextLevel = true;
            }
            else
            {
                isNextLevel = false;
            }

            SwitcLevel();

            if (levels.Count == 0)
            {
                for (int i = 0; i < maxLevel; i++)
                {
                    levels.Add(GridMake(i));
                }
            }
            


            int validLevels = 0;
            for (int i = 0; i < maxLevel; i++)
            {
                if (levels[i] != null)
                {
                    validLevels++;
                }
            }
            if (validLevels == 0)
            {
                throw new Exception();
            }

            while (levels[level] == null)
            {
                SwitcLevel();
            }

            laterIndex = index;
            return levels[level];
        }


        private GridFillWords GridMake(int levelIndex)
        {
            GridFillWords grid;
            int gridLenght = 0;
            for (int i = 0; i < splitLevels.Length; i++)
            {
                gridLenght = CountNumberOfGrid(levelIndex);
                if (gridLenght != 0)
                {
                    break;
                }
                else
                {
                    return null;
                }
            }



            if (gridLenght == 0)
            {
                throw new Exception();
            }

            grid = new GridFillWords(new Vector2Int(gridLenght, gridLenght));

            if (!WordCheck(ref grid, gridLenght, levelIndex))
            {
                return null;
            }
            return grid;
        }


        private int CountNumberOfGrid(int levelIndex)
        {
            int numberOfSpace = 0;
            int numberOfSemicolon = 0;

            for (int i = 0; i < splitLevels[levelIndex].Length; i++)
            {
                if (splitLevels[levelIndex][i] == ' ')
                {
                    numberOfSpace++;
                }
            }

            for (int i = 0; i < splitLevels[levelIndex].Length; i++)
            {
                if (splitLevels[levelIndex][i] == ';')
                {
                    numberOfSemicolon++;
                }
            }

            float gridLenght = MathF.Sqrt((numberOfSpace + 1) / 2 + numberOfSemicolon);

            if (gridLenght % (int)gridLenght == 0)
            {
                return (int)gridLenght;
            }
            else
            {
                return 0;
            }
        }


        public bool WordCheck(ref GridFillWords grid, int gridLenght, int levelIndex)
        {
            List<int> wordNumber = new List<int>();
            List<List<int>> numberOfLettersInAWord = new List<List<int>>();
            string[] wordsAndLettersNumbers = splitLevels[levelIndex].Split(' ');



            for (int i = 0; i < wordsAndLettersNumbers.Length; i += 2)
            {
                wordNumber.Add(int.Parse(wordsAndLettersNumbers[i]));
            }

            for (int i = 0; i < wordNumber.Count; i++)
            {
                numberOfLettersInAWord.Add(new List<int>(wordNumber[i]));
            }

            for (int i = 1, j = 0; i < wordsAndLettersNumbers.Length; i += 2, j++)
            {
                string[] oneWordSplit = wordsAndLettersNumbers[i].Split(';');
                for (int k = 0; k < oneWordSplit.Length; k++)
                {
                    numberOfLettersInAWord[j].Add((int.Parse(oneWordSplit[k])));

                }
            }

            for (int k = 0; k < gridLenght * gridLenght; k++)
            {
                int numberOfIdenticalElements = 0;
                for (int i = 0; i < numberOfLettersInAWord.Count; i++)
                {
                    for (int j = 0; j < numberOfLettersInAWord[i].Count; j++)
                    {
                        if (numberOfLettersInAWord[i][j] == k)
                        {
                            numberOfIdenticalElements++;
                        }
                    }
                }
                if (numberOfIdenticalElements != 1)
                {
                    return false;
                }
            }



            for (int i = 0; i < numberOfLettersInAWord.Count; i++)
            {
                int sumLetters = 0;
                for (int j = 0; j < numberOfLettersInAWord[i].Count; j++)
                {
                    sumLetters++;
                }
                if (sumLetters != splitWords[wordNumber[i]].Length - 1)
                {
                    return false;
                }
            }

            for (int i = 0; i < numberOfLettersInAWord.Count; i++)
            {
                for (int j = 0; j < numberOfLettersInAWord[i].Count; j++)
                {
                    if (numberOfLettersInAWord[i][j] >= gridLenght * gridLenght)
                    {
                        return false;
                    }
                }
            }


            CharGridModel letter = new CharGridModel('\0');
            for (int k = 0; k < gridLenght * gridLenght; k++)
            {
                for (int i = 0; i < numberOfLettersInAWord.Count; i++)
                {
                    for (int j = 0; j < numberOfLettersInAWord[i].Count; j++)
                    {
                        if (numberOfLettersInAWord[i][j] == k)
                        {
                            letter = new CharGridModel(splitWords[wordNumber[i]][j]);
                            grid.Set(k / gridLenght, k % gridLenght, letter);
                            continue;
                        }
                    }
                }
            }
            return true;
        }
        

        private void SwitcLevel()
        {
            if (isNextLevel)
            {
                if (level == maxLevel - 1)
                {
                    level = 0;
                }
                else
                {
                    level++;
                }
            }
            else
            {
                if (level == 0)
                {
                    level = maxLevel - 1;
                }
                else
                {
                    level--;
                }
            }
        }
    }
}