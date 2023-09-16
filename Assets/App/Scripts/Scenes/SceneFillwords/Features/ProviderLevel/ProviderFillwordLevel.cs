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
        private int maxLevel = 9;
        private int level = -1;
        private int laterIndex = 0;
        private bool isNextLevel = true;
        private string[] splitLevels = File.ReadAllText(@"Assets/App/Resources/Fillwords/pack_0.txt").Split('\n');
        private string[] splitWords = File.ReadAllText(@"Assets/App/Resources/Fillwords/words_list.txt").Split('\n');
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


            GridFillWords grid = null;
            bool isNormalGrid = false;
            int numberOfInvalidLevels = 0;
            while (!isNormalGrid)
            {
                if (!GridMake(ref grid))
                {
                    numberOfInvalidLevels++;
                    SwitcLevel();
                }
                else
                {
                    isNormalGrid = true;
                }
                if (numberOfInvalidLevels == maxLevel)
                {
                    throw new Exception();
                }
            }
            laterIndex = index;
            return grid;
        }


        private bool GridMake(ref GridFillWords grid)
        {
            int gridLenght = 0;
            for (int i = 0; i < splitLevels.Length; i++)
            {
                gridLenght = CountNumberOfGrid();
                if (gridLenght != 0)
                {
                    break;
                }
                else
                {
                    return false;
                }
            }


            //Debug.Log(splitLevels[level]);

            if (gridLenght == 0)
            {
                throw new Exception();
            }

            grid = new GridFillWords(new Vector2Int(gridLenght, gridLenght));

            if (!WordCheck(ref grid, gridLenght))
            {
                return false;
            }
            return true;
        }


        private int CountNumberOfGrid()
        {
            int numberOfSpace = 0;
            int numberOfSemicolon = 0;

            for (int i = 0; i < splitLevels[level].Length; i++)
            {
                if (splitLevels[level][i] == ' ')
                {
                    numberOfSpace++;
                }
            }

            for (int i = 0; i < splitLevels[level].Length; i++)
            {
                if (splitLevels[level][i] == ';')
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


        public bool WordCheck(ref GridFillWords grid, int gridLenght)
        {
            List<int> wordNumber = new List<int>();
            List<List<int>> numberOfLettersInAWord = new List<List<int>>();
            string[] wordsAndLettersNumbers = splitLevels[level].Split(' ');



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
                if (level == maxLevel)
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
                    level = maxLevel;
                }
                else
                {
                    level--;
                }
            }
        }

        private void Cheks()
        {

        }
    }
}