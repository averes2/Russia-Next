// Decompiled with JetBrains decompiler
// Type: MyDAMapLib.MyDAMap
// Assembly: DAWalkerWorldMapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4D5A0B7E-C7CF-42F2-8A86-EC18BB1CD35B
// Assembly location: C:\Users\adm\Downloads\DAWalker\DAWalkerWorldMapEditor.exe

//using MyResources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleDA
{
    public class MyDAMap
    {
        public Client Client;

        public bool TryMakeDAMapRhombusImage(string mapFile, ushort mapDimX, ushort mapDimY, double scale, Color bgColor, PixelFormat pixelFormat, out Bitmap daMapImage)
        {
            ushort num1 = (ushort)(10.0 * scale);
            ushort num2 = (ushort)(5.0 * scale);
            if ((int)num1 < 2 || (int)num2 < 1)
            {
                num1 = (ushort)2;
                num2 = (ushort)1;
            }
            int width = (int)num1 * ((int)mapDimX + 2 + (int)mapDimY);
            int height = (int)num2 * ((int)mapDimX + 2 + (int)mapDimY);
            if (width * height > 15000000)
            {
                if (MessageBox.Show("Attempting to make a " + width.ToString((IFormatProvider)CultureInfo.InvariantCulture) + " by " + height.ToString((IFormatProvider)CultureInfo.InvariantCulture) + " dimention image...  Is this intended? Propabably Not! Suggestion:  Change Scale Value, or Change Dimention (If Rhombus).  Continue?", "Warning, Continue?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    daMapImage = (Bitmap)null;
                    return false;
                }
            }
            daMapImage = new Bitmap(width, height, pixelFormat);
            using (Graphics graphics = Graphics.FromImage((Image)daMapImage))
            {
                using (SolidBrush solidBrush = new SolidBrush(bgColor))
                    graphics.FillRectangle((Brush)solidBrush, 0, 0, daMapImage.Width, daMapImage.Height);
                bool[,] makeDaMapBool = this.GetMakeDAMapBool(mapFile, mapDimX, mapDimY);
                if (makeDaMapBool == null)
                {
                    daMapImage = (Bitmap)null;
                    return false;
                }
                using (TextureBrush textureBrush = new TextureBrush((Image)Resources.texTRANS, WrapMode.Tile))
                {
                    for (ushort index1 = (ushort)0; (int)index1 < (int)mapDimY; ++index1)
                    {
                        for (ushort index2 = (ushort)0; (int)index2 < (int)mapDimX; ++index2)
                        {
                            if (makeDaMapBool[(int)index2, (int)index1])
                            {
                                Point[] points = new Point[4]
                {
                  new Point((int) num1 * ((int) mapDimY + 1 - (int) index1 + (int) index2), (int) num2 * ((int) index2 + (int) index1)),
                  new Point((int) num1 * ((int) mapDimY + 2 - (int) index1 + (int) index2), (int) num2 * ((int) index2 + (int) index1 + 1)),
                  new Point((int) num1 * ((int) mapDimY + 1 - (int) index1 + (int) index2), (int) index2 * (int) num2 + (int) index1 * (int) num2 + (int) num1),
                  new Point((int) num1 * ((int) mapDimY - (int) index1 + (int) index2), (int) num2 * ((int) index2 + (int) index1 + 1))
                };
                                graphics.FillPolygon((Brush)textureBrush, points);
                                if ((int)index2 > 0 && !makeDaMapBool[(int)index2 - 1, (int)index1])
                                    graphics.DrawLine(new Pen(Color.White), points[3], points[0]);
                                if ((int)index1 > 0 && !makeDaMapBool[(int)index2, (int)index1 - 1])
                                    graphics.DrawLine(new Pen(Color.White), points[0], points[1]);
                                if ((int)index2 < (int)mapDimX - 1 && !makeDaMapBool[(int)index2 + 1, (int)index1])
                                    graphics.DrawLine(new Pen(Color.White), points[1], points[2]);
                                if ((int)index1 < (int)mapDimY - 1 && !makeDaMapBool[(int)index2, (int)index1 + 1])
                                    graphics.DrawLine(new Pen(Color.White), points[2], points[3]);
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool TryMakeDAMapSquareImage(string mapFilePath, ushort mapDimX, ushort mapDimY, double scale, Color bgColor, PixelFormat pixelFormat, out Bitmap daMapImage)
        {
            ushort num = (ushort)(12.0 * scale);
            if ((int)num < 1)
                num = (ushort)1;
            int width = (int)mapDimX * (int)num;
            int height = (int)mapDimY * (int)num;
            if (width * height > 15000000)
            {
                if (MessageBox.Show("Attempting to make a " + width.ToString((IFormatProvider)CultureInfo.InvariantCulture) + " by " + height.ToString((IFormatProvider)CultureInfo.InvariantCulture) + " dimention image...  Is this intended? Propabably Not! Suggestion:  Change Scale Value, or Change Dimention (If Rhombus).  Continue?", "Warning, Continue?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    daMapImage = (Bitmap)null;
                    return false;
                }
            }
            daMapImage = new Bitmap(width, height, pixelFormat);
            using (Graphics graphics = Graphics.FromImage((Image)daMapImage))
            {
                using (SolidBrush solidBrush = new SolidBrush(bgColor))
                    graphics.FillRectangle((Brush)solidBrush, 0, 0, daMapImage.Width, daMapImage.Height);
                bool[,] makeDaMapBool = this.GetMakeDAMapBool(mapFilePath, mapDimX, mapDimY);
                if (makeDaMapBool == null)
                {
                    daMapImage = (Bitmap)null;
                    return false;
                }
                using (TextureBrush textureBrush = new TextureBrush((Image)Resources.texTRANS, WrapMode.Tile))
                {
                    for (ushort index1 = (ushort)0; (int)index1 < (int)mapDimY; ++index1)
                    {
                        for (ushort index2 = (ushort)0; (int)index2 < (int)mapDimX; ++index2)
                        {
                            if (makeDaMapBool[(int)index2, (int)index1])
                            {
                                graphics.FillRectangle((Brush)textureBrush, (int)index2 * (int)num, (int)index1 * (int)num, (int)num, (int)num);
                                if ((int)index2 > 0 && !makeDaMapBool[(int)index2 - 1, (int)index1])
                                    graphics.DrawLine(new Pen(Color.White), (int)index2 * (int)num, (int)index1 * (int)num, (int)index2 * (int)num, ((int)index1 + 1) * (int)num);
                                if ((int)index1 > 0 && !makeDaMapBool[(int)index2, (int)index1 - 1])
                                    graphics.DrawLine(new Pen(Color.White), (int)index2 * (int)num, (int)index1 * (int)num, ((int)index2 + 1) * (int)num, (int)index1 * (int)num);
                                if ((int)index2 < (int)mapDimX - 1 && !makeDaMapBool[(int)index2 + 1, (int)index1])
                                    graphics.DrawLine(new Pen(Color.White), ((int)index2 + 1) * (int)num, (int)index1 * (int)num, ((int)index2 + 1) * (int)num, ((int)index1 + 1) * (int)num);
                                if ((int)index1 < (int)mapDimY - 1 && !makeDaMapBool[(int)index2, (int)index1 + 1])
                                    graphics.DrawLine(new Pen(Color.White), (int)index2 * (int)num, ((int)index1 + 1) * (int)num, ((int)index2 + 1) * (int)num, ((int)index1 + 1) * (int)num);
                            }
                        }
                    }
                }
                Aisling[] aislingArray = new Aisling[this.Client.Base.Aislings.Count];
                Array.Copy((Array)Enumerable.ToArray<Aisling>((IEnumerable<Aisling>)this.Client.Base.Aislings.Values), (Array)aislingArray, aislingArray.Length);

                Entity[] entityArray = new Entity[this.Client.Base.Entitys.Count];
                Array.Copy((Array)Enumerable.ToArray<Entity>((IEnumerable<Entity>)this.Client.Base.Entitys.Values), (Array)entityArray, entityArray.Length);
                    
                
                foreach (Aisling playerOnMap in aislingArray)
                {
                    if (playerOnMap.Serial == this.Client.Base.Serial)
                    {
                        using (TextureBrush meBrush = new TextureBrush((Image)Resources.textME, WrapMode.Tile))
                        {
                            graphics.FillRectangle((Brush)meBrush, (int)this.Client.Base.MyServerPosition.X * (int)num, (int)this.Client.Base.MyServerPosition.Y * (int)num, (int)num, (int)num);
                        }
                    }
                    else if (playerOnMap.Map == this.Client.Base.DaMap.Number && playerOnMap.Map != playerOnMap.lastMap)
                    {
                        using(TextureBrush playerBrush = new TextureBrush((Image)Resources.textPLAYER,WrapMode.Tile)) 
                        {
                            graphics.FillRectangle((Brush)playerBrush, (int)playerOnMap.Position.X * (int)num, (int)playerOnMap.Position.Y * (int)num, (int)num, (int)num);
                        }
                    }
                    
                }
                foreach (Entity entityOnMap in entityArray)
                {
                    if (!this.Client.Base.Aislings.ContainsKey(entityOnMap.Serial))
                    {
                        if (entityOnMap.Type == EntityType.Monster && entityOnMap.Serial != this.Client.Base.Me.Serial)
                        {
                            using (TextureBrush monsterBrush = new TextureBrush((Image)Resources.textMONSTER, WrapMode.Tile))
                            {
                                graphics.FillRectangle((Brush)monsterBrush, (int)entityOnMap.Position.X * (int)num, (int)entityOnMap.Position.Y * (int)num, (int)num, (int)num);
                            }
                        }
                        else if (entityOnMap.Type == EntityType.Item && entityOnMap.Serial != this.Client.Base.Me.Serial)
                        {
                            using (TextureBrush itemBrush = new TextureBrush((Image)Resources.textITEM, WrapMode.Tile))
                            {
                                graphics.FillRectangle((Brush)itemBrush, (int)entityOnMap.Position.X * (int)num, (int)entityOnMap.Position.Y * (int)num, (int)num, (int)num);
                            }
                        }
                        else if ((entityOnMap.Type == EntityType.NPC || entityOnMap.Type == EntityType.Pet) && entityOnMap.Serial != this.Client.Base.Me.Serial)
                        {
                            using (TextureBrush specialBrush = new TextureBrush((Image)Resources.textSPECIALENTITY, WrapMode.Tile))
                            {
                                graphics.FillRectangle((Brush)specialBrush, (int)entityOnMap.Position.X * (int)num, (int)entityOnMap.Position.Y * (int)num, (int)num, (int)num);
                            }
                        }
                        else if (entityOnMap.Serial != this.Client.Base.Me.Serial)
                        {
                            using (TextureBrush otherBrush = new TextureBrush((Image)Resources.textOTHER, WrapMode.Tile))
                            {
                                graphics.FillRectangle((Brush)otherBrush, (int)entityOnMap.Position.X * (int)num, (int)entityOnMap.Position.Y * (int)num, (int)num, (int)num);
                            }
                        }
                    }
                }
                foreach (KeyValuePair<int, List<Location>> waypoints in this.Client.Base.Waypoints)
               {
                   if (waypoints.Key == this.Client.Base.DaMap.Number)
                   {
                       foreach (Location waypoint in waypoints.Value)
                       {
                           using (TextureBrush waypointBrush = new TextureBrush((Image)Resources.textWAYPOINT, WrapMode.Tile))
                           {
                               graphics.FillRectangle((Brush)waypointBrush, (int)waypoint.X * (int)num, (int)waypoint.Y * (int)num, (int)num, (int)num);
                           }
                       }
                       break;
                   }
               }
              foreach (KeyValuePair<int, List<Location>> blocks in this.Client.Base.Blocks)
               {
                   if (blocks.Key == this.Client.Base.DaMap.Number)
                   {
                       foreach (Location block in blocks.Value)
                       {
                           using (TextureBrush blockBrush = new TextureBrush((Image)Resources.textBLOCK, WrapMode.Tile))
                           {
                               graphics.FillRectangle((Brush)blockBrush, (int)block.X * (int)num, (int)block.Y * (int)num, (int)num, (int)num);
                           }
                       }
                       break;
                   }
               }
               foreach (KeyValuePair<int, List<Location>> doors in this.Client.Base.Doors)
               {
                   if (doors.Key == this.Client.Base.DaMap.Number)
                   {
                       foreach (Location door in doors.Value)
                       {
                           using (TextureBrush doorBrush = new TextureBrush((Image)Resources.textDOOR, WrapMode.Tile))
                           {
                               graphics.FillRectangle((Brush)doorBrush, (int)door.X * (int)num, (int)door.Y * (int)num, (int)num, (int)num);
                           }
                       }
                       break;
                   }
               }
            }
            return true;
        }

        public byte[,] GetMakeDAMapByte(string daMapFilePath, ushort dimX, ushort dimY)
        {
            FileInfo fileInfo = new FileInfo(daMapFilePath);
            if (!fileInfo.Exists || fileInfo.Length / 6L != (long)((int)dimX * (int)dimY))
                return (byte[,])null;
            byte[,] numArray = new byte[(int)dimX, (int)dimY];
            using (BinaryReader binaryReader = new BinaryReader((Stream)new FileStream(daMapFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                for (ushort index1 = (ushort)0; (int)index1 < (int)dimY; ++index1)
                {
                    for (ushort index2 = (ushort)0; (int)index2 < (int)dimX; ++index2)
                    {
                        int num = (int)binaryReader.ReadUInt16();
                        if (this.isBlock(binaryReader.ReadUInt16(), binaryReader.ReadUInt16()))
                            numArray[(int)index2, (int)index1] = (byte)1;
                    }
                }
            }
            return numArray;
        }

        public char[,] GetMakeDAMapChar(string daMapFilePath, ushort dimX, ushort dimY)
        {
            FileInfo fileInfo = new FileInfo(daMapFilePath);
            if (!fileInfo.Exists || fileInfo.Length / 6L != (long)((int)dimX * (int)dimY))
                return (char[,])null;
            char[,] chArray = new char[(int)dimX, (int)dimY];
            using (BinaryReader binaryReader = new BinaryReader((Stream)new FileStream(daMapFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                for (ushort index1 = (ushort)0; (int)index1 < (int)dimY; ++index1)
                {
                    for (ushort index2 = (ushort)0; (int)index2 < (int)dimX; ++index2)
                    {
                        int num = (int)binaryReader.ReadUInt16();
                        if (this.isBlock(binaryReader.ReadUInt16(), binaryReader.ReadUInt16()))
                            chArray[(int)index2, (int)index1] = 'W';
                    }
                }
            }
            return chArray;
        }

        public bool[,] GetMakeDAMapBool(string daMapFilePath, ushort dimX, ushort dimY)
        {
            FileInfo fileInfo = new FileInfo(daMapFilePath);
            if (!fileInfo.Exists || fileInfo.Length / 6L != (long)((int)dimX * (int)dimY))
                return (bool[,])null;
            bool[,] flagArray = new bool[(int)dimX, (int)dimY];
            using (BinaryReader binaryReader = new BinaryReader((Stream)new FileStream(daMapFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                for (ushort index1 = (ushort)0; (int)index1 < (int)dimY; ++index1)
                {
                    for (ushort index2 = (ushort)0; (int)index2 < (int)dimX; ++index2)
                    {
                        int num = (int)binaryReader.ReadUInt16();
                        if (this.isBlock(binaryReader.ReadUInt16(), binaryReader.ReadUInt16()))
                            flagArray[(int)index2, (int)index1] = true;
                    }
                }
            }
            return flagArray;
        }

        private bool isBlock(ushort x, ushort y)
        {
            if (Map.sotpData == null)
            {
                Map.LoadSotp(Options.iaDatPath);
            }
            //return (int)x != 0 && (int)Map.sotpData[(int)x - 1] != 0 || (int)y != 0 && (int)Map.sotpData[(int)y - 1] != 0;
            /*if ((int)x == 0 && (int)y == 0)
                return false;
            if ((int)x == 0)
                return Map.sotpData[(int)y - 1] != 0;
            if ((int)y == 0)
                return Map.sotpData[(int)x - 1] != 0;
            if (!(Map.sotpData[(int)x - 1] != 0))
                return Map.sotpData[(int)y - 1] != 0;*/
            if ((int)x == 0 && (int)y == 0)
                return false;
            if ((int)x == 0)
                return Map.sotpData[(int)y - 1] != 0;
            if ((int)y == 0)
                return Map.sotpData[(int)x - 1] != 0;
            return Map.sotpData[(int)x - 1] != 0 || Map.sotpData[(int)y - 1] != 0;
            //return true;
        }

        public static bool TryColorBlockRhombus(int[] portals, ushort mapDimX, ushort mapDimY, Color fillColor, double scale, ref Bitmap daMapImage)
        {
            if (daMapImage == null)
                return false;
            ushort num1 = (ushort)(10.0 * scale);
            ushort num2 = (ushort)(5.0 * scale);
            if ((int)num1 < 2 || (int)num2 < 1)
            {
                num1 = (ushort)2;
                num2 = (ushort)1;
            }
            using (Graphics graphics = Graphics.FromImage((Image)daMapImage))
            {
                using (SolidBrush solidBrush = new SolidBrush(fillColor))
                {
                    byte num3 = (byte)0;
                    while ((int)num3 < portals.Length - 1)
                    {
                        if (portals[(int)num3] > (int)mapDimX || portals[(int)num3 + 1] > (int)mapDimY)
                            return false;
                        graphics.FillPolygon((Brush)solidBrush, new Point[4]
            {
              new Point((int) num1 * ((int) mapDimY + 1 - portals[(int) num3 + 1] + portals[(int) num3]), (int) num2 * (portals[(int) num3] + portals[(int) num3 + 1])),
              new Point((int) num1 * ((int) mapDimY + 2 - portals[(int) num3 + 1] + portals[(int) num3]) - 1, (int) num2 * (portals[(int) num3] + portals[(int) num3 + 1] + 1)),
              new Point((int) num1 * ((int) mapDimY + 1 - portals[(int) num3 + 1] + portals[(int) num3]), portals[(int) num3] * (int) num2 + portals[(int) num3 + 1] * (int) num2 + (int) num1 - 1),
              new Point((int) num1 * ((int) mapDimY - portals[(int) num3 + 1] + portals[(int) num3]) + 1, (int) num2 * (portals[(int) num3] + portals[(int) num3 + 1] + 1))
            });
                        num3 += (byte)2;
                    }
                }
            }
            return true;
        }

        public static bool TryColorBlockSquare(ushort mapDimX, ushort mapDimY, double scale, Color bgColor, ref Bitmap daMapImage)
        {
            if (daMapImage == null)
                return false;
            ushort num1 = (ushort)(12.0 * scale);
            if ((int)num1 < 1)
                num1 = (ushort)1;
            using (Graphics graphics = Graphics.FromImage((Image)daMapImage))
            {
                if ((mapDimX + 1) * (int)num1 > daMapImage.Width || (mapDimY + 1) * (int)num1 > daMapImage.Height)
                            return false;
                        using (TextureBrush otherBrush = new TextureBrush((Image)Resources.textWAYPOINT, WrapMode.Tile))
                        {
                            graphics.FillRectangle((Brush)otherBrush, mapDimX * (int)num1 + 1, mapDimY * (int)num1 + 1, (int)num1 - 1, (int)num1 - 1);
                        }
            }
            return true;
        }

        public static List<ushort[]> GetFactors(string mapFilePath)
        {
            FileInfo fileInfo = new FileInfo(mapFilePath);
            if (!fileInfo.Exists)
                return (List<ushort[]>)null;
            int a = (int)(fileInfo.Length / 6L);
            double num1 = Math.Sqrt((double)a);
            List<ushort[]> list = new List<ushort[]>();
            for (byte index = (byte)1; (double)index <= num1; ++index)
            {
                int result;
                int num2 = Math.DivRem(a, (int)index, out result);
                if (result == 0)
                    list.Add(new ushort[2]
          {
            (ushort) index,
            (ushort) num2
          });
            }
            foreach (ushort[] numArray in list.ToArray())
            {
                if ((int)numArray[0] != (int)numArray[1])
                    list.Add(new ushort[2]
          {
            numArray[1],
            numArray[0]
          });
            }
            list.Reverse((int)Math.Ceiling((double)list.Count / 2.0), list.Count / 2);
            return list;
        }
    }
}
