// Decompiled with JetBrains decompiler
// Type: s2.Options
// Assembly: s2, Version=1.0.1.3, Culture=neutral, PublicKeyToken=null
// MVID: 43510899-C91A-4623-9F79-2D24FD18A057
// Assembly location: C:\Users\GOON\Downloads\s2(1)\s2.exe

using System;
using System.IO;
using System.Xml.Linq;

namespace ConsoleDA
{
    public static class Options
    {
        public static string DarkAgesPath { get; set; }

        public static string DataPath { get; set; }

        public static string iaDatPath
        {
            get
            {
                return Options.DarkAgesDirectory + "\\ia.dat";
            }
        }
        public static string DarkAgesDirectory
        {
            get
            {
                return Path.GetDirectoryName(Options.DarkAgesPath);
            }
        }

        public static string MapsDirectory
        {
            get
            {
                return Path.Combine(Options.DataPath, "maps");
            }
        }

        static Options()
        {
            Options.DarkAgesPath = "C:\\Program Files (x86)\\KRU\\Dark Ages 3\\DeanAgesDall.exe";
            Options.DataPath = "C:\\Program Files (x86)\\KRU\\Dark Ages 3";
        }

        public static void Load()
        {
            if (!File.Exists(Program.StartupPath + "\\settings.xml"))
                return;
            XDocument xdocument = XDocument.Load(Program.StartupPath + "\\settings.xml");
            if (xdocument.Element((XName)"Settings") == null)
                return;
            XElement xelement = xdocument.Element((XName)"Settings");
            if (xelement.Element((XName)"DarkAgesPath") != null)
                Options.DarkAgesPath = xelement.Element((XName)"DarkAgesPath").Value;
            if (xelement.Element((XName)"DataPath") == null)
                return;
            Options.DataPath = xelement.Element((XName)"DataPath").Value;
        }

        /*public static void Save()
        {
          XDocument xdocument = new XDocument();
          xdocument.Add((object) new XElement((XName) "Settings", new object[2]
          {
            (object) new XElement((XName) "DarkAgesPath", (object) Options.DarkAgesPath),
            (object) new XElement((XName) "DataPath", (object) Options.DataPath)
          }));
          xdocument.Save(Program.StartupPath + "\\settings.xml");
        }*/
        /*public static void Load()
        {
          Options.DarkAgesPath = "C:\\Program Files (x86)\\KRU\\Dark Ages\\Darkages.exe";
          Options.DataPath = "C:\\Program Files (x86)\\KRU\\Dark Ages\\";
          /*if (!File.Exists(Program.StartupPath + "\\settings.xml"))
            return;
          XDocument xdocument = XDocument.Load(Program.StartupPath + "\\settings.xml");
          //XDocument xdocument;
          if (xdocument.Element((XName) "Settings") == null)
            return;
          if (xdocument.Element((XName) "Settings").Element((XName) "DarkAgesPath") != null)
            Options.DarkAgesPath = xdocument.Element((XName) "Settings").Element((XName) "DarkAgesPath").Value;
          if (xdocument.Element((XName) "Settings").Element((XName) "DataPath") == null)
            return;
          Options.DataPath = xdocument.Element((XName) "Settings").Element((XName) "DataPath").Value;
        }*/

        public static void Save()
        {
            XDocument xdocument = new XDocument();
            xdocument.Add((object)new XElement((XName)"Settings"));
            xdocument.Element((XName)"Settings").Add((object)new XElement((XName)"DarkAgesPath", (object)Options.DarkAgesPath));
            xdocument.Element((XName)"Settings").Add((object)new XElement((XName)"DataPath", (object)Options.DataPath));
            xdocument.Save(Program.StartupPath + "\\settings.xml");
        }
    }
}
