using BuildingXYAdj_Concept;
using devDept.Eyeshot;
using devDept.Eyeshot.Labels;
using devDept.Geometry;
using DevExpress.ExpressApp.Win.Templates.Controls;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using ConsoleDA.Hunting;
using System.IO;
using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Runtime.Remoting.Messaging;
using System.Data;
using System.Reflection;
using ConsoleDA;

namespace ConsoleDA
{
    public partial class ClientTab : TabPage//change to : TabPage for debug/release, : UserControl for designer (replace with creating a tabpage and add the usercontrol to it)
    {
        //private MyMemoryReadSearch pReader = new MyMemoryReadSearch();
        #region Class members
        private MapPreview MapPreviewForm;
        private List<ushort[]> myFactors = new List<ushort[]>();
        private MyDAMap myDAMap = new MyDAMap();
        private Bitmap DAMapImage;
        public Color colorWaypoint = Color.FromArgb(255, 255, 255, 0);
        public Color colorDoor = Color.FromArgb(255, 0, 100, 100);
        public Color colorBlock = Color.FromArgb(255, 125, 50, 0);
        public Color colorWalk = Color.FromArgb(255, 255, 255, 255);

        public Client Client { get; set; }
        public Thread RunningThread = (Thread)null;
        public Thread SettingsThread = (Thread)null;
        public Thread PrimaryThread = (Thread)null;
        public Script RunningScript;
        private Thread addonThread = (Thread)null;

        public Dictionary<Thread, ConsoleDA.Client> clientlessClients = new Dictionary<Thread, ConsoleDA.Client>();
        public Thread ClientlessThread = (Thread)null;
        public Thread WalkThread = (Thread)null;
        public Thread WaypointsThread = (Thread)null;
        private Thread RefreshThread = (Thread)null;

        public Location walkingLocation = null;
        public Location offScreenLocation { get; set; }
        public uint followSerial = 0;
        public int walkMsDelay = 385;
        public double walkDistanceOffset = 0;
        #endregion

        #region constructor/initial load event
        public ClientTab(Client client)
        {
            InitializeComponent();
            this.textBoxMapFolder.Text = Options.DarkAgesDirectory;
            this.Client = client;
            this.ClearWorld();
            this.OpenWorldMap();
            this.MapPreviewForm = new MapPreview(this);

        }

        private void clientTab_Load(object sender, EventArgs e)
        {

            #region Persistent settings
            /*persistentJSON j = new persistentJSON();
            j.LoadCharacterControls(this);
            List<Control> clientControls = new List<Control>();
            this.RecursiveAllControls(this, ref clientControls);
            //CheckBoxRadioButtonListBoxTextBoxNumericUpDownComboBox TrackBar 
            foreach (Control control in clientControls)
            {
                if (control.GetType() == typeof(CheckBox))
                {
                    (control as CheckBox).Click += new System.EventHandler(this.savePersistentSettings_click);
                }
                else if (control.GetType() == typeof(RadioButton))
                {
                    (control as RadioButton).Click += new System.EventHandler(this.savePersistentSettings_click);
                }
                else if (control.GetType() == typeof(ListBox))
                {
                    (control as ListBox).SelectedIndexChanged += new System.EventHandler(this.savePersistentSettings_click);
                }
                else if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    (control as System.Windows.Forms.ComboBox).SelectedIndexChanged += new System.EventHandler(this.savePersistentSettings_click);
                }
                else if (control.GetType() == typeof(TextBox))
                {
                    (control as TextBox).TextChanged += new System.EventHandler(this.savePersistentSettings_click);
                }
                else if (control.GetType() == typeof(NumericUpDown))
                {
                    (control as NumericUpDown).ValueChanged += new System.EventHandler(this.savePersistentSettings_click);
                }
                else if (control.GetType() == typeof(TrackBar))
                {
                    (control as TrackBar).ValueChanged += new System.EventHandler(this.savePersistentSettings_click);
                }
            }*/
            #endregion


            if (!this.pathNone.Checked)
            {
                if (this.WalkThread == null)
                {
                    this.WalkThread = new Thread(new ThreadStart(this.Walker));
                    this.WalkThread.Start();

                }
            }

            //ThreadPool.QueueUserWorkItem(new WaitCallback(delagateUpdateMap));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(delagateUpdateDynamicView));
            factory.StartNew(this.delagateUpdateMap);

            this.clientStart.Enabled = true;
            this.clientStop.Enabled = false;
            if (this.RunningThread == null)
                return;
            this.RunningThread.Abort();
            this.RunningThread.Join();
            this.RunningThread = (Thread)null;
        }
        #endregion

        #region Thread Wait Handles
        public ManualResetEvent waypointsThreadWaitHandle = new ManualResetEvent(true);
        public ManualResetEvent walkThreadWaitHandle = new ManualResetEvent(true);
        public ManualResetEvent updateMapThreadWaitHandle = new ManualResetEvent(true);
        /*Its magically saving our position in the thread, were aborting the thread, and even when we set it to null
         it STILL remembered where we were in the thread. WHY does it do this? We need to figure this out in order to
         properly save our position in our current waypoint path*/
        

        public void pauseWalk()
        {
            walkThreadWaitHandle.Reset();
        }
        public void resumeWalk()
        {
            walkThreadWaitHandle.Set();
        }
        public void pauseDynamicMap()
        {
            updateMapThreadWaitHandle.Reset();
        }
        public void resumeDynamicMap()
        {
            updateMapThreadWaitHandle.Set();
        }
        #endregion

        #region Utility
        public void Refresher()
        {
            while (this.Client.access)
            {
                if (this.RunningThread != (Thread)null)
                    this.Refresh();

                if (this.RefreshThread != (Thread)null)
                {
                    this.RefreshThread.Abort();
                    this.RefreshThread.Join();
                    this.RefreshThread = (Thread)null;
                }
            }
        }
        #endregion

        #region Primary Walker Thread Functions
        public object syncObj = new object();
        private bool paused;

        #region Waypoint Walker
        public delegate void WaypointsDelegate();
        public void delagateWaypointsWalker()
        {
            // (Omitted)
            // Do something important using 'state'.

            // Tell the UI we are done.
            while (this.Client.access)
            {
                //waypointsThreadWaitHandle.WaitOne();
                try
                {
                    // Invoke the delegate on the form.
                    //if (InvokeRequired)
                    //    Invoke((MethodInvoker)delegate() { waypointsWalker(); });
                    //else
                    WaypointsDelegate wd = new WaypointsDelegate(delegateWaypoints);
                    IAsyncResult ar = wd.BeginInvoke(asyncDelegateWaypoints, wd);
                    ar.AsyncWaitHandle.WaitOne();
                    if (ar.IsCompleted)
                        wd.EndInvoke(ar);
                }
                catch
                {
                    // Some problem occurred but we can recover.
                }
                Thread.Sleep(25);
            }
        }
        public void delegateWaypoints()
        {
            //waypointsThreadWaitHandle.WaitOne();
            //if (this.RunningThread != (Thread)null)
            this.Client.Base.Travel();
        }
        public void asyncDelegateWaypoints(IAsyncResult ar)
        {
            //if (this.RunningThread != (Thread)null)
            //    this.Client.Base.Travel();
            //waypointsThreadWaitHandle.WaitOne();
            WaypointsDelegate fd = (WaypointsDelegate)((AsyncResult)ar).AsyncDelegate;
            //fd.EndInvoke(ar);
        }
        private delegate void waypointDelegate();
        public void pauseWaypoints()
        {
            if (this.paused == false)
            {
                Monitor.Enter(syncObj);
                this.paused = true;
            }
            //this.waypointsThreadWaitHandle.Reset();
            //this.WaypointsThread.Abort();
            //this.WaypointsThread.Join();
            //this.WaypointsThread = (Thread)null;
        }
        public void resumeWaypoints()
        {
            if (paused) 
            {
                paused = false;
                Monitor.Exit(syncObj);
            }
            //this.waypointsThreadWaitHandle.Set();
            //this.WaypointsThread = new Thread(new ThreadStart(this.waypointsWalker));
            //this.WaypointsThread.IsBackground = true;
            //this.WaypointsThread.Start();
        }
        #endregion
        public void Walker()
        {
            while (this.Client.access)
            {
                if (/*this.RunningThread != (Thread)null &&*/ this.walkingLocation != null)
                {
                    //Map.UpdateBlocks(this.Client);
                    if (this.Client.Base.DistanceFrom(this.walkingLocation) > this.walkDistanceOffset)
                        this.Walk(this.walkingLocation, this.walkDistanceOffset);
                }
                Thread.Sleep(25);
            }
        }
        
        /*  public void waypointsWalker(object state)
        {
            while (true)
            {
                waypointsThreadWaitHandle.WaitOne();
                while (this.Client.access)
                {
                    if (this.RunningThread != (Thread)null)
                        this.Client.Base.Travel();
                    Thread.Sleep(25);
                }
            }
        }
        public void waypointsWalker()
        {
            while (true)
            {
                waypointsThreadWaitHandle.WaitOne();
                while (this.Client.access)
                {
                    if (this.RunningThread != (Thread)null)
                        this.Client.Base.Travel();
                    Thread.Sleep(25);
                }
            }
        }*/
       
        private void Walk(Location toGo, double distance)
        {
            if (toGo == null)
            {
                try
                {
                    if (!this.pathFollowTarget.Text.Equals(""))
                        toGo = this.Client.Base.Aislings[this.followSerial].Position;
                }
                catch
                {
                }
                finally
                {
                    //this.Defense();
                }
            }
            else
            {
                if (toGo == null)
                    return;
                try
                {
                    List<PathFinder.PathFinderNode> path = null;
                    if (/*(int)basher.Base.DaMap.Number == (int)this.Client.Base.DaMap.Number &&*/ toGo != null /*&& this.Client.Base.FindPath(toGo) != null && !this.Client.Base.WithinRange(toGo, distance)*/)
                    {
                        if ((path = this.Client.Base.FindPath(toGo)) != null)
                        {
                            try
                            {
                                this.Client.Base.WalkPlayerThroughServerPath(path, (int)this.walkDistanceOffset);
                                //this.Client.Base.WalkPlayerToEntity(this.walkingLocation);
                                //this.walkDistanceOffset = !this.pathFollowTarget.Text.Equals("") ? (int)this.pathFollowDistance.Value : 0;
                            }
                            catch
                            {
                            }
                            finally
                            {
                                Thread.Sleep(25);
                                //this.Defense();
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }
        #endregion

        #region Primary loop (break this down)
        void PrimaryLoop()
        {
            this.Client.Base.MyPosition = this.Client.Base.MyServerPosition;
            try
            {
                while (true)
                {
                   // if (this.Client.Base.MyPosition == null || ((this.Client.Base.MyPosition.X > 60000 || this.Client.Base.MyPosition.X < 0) || (this.Client.Base.MyPosition.Y > 60000 || this.Client.Base.MyPosition.Y < 0)))
                    //    if (this.Client.Base.MyServerPosition != null)
                    //this.Client.Base.MyPosition = this.Client.Base.MyServerPosition;
                    //this.Client.Base.MyPosition.X = this.Client.Base.Entitys[this.Client.Base.Serial].Position.X;
                    //this.Client.Base.MyPosition.Y = this.Client.Base.Entitys[this.Client.Base.Serial].Position.Y;
                    #region Initial Threadsafe Locks
                    //lock (this.Client.Base.threadSafeAislings)
                    //{
                        Aisling aOut;
                        //lock (this.Client.Base.Aislings) 
                        //{
                            foreach (KeyValuePair<uint, Aisling> aisling in this.Client.Base.threadSafeAislings)
                            {
                                if(!this.Client.Base.Aislings.ContainsKey(aisling.Key))
                                    this.Client.Base.threadSafeAislings.TryRemove(aisling.Key, out aOut);
                            }
                        //}
                    //}
                    //lock (this.Client.Base.Aislings)
                    //{
                        foreach (KeyValuePair<uint, Aisling> player in this.Client.Base.Aislings)
                        {
                            this.Client.Base.threadSafeAislings.AddOrUpdate(player.Key, player.Value,
                                (Func<uint, Aisling, Aisling>)((oldkey, oldvalue) => player.Value));
                        }
                    //}
                    //lock (this.Client.Base.threadSafeEntitys)
                    //{
                        Entity eOut;
                        //lock (this.Client.Base.Entitys) 
                        {
                            foreach (KeyValuePair<uint, Entity> entity in this.Client.Base.threadSafeEntitys)
                            {
                                if(!this.Client.Base.Entitys.ContainsKey(entity.Key))
                                    this.Client.Base.threadSafeEntitys.TryRemove(entity.Key, out eOut);
                            }
                        //}
                    //}
                    //lock (this.Client.Base.Entitys)
                    //{
                        foreach (KeyValuePair<uint, Entity> entity in this.Client.Base.Entitys)
                        {
                            this.Client.Base.threadSafeEntitys.AddOrUpdate(entity.Key, entity.Value,
                                (Func<uint, Entity, Entity>)((oldkey, oldvalue) => entity.Value));
                        }
                    }
                    //lock (this.Client.Base.Spells)
                    //{
                        foreach (KeyValuePair<string, Spell> spell in this.Client.Base.Spells)
                        {
                            if (!this.spellsKnownSpellsList.Items.Contains(spell.Key))
                                this.spellsKnownSpellsList.Items.Add(spell.Key + ", " + spell.Value.Lines + " lines");
                        }
                    //}
                    #endregion

                    for (int i = 0; i < this.Client.Base.Waypoints[this.Client.Base.DaMap.Number].Count; i++)
                        if (!this.controlMapWaypoints.Items.Contains(this.Client.Base.Waypoints[this.Client.Base.DaMap.Number][i].X + "," + this.Client.Base.Waypoints[this.Client.Base.DaMap.Number][i].Y))
                            this.controlMapWaypoints.Items.Add(this.Client.Base.Waypoints[this.Client.Base.DaMap.Number][i].X + "," + this.Client.Base.Waypoints[this.Client.Base.DaMap.Number][i].Y);
                    
                    //refresh the client roughly every 6 seconds -- may need major tweaks
                    if ((DateTime.Now - this.Client.Base.MyServerPosition.LastActive).TotalSeconds > 4.0 && (DateTime.Now - this.Client.Base.LastRefreshed).TotalSeconds >= 4.0)
                    {
                        if (this.RefreshThread == null)
                        {
                            this.RefreshThread = new Thread(new ThreadStart(this.Refresher));
                            this.RefreshThread.Start();
                        }
                        this.Client.ShouldUpdateMap = true;
                    }
                    if (this.walkingLocation != null)
                    {
                        this.walkDistanceOffset = 0;
                        if (this.Client.Base.MyServerPosition.X == this.walkingLocation.X && this.Client.Base.MyServerPosition.Y == this.walkingLocation.Y)
                            this.walkingLocation = null;
                    }
                    if (!this.pathFollowTarget.Text.Equals(""))
                    {
                        Aisling followTarget = null;
                        Aisling[] aislingArray = new Aisling[this.Client.Base.Entitys.Count];
                        Array.Copy((Array)Enumerable.ToArray<Entity>((IEnumerable<Entity>)this.Client.Base.Entitys.Values), (Array)aislingArray, aislingArray.Length);

                        lock (Program.SyncObj)
                        {
                            foreach (Aisling item_1 in aislingArray)
                                if (item_1.Name.Equals(pathFollowTarget.Text))
                                {
                                    followTarget = item_1;
                                    this.followSerial = followTarget.Serial;
                                    this.offScreenLocation = followTarget.Position;
                                }
                        }
                        if ((followTarget != null && this.Client.Base.Entitys[followTarget.Serial] != null) || this.offScreenLocation != null)
                        {
                            this.walkDistanceOffset = (double)this.pathFollowDistance.Value;
                            walkingLocation = followTarget != null ? followTarget.Position : this.Client.Base.Aislings[this.followSerial].Position;
                            if (followTarget == null)
                            {
                                this.walkDistanceOffset = 0;
                                //this.walkingLocation = this.offScreenLocation;
                            }
                            if (this.Client.Base.DistanceFrom(this.walkingLocation) <= this.walkDistanceOffset)
                                this.walkingLocation = null;
                            //this.Client.SendMessage("Following: " + this.Client.Base.Aislings[this.followSerial].Position.X + ", " + this.Client.Base.Aislings[this.followSerial].Position.Y, (byte)0);
                            //this.Client.SendMessage("My position: " + this.Client.Base.MyPosition.X + ", " + this.Client.Base.MyPosition.Y, (byte)0);

                        }
                        else
                            this.walkingLocation = null;
                        Thread.Sleep(50);

                    }
                    /*if (Client.ShouldUpdateMap)
                    {
                        mapListGotoCurrent_Click((object)this, null);
                        this.controlCurrentMapLabel.Text = "Current Map: " + this.Client.Base.DaMap.Number;
                        Client.ShouldUpdateMap = false;
                    }*/
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

        #region Dynamic Map View
        private delegate void UpdateMap();
        public void delagateUpdateMap()
        {
            // (Omitted)
            // Do something important using 'state'.

            // Tell the UI we are done.
            while (this.Client.access)
            {
                updateMapThreadWaitHandle.WaitOne();
                try
                {
                    // Invoke the delegate on the form.
                    //if (InvokeRequired)
                    //    Invoke((MethodInvoker)delegate() { waypointsWalker(); });
                    //else
                    UpdateMap wd = new UpdateMap(delegateUpdateMap);
                    object b = new object();
                    IAsyncResult ar = wd.BeginInvoke(new AsyncCallback(asyncDelegateUpdateMap), b);
                    ar.AsyncWaitHandle.WaitOne();
                    if (ar.IsCompleted)
                        wd.EndInvoke(ar);
                }
                catch
                {
                    // Some problem occurred but we can recover.
                }
                Thread.Sleep(25);
            }
        }
        public void delegateUpdateMap() //we need to fix this, why does it crash often when we close it??
        {
            //updateMapThreadWaitHandle.WaitOne();
            //if (this.RunningThread != (Thread)null)
            if(!this.IsDisposed)
                if(!this.Disposing)
                    if(InvokeRequired)
                        this.Invoke(new UpdateMap(updateDynamicView));
        }
        public void asyncDelegateUpdateMap(IAsyncResult ar)
        {
            //if (this.RunningThread != (Thread)null)
            //    this.Client.Base.Travel();
            //waypointsThreadWaitHandle.WaitOne();
            UpdateMap fd = (UpdateMap)((AsyncResult)ar).AsyncDelegate;
            //fd.EndInvoke(ar);
        }

       /* public void delagateUpdateDynamicView(object state)
        {
            // (Omitted)
            // Do something important using 'state'.

            // Tell the UI we are done.
             while (this.Client.access)
             {
                try
                {
                    // Invoke the delegate on the form.
                    this.BeginInvoke(new UpdateMap(updateDynamicView));
                }
                catch
                {
                    // Some problem occurred but we can recover.
                }
                Thread.Sleep(25);
             }
        }*/

        private void updateDynamicView()
        {
                if (this.Client.Base.DaMap != null)
                {
                    string number = this.Client.Base.DaMap.Number.ToString();
                    this.MapPreviewForm.OpenDAMapFile(number);
                    //this.mapDynamicView.Image = new Bitmap(this.mapDynamicView.Image);
                    this.mapDynamicView.Image = this.MapPreviewForm.CreateDABitMap().Clone() as Bitmap;
                    int index = this.listBoxWorldMaps.FindString(number, 0);
                    this.listBoxWorldMaps.SelectedIndex = index;
                    this.controlCurrentMapLabel.Text = "Current Map: " + this.Client.Base.DaMap.Number;
                    
                }
        }
        public void asyncDelegateDynamicMap(IAsyncResult ar)
        {
            //if (this.RunningThread != (Thread)null)
            //    this.Client.Base.Travel();
            //waypointsThreadWaitHandle.WaitOne();
            UpdateMap fd = (UpdateMap)((AsyncResult)ar).AsyncDelegate;
            //this.EndInvoke(ar);
        }

        private void mapListGotoCurrent_Click(object sender, EventArgs e)
        {
            string number = this.Client.Base.DaMap.Number.ToString();
            this.MapPreviewForm.OpenDAMapFile(number);
            this.mapDynamicView.Image = new Bitmap(this.mapDynamicView.Image);
            this.mapDynamicView.Image = this.MapPreviewForm.CreateDABitMap();
            int index = this.listBoxWorldMaps.FindString(number, 0);
            this.listBoxWorldMaps.SelectedIndex = index;
        }

        //this locker may be pointless...
        private static object _mapDynamicUpdateLocker = new object();
        public void PictureBoxMapPreview_MouseClick(object sender, MouseEventArgs e)
        {

            lock (_mapDynamicUpdateLocker)
            {

                Color ourPlacement = new Color();
                if (this.mapAddBlocks.Checked)
                    ourPlacement = this.colorBlock;
                else if (this.mapAddDoors.Checked)
                    ourPlacement = this.colorDoor;
                else if (this.mapAddWaypoints.Checked)
                    ourPlacement = this.colorWaypoint;
                else
                    ourPlacement = this.colorWalk;

                if (this.mapDynamicView.Image == null)
                    return;

                int radius = 6; //Set the number of pixel you wan to use here
                bool unBlocked = true; //make sure were not putting something on a blocked tile if its a waypoint
                //Calculate the numbers based on radius
                Bitmap bm = new Bitmap(this.mapDynamicView.Image);//as Bitmap;
                Rectangle bounds = this.mapDynamicView.Bounds;
                int x0 = Math.Max(e.X - (radius / 2), (bounds.Width - bm.Width) / 2),
                    y0 = Math.Max(e.Y - (radius / 2), (bounds.Height - bm.Height) / 2),
                    x1 = Math.Min(e.X + (radius / 2), ((bounds.Width - bm.Width) / 2) + bm.Width),
                    y1 = Math.Min(e.Y + (radius / 2), ((bounds.Height - bm.Height) / 2) + bm.Height);
                //Get the bitmap (assuming it is stored that way)
                for (int ix = x0; ix < x1; ix++)
                {
                    for (int iy = y0; iy < y1; iy++)
                    {
                        Color currentPixel = bm.GetPixel(ix - (bounds.Width - bm.Width) / 2, iy - (bounds.Height - bm.Height) / 2);
                        //bm.SetPixel(ix - (this.mapDynamicView.Bounds.Width - this.mapDynamicView.Image.Width) / 2, iy - (this.mapDynamicView.Bounds.Height - this.mapDynamicView.Image.Height) / 2, Color.FromArgb(255, 255, 255, 0)); //Change the pixel color, maybe should be relative to bitmap
                        if (currentPixel == this.colorWaypoint || currentPixel == this.colorDoor || currentPixel == this.colorBlock || currentPixel == this.colorWalk)
                        {
                            unBlocked = false;
                        }
                    }
                }

                int realX = -1, realY = -1;
                if (unBlocked)
                {
                    for (int ix = x0; ix < x1; ix++)
                    {
                        for (int iy = y0; iy < y1; iy++)
                        {
                            realX = ix - (bounds.Width - bm.Width) / 2;
                            realY = iy - (bounds.Height - bm.Height) / 2;
                            bm.SetPixel((int)realX, (int)realY, ourPlacement); //Change the pixel color, maybe should be relative to bitmap

                        }
                    }
                }
                realX = realX / 6;
                realY = realY / 6;
                if (realX >= 0 && realY >= 0)
                {
                    Location realLoc = new Location();
                    realLoc.X = (ushort)realX;
                    realLoc.Y = (ushort)realY;
                    if (ourPlacement == this.colorWaypoint)
                        if (this.Client.Base.Waypoints.ContainsKey(this.Client.Base.DaMap.Number))
                            this.Client.Base.Waypoints[this.Client.Base.DaMap.Number].Add(realLoc);
                        else
                        {
                            List<Location> mapWaypoints = new List<Location>();
                            mapWaypoints.Add(realLoc);
                            this.Client.Base.Waypoints.Add(this.Client.Base.DaMap.Number, mapWaypoints);
                        }
                    else if (ourPlacement == this.colorBlock)
                        if (this.Client.Base.Blocks.ContainsKey(this.Client.Base.DaMap.Number))
                            this.Client.Base.Blocks[this.Client.Base.DaMap.Number].Add(realLoc);
                        else
                        {
                            List<Location> mapBlocks = new List<Location>();
                            mapBlocks.Add(realLoc);
                            this.Client.Base.Blocks.Add(this.Client.Base.DaMap.Number, mapBlocks);
                        }
                    else if (ourPlacement == this.colorDoor)
                        if (this.Client.Base.Doors.ContainsKey(this.Client.Base.DaMap.Number))
                            this.Client.Base.Doors[this.Client.Base.DaMap.Number].Add(realLoc);
                        else
                        {
                            List<Location> mapDoors = new List<Location>();
                            mapDoors.Add(realLoc);
                            this.Client.Base.Doors.Add(this.Client.Base.DaMap.Number, mapDoors);
                        }
                    else
                    {
                        List<PathFinder.PathFinderNode> clickedPath = PathFinder.FindPath(this.Client.Base.Map, this.Client.Base.MyServerPosition, realLoc);
                        //this.Client.SendMessage(realX + "," + realY, (byte)0);
                        this.Client.console.WriteLine(realX + "," + realY);
                        //this.Client.Base.WalkPlayerToward(realLoc);
                        this.walkingLocation = realLoc;
                    }
                    this.mapDynamicView.Refresh();//Force refresh
                    //this.Client.Base.WalkPlayerThroughServerPath(clickedPath, 1);
                }
            }

        }

        public void PictureBoxMapPreviewRhombus_MouseClick(object sender, MouseEventArgs e)
        {

        }

        public void controlMapWaypoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MapPreviewForm.CreateDABitMap();
            string[] strArray = new string[0]; ;
            if (this.controlMapWaypoints.Items.Count > 0)
            {
                strArray = ((string)this.controlMapWaypoints.SelectedItem).Split(new char[1]
                      {
                        ','
                      }, StringSplitOptions.RemoveEmptyEntries);
            }
            this.controlEditWaypoint.Text = strArray[0] + "," + strArray[1];        
        }

        private void controlEditWaypoint_TextChanged(Object sender, EventArgs e)
        {
            if (this.controlMapWaypoints.SelectedIndex < 0 || this.controlMapWaypoints.SelectedIndex == null)
                return;
            this.controlMapWaypoints.SelectedItem = this.controlEditWaypoint.Text;
        }

        public void MapDimList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MapPreviewForm.CreateDABitMap();
            string[] strArray = new string[0]; ;
            if (this.mapDimList.Items.Count > 0)
            {
                strArray = ((string)this.mapDimList.SelectedItem).Split(new char[1]
                      {
                        'x'
                      }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (strArray.Length == 2 && strArray[0] != strArray[1])
                this.textBoxMapAxis.Text = strArray[0] + " " + strArray[1];
            else
                this.textBoxMapAxis.Text = "-1 -1";
        }


        private void ButtonOpenMapViewer_Click(object sender, EventArgs e)
        {
            if (this.MapPreviewForm == null)
                this.MapPreviewForm = new MapPreview(this);
            this.MapPreviewForm.OpenDAMapFile(this.textBoxMapNum.Text);
            this.ListBoxWorldMaps_SelectedIndexChanged((object)null, (EventArgs)null);
        }

        private void ListBoxWorldMaps_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.listBoxWorldMaps.DataSource == null)
                return;
            if (this.listBoxWorldMaps.SelectedItem != null)
            {
                int index = ((ClientTab.Node)this.listBoxWorldMaps.SelectedItem).mapNum;
                //this.CheckBoxOtherList.Checked = DAWalkerWorldMapEditorForm.mapNodes[index].otherList;
                //this.textBoxName.Text = DAWalkerWorldMapEditorForm.mapNodes[index].name;
                this.textBoxMapNum.Text = MainForm.mapNodes[index].mapNum.ToString();
                this.textBoxMapAxis.Text = MainForm.mapNodes[index].mapAxis[0].ToString() + " " + MainForm.mapNodes[index].mapAxis[1].ToString();
                //this.ListBoxCurrPortals.DataSource = (object)new List<ClientTab.Node.infoMap>((IEnumerable<DAWalkerWorldMapEditorForm.Node.infoMap>)DAWalkerWorldMapEditorForm.mapNodes[index].infoPortals.Values);
            }
            if (this.MapPreviewForm == null)
                this.MapPreviewForm = new MapPreview(this);
            this.MapPreviewForm.OpenDAMapFile(this.textBoxMapNum.Text);
            this.mapDynamicView.Image = new Bitmap(this.MapPreviewForm.CreateDABitMap());
            //this.ListBoxWorldMaps_SelectedIndexChanged((object)null, (EventArgs)null);
        }
        #endregion

        #region Settings

        public void ResizeAllControls()
        {
            //this.controlTabs.Size = this.Size;
            //this.packetTab.Size = this.controlTabs.Size;

          /*  this.textConsoleOutput.Size = new System.Drawing.Size(this.packetTab.Width, (this.packetTab.Height / 4) * 3);
            this.textConsoleInput.Size = new System.Drawing.Size(this.textConsoleOutput.Width, (this.packetTab.Height / 4));
            //this.packetTab.Size = new System.Drawing.Size(this.controlTabs.Width, this.controlTabs.Height);
            this.textConsoleOutput.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Location = new System.Drawing.Point(0, this.textConsoleOutput.Height);
            this.textConsoleInput.Location = new System.Drawing.Point(textConsoleOutput.Location.X, this.textConsoleOutput.Height + toolStrip2.Height);
            this.buttonSend.Location = new System.Drawing.Point(this.textConsoleInput.Width, this.textConsoleOutput.Height);
            this.buttonRecv.Location = new System.Drawing.Point(this.textConsoleInput.Width + this.buttonSend.Width, this.textConsoleOutput.Height);
            this.toolStrip2.Size = new System.Drawing.Size(this.textConsoleOutput.Width, 25);
            this.buttonSend.Size = new System.Drawing.Size((this.textConsoleInput.Width / 4), this.textConsoleInput.Height);
            this.buttonRecv.Size = new System.Drawing.Size((this.textConsoleInput.Width / 4), this.textConsoleInput.Height);*/
        }

        public void loadPersistentSettings(string characterName)
        {
            characterName = characterName.ToLower();
            persistentJSON persistent = new persistentJSON();
        }

        private bool DoesSettingExist(string settingName)
        {
            return true;
            //return Russia.Properties.Settings.Default.Properties.Cast<System.Configuration.SettingsProperty>().Any(prop => prop.Name == settingName);
        }

        public void RecursiveAllControls(Control parentControl, ref List<Control> controls)
        {
          foreach (Control c in parentControl.Controls)
          {
              controls.Add(c);
              RecursiveAllControls(c, ref controls);
          }
          return;
           //Do whatever to the control here.
        }
        public void savePersistentSettings_click(object sender, EventArgs e) 
        {
            this.savePersistentSettings();
        }
        public void savePersistentSettings()
        {
                persistentJSON js = new persistentJSON();
                js.SaveCharacterControls(this);
        }
        #endregion

        #region Misc (logging etc)
        public void LogIncomingPacket(string format, params object[] args)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate() { LogIncomingPacket(format, args); });
            }
            else
            {
                Console.WriteLine(string.Format(format, args));
                if (checkRecv.Checked)
                {
                    textConsoleOutput.Text +=(string.Format(format, args));
                    textConsoleOutput.Text +=(System.Environment.NewLine);
                }
            }
        }
        public void LogOutgoingPacket(string format, params object[] args)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate() { LogOutgoingPacket(format, args); });
            }
            else
            {
                Console.WriteLine(string.Format(format, args));
                if (checkSend.Checked)
                {
                    textConsoleOutput.Text +=(string.Format(format, args));
                    textConsoleOutput.Text +=(System.Environment.NewLine);
                }
            }
        }
        #endregion
       
        #region Events

        private void pathFollowPlayer_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void buttonSend_Click(object sender, EventArgs e)
        {
            var lines = textConsoleInput.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                ClientPacket msg = null;
                var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    byte value;
                    if (byte.TryParse(word, NumberStyles.HexNumber, null, out value))
                    {
                        if (msg == null)
                            msg = new ClientPacket(value);
                        else
                            msg.WriteByte(value);
                    }
                }

                if (msg.Opcode == 0x39 || msg.Opcode == 0x3A)
                    msg.GenerateDialogHeader();

                if (msg.UseDefaultKey)
                {
                    msg.WriteByte(0x00);
                }
                else
                {
                    msg.WriteByte(0x00);
                    msg.WriteByte(msg.Opcode);
                }

                msg.Write(new byte[] { 0x00, 0x00, 0x00 });

                new Thread(() => { Client.Send(msg); }).Start();
            }
        }
        private void buttonRecv_Click(object sender, EventArgs e)
        {
            /*    var lines = textConsoleInput.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    ServerPacket msg = null;
                    var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        byte value;
                        if (byte.TryParse(word, NumberStyles.HexNumber, null, out value))
                        {
                            if (msg == null)
                                msg = new ServerPacket(value);
                            else
                                msg.WriteByte(value);
                        }
                    }

                    msg.Write(new byte[] { 0x00, 0x00, 0x00 });

                    new Thread(() => { Client.Enqueue(msg); }).Start();
                }*/
        }
        private void pathSingle_Click(object sender, EventArgs e)
        {
            if (this.WalkThread == null)
            {
                this.WalkThread = new Thread(new ThreadStart(this.Walker));
                this.WalkThread.Start();
            }
        }

        private void pathDyanmic_Click(object sender, EventArgs e)
        {
            if (this.WalkThread == null)
            {
                this.WalkThread = new Thread(new ThreadStart(this.Walker));
                this.WalkThread.Start();
            }
        }
        private void clientBasherStart_Click(object sender, EventArgs e)
        {
            this.ResizeAllControls();
            if (this.RunningScript == null)
            {
                this.RunningScript = (Script)new Basher();
                this.RunningScript.client = this.Client;
            }
            if (this.RunningThread != null)
                return;
            this.RunningThread = new Thread(new ThreadStart(this.RunningScript.Start));
            //ThreadPool.QueueUserWorkItem(this.PrimaryLoop);
            this.clientStart.Enabled = false;
            this.clientStop.Enabled = true;
            //this.PrimaryThread = new Thread(new ThreadStart(this.PrimaryLoop));
            //this.PrimaryThread.Start();
            this.RunningThread.Start();
            this.RunningScript.Running = true;

        }

        private void clientStart_Click(object sender, EventArgs e)
        {
            this.clientStart.Enabled = false;
            this.clientStop.Enabled = true;

            if (this.PrimaryThread != null)
                return;
            
            if (this.addonThread != null)
                this.addonThread.Start();
               

        }

        private void clientStop_Click(object sender, EventArgs e)
        {
            this.clientStart.Enabled = true;
            this.clientStop.Enabled = false;
            if (this.RunningThread == null)
                return;

            
            //this.RunningScript.Running = false;
            //if (this.RunningScript != null)
             //   this.RunningScript.Stop();

            if (this.RunningThread != null)
            {
                this.RunningThread.Abort();
                this.RunningThread.Join();
                this.RunningThread = (Thread)null;
            }
            if (this.PrimaryThread != null)
            {
                this.PrimaryThread.Abort();
                this.PrimaryThread.Join();
                this.PrimaryThread = (Thread)null;
            }
            if (this.addonThread != null)
            {
                this.addonThread.Abort();
                this.addonThread.Join();
                this.addonThread = (Thread)null;
            }

            /*if (this.WalkThread != null)
            {
                this.WalkThread.Abort();
                this.WalkThread.Join();
                this.WalkThread = (Thread)null;
            }*/
        }

        private void controlConnect_Click(object sender, EventArgs e)
        {
            string username = this.controlConsoleUsername.Text, password = this.controlConsolePassword.Text;
            //ConsoleDA.Client clientless = new ConsoleDA.Client(username,password);
            ConsoleDA.Client console;
            Thread newThread = (Thread)null;
            ConsoleDA.Client.Run(username,password, ref newThread, null, out console);

            this.clientlessClients.Add(newThread, console);
        }
        private void controlDisconnect_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<Thread, ConsoleDA.Client> entry in this.clientlessClients)
            {
                if (entry.Value.username.Equals(this.controlConsoleUsername.Text))
                {
                    entry.Value.Disconnect();
                    entry.Key.Abort();
                    entry.Key.Join();
                }
                //entry.Key = (Thread)null; what do i do here?
            }
                
        }

        private void pathingSpeedControl_change(object sender, EventArgs e)
        {
            this.pathMsDelay.Text = this.pathingSpeedControl.Value.ToString();
            this.walkMsDelay = this.pathingSpeedControl.Value;
        }

        private void pathWalkingDefaults_Click(object sender, EventArgs e)
        {
            this.pathingSpeedControl.Value = 385;
            this.pathingSpeedType.Text = "No Change";
        }

        private void pathNone_Click(object sender, EventArgs e)
        {
            if (this.WalkThread != null)
            {
                this.WalkThread.Abort();
                this.WalkThread.Join();
                this.WalkThread = (Thread)null;
            }
        }
        public RegisteredWaitHandle reg { get; set; }
        public TaskFactory factory = new TaskFactory(CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskContinuationOptions.None, TaskScheduler.Default);
           
        private void mapWaypointsPath_Click(object sender, EventArgs e)
        { 
           

            /*Task clientTasks = factory.StartNew(() =>
                {
                    //factory.FromAsync((IAsyncResult)this.asyncDelegateWaypoints,new Action(this.delegateWaypoints));
                    factory.StartNew(this.delagateWaypointsWalker);
                    factory.StartNew(newCaster.DionPrimary);
                    factory.StartNew(newCaster.SelfAoPrimary);
                    factory.StartNew(newCaster.SelfHealPrimary);
                    factory.StartNew(newCaster.SelfBuffPrimary);
                    factory.StartNew(newCaster.DebuffPrimary);
                    factory.StartNew(newCaster.AttackPrimary);
                    factory.StartNew(newCaster.PriorityPrimary);
                    factory.StartNew(newCaster.Start);
                    factory.StartNew(this.delagateUpdateMap); //CHANGE THIS BACK TO ON TAB LOAD!!!!!! and check why 
                    //it only blocks for the map update and not waypoints...
                });*/

            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.delagateWaypointsWalker));

            //ThreadPool.SetMinThreads(8, 8);
            //this.reg = ThreadPool.RegisterWaitForSingleObject(this.waypointsThreadWaitHandle, this.delagateWaypointsWalker, "Some Data", -1, true);
            
            /*if (this.WaypointsThread == null)
            {
                this.WaypointsThread = new Thread(new ThreadStart(this.waypointsWalker));
                //this.WaypointsThread.IsBackground = true;
                this.WaypointsThread.Start();
            }
            else
            {
                 this.WaypointsThread.Abort();
                 this.WaypointsThread.Join();
                 this.WaypointsThread = (Thread)null;
                 
            }*/
        }

        private void controlPauseWaypointsThread_Click(object sender, EventArgs e)
        {
            this.pauseWaypoints();
        }

        private void controlResumeWaypointsThread_Click(object sender, EventArgs e)
        {
            this.resumeWaypoints();
        }

        private void mapPauseDynamicMap_Click(object sender, EventArgs e)
        {
            this.pauseDynamicMap();
        }

        private void mapResumeDynamicMap_Click(object sender, EventArgs e)
        {
            this.resumeDynamicMap();
        }

        #region To delete probably?
        private void mapEditingRadios_WaypointsClick(object sender, MouseEventArgs e)
        {
            this.mapAddWaypoints.Checked = !this.mapAddWaypoints.Checked;
        }
        private void mapEditingRadios_DoorsClick(object sender, MouseEventArgs e)
        {
            this.mapAddDoors.Checked = !this.mapAddDoors.Checked;
        }
        private void mapEditingRadios_BlocksClick(object sender, MouseEventArgs e)
        {
            this.mapAddBlocks.Checked = !this.mapAddBlocks.Checked;
        }
        #endregion

        #endregion


        #region Map
        private void ClearWorld()
        {
            MainForm.mapNodes = new SortedList<int, ClientTab.Node>();
            //this.ListBoxCurrPortals.DataSource = (object)null;
            //this.ListBoxCurrPortals.Items.Clear();
            this.listBoxWorldMaps.DataSource = (object)null;
            this.listBoxWorldMaps.Items.Clear();
            this.textBoxMapAxis.Clear();
            this.textBoxMapNum.Clear();
            //this.textBoxName.Clear();
            //this.textBoxSpecialEndOps.Clear();
            this.textBoxWarpPortalLocs.Clear();
            //this.textBoxWarpsTo.Clear();
        }

        private void ButtonLoadMapFile_Click(object sender, EventArgs e)
        {
            this.ClearWorld();
            this.OpenWorldMap();
        }

        private void OpenWorldMap()
        {
            MainForm.PreloadMaps();
            this.listBoxWorldMaps.DataSource = (object)new List<ClientTab.Node>((IEnumerable<ClientTab.Node>)MainForm.mapNodes.Values);
        }

        public struct Node
        {
            public int mapNum;
            public int[] mapAxis;
            public string name;
            public bool otherList;
            public SortedList<int, ClientTab.Node.infoMap> infoPortals;

            public Node(int mapNum, int[] mapAxis, bool otherList)
            {
                this.mapNum = mapNum;
                this.mapAxis = mapAxis;
                this.name = "";
                this.otherList = otherList;
                this.infoPortals = new SortedList<int, ClientTab.Node.infoMap>();
            }

            public Node(int mapNum, int[] mapAxis, string name, bool otherList)
            {
                this.mapNum = mapNum;
                this.mapAxis = mapAxis;
                this.name = name;
                this.otherList = otherList;
                this.infoPortals = new SortedList<int, ClientTab.Node.infoMap>();
            }

            public void addExit(int destinationMap, int[] mapPortalLocs, int[] specialOps)
            {
                this.infoPortals[destinationMap] = new ClientTab.Node.infoMap(new int[3][]
        {
          new int[1]
          {
            destinationMap
          },
          mapPortalLocs,
          specialOps
        });
            }

            public void removeExit(int infoPortal)
            {
                this.infoPortals.RemoveAt(infoPortal);
            }

            public int[] getWarpPortalLocs(int destinationMap)
            {
                foreach (KeyValuePair<int, ClientTab.Node.infoMap> keyValuePair in this.infoPortals)
                {
                    if (keyValuePair.Value[0, 0] == destinationMap)
                        return keyValuePair.Value[1];
                }
                return (int[])null;
            }

            public int[] getSpecialOps(int destinationMap)
            {
                foreach (KeyValuePair<int, ClientTab.Node.infoMap> keyValuePair in this.infoPortals)
                {
                    if (keyValuePair.Value[0, 0] == destinationMap)
                        return keyValuePair.Value[2];
                }
                return (int[])null;
            }

            public override string ToString()
            {
                if (this.name == "")
                    return "(" + this.mapNum + ")";
                return this.name + " (" + this.mapNum + ")";
            }

            public struct infoMap
            {
                private int[][] currInfoMap;

                public int this[int nodes, int index]
                {
                    get
                    {
                        return this.currInfoMap[nodes][index];
                    }
                }

                public int[] this[int nodes]
                {
                    get
                    {
                        return this.currInfoMap[nodes];
                    }
                }

                public infoMap(int[][] infoMap)
                {
                    this.currInfoMap = infoMap;
                }

                public override string ToString()
                {
                    string str1 = "{";
                    for (int index = 0; index < this.currInfoMap[1].Length; ++index)
                        str1 = str1 + this.currInfoMap[1][index].ToString() + " ";
                    string str2 = str1.TrimEnd(' ') + "}";
                    string str3 = "{";
                    for (int index = 0; index < this.currInfoMap[2].Length; ++index)
                        str3 = str3 + this.currInfoMap[2][index].ToString() + " ";
                    string str4 = str3.TrimEnd(' ') + "}";
                    string str5 = "";
                    if (MainForm.mapNodes.ContainsKey(this.currInfoMap[0][0]) && MainForm.mapNodes[this.currInfoMap[0][0]].name != "")
                        str5 = MainForm.mapNodes[this.currInfoMap[0][0]].name + " ";
                    return str5 + "(" + this.currInfoMap[0][0].ToString() + ") " + str2 + " " + str4;
                }
            }
        }
        #endregion

        private void unusedfunction(object sender, EventArgs e)
        {
       
            
        }

        private void controlRunTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void controlLaunchWatcher_Click(object sender, EventArgs e)
        {
            /*string username = this.controlConsoleUsername.Text, password = this.controlConsolePassword.Text;
            //ConsoleDA.Client clientless = new ConsoleDA.Client(username,password);
            ConsoleDA.Client console;
            Thread newThread = (Thread)null;
            ConsoleDA.Client.Run(username, password, ref newThread, MainForm.WatcherClientlessThreadLoop, out console);

            this.clientlessClients.Add(newThread, console);*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                for (int i = 0; this.controlChatTextBox.Text.Length >= 60; i++)
                {

                    this.Client.Say(this.controlChatTextBox.Text.Substring(0, 60));
                    this.controlChatTextBox.Text = this.controlChatTextBox.Text.Remove(0, 60);
                    Thread.Sleep(1000);
                }
                this.Client.Say(this.controlChatTextBox.Text);
                this.controlChatTextBox.Clear();
            }).Start();
        }


        
        
    }
}