﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PoskusCiv2.Imagery
{
    public static class Images
    {
        public static Bitmap[] Desert, Plains, Grassland, ForestBase, HillsBase, MtnsBase, Tundra, Glacier, Swamp, Jungle, Ocean, River, Forest, Mountains, Hills,  RiverMouth, Road, Railroad, Units, UnitShield, CityFlag;
        public static Bitmap[,] Coast, City, CityWall;
        public static Bitmap Irrigation, Farmland, Mining, Pollution, Fortified, Fortress, Airbase, AirbasePlane, Shield, ViewingPieces, WallpaperMapForm, WallpaperStatusForm, BlackUnitShield, GridLines, GridLinesVisible, Blank;
        public static int[,] unitShieldLocation = new int[63, 2];
        public static int[,,] cityFlagLoc, cityWallFlagLoc, citySizeWindowLoc, cityWallSizeWindowLoc;
        //public static int[,,] cityWallFlagLoc = new int[6, 4, 2];
        public static Color[] CivColors;
        public static Bitmap CityWallpaper;

        public static void LoadTerrain(string terrainLoc1, string terrainLoc2)
        {
            Bitmap terrain1 = new Bitmap(terrainLoc1);
            Bitmap terrain2 = new Bitmap(terrainLoc2);
            Desert = new Bitmap[4];
            Plains = new Bitmap[4];
            Grassland = new Bitmap[4];
            ForestBase = new Bitmap[4];
            HillsBase = new Bitmap[4];
            MtnsBase = new Bitmap[4];
            Tundra = new Bitmap[4];
            Glacier = new Bitmap[4];
            Swamp = new Bitmap[4];
            Jungle = new Bitmap[4];
            Ocean = new Bitmap[4];
            Coast = new Bitmap[8, 4];
            River = new Bitmap[16];
            Forest = new Bitmap[16];
            Mountains = new Bitmap[16];
            Hills = new Bitmap[16];
            RiverMouth = new Bitmap[4];
            Road = new Bitmap[9];
            Railroad = new Bitmap[9];

            //define transparent colors
            Color transparentGray = Color.FromArgb(135, 135, 135);    //define transparent back color (gray)
            Color transparentPink = Color.FromArgb(255, 0, 255);    //define transparent back color (pink)
            Color transparentCyan = Color.FromArgb(0, 255, 255);    //define transparent back color (cyan)

            //Tiles
            for (int i = 0; i < 4; i++)
            {
                Desert[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 1, 64, 32), terrain1.PixelFormat);
                Desert[i].MakeTransparent(transparentGray);
                Desert[i].MakeTransparent(transparentPink);
                Plains[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 2 * 1 + 1 * 32, 64, 32), terrain1.PixelFormat);
                Plains[i].MakeTransparent(transparentGray);
                Plains[i].MakeTransparent(transparentPink);
                Grassland[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 3 * 1 + 2 * 32, 64, 32), terrain1.PixelFormat);
                Grassland[i].MakeTransparent(transparentGray);
                Grassland[i].MakeTransparent(transparentPink);
                ForestBase[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 4 * 1 + 3 * 32, 64, 32), terrain1.PixelFormat);
                ForestBase[i].MakeTransparent(transparentGray);
                ForestBase[i].MakeTransparent(transparentPink);
                HillsBase[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 5 * 1 + 4 * 32, 64, 32), terrain1.PixelFormat);
                HillsBase[i].MakeTransparent(transparentGray);
                HillsBase[i].MakeTransparent(transparentPink);
                MtnsBase[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 6 * 1 + 5 * 32, 64, 32), terrain1.PixelFormat);
                MtnsBase[i].MakeTransparent(transparentGray);
                MtnsBase[i].MakeTransparent(transparentPink);
                Tundra[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 7 * 1 + 6 * 32, 64, 32), terrain1.PixelFormat);
                Tundra[i].MakeTransparent(transparentGray);
                Tundra[i].MakeTransparent(transparentPink);
                Glacier[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 8 * 1 + 7 * 32, 64, 32), terrain1.PixelFormat);
                Glacier[i].MakeTransparent(transparentGray);
                Glacier[i].MakeTransparent(transparentPink);
                Swamp[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 9 * 1 + 8 * 32, 64, 32), terrain1.PixelFormat);
                Swamp[i].MakeTransparent(transparentGray);
                Swamp[i].MakeTransparent(transparentPink);
                Jungle[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 10 * 1 + 9 * 32, 64, 32), terrain1.PixelFormat);
                Jungle[i].MakeTransparent(transparentGray);
                Jungle[i].MakeTransparent(transparentPink);
                Ocean[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 11 * 1 + 10 * 32, 64, 32), terrain1.PixelFormat);
                Ocean[i].MakeTransparent(transparentGray);
                Ocean[i].MakeTransparent(transparentPink);
            }

            //Blank tile
            Blank = (Bitmap)terrain1.Clone(new Rectangle(131, 447, 64, 32), terrain1.PixelFormat);
            Blank.MakeTransparent(transparentGray);

            //Rivers, Forest, Mountains, Hills
            for (int i = 0; i < 16; i++)
            {
                River[i] = (Bitmap)terrain2.Clone(new Rectangle(i % 8 + 1 + (i % 8) * 64, 3 + i / 8 + (2 + i / 8) * 32, 64, 32), terrain2.PixelFormat);
                River[i].MakeTransparent(transparentGray);
                River[i].MakeTransparent(transparentPink);
                Forest[i] = (Bitmap)terrain2.Clone(new Rectangle(i % 8 + 1 + (i % 8) * 64, 5 + i / 8 + (4 + i / 8) * 32, 64, 32), terrain2.PixelFormat);
                Forest[i].MakeTransparent(transparentGray);
                Forest[i].MakeTransparent(transparentPink);
                Mountains[i] = (Bitmap)terrain2.Clone(new Rectangle(i % 8 + 1 + (i % 8) * 64, 7 + i / 8 + (6 + i / 8) * 32, 64, 32), terrain2.PixelFormat);
                Mountains[i].MakeTransparent(transparentGray);
                Mountains[i].MakeTransparent(transparentPink);
                Hills[i] = (Bitmap)terrain2.Clone(new Rectangle(i % 8 + 1 + (i % 8) * 64, 9 + i / 8 + (8 + i / 8) * 32, 64, 32), terrain2.PixelFormat);
                Hills[i].MakeTransparent(transparentGray);
                Hills[i].MakeTransparent(transparentPink);
            }

            //River mouths
            for (int i = 0; i < 4; i++)
            {
                RiverMouth[i] = (Bitmap)terrain2.Clone(new Rectangle(i + 1 + i * 64, 11 * 1 + 10 * 32, 64, 32), terrain2.PixelFormat);
                RiverMouth[i].MakeTransparent(transparentGray);
                RiverMouth[i].MakeTransparent(transparentPink);
                RiverMouth[i].MakeTransparent(transparentCyan);
            }

            //Coast
            for (int i = 0; i < 8; i++)
            {
                Coast[i, 0] = (Bitmap)terrain2.Clone(new Rectangle(2 * i + 1 + 2 * i * 32, 429, 32, 16), terrain2.PixelFormat);  // N
                Coast[i, 0].MakeTransparent(transparentGray);
                Coast[i, 1] = (Bitmap)terrain2.Clone(new Rectangle(2 * i + 1 + 2 * i * 32, 429 + 1 * 1 + 1 * 16, 32, 16), terrain2.PixelFormat);  // S
                Coast[i, 1].MakeTransparent(transparentGray);
                Coast[i, 2] = (Bitmap)terrain2.Clone(new Rectangle(2 * i + 1 + 2 * i * 32, 429 + 2 * 1 + 2 * 16, 32, 16), terrain2.PixelFormat);  // W
                Coast[i, 2].MakeTransparent(transparentGray);
                Coast[i, 3] = (Bitmap)terrain2.Clone(new Rectangle(2 * (i + 1) + (2 * i + 1) * 32, 429 + 2 * 1 + 2 * 16, 32, 16), terrain2.PixelFormat);  // E
                Coast[i, 3].MakeTransparent(transparentGray);
            }

            //Road & railorad
            for (int i = 0; i < 9; i++)
            {
                Road[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 364, 64, 32), terrain1.PixelFormat);
                Road[i].MakeTransparent(transparentGray);
                Road[i].MakeTransparent(transparentPink);

                Railroad[i] = (Bitmap)terrain1.Clone(new Rectangle(i + 1 + i * 64, 397, 64, 32), terrain1.PixelFormat);
                Railroad[i].MakeTransparent(transparentGray);
                Railroad[i].MakeTransparent(transparentPink);
            }

            Irrigation = (Bitmap)terrain1.Clone(new Rectangle(456, 100, 64, 32), terrain1.PixelFormat);
            Irrigation.MakeTransparent(transparentGray);
            Irrigation.MakeTransparent(transparentPink);

            Farmland = (Bitmap)terrain1.Clone(new Rectangle(456, 133, 64, 32), terrain1.PixelFormat);
            Farmland.MakeTransparent(transparentGray);
            Farmland.MakeTransparent(transparentPink);

            Mining = (Bitmap)terrain1.Clone(new Rectangle(456, 166, 64, 32), terrain1.PixelFormat);
            Mining.MakeTransparent(transparentGray);
            Mining.MakeTransparent(transparentPink);

            Pollution = (Bitmap)terrain1.Clone(new Rectangle(456, 199, 64, 32), terrain1.PixelFormat);
            Pollution.MakeTransparent(transparentGray);
            Pollution.MakeTransparent(transparentPink);

            Shield = (Bitmap)terrain1.Clone(new Rectangle(456, 232, 64, 32), terrain1.PixelFormat);
            Shield.MakeTransparent(transparentGray);
            Shield.MakeTransparent(transparentPink);

        }

        public static void LoadCities(string cityLoc)
        {
            Bitmap cities = new Bitmap(cityLoc);
            City = new Bitmap[6, 4];
            CityFlag = new Bitmap[9];
            CityWall = new Bitmap[6, 4];
            cityFlagLoc = new int[6, 4, 2];
            cityWallFlagLoc = new int[6, 4, 2];
            citySizeWindowLoc = new int[6, 4, 2];
            cityWallSizeWindowLoc = new int[6, 4, 2];

            //define transparent colors
            Color transparentGray = Color.FromArgb(135, 135, 135);    //define transparent back color (gray)
            Color transparentPink = Color.FromArgb(255, 0, 255);    //define transparent back color (pink)
            Color transparentCyan = Color.FromArgb(0, 255, 255);    //define transparent back color (cyan)

            //Get city bitmaps
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    City[row, col] = (Bitmap)cities.Clone(new Rectangle(1 + 65 * col, 39 + 49 * row, 64, 48), cities.PixelFormat);
                    City[row, col].MakeTransparent(transparentGray);
                    City[row, col].MakeTransparent(transparentPink);

                    CityWall[row, col] = (Bitmap)cities.Clone(new Rectangle(334 + 65 * col, 39 + 49 * row, 64, 48), cities.PixelFormat);
                    CityWall[row, col].MakeTransparent(transparentGray);
                    CityWall[row, col].MakeTransparent(transparentPink);

                    //determine where the city size window is located (x-y)
                    for (int ix = 0; ix < 64; ix++) //in x-direction
                    {
                        if (cities.GetPixel(65 * col + ix, 38 + 49 * row) == Color.FromArgb(0, 0, 255)) { cityFlagLoc[row, col, 0] = ix; }  //if pixel on border is blue
                        if (cities.GetPixel(333 + 65 * col + ix, 38 + 49 * row) == Color.FromArgb(0, 0, 255)) { cityWallFlagLoc[row, col, 0] = ix; }  //for cities with wall
                    }
                    for (int iy = 0; iy < 48; iy++) //in y-direction
                    {
                        if (cities.GetPixel(65 * col, 38 + 49 * row + iy) == Color.FromArgb(0, 0, 255)) { cityFlagLoc[row, col, 1] = iy; }
                        if (cities.GetPixel(333 + 65 * col, 38 + 49 * row + iy) == Color.FromArgb(0, 0, 255)) { cityWallFlagLoc[row, col, 1] = iy; }
                    }
                }
            }

            //Get flag bitmaps
            for (int col = 0; col < 9; col++)
            {
                CityFlag[col] = (Bitmap)cities.Clone(new Rectangle(1 + 15 * col, 425, 14, 22), cities.PixelFormat);
                CityFlag[col].MakeTransparent(transparentGray);
            }

            //Locations of city size windows
            citySizeWindowLoc[0, 0, 0] = 13;    //stone age
            citySizeWindowLoc[0, 0, 1] = 23;
            citySizeWindowLoc[0, 1, 0] = 52;
            citySizeWindowLoc[0, 1, 1] = 18;
            citySizeWindowLoc[0, 2, 0] = 0;
            citySizeWindowLoc[0, 2, 1] = 23;
            citySizeWindowLoc[0, 3, 0] = 24;
            citySizeWindowLoc[0, 3, 1] = 29;
            citySizeWindowLoc[1, 0, 0] = 10;    //ancient
            citySizeWindowLoc[1, 0, 1] = 23;
            citySizeWindowLoc[1, 1, 0] = 50;
            citySizeWindowLoc[1, 1, 1] = 25;
            citySizeWindowLoc[1, 2, 0] = 1;
            citySizeWindowLoc[1, 2, 1] = 17;
            citySizeWindowLoc[1, 3, 0] = 12;
            citySizeWindowLoc[1, 3, 1] = 27;
            citySizeWindowLoc[2, 0, 0] = 3;    //far east
            citySizeWindowLoc[2, 0, 1] = 20;
            citySizeWindowLoc[2, 1, 0] = 48;
            citySizeWindowLoc[2, 1, 1] = 7;
            citySizeWindowLoc[2, 2, 0] = 50;
            citySizeWindowLoc[2, 2, 1] = 5;
            citySizeWindowLoc[2, 3, 0] = 28;
            citySizeWindowLoc[2, 3, 1] = 27;
            citySizeWindowLoc[3, 0, 0] = 5;    //medieval
            citySizeWindowLoc[3, 0, 1] = 22;
            citySizeWindowLoc[3, 1, 0] = 2;
            citySizeWindowLoc[3, 1, 1] = 18;
            citySizeWindowLoc[3, 2, 0] = 0;
            citySizeWindowLoc[3, 2, 1] = 18;
            citySizeWindowLoc[3, 3, 0] = 27;
            citySizeWindowLoc[3, 3, 1] = 27;
            citySizeWindowLoc[4, 0, 0] = 4;    //industrial
            citySizeWindowLoc[4, 0, 1] = 20;
            citySizeWindowLoc[4, 1, 0] = 1;
            citySizeWindowLoc[4, 1, 1] = 20;
            citySizeWindowLoc[4, 2, 0] = 2;
            citySizeWindowLoc[4, 2, 1] = 22;
            citySizeWindowLoc[4, 3, 0] = 28;
            citySizeWindowLoc[4, 3, 1] = 30;
            citySizeWindowLoc[5, 0, 0] = 8;    //modern
            citySizeWindowLoc[5, 0, 1] = 18;
            citySizeWindowLoc[5, 1, 0] = 2;
            citySizeWindowLoc[5, 1, 1] = 19;
            citySizeWindowLoc[5, 2, 0] = 8;
            citySizeWindowLoc[5, 2, 1] = 20;
            citySizeWindowLoc[5, 3, 0] = 27;
            citySizeWindowLoc[5, 3, 1] = 30;
            cityWallSizeWindowLoc[0, 0, 0] = 12;    //stone + wall
            cityWallSizeWindowLoc[0, 0, 1] = 23;
            cityWallSizeWindowLoc[0, 1, 0] = 52;
            cityWallSizeWindowLoc[0, 1, 1] = 22;
            cityWallSizeWindowLoc[0, 2, 0] = 0;
            cityWallSizeWindowLoc[0, 2, 1] = 19;
            cityWallSizeWindowLoc[0, 3, 0] = 24;
            cityWallSizeWindowLoc[0, 3, 1] = 29;
            cityWallSizeWindowLoc[1, 0, 0] = 10;    //ancient + wall
            cityWallSizeWindowLoc[1, 0, 1] = 13;
            cityWallSizeWindowLoc[1, 1, 0] = 50;
            cityWallSizeWindowLoc[1, 1, 1] = 21;
            cityWallSizeWindowLoc[1, 2, 0] = 1;
            cityWallSizeWindowLoc[1, 2, 1] = 17;
            cityWallSizeWindowLoc[1, 3, 0] = 11;
            cityWallSizeWindowLoc[1, 3, 1] = 22;
            cityWallSizeWindowLoc[2, 0, 0] = 4;    //far east + wall
            cityWallSizeWindowLoc[2, 0, 1] = 18;
            cityWallSizeWindowLoc[2, 1, 0] = 48;
            cityWallSizeWindowLoc[2, 1, 1] = 6;
            cityWallSizeWindowLoc[2, 2, 0] = 51;
            cityWallSizeWindowLoc[2, 2, 1] = 4;
            cityWallSizeWindowLoc[2, 3, 0] = 28;
            cityWallSizeWindowLoc[2, 3, 1] = 27;
            cityWallSizeWindowLoc[3, 0, 0] = 3;    //medieval + wall
            cityWallSizeWindowLoc[3, 0, 1] = 18;
            cityWallSizeWindowLoc[3, 1, 0] = 2;
            cityWallSizeWindowLoc[3, 1, 1] = 20;
            cityWallSizeWindowLoc[3, 2, 0] = 1;
            cityWallSizeWindowLoc[3, 2, 1] = 15;
            cityWallSizeWindowLoc[3, 3, 0] = 27;
            cityWallSizeWindowLoc[3, 3, 1] = 29;
            cityWallSizeWindowLoc[4, 0, 0] = 4;    //industrial + wall
            cityWallSizeWindowLoc[4, 0, 1] = 18;
            cityWallSizeWindowLoc[4, 1, 0] = 1;
            cityWallSizeWindowLoc[4, 1, 1] = 20;
            cityWallSizeWindowLoc[4, 2, 0] = 1;
            cityWallSizeWindowLoc[4, 2, 1] = 18;
            cityWallSizeWindowLoc[4, 3, 0] = 26;
            cityWallSizeWindowLoc[4, 3, 1] = 28;
            cityWallSizeWindowLoc[5, 0, 0] = 3;    //modern + wall
            cityWallSizeWindowLoc[5, 0, 1] = 21;
            cityWallSizeWindowLoc[5, 1, 0] = 0;
            cityWallSizeWindowLoc[5, 1, 1] = 20;
            cityWallSizeWindowLoc[5, 2, 0] = 8;
            cityWallSizeWindowLoc[5, 2, 1] = 20;
            cityWallSizeWindowLoc[5, 3, 0] = 27;
            cityWallSizeWindowLoc[5, 3, 1] = 30;
            
            Fortified = (Bitmap)cities.Clone(new Rectangle(143, 423, 64, 48), cities.PixelFormat);
            Fortified.MakeTransparent(transparentGray);
            Fortified.MakeTransparent(transparentPink);

            Fortress = (Bitmap)cities.Clone(new Rectangle(208, 423, 64, 48), cities.PixelFormat);
            Fortress.MakeTransparent(transparentGray);
            Fortress.MakeTransparent(transparentPink);

            Airbase = (Bitmap)cities.Clone(new Rectangle(273, 423, 64, 48), cities.PixelFormat);
            Airbase.MakeTransparent(transparentGray);
            Airbase.MakeTransparent(transparentPink);

            AirbasePlane = (Bitmap)cities.Clone(new Rectangle(338, 423, 64, 48), cities.PixelFormat);
            AirbasePlane.MakeTransparent(transparentGray);
            AirbasePlane.MakeTransparent(transparentPink);

        }

        public static void LoadUnits(string unitLoc)
        {
            Bitmap units = new Bitmap(unitLoc);

            Units = new Bitmap[63];
            UnitShield = new Bitmap[8];
            CivColors = new Color[8];
            CivColors[0] = Color.FromArgb(243, 0, 0);       //Red
            CivColors[1] = Color.FromArgb(255, 255, 255);   //White
            CivColors[2] = Color.FromArgb(0, 255, 0);     //Green
            //CivColors[3] = Color.FromArgb(75, 95, 183);     //Blue
            CivColors[3] = Color.FromArgb(0, 115, 255);     //Blue
            CivColors[4] = Color.FromArgb(255, 255, 0);     //Yellow
            //CivColors[5] = Color.FromArgb(55, 175, 191);    //Cyan
            CivColors[5] = Color.FromArgb(63, 187, 199);    //Cyan
            CivColors[6] = Color.FromArgb(243, 183, 7);    //Orange
            //CivColors[7] = Color.FromArgb(131, 103, 179);   //Violet
            CivColors[7] = Color.FromArgb(183, 147, 255);   //Violet

            //define transparent colors
            Color transparentGray = Color.FromArgb(135, 83, 135);    //define transparent back color (gray)
            Color transparentPink = Color.FromArgb(255, 0, 255);    //define transparent back color (pink)

            int stej = 0;
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Units[stej] = (Bitmap)units.Clone(new Rectangle(64 * col + 1 + col, 48 * row + 1 + row, 64, 48), units.PixelFormat);
                    Units[stej].MakeTransparent(transparentGray);
                    Units[stej].MakeTransparent(transparentPink);

                    //determine where the unit shield is located (x-y)
                    for (int ix = 0; ix < 64; ix++) //in x-direction
                    {
                        if (units.GetPixel(65 * col + ix, 49 * row) == Color.FromArgb(0, 0, 255)) { unitShieldLocation[stej, 0] = ix; }  //if pixel on border is blue
                    }
                    for (int iy = 0; iy < 48; iy++) //in y-direction
                    {
                        if (units.GetPixel(65 * col, 49 * row + iy) == Color.FromArgb(0, 0, 255)) { unitShieldLocation[stej, 1] = iy; }
                    }

                    stej += 1;
                }
            }

            //Extract shield with black border
            BlackUnitShield = (Bitmap)units.Clone(new Rectangle(599, 1, 12, 20), units.PixelFormat);
            BlackUnitShield.MakeTransparent(transparentGray);
            BlackUnitShield.MakeTransparent(transparentPink);

            //Extract unit shield
            Bitmap _unitShield = (Bitmap)units.Clone(new Rectangle(597, 30, 12, 20), units.PixelFormat);
            _unitShield.MakeTransparent(transparentGray);  //gray

            //Make shields of different colors for 8 different civs
            UnitShield[0] = CreateNonIndexedImage(_unitShield); //convert GIF to non-indexed picture
            UnitShield[1] = CreateNonIndexedImage(_unitShield);
            UnitShield[2] = CreateNonIndexedImage(_unitShield);
            UnitShield[3] = CreateNonIndexedImage(_unitShield);
            UnitShield[4] = CreateNonIndexedImage(_unitShield);
            UnitShield[5] = CreateNonIndexedImage(_unitShield);
            UnitShield[6] = CreateNonIndexedImage(_unitShield);
            UnitShield[7] = CreateNonIndexedImage(_unitShield);
            //Replace colors
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (_unitShield.GetPixel(x, y) == transparentPink)    //pink
                    {
                        UnitShield[0].SetPixel(x, y, CivColors[0]);  //red
                        UnitShield[1].SetPixel(x, y, CivColors[1]);  //white
                        UnitShield[2].SetPixel(x, y, CivColors[2]);    //green
                        UnitShield[3].SetPixel(x, y, CivColors[3]);    //blue
                        UnitShield[4].SetPixel(x, y, CivColors[4]);    //yellow
                        UnitShield[5].SetPixel(x, y, CivColors[5]);   //cyan
                        UnitShield[6].SetPixel(x, y, CivColors[6]);   //orange
                        UnitShield[7].SetPixel(x, y, CivColors[7]);   //violet
                    }
                }
            }

        }

        public static void LoadIcons(string iconLoc)
        {
            Bitmap icons = new Bitmap(iconLoc);

            //define transparent colors
            Color transparentGray = Color.FromArgb(135, 83, 135);    //define transparent back color (gray)
            Color transparentLightPink = Color.FromArgb(255, 159, 163);//define transparent back color (light pink)
            Color transparentPink = Color.FromArgb(255, 0, 255);    //define transparent back color (pink)

            ViewingPieces = (Bitmap)icons.Clone(new Rectangle(199, 256, 64, 32), icons.PixelFormat);
            ViewingPieces.MakeTransparent(transparentLightPink);  //light pink
            ViewingPieces.MakeTransparent(transparentPink);  //pink

            GridLines = (Bitmap)icons.Clone(new Rectangle(183, 430, 64, 32), icons.PixelFormat);
            GridLines.MakeTransparent(transparentLightPink);  //light pink
            GridLines.MakeTransparent(transparentPink);  //pink

            GridLinesVisible = (Bitmap)icons.Clone(new Rectangle(248, 430, 64, 32), icons.PixelFormat);
            GridLinesVisible.MakeTransparent(transparentLightPink);  //light pink
            GridLinesVisible.MakeTransparent(transparentPink);  //pink

            WallpaperMapForm = (Bitmap)icons.Clone(new Rectangle(199, 322, 64, 32), icons.PixelFormat);
            WallpaperStatusForm = (Bitmap)icons.Clone(new Rectangle(299, 190, 31, 31), icons.PixelFormat);
        }

        public static void LoadWallpapers(string cityWallpaperLoc)
        {
            Bitmap cityWallpaper = new Bitmap(cityWallpaperLoc);
            
            CityWallpaper = (Bitmap)cityWallpaper;
            CityWallpaper = ModifyImage.CropImage(CityWallpaper, new Rectangle(0, 0, 640, 420));
        }

        //Converting GIFs to non-indexed images (required for SetPixel method)
        private static Bitmap CreateNonIndexedImage(Image src)
        {
            Bitmap newBmp = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics gfx = Graphics.FromImage(newBmp))
            {
                gfx.DrawImage(src, 0, 0);
            }

            return newBmp;
        }
    }
}
