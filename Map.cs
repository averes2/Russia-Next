// Decompiled with JetBrains decompiler
// Type: Dean.Map
// Assembly: Dean, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Dean\Dean.exe

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
  public class Map
  {
    //public static readonly BitArray sotpData = new BitArray(Convert.FromBase64String(XElement.Load(Environment.CurrentDirectory + "\\DAData.xml").Element((XName) "Data").Element((XName) "sotp").Value));
   //maybe try to use a BitArray again using Convert.FromBase64String
    public static byte[] sotpData { get; set; }
    //public static BitArray sotpData { get; set; }
    public Tile[,] Grid;
    public int Height;
    public Tile[,] Matrix;
    public short Number;
    public int Width;
    public ClientTab Tab;

    public Map(ClientTab tab, short number, int width, int height)
    {
      this.Width = width;
      this.Height = height;
      this.Number = number;
      this.Grid = new Tile[width, height];
      this.Matrix = new Tile[width + 1, height + 1];
      this.Tab = tab;
      DateTime now = DateTime.Now;
      bool flag = false;
      do
           if (this.Initlize())
                break;
      while ((DateTime.Now - now).TotalSeconds < 25.0);
      if (!flag)
        ;
    }

    public bool Initlize()
    {
        /*    if (string.IsNullOrEmpty(Program.StartupPath))
            {
              try
              {
                Program.StartupPath = File.ReadAllText(Environment.CurrentDirectory + "\\Settings.txt");
              }
              catch
              {
                return false;
              }
            }*/
              /*if (!Map.LoadSotp(Options.DarkAgesDirectory + "\\ia.dat"))
              {
                  return false;
                  //int num = (int)MessageBox.Show("Dark Ages path is incorrect; map pathfinding functions will not work for this client.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              }*/
              string path = Path.Combine(Options.DarkAgesDirectory, "maps") + "\\lod" + this.Number.ToString((IFormatProvider)CultureInfo.InvariantCulture) + ".map";
      if (!File.Exists(path))
        return false;
      using (BinaryReader binaryReader = new BinaryReader((Stream) new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
      {
        try
        {
          for (ushort index1 = (ushort) 0; (int) index1 < this.Height; ++index1)
          {
            for (ushort index2 = (ushort) 0; (int) index2 < this.Width; ++index2)
            {
              int num = (int) binaryReader.ReadUInt16();
              this.Grid[(int) index2, (int) index1] = !this.isBlock(binaryReader.ReadUInt16(), binaryReader.ReadUInt16()) ? Tile.Empty : Tile.Wall;
            }
          }
        }
        catch (Exception ex)
        {
          return false;
        }
        finally
        {
          binaryReader.Close();
        }
      }
      //Map.UpdateBlocks(this.Tab.Client);
      return true;
    }


    /// <summary>
    /// This must be tested fully before used. Static SOTPdata does work with previous basher bot so no reason
    /// it shouldnt here as well.
    /// </summary>
    /// <param name="iaDatPath"></param>
    /// <returns>Map sotpdata exists</returns>
    public static bool LoadSotp(string iaDatPath)
    {
        //private static readonly BitArray sotpData = new BitArray(Convert.FromBase64String(XElement.Load(Environment.CurrentDirectory + "\\DAData.xml").Element((XName) "Data").Element((XName) "sotp").Value));
        //byte[] toreturn = System.Convert.FromBase64String(XElement.Load(System.Environment.CurrentDirectory + "\\DAData.xml").Element((XName)"Data").Element((XName)"sotp").Value);
        //Map.Sotp = toreturn;
        if (Map.sotpData == null && File.Exists(iaDatPath))
        {
            using (FileStream fileStream = File.Open(iaDatPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BinaryReader binaryReader = new BinaryReader((Stream)fileStream))
                {
                    int count = binaryReader.ReadInt32() - 1;
                    for (int index = 0; index < count; ++index)
                    {
                        int num = binaryReader.ReadInt32();
                        byte[] bytes = binaryReader.ReadBytes(13);
                        binaryReader.ReadInt32();
                        if (Encoding.ASCII.GetString(bytes).StartsWith("sotp.dat\0"))
                        {
                            fileStream.Position = (long)num;
                            Map.sotpData = binaryReader.ReadBytes(count);
                            //Map.sotpData = new BitArray(Convert.FromBase64String(binaryReader.ReadBytes(count).ToString()));
                            

                            //Encoding.GetEncoding(949).GetBytes(Encoding.GetEncoding(949).GetString(binaryReader.ReadBytes(count)).ToCharArray());
                            break;
                        }
                        fileStream.Position -= 4L;
                    }
                }
            }
        }
        return Map.sotpData != null;
    }

    public bool isBlock(ushort x, ushort y)
    {
        if (Map.sotpData == null)
        {
            Map.LoadSotp(Options.iaDatPath);
        }
        //return (int)x != 0 && (int)Map.sotpData[(int)x - 1] != 0 || (int)y != 0 && (int)Map.sotpData[(int)y - 1] != 0;
        
      if ((int) x == 0 && (int) y == 0)
        return false;
      if ((int) x == 0)
          return Map.sotpData[(int)y - 1] != 0;
      if ((int) y == 0)
          return Map.sotpData[(int)x - 1] != 0;
      return Map.sotpData[(int)x - 1] != 0 || Map.sotpData[(int)y - 1] != 0;
    }

    public static void UpdateBlocks(Client client)
    {
        lock (client.Base.Entitys)
        {
            Entity[] entityArray = new Entity[client.Base.Entitys.Count];
            Array.Copy((Array)Enumerable.ToArray<Entity>((IEnumerable<Entity>)client.Base.Entitys.Values), (Array)entityArray, entityArray.Length);

            foreach (Entity item_0 in entityArray)
            {
                //client.SendMessage(item_0.Type.ToString() + " an entity!? at " + item_0.Position.X + ", " + item_0.Position.Y, (byte)0);
                try
                {
                    if (item_0 != null)
                    {
                        if ((int)item_0.Serial != (int)client.Base.Me.Serial)
                        {
                            if (item_0.Type == EntityType.NPC)
                            {
                                if (item_0.Type != EntityType.Pet)
                                {
                                    if (item_0.Map == client.Base.DaMap.Number)
                                    {
                                        if (item_0.Type == EntityType.Item)
                                            continue;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    continue;
                            }
                            client.Base.Map[item_0.Position.X, item_0.Position.Y] = Tile.Wall;
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
  }
}

