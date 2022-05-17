﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Serilog;
using XIVLauncher.Common.Dalamud;
using XIVLauncher.Common.PlatformAbstractions;
using XIVLauncher.Common.Unix.Compatibility;

namespace XIVLauncher.Common.Unix;

public class UnixGameRunner : IGameRunner
{
    private readonly CompatibilityTools compatibility;
    private readonly DalamudLauncher dalamudLauncher;
    private readonly bool dalamudOk;

    public static readonly Regex ArgumentRegex = new(@"(\/\/\*\*sqex[^\s]+|\s*[^\s=]+\s*=\s*([^=]*$|[^=]*\s(?=[^\s=]+))\s*)", RegexOptions.Compiled);

    public UnixGameRunner(CompatibilityTools compatibility, DalamudLauncher dalamudLauncher, bool dalamudOk)
    {
        this.compatibility = compatibility;
        this.dalamudLauncher = dalamudLauncher;
        this.dalamudOk = dalamudOk;
    }

    public Process? Start(string path, string workingDirectory, string arguments, IDictionary<string, string> environment, DpiAwareness dpiAwareness)
    {
        if (dalamudOk)
        {
            return this.dalamudLauncher.Run(new FileInfo(path), arguments, environment);
        }
        else
        {
            var launchArguments = new List<string> { path };

            // Ideally game arguments would already be an array
            foreach (Match match in ArgumentRegex.Matches(arguments))
                launchArguments.Add(match.Value);

            return compatibility.RunInPrefix(launchArguments.ToArray(), workingDirectory, environment);
        }
    }
}