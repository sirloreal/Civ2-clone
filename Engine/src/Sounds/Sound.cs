﻿using System;
using System.Media;
using Civ2engine.Events;
using Civ2engine.Enums;

namespace Civ2engine.Sounds
{
    public static class Sound
    {
        //private static SoundPlayer MOVPIECE;

        public static void LoadSounds(string directoryPath)
        {
            //MOVPIECE = new SoundPlayer(directoryPath + "Sound" + Path.DirectorySeparatorChar + "MOVPIECE.WAV");

            Game.OnUnitEvent += UnitEventHappened;
        }

        private static void UnitEventHappened(object sender, UnitEventArgs e)
        {
            //if (e.EventType == UnitEventType.MoveCommand && e.Counter == 0) MOVPIECE.Play();
        }
    }
}
