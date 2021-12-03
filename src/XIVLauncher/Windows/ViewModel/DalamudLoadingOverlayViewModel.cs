﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheapLoc;

namespace XIVLauncher.Windows.ViewModel
{
    class DalamudLoadingOverlayViewModel
    {
        public DalamudLoadingOverlayViewModel()
        {
            SetupLoc();
        }

        public void SetupLoc()
        {
            DalamudUpdateLoc = Loc.Localize("DalamudUpdate", "Updating Dalamud...");
        }

        public string DalamudUpdateLoc { get; private set; }
    }
}