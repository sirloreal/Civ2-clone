using Civ2engine.Terrains;
using Civ2engine.Units;
using Model.Core;
using Model.Core.GoodyHuts;
using Model.Core.Mapping;

namespace Civ2engine.MapObjects
{
    public class Tile : IMapItem
    {
        private City? _workedBy;

        public int Owner { get; set; } = -1;
        
        private Terrain _terrain;
        
        private bool[] _visibility;
        public int X { get; }
        public int Y { get; }
        
        public int Z { get;}

        public int Odd { get; }

        public Terrain Terrain
        {
            get => _terrain;
            set
            {
                if (_terrain == value) return;
                _terrain = value;
                if (Special != -1 && Special < _terrain.Specials.Length)
                {
                    EffectiveTerrain = _terrain.Specials[Special];
                }
                else
                {
                    EffectiveTerrain = _terrain;
                }
            }
        }

        public Map Map { get; }
        public int XIndex { get; }

        public ITerrain EffectiveTerrain { get; private set; }

        public TerrainType Type => Terrain.Type;

        public int Special { get; }

        // Get special resource type based on map seed & tile location
        public Tile(int x, int y, Terrain terrain, int seed, Map map, int xIndex, bool[] visibility)
        {
            _visibility = visibility;
        
            // Courtesy of Civfanatics
            // https://forums.civfanatics.com/threads/is-there-really-no-way-to-do-this-add-resources-on-map.518649/#post-13002282
            X = x;
            Y = y;
            Z = map.MapIndex;
            Odd = y % 2;
            Map = map;
            XIndex = xIndex;

            HasShield = HasSheild();

            // Special resource type based on map seed & tile location
            var a = (X + Y) >> 1;
            var b = X - a;
            var c = 13 * (b >> 2) + 11 * ((X + Y) >> 3) + seed;
            
            if ((a & 3) + 4 * (b & 3) != (c & 15))
            {
                Special = -1;
            }
            else
            {
                var d = 1 << ((seed >> 4) & 3);
                Special = (d & a) == (d & b) ? 1 : 0;
            }

            IsGoodyHutTile = GoodyHutAlgo2(seed);
            Console.WriteLine("Has GoodyHut: " + IsGoodyHutTile);

            // Terrain must be set after special to get the correct EffectiveTerrain type for specials
            Terrain = terrain;
        }

        private bool GoodyHutAlgo2(int seed)
        {
            // This one is close but not quite right. Has the patterns but offset slightly on the map.
            var nSum = (X + Y) / 2;
            var nDiff = (X - Y) / 2;
            nDiff = (nDiff + 4096) % 4096;
            var hash = (nSum / 4) * 11 + (nDiff / 4) * 13 + 8;
            hash = (hash + seed) % 32;
            var expectedHash = (nSum % 4) + (nDiff % 4) * 4;

            return hash == expectedHash;
        }

        private bool GoodyHutAlgo1(int seed)
        {
            var nSum = (X + Y) / 2;
            var nDiff = (X - Y) / 2;
            nDiff = (nDiff + 4096) % 4096;

            int nSumDiv4 = nSum / 4;
            int nDiffDiv4 = nDiff / 4;
            int hash = nSumDiv4 * 11 + nDiffDiv4 * 13 + 8;

            int seedComponent = (nSumDiv4 % 4) + (nDiffDiv4 % 4) * 4 - (hash / 32) % 32;
            int finalSeed = (seedComponent + 32) % 32;

            Console.WriteLine("Seed: " + seed);
            Console.WriteLine("FinalSeed: " + finalSeed);

            // Adjusted hash for comparison
            int adjustedHash = (hash + seed) % 32;
            int expectedHash = (nSumDiv4 % 4) + (nDiffDiv4 % 4) * 4;

            // TODO: Is Land Tile
            return adjustedHash == expectedHash;
        }

        public bool HasShield { get; }
        
        private GoodyHut? _goodyHut = new GoodyHut();
        public bool IsGoodyHutTile { get; private set; }
        public bool HasGoodyHut { get { return _goodyHut != null;  } }      
        public void ConsumeGoodyHut(Unit unit)
        {
            _goodyHut?.Trigger(unit);
            _goodyHut = null; // Consume / remove the goody hut from the game.
        }

        private bool HasSheild()
        {
            // Formula for determining where grassland shield is based on X-Y coords
            int rez4 = (Y / 2 + 2 * (Y % 2)) % 4;
            int rez3 = 8 - 2 * (rez4 % 4);

            int rez = (X - (Y % 2) + rez3) % 8;
            return rez < 4;
        }



        // From RULES.TXT
        public string Name => _terrain.Name;
        public string? SpecialsName => Special != -1 && Special < _terrain.Specials.Length ? _terrain.Specials[Special].Name : null;

        public int MoveCost => EffectiveTerrain.MoveCost;
        public int Defense => (River ? EffectiveTerrain.Defense + 1 : EffectiveTerrain.Defense) / 2;

        
        public bool Resource { get; set; }
        public bool River { get; set; }
        public bool IsUnitPresent => UnitsHere.Count > 0;
        public bool IsCityPresent => CityHere != null;
        public int Island { get; set; }
        public decimal Fertility { get; set; } = -1;
        
        public void SetVisible(int civId, bool visible = true)
        {
            if (_visibility.Length <= civId)
            {
                _visibility = _visibility.Concat(Enumerable.Repeat(false, civId - _visibility.Length +1)).ToArray();
            }

            _visibility[civId] = visible;
        }
        
        public bool IsVisible(int civId)
        {
            return civId < _visibility.Length && _visibility[civId];
        }

        public List<Unit> UnitsHere { get; } = new();

        public City? CityHere { get; set; }

        public City? WorkedBy
        {
            get => _workedBy;
            set
            {
                if (value != _workedBy)
                {
                    _workedBy?.WorkedTiles.Remove(this);
                    _workedBy = CityHere ??
                                value; //If there is a city here then that's to only city that can work this tile 
                }

                if (_workedBy?.WorkedTiles.Contains(this) == false)
                {
                    _workedBy.WorkedTiles.Add(this);
                }

                if (_workedBy != null)
                {
                    Owner = _workedBy.OwnerId;
                }
            }
        }

        public List<ConstructedImprovement> Improvements { get; init; } = new();

        public List<ActiveEffect> EffectsList { get; } = new();
        
        public PlayerTile?[]? PlayerKnowledge { get; set; }

        public bool[] Visibility => _visibility;
    }
}