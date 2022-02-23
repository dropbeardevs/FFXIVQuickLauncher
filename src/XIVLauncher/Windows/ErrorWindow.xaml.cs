﻿using System;
using System.Diagnostics;
using System.Media;
using System.Threading;
using System.Windows;
using XIVLauncher.Common;
using XIVLauncher.Common.Game;
using XIVLauncher.Support;
using XIVLauncher.Windows.ViewModel;

namespace XIVLauncher.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        private ErrorWindow(Exception exc, string message, string context)
        {
            InitializeComponent();

            DiscordButton.Click += SupportLinks.OpenDiscord;
            FaqButton.Click += SupportLinks.OpenFaq;
            DataContext = new ErrorWindowViewModel();

            ExceptionTextBox.AppendText(exc.ToString());
            ExceptionTextBox.AppendText("\nVersion: " + AppUtil.GetAssemblyVersion());
            ExceptionTextBox.AppendText("\nGit Hash: " + AppUtil.GetGitHash());
            ExceptionTextBox.AppendText("\nContext: " + context);
            ExceptionTextBox.AppendText("\nOS: " + Environment.OSVersion);
            ExceptionTextBox.AppendText("\n64bit? " + Environment.Is64BitProcess);

            if (App.Settings != null)
            {
                ExceptionTextBox.AppendText("\nDX11? " + App.Settings.IsDx11);
                ExceptionTextBox.AppendText("\nAddons Enabled? " + App.Settings.InGameAddonEnabled);
                ExceptionTextBox.AppendText("\nAuto Login Enabled? " + App.Settings.AutologinEnabled);
                ExceptionTextBox.AppendText("\nLanguage: " + App.Settings.Language);
                ExceptionTextBox.AppendText("\nLauncherLanguage: " + App.Settings.LauncherLanguage);
                ExceptionTextBox.AppendText("\nGame path: " + App.Settings.GamePath);

                // When this happens we probably don't want them to run into it again, in case it's an issue with a moved game for example
                App.Settings.AutologinEnabled = false;
            }

#if DEBUG
            ExceptionTextBox.AppendText("\nDebugging");
#endif

            ContextTextBlock.Text = message;

            SystemSounds.Hand.Play();

            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GitHubButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start($"{App.RepoUrl}/issues/new");
        }

        public static void Show(Exception exc, string message, string context)
        {
            var signal = new ManualResetEvent(false);

            var newWindowThread = new Thread(() =>
            {
                var emb = new ErrorWindow(exc, message, context);
                emb.Topmost = true;
                emb.ShowDialog();

                signal.Set();
            });

            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();

            signal.WaitOne();
        }
    }
}