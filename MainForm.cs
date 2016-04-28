using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace ConsoleDA
{
    public partial class MainForm : Form
    {
        public static Server Server { get; private set; }
        public static SortedList<int, ClientTab.Node> mapNodes = new SortedList<int, ClientTab.Node>();
        public Thread mainConsoleThread;

        public MainForm()
        {
            InitializeComponent();
            Options.Load();//
        }

        #region Maps
        public static List<string> PreloadMaps()
        {
            List<string> maps = new List<string>();
            string path = string.Concat(new object[1]
              {
                (object) Options.MapsDirectory
              });
            string searchSeq = string.Concat(new object[3] {
              (object) "lod",
              (object) "*",
              (object) ".map"
           });
            string[] files = Directory.GetFiles(path, searchSeq, SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string fileName = file.Substring(file.IndexOf("lod"));
                string mapNum = fileName.Substring(3);
                int mapNumInt = int.Parse(mapNum.Substring(0, mapNum.Length - 4));
                if (true)
                {
                    MainForm.mapNodes[mapNumInt] = new ClientTab.Node(mapNumInt, new int[2]
                    {
                      int.Parse("100"),
                      int.Parse("100")
                    }, 1 != 0);
                }

                maps.Add(mapNum.ToString());
            }
            return maps;
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainForm.Server = new Server(this);
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private void launchDarkAgesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //ghdfgh
        }

        public void AddTab(ClientTab clientTab)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate()
                {
                    mainTabControl.TabPages.Add(clientTab);
                });
            }
            else
            {
                mainTabControl.TabPages.Add(clientTab);
            }
        }

        public void RemoveTab(ClientTab clientTab)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate()
                {
                   /* if (clientTab.SettingsThread != null)
                    {
                        clientTab.SettingsThread.Abort();
                        clientTab.SettingsThread.Join();
                        clientTab.SettingsThread = (Thread)null;
                    }*/
                    if(clientTab.reg != null)
                        clientTab.reg.Unregister(clientTab.waypointsThreadWaitHandle);
                    clientTab.Dispose();
                });
            }
            else
            {
                /*if (clientTab.SettingsThread != null)
                {
                    clientTab.SettingsThread.Abort();
                    clientTab.SettingsThread.Join();
                    clientTab.SettingsThread = (Thread)null;
                }*/
                if (clientTab.reg != null)
                    clientTab.reg.Unregister(clientTab.waypointsThreadWaitHandle);
                clientTab.Dispose();
            }
        }

        private void launchClientlessDAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
       /* public static void WatcherClientlessThreadLoop(ConsoleDA.Client console)
        {
            Location rogueExit, rogueEntrance, rogueIdle, bankExit, bankEntrance, bankIdle, reHide;
            rogueExit = new Location() { X = 14, Y = 9 };
            rogueEntrance = new Location() { X = 9, Y = 34 };
            rogueIdle = new Location() { X = 2, Y = 1 };
            bankExit = new Location() { X = 6, Y = 14 };
            bankEntrance = new Location() { X = 42, Y = 11 };
            bankIdle = new Location() { };
            reHide = new Location() { X = 0, Y = 0 };

            int iterations = 0;

            Dictionary<uint, ConsoleDA.Entity> nearbyPlayers = new Dictionary<uint, ConsoleDA.Entity>();
            Dictionary<uint, double> oldDistance = new Dictionary<uint, double>();
            Dictionary<uint, double> newDistance = new Dictionary<uint, double>();
            Dictionary<uint, double> movingSerials = new Dictionary<uint, double>();
            while (true)
            {
                try
                {
                    Dictionary<uint, ConsoleDA.Aisling> foundPlayers = console.Base.Players;

                    ConsoleDA.Aisling[] aislingArray = new ConsoleDA.Aisling[foundPlayers.Count];
                    Array.Copy((Array)Enumerable.ToArray<ConsoleDA.Entity>((IEnumerable<ConsoleDA.Entity>)console.Base.Players.Values), (Array)aislingArray, aislingArray.Length);

                    foreach (ConsoleDA.Aisling aisling in aislingArray)
                    {
                        if (!nearbyPlayers.ContainsKey(aisling.Serial))
                            nearbyPlayers.Add(aisling.Serial, aisling);
                    }
                    foreach (KeyValuePair<uint, ConsoleDA.Entity> keyValue in nearbyPlayers)
                    {
                        if (!foundPlayers.ContainsValue(keyValue.Value as ConsoleDA.Aisling))
                            nearbyPlayers.Remove(keyValue.Value.Serial);
                    }
                    foreach (KeyValuePair<uint, ConsoleDA.Entity> keyValue in nearbyPlayers)
                    {
                        double warningDistance = console.Base.DistanceFrom(new Location() { X = rogueEntrance.X, Y = rogueEntrance.Y });
                        if (keyValue.Key != console.Base.Serial && !console.Base.Equals("form") && keyValue.Key != 0)
                            console.console.WriteLine(keyValue.Value.Name + " is near at " + console.Base.DistanceFrom(keyValue.Value.Position) + " distance, and " + warningDistance + " distance to rogue entrace.");
                        if (!oldDistance.ContainsKey(keyValue.Value.Serial))
                            oldDistance.Add(keyValue.Value.Serial, warningDistance);
                        else
                            oldDistance[keyValue.Value.Serial] = warningDistance;
                    }
                    if (newDistance.Count > 0 && iterations % 3 == 0)
                    {
                        newDistance = new Dictionary<uint, double>();
                    }
                    foreach (KeyValuePair<uint, double> keyValue in oldDistance)
                    {
                        double change = 0.0;
                        if (newDistance.ContainsKey(keyValue.Key))
                            change = Math.Abs(newDistance[keyValue.Key] - keyValue.Value);
                        else
                            newDistance.Add(keyValue.Key, keyValue.Value);
                        if (change != 0.0)
                            if (!movingSerials.ContainsKey(keyValue.Key))
                                movingSerials.Add(keyValue.Key, change);
                            else
                                if (movingSerials.ContainsKey(keyValue.Key))
                                    movingSerials.Remove(keyValue.Key);
                    }
                    foreach (KeyValuePair<uint, double> keyValue in movingSerials)
                    {
                        if(keyValue.Key != console.Base.Serial)
                            console.console.WriteLine(nearbyPlayers[keyValue.Key].Name + " moving at " + movingSerials[keyValue.Key]);
                    }

                    iterations++;
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
            }
        }*/
        private void launchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //user this.server.MainForm in clienttab to get back to mainform!!!////
            ConsoleDA.Client console;
            Thread newThread = (Thread)null;
            //Client attach = new Client();
            //Client attach = Enumerable.FirstOrDefault<Client>((IEnumerable<Client>)this.Server.Clients);
            ConsoleDA.Client.Run(this.toolStripTextBoxClientlessUsername.Text, this.toolStripTextBoxClientlessPassword.Text, ref newThread, null, out console);
        }
    }
}