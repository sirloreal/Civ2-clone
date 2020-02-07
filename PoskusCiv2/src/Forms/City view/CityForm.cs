﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTciv2.Imagery;
using RTciv2.Units;
using RTciv2.Improvements;

namespace RTciv2.Forms
{
    //public partial class CityForm : Form
    public partial class CityForm : Civ2form
    {
        public MainCiv2Window mainCiv2Window;
        //Draw Draw = new Draw();
        City ThisCity;
        //Bitmap CityDrawing;
        DoubleBufferedPanel WallpaperPanel, Faces, ResourceMap, CityResources, UnitsFromCity, UnitsInCity, FoodStorage, ProductionPanel;
        DoubleBufferedPanel CallingForm;
        VScrollBar ImprovementsBar;
        int[,] offsets;
        int ProductionItem;

        public CityForm(MainCiv2Window _mainCiv2Window)
        {
            InitializeComponent();
            mainCiv2Window = _mainCiv2Window;
        }

        public CityForm(DoubleBufferedPanel _callingForm, City city)
        {
            InitializeComponent();

            ThisCity = city;

            CallingForm = _callingForm;

            Size = new Size(976, 681);  //normalen zoom = (657,459)
            FormBorderStyle = FormBorderStyle.None;
            BackgroundImage = Images.WallpaperMapForm;
            
            this.Load += new EventHandler(CityForm_Load);
            this.Paint += new PaintEventHandler(CityForm_Paint);

            #region Panels
            //Panel for wallpaper
            WallpaperPanel = new DoubleBufferedPanel
            {
                Location = new Point(11, 39),    //normal zoom = (8,25)
                Size = new Size(954, 631),      //normal zoom = (640,420)
                BackgroundImage = ModifyImage.ResizeImage(Images.CityWallpaper, 954, 631)
            };
            Controls.Add(WallpaperPanel);
            WallpaperPanel.Paint += new PaintEventHandler(WallpaperPanel_Paint);
            WallpaperPanel.Paint += new PaintEventHandler(ImprovementsList_Paint);

            //Resource map panel
            ResourceMap = new DoubleBufferedPanel
            {
                Location = new Point(7, 125),
                Size = new Size(4 * 72, 4 * 36),    //stretched by 12.5 %
                BackColor = Color.Transparent
            };
            //WallpaperPanel.Controls.Add(ResourceMap);
            //ResourceMap.Paint += new PaintEventHandler(ResourceMap_Paint);

            //City resources panel
            CityResources = new DoubleBufferedPanel
            {
                Location = new Point(300, 70),
                Size = new Size(350, 245),    //stretched by 12.5 %
                BackColor = Color.Transparent
            };
            //WallpaperPanel.Controls.Add(CityResources);
            //CityResources.Paint += new PaintEventHandler(CityResources_Paint);

            //Units from city panel
            UnitsFromCity = new DoubleBufferedPanel
            {
                Location = new Point(10, 321),
                Size = new Size(270, 104),
                BackColor = Color.Transparent
            };
            //WallpaperPanel.Controls.Add(UnitsFromCity);
            //UnitsFromCity.Paint += new PaintEventHandler(UnitsFromCity_Paint);

            //Units in city panel
            UnitsInCity = new DoubleBufferedPanel
            {
                Location = new Point(288, 322),
                Size = new Size(360, 245),
                BackColor = Color.Transparent
            };
            //WallpaperPanel.Controls.Add(UnitsInCity);
            //UnitsInCity.Paint += new PaintEventHandler(UnitsInCity_Paint);
            #endregion

            #region Buttons
            //Buy button
            Civ2button BuyButton = new Civ2button
            {
                Location = new Point(651 + 8, 248 + 24),
                Size = new Size(102, 36),
                Font = new Font("Arial", 10.5F),
                Text = "Buy"
            };
            WallpaperPanel.Controls.Add(BuyButton);
            BuyButton.Click += new EventHandler(BuyButton_Click);

            //Change button
            Civ2button ChangeButton = new Civ2button
            {
                Location = new Point(651 + 180, 248 + 24),
                Size = new Size(102, 36),
                Font = new Font("Arial", 10.5F),
                Text = "Change"
            };
            WallpaperPanel.Controls.Add(ChangeButton);
            ChangeButton.Click += new EventHandler(ChangeButton_Click);

            //Info button
            Civ2button InfoButton = new Civ2button
            {
                Location = new Point(684, 546), //original (461, 366)
                Size = new Size(85, 36),  //original (57, 24)
                Font = new Font("Arial", 10.5F),
                Text = "Info"
            };
            WallpaperPanel.Controls.Add(InfoButton);
            InfoButton.Click += new EventHandler(InfoButton_Click);

            //Map button
            Civ2button MapButton = new Civ2button
            {
                Location = new Point(771, 546), //original (519, 366)
                Size = new Size(85, 36),  //original (57, 24)
                Font = new Font("Arial", 10.5F),
                Text = "Map"
            };
            WallpaperPanel.Controls.Add(MapButton);
            MapButton.Click += new EventHandler(MapButton_Click);

            //Rename button
            Civ2button RenameButton = new Civ2button
            {
                Location = new Point(858, 546), //original (577, 366)
                Size = new Size(85, 36),  //original (57, 24)
                Font = new Font("Arial", 10.5F),
                Text = "Rename"
            };
            WallpaperPanel.Controls.Add(RenameButton);
            RenameButton.Click += new EventHandler(RenameButton_Click);

            //Happy button
            Civ2button HappyButton = new Civ2button
            {
                Location = new Point(684, 583), //original (461, 391)
                Size = new Size(85, 36),  //original (57, 24)
                Font = new Font("Arial", 10.5F),
                Text = "Happy"
            };
            WallpaperPanel.Controls.Add(HappyButton);
            HappyButton.Click += new EventHandler(HappyButton_Click);

            //View button
            Civ2button ViewButton = new Civ2button
            {
                Location = new Point(771, 583), //original (519, 391)
                Size = new Size(85, 36),  //original (57, 24)
                Font = new Font("Arial", 10.5F),
                Text = "View"
            };
            WallpaperPanel.Controls.Add(ViewButton);
            ViewButton.Click += new EventHandler(ViewButton_Click);

            //Exit button
            Civ2button ExitButton = new Civ2button
            {
                Location = new Point(858, 583), //original (577, 391)
                Size = new Size(85, 36),  //original (57, 24)
                Font = new Font("Arial", 10.5F),
                Text = "Exit"
            };
            WallpaperPanel.Controls.Add(ExitButton);
            ExitButton.Click += new EventHandler(ExitButton_Click);

            //Next city (UP) button
            NoSelectButton NextCityButton = new NoSelectButton
            {
                Location = new Point(652, 548), //original (440, 367)
                Size = new Size(32, 36),  //original (21, 24)
                BackColor = Color.FromArgb(107, 107, 107)
            };
            NextCityButton.FlatStyle = FlatStyle.Flat;
            WallpaperPanel.Controls.Add(NextCityButton);
            NextCityButton.Click += new EventHandler(NextCityButton_Click);
            NextCityButton.Paint += new PaintEventHandler(NextCityButton_Paint);

            //Previous city (DOWN) button
            NoSelectButton PrevCityButton = new NoSelectButton
            {
                Location = new Point(652, 585), //original (440, 392)
                Size = new Size(32, 36),  //original (21, 24)
                BackColor = Color.FromArgb(107, 107, 107)
            };
            PrevCityButton.FlatStyle = FlatStyle.Flat;
            WallpaperPanel.Controls.Add(PrevCityButton);
            PrevCityButton.Click += new EventHandler(PrevCityButton_Click);
            PrevCityButton.Paint += new PaintEventHandler(PrevCityButton_Paint);
            #endregion

            //Improvements vertical bar
            ImprovementsBar = new VScrollBar()
            {
                Location = new Point(270, 433),
                Size = new Size(15, 190),
                Maximum = 66 - 9 + 9    //max improvements=66, 9 can be shown, because of slider size it's 9 elements smaller
            };
            WallpaperPanel.Controls.Add(ImprovementsBar);
            ImprovementsBar.ValueChanged += new EventHandler(ImprovementsBarValueChanged);

            //Define offset map array
            offsets = new int[20, 2] { { -2, 0 }, { -1, -1 }, { 0, -2 }, { 1, -1 }, { 2, 0 }, { 1, 1 }, { 0, 2 }, { -1, 1 }, { -3, -1 }, { -2, -2 }, { -1, -3 }, { 1, -3 }, { 2, -2 }, { 3, -1 }, { 3, 1 }, { 2, 2 }, { 1, 3 }, { -1, 3 }, { -2, 2 }, { -3, 1 } };

            ProductionItem = 0; //item appearing in production menu on loadgame
        }

        //Once slider value changes --> redraw improvements list
        private void ImprovementsBarValueChanged(object sender, EventArgs e)
        {
            WallpaperPanel.Invalidate();
        }

        private void CityForm_Load(object sender, EventArgs e)
        {
            Location = new Point(CallingForm.Width / 2 - this.Width / 2, CallingForm.Height / 2 - this.Height / 2 + 60);
        }

        private void CityForm_Paint(object sender, PaintEventArgs e)
        {
            //Main text
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            string bcad = (Data.GameYear < 0) ? "B.C.": "A.D.";
            string text = 
                String.Format("City of " + ThisCity.Name + ", " + Math.Abs(Data.GameYear).ToString() + " " + bcad.ToString() +
                ", Population " + ThisCity.Population.ToString("###,###", new NumberFormatInfo() { NumberDecimalSeparator = "," }) +
                " (Treasury: " + Game.Civs[ThisCity.Owner].Money.ToString() + " Gold)");
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.DrawString(text, new Font("Times New Roman", 15), new SolidBrush(Color.Black), new Point(this.Width / 2 + 1, 20 + 1), sf);
            e.Graphics.DrawString(text, new Font("Times New Roman", 15), new SolidBrush(Color.FromArgb(135, 135, 135)), new Point(this.Width / 2, 20), sf);
            //Border of wallpaper
            e.Graphics.DrawLine(new Pen(Color.FromArgb(67, 67, 67)), 9, 37, this.Width - 11, 37);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(67, 67, 67)), 10, 38, this.Width - 12, 38);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(67, 67, 67)), 9, 37, 9, this.Height - 11);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(67, 67, 67)), 10, 38, 10, this.Height - 12);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(223, 223, 223)), this.Width - 11, 38, this.Width - 11, this.Height - 11);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(223, 223, 223)), this.Width - 10, 37, this.Width - 10, this.Height - 11);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(223, 223, 223)), 9, this.Height - 10, this.Width - 10, this.Height - 10);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(223, 223, 223)), 10, this.Height - 11, this.Width - 11, this.Height - 11);
            sf.Dispose();
            e.Dispose();
        }

        private void WallpaperPanel_Paint(object sender, PaintEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            //Texts
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.DrawString("Citizens", new Font("Arial", 11), new SolidBrush(Color.FromArgb(67, 67, 67)), new Point(117, 71));
            e.Graphics.DrawString("Citizens", new Font("Arial", 11), new SolidBrush(Color.FromArgb(223, 187, 63)), new Point(116, 70));
            e.Graphics.DrawString("Resource Map", new Font("Arial", 11), new SolidBrush(Color.FromArgb(0, 51, 0)), new Point(91, 284));
            e.Graphics.DrawString("Resource Map", new Font("Arial", 11), new SolidBrush(Color.FromArgb(223, 187, 63)), new Point(90, 283));
            e.Graphics.DrawString("City Resources", new Font("Arial", 11), new SolidBrush(Color.FromArgb(67, 67, 67)), new Point(412, 71));
            e.Graphics.DrawString("City Resources", new Font("Arial", 11), new SolidBrush(Color.FromArgb(223, 187, 63)), new Point(411, 70));
            e.Graphics.DrawString("City Improvements", new Font("Arial", 11), new SolidBrush(Color.FromArgb(67, 67, 67)), new Point(68, 436));
            e.Graphics.DrawString("City Improvements", new Font("Arial", 11), new SolidBrush(Color.FromArgb(223, 187, 63)), new Point(67, 435));
            e.Graphics.DrawString("Units Supported", new Font("Arial", 11), new SolidBrush(Color.FromArgb(67, 67, 67)), new Point(80, 327));
            e.Graphics.DrawString("Units Supported", new Font("Arial", 11), new SolidBrush(Color.FromArgb(223, 187, 63)), new Point(79, 326));
            e.Graphics.DrawString("Units Present", new Font("Arial", 11), new SolidBrush(Color.FromArgb(67, 67, 67)), new Point(414, 327));
            e.Graphics.DrawString("Units Present", new Font("Arial", 11), new SolidBrush(Color.FromArgb(223, 187, 63)), new Point(413, 326));
            e.Graphics.DrawString("Food Storage", new Font("Arial", 11), new SolidBrush(Color.FromArgb(0, 0, 0)), new Point(743, 1));
            e.Graphics.DrawString("Food Storage", new Font("Arial", 11), new SolidBrush(Color.FromArgb(75, 155, 35)), new Point(742, 0));

            //Faces
            e.Graphics.DrawImage(Images.DrawFaces(ThisCity, 1.5), 0, 0);

            //Food storage
            e.Graphics.DrawImage(Images.DrawFoodStorage(ThisCity), new Point(651, 0));

            #region Production panel
            //Show item currently in production (ProductionItem=0...61 are units, 62...127 are improvements)
            //Units are scaled by 1.15 compared to original, improvements are size 54x30
            if (ThisCity.ItemInProduction < 62)    //units
            {
                e.Graphics.DrawImage(ModifyImage.ResizeImage(Images.Units[ThisCity.ItemInProduction], 74, 55), new Point(651 + 106, 248 + 7));
            }
            else    //improvements
            {
                e.Graphics.DrawString(ReadFiles.ImprovementName[ThisCity.ItemInProduction - 62 + 1], new Font("Arial", 14), new SolidBrush(Color.Black), 651 + 146 + 1, 248 + 3 + 1, sf);
                e.Graphics.DrawString(ReadFiles.ImprovementName[ThisCity.ItemInProduction - 62 + 1], new Font("Arial", 14), new SolidBrush(Color.FromArgb(63, 79, 167)), 651 + 146, 248 + 3, sf);
                e.Graphics.DrawImage(Images.ImprovementsLarge[ThisCity.ItemInProduction - 62 + 1], new Point(651 + 119, 248 + 28));
            }
            e.Graphics.DrawImage(Images.DrawCityProduction(ThisCity), new Point(651, 248));  //draw production shields and sqare around them
            #endregion

            //Map around city
            Bitmap CityDrawing = Images.DrawCityFormMap(ThisCity);
            e.Graphics.DrawImage(ModifyImage.ResizeImage(CityDrawing, (int)((double)CityDrawing.Width * 1.125), (int)((double)CityDrawing.Height * 1.125)), 7, 125);
            //Food/shield/trade icons around the city (21 of them altogether)
            //for (int i = 0; i <= ThisCity.Size; i++)
            //for (int i = 0; i <= 0; i++)
            //{
            //e.Graphics.DrawImage(Images.DrawCityFormMapIcons(ThisCity, ThisCity.PriorityOffsets[i, 0], ThisCity.PriorityOffsets[i, 1]), 7 + 36 * (ThisCity.PriorityOffsets[i, 0] + 3) + 13, 125 + 18 * (ThisCity.PriorityOffsets[i, 1] + 3) + 11);
            int i = 0;
            e.Graphics.DrawImage(Images.DrawCityFormMapIcons(ThisCity, ThisCity.PriorityOffsets[i, 0], ThisCity.PriorityOffsets[i, 1]), 7 + 36 * (ThisCity.PriorityOffsets[i, 0] + 3) + 13, 125 + 18 * (ThisCity.PriorityOffsets[i, 1] + 3) + 11);
            //e.Graphics.DrawImage(Images.Desert[0], 7 + 36 * (ThisCity.PriorityOffsets[i, 0] + 3) + 13, 125 + 18 * (ThisCity.PriorityOffsets[i, 1] + 3) + 11);
            //}

            sf.Dispose();
            e.Dispose();
        }

        //Draw city map
        private void ResourceMap_Paint(object sender, PaintEventArgs e)
        {
            //Map around city
            //e.Graphics.DrawImage(ModifyImage.ResizeImage(CityDrawing, (int)((double)CityDrawing.Width * 1.125), (int)((double)CityDrawing.Height * 1.125)), 0, 0);
            //Food/shield/trade icons around the city (21 of them altogether)
            for (int i = 0; i <= ThisCity.Size; i++)
            {
                //e.Graphics.DrawImage(Images.DrawCityFormMapIcons(ThisCity, ThisCity.PriorityOffsets[i, 0], ThisCity.PriorityOffsets[i, 1]), 7 + 36 * (ThisCity.PriorityOffsets[i, 0] + 3) + 13, 125 + 18 * (ThisCity.PriorityOffsets[i, 1] + 3) + 11);
            }
            e.Dispose();
        }

        //Draw city resources
        private void CityResources_Paint(object sender, PaintEventArgs e)
        {
            StringFormat sf1 = new StringFormat();
            StringFormat sf2 = new StringFormat();
            sf1.Alignment = StringAlignment.Far;
            sf2.Alignment = StringAlignment.Center;

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            
            //Draw food+surplus/hun$ger strings
            e.Graphics.DrawString($"Food: {ThisCity.Food}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(4, 24));
            e.Graphics.DrawString($"Food: {ThisCity.Food}", new Font("Arial", 11), new SolidBrush(Color.FromArgb(87, 171, 39)), new Point(3, 23));
            e.Graphics.DrawString($"Surplus: {ThisCity.Surplus}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(344, 24), sf1);
            e.Graphics.DrawString($"Surplus: {ThisCity.Surplus}", new Font("Arial", 11), new SolidBrush(Color.FromArgb(63, 139, 31)), new Point(343, 23), sf1);

            //Draw trade+corruption strings
            e.Graphics.DrawString($"Trade: {ThisCity.Trade}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(4, 85));
            e.Graphics.DrawString($"Trade: {ThisCity.Trade}", new Font("Arial", 11), new SolidBrush(Color.FromArgb(239, 159, 7)), new Point(3, 84));
            e.Graphics.DrawString($"Corruption: {ThisCity.Corruption}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(344, 85), sf1);
            e.Graphics.DrawString($"Corruption: {ThisCity.Corruption}", new Font("Arial", 11), new SolidBrush(Color.FromArgb(227, 83, 15)), new Point(343, 84), sf1);

            //Draw tax/lux/sci strings
            e.Graphics.DrawString($"40% Tax: {ThisCity.Tax}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(4, 165));
            e.Graphics.DrawString($"40% Tax: {ThisCity.Tax}", new Font("Arial", 11), new SolidBrush(Color.FromArgb(239, 159, 7)), new Point(3, 164));
            e.Graphics.DrawString($"0% Lux: {ThisCity.Lux}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(175, 165), sf2);
            e.Graphics.DrawString($"0% Lux: {ThisCity.Lux}", new Font("Arial", 11), new SolidBrush(Color.White), new Point(174, 164), sf2);
            e.Graphics.DrawString($"50% Sci: {ThisCity.Science}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(344, 165), sf1);
            e.Graphics.DrawString($"50% Sci: {ThisCity.Science}", new Font("Arial", 11), new SolidBrush(Color.FromArgb(63, 187, 199)), new Point(343, 164), sf1);

            //Support + production icons
            e.Graphics.DrawString($"Support: {ThisCity.Support}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(4, 227));
            e.Graphics.DrawString($"Support: {ThisCity.Support}", new Font("Arial", 11), new SolidBrush(Color.FromArgb(63, 79, 167)), new Point(3, 226));
            e.Graphics.DrawString($"Production: {ThisCity.Production}", new Font("Arial", 11), new SolidBrush(Color.Black), new Point(344, 227), sf1);
            e.Graphics.DrawString($"Production: {ThisCity.Production}", new Font("Arial", 11), new SolidBrush(Color.FromArgb(7, 11, 103)), new Point(343, 226), sf1);

            //Draw icons
            //e.Graphics.DrawImage(Draw.DrawCityIcons(ThisCity, 5, -2, 5, 3, 7, 2, 6, 5, 3), new Point(7, 42));

            sf1.Dispose();
            sf2.Dispose();
            e.Dispose();
        }

        private void UnitsFromCity_Paint(object sender, PaintEventArgs e)
        {
            int count = 0;
            int row = 0;
            int col = 0;
            double resize_factor = 1;  //orignal images are 0.67 of original, because of 50% scaling it is 0.67*1.5=1
            foreach (IUnit unit in Game.Units.Where(n => n.HomeCity == Game.Cities.FindIndex(x => x == ThisCity)))
            {
                col = count % 5;
                row = count / 5;
                //e.Graphics.DrawImage(Draw.DrawUnit(unit, false, resize_factor), new Point((int)(64 * resize_factor * col), (int)(48 * resize_factor * row)));
                count++;

                if (count >= 10)
                    break;
            }
        }

        private void UnitsInCity_Paint(object sender, PaintEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            int count = 0;
            int row = 0;
            int col = 0;
            double resize_factor = 1.125;  //orignal images are 25% smaller, because of 50% scaling it is 0.75*1.5=1.125
            foreach (IUnit unit in Game.Units.Where(unit => unit.X == ThisCity.X && unit.Y == ThisCity.Y ))
            {
                col = count % 5;
                row = count / 5;
                //e.Graphics.DrawImage(Draw.DrawUnit(unit, false, resize_factor), new Point((int)(64 * resize_factor * col), (int)(48 * resize_factor * row) + 5 * row));
                e.Graphics.DrawString(ThisCity.Name.Substring(0, 3), new Font("Arial", 12), new SolidBrush(Color.Black), new Point((int)(64 * resize_factor * col) + (int)(64 * resize_factor / 2), (int)(48 * resize_factor * row) + 5 * row + (int)(48 * resize_factor)), sf);
                count++;
            }
            sf.Dispose();
            e.Dispose();
        }

        private void ImprovementsList_Paint(object sender, PaintEventArgs e)
        {
            //Draw city improvements
            int x = 12;
            int y = 460;
            int starting = ImprovementsBar.Value;   //starting improvement to draw (changes with slider)
            for (int i = 0; i < 9; i++)
            {
                if ((i + starting) >= (ThisCity.Improvements.Count()))
                    break;  //break if no of improvements+wonders to small

                //draw improvements
                e.Graphics.DrawImage(Images.ImprovementsSmall[(int)ThisCity.Improvements[i + starting].Type], new Point(x, y + 15 * i + 2 * i));
                if ((int)ThisCity.Improvements[i + starting].Type < 39) //wonders don't have a sell icon
                {
                    e.Graphics.DrawImage(Images.SellIconLarge, new Point(x + 220, y + 15 * i + 2 * i - 2));
                }
                e.Graphics.DrawString(ThisCity.Improvements[i + starting].Name, new Font("Arial", 13), new SolidBrush(Color.Black), new Point(x + 36, y + 15 * i + 2 * i - 3));
                e.Graphics.DrawString(ThisCity.Improvements[i + starting].Name, new Font("Arial", 13), new SolidBrush(Color.White), new Point(x + 35, y + 15 * i + 2 * i - 3));
            }
            e.Dispose();
        }

        private void BuyButton_Click(object sender, EventArgs e)
        {
            //Use this so the form returns a chosen value (what it has chosen to produce)
            using (var CityBuyForm = new CityBuyForm(ThisCity))
            {
                CityBuyForm.Load += new EventHandler(CityBuyForm_Load);   //so you set the correct size of form
                var result = CityBuyForm.ShowDialog();
                if (result == DialogResult.OK)  //buying item activated
                {
                    int cost = 0;
                    if (ThisCity.ItemInProduction < 62) 
                        cost = ReadFiles.UnitCost[ThisCity.ItemInProduction];
                    else 
                        cost = ReadFiles.ImprovementCost[ThisCity.ItemInProduction - 62 + 1];
                    Game.Civs[1].Money -= 10 * cost - ThisCity.ShieldsProgress;
                    ThisCity.ShieldsProgress = 10 * cost;
                    ProductionPanel.Refresh();
                }
            }
        }

        private void CityBuyForm_Load(object sender, EventArgs e)
        {
            Form frm = sender as Form;
            frm.Location = new Point(250, 300);
            frm.Width = 758;
            frm.Height = 212;
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            //Use this so the form returns a chosen value (what it has chosen to produce)
            using (var CityChangeForm = new CityChangeForm(ThisCity))
            {
                CityChangeForm.Load += new EventHandler(CityChangeForm_Load);   //so you set the correct size of form
                var result = CityChangeForm.ShowDialog();
                if (result == DialogResult.OK)  //when form is closed
                {
                    ProductionPanel.Refresh();
                }
            }
        }

        private void CityChangeForm_Load(object sender, EventArgs e)
        {
            Form frm = sender as Form;
            frm.Width = 686;
            frm.Height = 458;
            frm.Location = new Point(200, 100);
        }

        private void InfoButton_Click(object sender, EventArgs e)
        {
        }

        private void MapButton_Click(object sender, EventArgs e)
        {
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            CityRenameForm CityRenameForm = new CityRenameForm(ThisCity);
            CityRenameForm.RefreshCityForm += RefreshThis;
            CityRenameForm.ShowDialog();
        }

        void RefreshThis()
        {
            Refresh();
        }

        private void HappyButton_Click(object sender, EventArgs e) { }

        private void ViewButton_Click(object sender, EventArgs e) { }

        private void NextCityButton_Click(object sender, EventArgs e) { }

        private void PrevCityButton_Click(object sender, EventArgs e) { }

        private void NextCityButton_Paint(object sender, PaintEventArgs e)
        {
            //Draw lines in button
            e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 1, 30, 1);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 2, 29, 2);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 1, 1, 33);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 2, 1, 2, 32);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 1, 34, 30, 34);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 2, 33, 30, 33);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 29, 2, 29, 33);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 30, 1, 30, 33);
            //Draw the arrow icon
            e.Graphics.DrawImage(Images.NextCityLarge, 2, 1);
            e.Dispose();
        }
                
        private void PrevCityButton_Paint(object sender, PaintEventArgs e)
        {
            //Draw lines in button
            e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 1, 30, 1);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 2, 29, 2);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 1, 1, 33);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 2, 1, 2, 32);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 1, 34, 30, 34);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 2, 33, 30, 33);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 29, 2, 29, 33);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 30, 1, 30, 33);
            //Draw the arrow icon
            e.Graphics.DrawImage(Images.PrevCityLarge, 2, 1);
            e.Dispose();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
