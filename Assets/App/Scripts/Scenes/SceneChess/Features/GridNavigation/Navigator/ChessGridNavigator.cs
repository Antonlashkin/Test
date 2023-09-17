using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using Unity.VisualScripting;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            bool[,] isBlocked = new bool[8, 8];
            List<List<bool>> locomotionPattern = new List<List<bool>>();
            float[,] costWayFromSatrt = new float[8, 8];
            float[,] sumCost = new float[8, 8];
            Vector2Int[,] root = new Vector2Int[8, 8];
            List<Vector2Int> path = new List<Vector2Int>();
            Blocked(grid, from, isBlocked);
            CreatingAPattern(unit, locomotionPattern);
            Vector2Int next = from;
            while (next != to)
            {
                next = NextStep(next, to, unit, isBlocked, locomotionPattern, costWayFromSatrt, sumCost, root)
                if (next.x == -1 || next.y == -1 )
                {
                    return new List<Vector2Int> { from };
                    //return null; //If you need NULL
                }
            }
            path = PathCreation(from, to, root, unit);
            return path;
        }




        private Vector2Int NextStep(Vector2Int now, Vector2Int to, ChessUnitType unit, bool[,] isBlocked, List<List<bool>> locomotionPattern, float[,] costWayFromSatrt, float[,] sumCost, Vector2Int[,] root)
        {
            int jMin;
            int jMax;
            int iMin;
            int iMax;
            int added;
            float minValue = Mathf.Infinity;
            if (unit.HumanName() != "Knight")
            {
                added = 1;
                jMin = -1;
                jMax = 1;
                iMin = -1;
                iMax = 1;
               

                if (now.x == 0)
                {
                    jMin = 0;
                }
                if (now.x == 7)
                {
                    jMax = 0;
                }


                if (now.y == 0)
                {
                    iMin = 0;
                }
                if (now.y == 7)
                {
                    iMax = 0;
                }
            }
            else
            {
                added = 2;
                jMin = -2;
                jMax = 2;
                iMin = -2;
                iMax = 2;


                if (now.x == 0)
                {
                    jMin = 0;
                }
                if (now.x == 7)
                {
                    jMax = 0;
                }

                if (now.y == 0)
                {
                    iMin = 0;
                }
                if (now.y == 7)
                {
                    iMax = 0;
                }



                if (now.x == 1)
                {
                    jMin = - 1;
                }
                if (now.x == 6)
                {
                    jMax = 1;
                }


                if (now.y == 1)
                {
                    iMin = - 1;
                }
                if (now.y == 6)
                {
                    iMax = 1;
                }
            }


            for (int i = iMin; i <= iMax; i++)
            {
                for (int j = jMin; j <= jMax; j++)
                {
                    if (((i == 0 && j == 0) || isBlocked[now.x + j, now.y + i]) || !locomotionPattern[i + added][j + added])
                    {
                        continue;
                    }

                    float wayFromStartNow = -(1/10) * (Mathf.Sqrt(Mathf.Pow(i, 2) + Mathf.Pow(j, 2))) + 1 + costWayFromSatrt[now.x, now.y];
                    if (costWayFromSatrt[now.x + j, now.y + i] > wayFromStartNow || costWayFromSatrt[now.x + j, now.y + i] == 0) 
                    {
                        root[now.x + j, now.y + i] = new Vector2Int(now.x, now.y);
                        costWayFromSatrt[now.x + j, now.y + i] = wayFromStartNow;
                    }
                    sumCost[now.x + j, now.y + i] = costWayFromSatrt[now.x + j, now.y + i] + (Mathf.Abs(now.x + j - to.x) + Mathf.Abs(now.y + i - to.y));
                }
            }
            Vector2Int next = new Vector2Int();
            next.x = -1;
            next.y = -1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (sumCost[i, j] == 0 || isBlocked[i,j])
                    {
                        continue;
                    }
                    if (sumCost[i,j] < minValue || (j == to.x && i == to.y))
                    {
                        next = new Vector2Int(i, j);           
                        minValue = sumCost[i,j];        
                    }
                }
            }
            if (next.x != -1 || next.y != -1)
            {
                isBlocked[next.x, next.y] = true;
            }
            return next;
        }



        private void Blocked(ChessGrid grid, Vector2Int from, bool[,] isBlocked)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    isBlocked[i, j] = false;
                }
            }
            foreach (var piece in grid.Pieces)
            { 
                 isBlocked[piece.CellPosition.x, piece.CellPosition.y] = true;
            }
        }

        private void CreatingAPattern(ChessUnitType unit, List<List<bool>> locomotionPattern)
        {
            if (unit.HumanName() == "Pon")
            {
                locomotionPattern.Add(new List<bool> { false, true, false});
                locomotionPattern.Add(new List<bool> { false, false, false });
                locomotionPattern.Add(new List<bool> { false, true, false });
            }
            if (unit.HumanName() == "King" || unit.HumanName() == "Queen")
            {
                locomotionPattern.Add(new List<bool> { true, true, true });
                locomotionPattern.Add(new List<bool> { true, false, true });
                locomotionPattern.Add(new List<bool> { true, true, true });
            }
            if (unit.HumanName() == "Rook")
            {
                locomotionPattern.Add(new List<bool> { false, true, false });
                locomotionPattern.Add(new List<bool> { true, false, true });
                locomotionPattern.Add(new List<bool> { false, true, false });
            }
            if (unit.HumanName() == "Knight")
            {
                locomotionPattern.Add(new List<bool> { false, true, false, true, false });
                locomotionPattern.Add(new List<bool> { true, false, false, false, true });
                locomotionPattern.Add(new List<bool> { false, false, false, false, false });
                locomotionPattern.Add(new List<bool> { true, false, false, false, true });
                locomotionPattern.Add(new List<bool> { false, true, false, true, false });
            }
            if (unit.HumanName() == "Bishop")
            {
                locomotionPattern.Add(new List<bool> { true, false, true });
                locomotionPattern.Add(new List<bool> { false, false, false });
                locomotionPattern.Add(new List<bool> { true, false, true });
            }
        }

        private List<Vector2Int> PathCreation(Vector2Int from, Vector2Int to, Vector2Int[,] root, ChessUnitType unit)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            List<Vector2Int> reversPath = new List<Vector2Int>();
            reversPath.Add(to);
            Vector2Int now = to;
            while (now != from)
            {
                now = root[now.x, now.y];
                reversPath.Add(now);
            }
            for (int i = reversPath.Count; i > 0; i--)
            {
                path.Add(reversPath[i - 1]);
            }
            if (unit.HumanName() == "Bishop" || unit.HumanName() == "Queen" || unit.HumanName() == "Rook")
            {
                int ellementsInOneLine = 0;
                for (int i = 0; i < path.Count - 2; i++)
                {
                    if (path[i].x - path[i + 1].x == path[i + 1].x - path[i + 2].x && path[i].y - path[i + 1].y == path[i + 1].y - path[i + 2].y)
                    {
                        ellementsInOneLine++;
                    }
                    else
                    {
                        for (int j = 0; j < ellementsInOneLine; j++)
                        {
                            path.Remove(path[i - ellementsInOneLine + 1]);
                        }
                        i -= ellementsInOneLine;
                        ellementsInOneLine = 0;
                    }
                    if (i == path.Count - 3)
                    {
                        for (int j = 0; j < ellementsInOneLine; j++)
                        {
                            path.Remove(path[i - j + 1]);
                        }
                    }
                }
            }
            return path;
        }
    }
}