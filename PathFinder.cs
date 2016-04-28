// Decompiled with JetBrains decompiler
// Type: Dean.PathFinder
// Assembly: Dean, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Dean\Dean.exe

using System.Collections.Generic;

namespace ConsoleDA
{
    public class PathFinder
    {
        public bool isWall(Tile tile)
        {
            return tile != Tile.Wall && tile != Tile.Player && tile != Tile.Monster && tile != Tile.Npc;
        }

        public static List<PathFinder.PathFinderNode> FindPath(Tile[,] Matrix, Location Start, Location End)
        {
            bool[,] flagArray = new bool[Matrix.GetUpperBound(0) + 1, Matrix.GetUpperBound(1) + 1];
            List<PathFinder.PathFinderNode> list1 = new List<PathFinder.PathFinderNode>((IEnumerable<PathFinder.PathFinderNode>)new PathFinder.PathFinderNode[1]
              {
                new PathFinder.PathFinderNode()
                {
                  X = (int) Start.X,
                  Y = (int) Start.Y,
                  Heuristic = 0
                }
              });
            PathFinder.PathFinderNode pathFinderNode1 = (PathFinder.PathFinderNode)null;
            int num = 0;
            List<PathFinder.PathFinderNode> list2;
            for (; pathFinderNode1 == null && list1.Count > 0; list1 = list2)
            {
                list2 = new List<PathFinder.PathFinderNode>();
                for (int index = 0; index < list1.Count; ++index)
                {
                    if (list1[index].Heuristic > num)
                    {
                        list2.Add(list1[index]);
                    }
                    else
                    {
                        if ((list1[index].X - 1 <= Matrix.GetUpperBound(0) && list1[index].X - 1 >= 0 && (!flagArray[list1[index].X - 1, list1[index].Y] && Matrix[list1[index].X - 1, list1[index].Y] != Tile.Wall)) || (list1[index].X - 1 == End.X && list1[index].Y == End.Y))
                        {
                            PathFinder.PathFinderNode pathFinderNode2 = new PathFinder.PathFinderNode()
                            {
                                LastNode = list1[index].LastNode,
                                X = list1[index].X,
                                Y = list1[index].Y,
                                Heuristic = list1[index].Heuristic
                            };
                            PathFinder.PathFinderNode pathFinderNode3 = new PathFinder.PathFinderNode()
                            {
                                X = list1[index].X - 1,
                                Y = list1[index].Y,
                                NextNode = (PathFinder.PathFinderNode)null,
                                Heuristic = pathFinderNode2.Heuristic + (int)(byte)Matrix[list1[index].X - 1, list1[index].Y]
                            };
                            pathFinderNode2.NextNode = pathFinderNode3;
                            pathFinderNode3.LastNode = pathFinderNode2;
                            if (list1[index].X - 1 == (int)End.X && list1[index].Y == (int)End.Y)
                            {
                                pathFinderNode1 = pathFinderNode3;
                                break;
                            }
                            flagArray[list1[index].X - 1, list1[index].Y] = true;
                            list2.Add(pathFinderNode3);
                        }
                        if ((list1[index].X + 1 <= Matrix.GetUpperBound(0) && list1[index].X + 1 >= 0 && (!flagArray[list1[index].X + 1, list1[index].Y] && Matrix[list1[index].X + 1, list1[index].Y] != Tile.Wall)) || (list1[index].X + 1 == End.X && list1[index].Y == End.Y))
                        {
                            PathFinder.PathFinderNode pathFinderNode2 = new PathFinder.PathFinderNode()
                            {
                                LastNode = list1[index].LastNode,
                                X = list1[index].X,
                                Y = list1[index].Y,
                                Heuristic = list1[index].Heuristic
                            };
                            PathFinder.PathFinderNode pathFinderNode3 = new PathFinder.PathFinderNode()
                            {
                                X = list1[index].X + 1,
                                Y = list1[index].Y,
                                NextNode = (PathFinder.PathFinderNode)null,
                                Heuristic = pathFinderNode2.Heuristic + (int)(byte)Matrix[list1[index].X + 1, list1[index].Y]
                            };
                            pathFinderNode2.NextNode = pathFinderNode3;
                            pathFinderNode3.LastNode = pathFinderNode2;
                            if (list1[index].X + 1 == (int)End.X && list1[index].Y == (int)End.Y)
                            {
                                pathFinderNode1 = pathFinderNode3;
                                break;
                            }
                            flagArray[list1[index].X + 1, list1[index].Y] = true;
                            list2.Add(pathFinderNode3);
                        }
                        if ((list1[index].Y - 1 <= Matrix.GetUpperBound(1) && list1[index].Y - 1 >= 0 && (!flagArray[list1[index].X, list1[index].Y - 1] && Matrix[list1[index].X, list1[index].Y - 1] != Tile.Wall)) || (list1[index].X == End.X && list1[index].Y - 1 == End.Y))
                        {
                            PathFinder.PathFinderNode pathFinderNode2 = new PathFinder.PathFinderNode()
                            {
                                LastNode = list1[index].LastNode,
                                X = list1[index].X,
                                Y = list1[index].Y,
                                Heuristic = list1[index].Heuristic
                            };
                            PathFinder.PathFinderNode pathFinderNode3 = new PathFinder.PathFinderNode()
                            {
                                X = list1[index].X,
                                Y = list1[index].Y - 1,
                                NextNode = (PathFinder.PathFinderNode)null,
                                Heuristic = pathFinderNode2.Heuristic + (int)(byte)Matrix[list1[index].X, list1[index].Y - 1]
                            };
                            pathFinderNode2.NextNode = pathFinderNode3;
                            pathFinderNode3.LastNode = pathFinderNode2;
                            if (list1[index].X == (int)End.X && list1[index].Y - 1 == (int)End.Y)
                            {
                                pathFinderNode1 = pathFinderNode3;
                                break;
                            }
                            flagArray[list1[index].X, list1[index].Y - 1] = true;
                            list2.Add(pathFinderNode3);
                        }
                        if ((list1[index].Y + 1 <= Matrix.GetUpperBound(1) && list1[index].Y + 1 >= 0 && (!flagArray[list1[index].X, list1[index].Y + 1] && Matrix[list1[index].X, list1[index].Y + 1] != Tile.Wall)) || (list1[index].X == End.X && list1[index].Y + 1 == End.Y))
                        {
                            PathFinder.PathFinderNode pathFinderNode2 = new PathFinder.PathFinderNode()
                            {
                                LastNode = list1[index].LastNode,
                                X = list1[index].X,
                                Y = list1[index].Y,
                                Heuristic = list1[index].Heuristic
                            };
                            PathFinder.PathFinderNode pathFinderNode3 = new PathFinder.PathFinderNode()
                            {
                                X = list1[index].X,
                                Y = list1[index].Y + 1,
                                NextNode = (PathFinder.PathFinderNode)null,
                                Heuristic = pathFinderNode2.Heuristic + (int)(byte)Matrix[list1[index].X, list1[index].Y + 1]
                            };
                            pathFinderNode2.NextNode = pathFinderNode3;
                            pathFinderNode3.LastNode = pathFinderNode2;
                            if (list1[index].X == (int)End.X && list1[index].Y + 1 == (int)End.Y)
                            {
                                pathFinderNode1 = pathFinderNode3;
                                break;
                            }
                            flagArray[list1[index].X, list1[index].Y + 1] = true;
                            list2.Add(pathFinderNode3);
                        }
                    }
                }
                ++num;
            }
            if (pathFinderNode1 == null)
                return (List<PathFinder.PathFinderNode>)null;
            List<PathFinder.PathFinderNode> list3 = new List<PathFinder.PathFinderNode>();
            for (; pathFinderNode1 != null; pathFinderNode1 = pathFinderNode1.LastNode)
                list3.Add(pathFinderNode1);
            list3.Reverse();
            return list3;
        }

        public class PathFinderNode
        {
            public PathFinder.PathFinderNode LastNode;
            public PathFinder.PathFinderNode NextNode;

            public int X { get; set; }

            public int Y { get; set; }

            public int Heuristic { get; set; }
        }
    }
}
