﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheapLoc;

namespace XIVLauncher.Windows.ViewModel
{
    class OtpInputDialogViewModel
    {
        public OtpInputDialogViewModel()
        {
            SetupLoc();
        }

        private void SetupLoc()
        {
            OtpInputPromptLoc = Loc.Localize("OtpInputPrompt", "Please enter your OTP key.");
            CancelWithShortcutLoc = Loc.Localize("CancelWithShortcut", "_Cancel");
            OkLoc = Loc.Localize("OK", "OK");
            OtpOneClickHintLoc = Loc.Localize("OtpOneClickHint", "Or use the app!\r\nClick here to learn more!");
        }

        public string OtpInputPromptLoc { get; private set; }
        public string CancelWithShortcutLoc { get; private set; }
        public string OkLoc { get; private set; }
        public string OtpOneClickHintLoc { get; private set; }
    }
}
