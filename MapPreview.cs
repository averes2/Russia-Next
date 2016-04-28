// Decompiled with JetBrains decompiler
// Type: DAWalkerWorldMapEditor.MapPreview
// Assembly: DAWalkerWorldMapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4D5A0B7E-C7CF-42F2-8A86-EC18BB1CD35B
// Assembly location: C:\Users\adm\Downloads\DAWalker\DAWalkerWorldMapEditor.exe

//using MyDAMapLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ConsoleDA
{
    public class MapPreview
    {
        private List<ushort[]> myFactors = new List<ushort[]>();
        private MyDAMap myDAMap = new MyDAMap();
        private ClientTab Tab;
        private Bitmap DAMapImage;
        private IContainer components;
        public ListBox MapDimList;

        public MapPreview(ClientTab Tab)
        {
            //this.InitializeComponent();
            this.Tab = Tab;
            this.MapDimList = Tab.mapDimList;
            myDAMap.Client = Tab.Client;
        }

        public void OpenDAMapFile(string mapNumber)
        {
            int initialDimIndex = 0, index = 0;

            if (this.Tab.textBoxMapNum.Text != mapNumber)
                this.Tab.textBoxMapNum.Text = mapNumber;
            this.MapDimList.Items.Clear();
            if ((this.myFactors = MyDAMap.GetFactors(Options.MapsDirectory + "\\lod" + mapNumber + ".map")) == null)
            {
                this.ClearMap();
            }
            else
            {
                foreach (ushort[] numArray in this.myFactors)
                {
                    if(this.Tab.Client.Base.LoggedIn)
                        if (numArray[0] == this.Tab.Client.Base.DaMap.Width && numArray[1] == this.Tab.Client.Base.DaMap.Height)
                            initialDimIndex = index;
                    this.MapDimList.Items.Add((object)numArray[0].ToString() + "x" + numArray[1].ToString());
                    index++;
                }
                string[] strArray = this.Tab.textBoxMapAxis.Text.Split(new char[1]
        {
          ' '
        }, StringSplitOptions.RemoveEmptyEntries);
                this.MapDimList.SelectedIndex = initialDimIndex;
                /*if (strArray.Length == 2 && strArray[0] != "-1" && (strArray[1] != "-1" && this.MapDimList.Items.Contains((object)(strArray[0] + "x" + strArray[1]))))
                    this.MapDimList.SelectedItem = (object)(strArray[0] + "x" + strArray[1]);
                else
                    this.MapDimList.SelectedIndex = this.myFactors.Count / 2;*/
            }
        }

        public Bitmap CreateDABitMap()
        {
            if (!this.myDAMap.TryMakeDAMapSquareImage(Options.MapsDirectory + "\\lod" + this.Tab.textBoxMapNum.Text + ".map", this.myFactors[this.MapDimList.SelectedIndex][0], this.myFactors[this.MapDimList.SelectedIndex][1], 0.5, Color.Black, PixelFormat.Format16bppRgb555, out this.DAMapImage))
            {
                //this.Tab.Client.SendMessage("Didnt make a map!", (byte)0);
                this.Tab.Client.console.WriteLine("Didnt make a map!");
                this.ClearMap();
            }
            else
            {
               /* this.Tab.Client.Server.Form.Width = 800;
                this.Tab.Client.Server.Form.Height = 400;
                this.Tab.controlTabs.Width = this.Tab.Client.Server.Form.Width;
                this.Tab.controlTabs.Height = this.Tab.Client.Server.Form.Height;

                this.Tab.mapDynamicView.Width = this.DAMapImage.Width + 1;// + 118;
                this.Tab.mapDynamicView.Height = this.DAMapImage.Height + 1;
                this.Tab.controlTabs.Width += this.Tab.mapDynamicView.Width/2;
                this.Tab.controlTabs.Height += this.Tab.mapDynamicView.Height;

                this.Tab.Client.Server.Form.Width = this.Tab.controlTabs.Width;
                this.Tab.Client.Server.Form.Height = this.Tab.controlTabs.Height;*/

                //This is what we were doing before!
                /*this.Tab.controlTabs.Width = 800 + this.Tab.mapDynamicView.Width / 2;
                this.Tab.controlTabs.Height = 400 + this.Tab.mapDynamicView.Height;
                this.Tab.Client.Server.Form.Width = this.Tab.controlTabs.Width;
                this.Tab.Client.Server.Form.Height = this.Tab.controlTabs.Height;*/

                this.Tab.mapDynamicView.Width = this.DAMapImage.Width + 6;// + 118;
                this.Tab.mapDynamicView.Height = this.DAMapImage.Height + 6;

                //this.Tab.Height = this.Tab.mapDynamicView.Height;
                //this.Tab.Width = this.Tab.mapDynamicView.Width;
                //this.Tab.controlTabs.Width = this.Tab.mapDynamicView.Width + 150;
                //this.Tab.controlTabs.Height = (this.Tab.controlTabs.Height - this.Tab.mapDynamicView.Height) + this.Tab.mapDynamicView.Height + 50;
                //this.Tab.Client.Server.Form.Width = this.Tab.controlTabs.Width + 400;
                //this.Tab.Client.Server.Form.Height = this.Tab.controlTabs.Height + 150;
                

                //this.Tab.Client.SendMessage("Width: " + this.Tab.Client.Server.Form.Width + ", Height: " + this.Tab.Client.Server.Form.Height, (byte)0);
                //this.Tab.mapDynamicView.Image = this.DAMapImage;
            }
            return this.DAMapImage;
        }

        public void ClearMap()
        {
            this.myFactors.Clear();
            this.MapDimList.Items.Clear();
            //this.DAMapImage = (Bitmap)null;
            //this.Tab.mapDynamicView.Image = (Image)null;
        }


        public Point FindMapBlock(Bitmap searchMap, Color searchColor)
        {
            Bitmap bmp = new Bitmap(searchMap);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] r = new byte[bytes / 3];
            byte[] g = new byte[bytes / 3];
            byte[] b = new byte[bytes / 3];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int count = 0;
            int stride = bmpData.Stride;

            Point p = new Point(-1,-1);

            for (int column = 0; column < bmpData.Height; column++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    b[count] = (byte)(rgbValues[(column * stride) + (row * 3)]);
                    g[count] = (byte)(rgbValues[(column * stride) + (row * 3) + 1]);
                    r[count+1] = (byte)(rgbValues[(column * stride) + (row * 3) + 2]);
                    Color newColor = Color.FromArgb(b[count],g[count],r[count+1]);
                    if(newColor == searchColor) {
                        p = new Point(row,column);
                    }
                    count++;
                }
            }

            return p;
        }

        public void DrawSquareBlockAndUpdate(int X, int Y, Color color)
        {
            if (!MyDAMap.TryColorBlockSquare((ushort)X, (ushort)Y, 0.5, color, ref this.DAMapImage))
                return;
            this.Tab.mapDynamicView.Image = (Image)this.DAMapImage;
        }
        

        public void DrawRhombusBlockAndUpdate(int X, int Y)
        {
            this.DrawRhombusBlockAndUpdate(new int[2]
      {
        X,
        Y
      });
        }

        public void DrawRhombusBlockAndUpdate(int[] portals)
        {
            if (this.myFactors == null || !MyDAMap.TryColorBlockRhombus(portals, this.myFactors[this.MapDimList.SelectedIndex][0], this.myFactors[this.MapDimList.SelectedIndex][1], Color.Red, 0.5, ref this.DAMapImage))
                return;
            this.Tab.mapDynamicView.Image = (Image)this.DAMapImage;
        }


        /*private void InitializeComponent()
        {
            this.MapDimList = new ListBox();
            ((ISupportInitialize)this.Tab.mapDynamicView).BeginInit();
            this.SuspendLayout();
            //this.Tab.mapDynamicView.BorderStyle = BorderStyle.FixedSingle;
            this.Tab.mapDynamicView.Location = new Point(12, 12);
            this.Tab.mapDynamicView.
         * 
         * "Tab.mapDynamicView";
            this.Tab.mapDynamicView.Size = new Size(268, 249);
            //this.Tab.mapDynamicView.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Tab.mapDynamicView.TabIndex = 0;
            this.Tab.mapDynamicView.TabStop = false;
            this.Tab.mapDynamicView.MouseClick += new MouseEventHandler(this.PictureBoxMapPreview_MouseClick);
            this.MapDimList.Dock = DockStyle.Right;
            this.MapDimList.FormattingEnabled = true;
            this.MapDimList.Location = new Point(294, 0);
            this.MapDimList.Name = "MapDimList";
            this.MapDimList.Size = new Size(86, 264);
            this.MapDimList.TabIndex = 1;
            this.MapDimList.SelectedIndexChanged += new EventHandler(this.MapDimList_SelectedIndexChanged);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(380, 269);
            this.Controls.Add((Control)this.MapDimList);
            this.Controls.Add((Control)this.Tab.mapDynamicView);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Name = "MapPreview";
            this.Text = "MapPreview";
            this.FormClosing += new FormClosingEventHandler(this.MapPreview_FormClosing);
            ((ISupportInitialize)this.Tab.mapDynamicView).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }*/
    }
}
