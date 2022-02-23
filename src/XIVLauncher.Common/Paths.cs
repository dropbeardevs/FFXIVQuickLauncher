﻿using System;
using System.IO;

namespace XIVLauncher.Common
{
    public class Paths
    {
        static Paths()
        {
            RoamingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncher");
        }

        public static string RoamingPath { get; private set; }

        public static string ResourcesPath => Path.Combine(Path.GetDirectoryName(typeof(Paths).Assembly.Location), "Resources");

        public static void OverrideRoamingPath(string path)
        {
            RoamingPath = path;
        }
    }
}