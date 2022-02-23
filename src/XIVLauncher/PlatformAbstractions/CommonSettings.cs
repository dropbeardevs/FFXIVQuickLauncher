using System.IO;
using XIVLauncher.Common;
using XIVLauncher.Common.Game.Patch.Acquisition;
using XIVLauncher.Common.PlatformAbstractions;

namespace XIVLauncher.PlatformAbstractions
{
    public class CommonSettings : ISettings
    {
        private static CommonSettings _instance;
        
        public static CommonSettings Instance
        {
            get
            {
                _instance ??= new CommonSettings();
                return _instance;
            }
        }

        public string AcceptLanguage => App.Settings.AcceptLanguage;
        public ClientLanguage? ClientLanguage => App.Settings.Language;
        public bool? KeepPatches => App.Settings.KeepPatches;
        public DirectoryInfo PatchPath => App.Settings.PatchPath;
        public DirectoryInfo GamePath => App.Settings.GamePath;
        public AcquisitionMethod? PatchAcquisitionMethod => App.Settings.PatchAcquisitionMethod;
        public long SpeedLimitBytes => App.Settings.SpeedLimitBytes;
    }
}