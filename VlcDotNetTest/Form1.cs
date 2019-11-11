using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace VlcDotNetTest
{
    public partial class Form1 : Form
    {
        public VlcControl VlcControl { get; private set; }
        private static string[] _vlcOptions;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;
            Width = 1080;
            Height = 1920;

            VlcControl = new VlcControl()
            {
                Location = Location,
                Size = Size,
                BackColor = Color.FromArgb(110, 110, 110),
                Dock = DockStyle.Fill,
            };

            VlcControl.BeginInit();
            //LoadVlcConfig();

//            var vlcPath = Environment.Is64BitOperatingSystem
//                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"VideoLAN\VLC")
//                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"VideoLAN\VLC");
//
//            VlcControl.VlcLibDirectory = new DirectoryInfo(vlcPath);
            VlcControl.VlcLibDirectory = new DirectoryInfo(@"libvlc\win-x86");
            VlcControl.EndInit();

            Controls.Add(VlcControl);

            VlcControl.Stopped += (o, args) =>
            {
                VlcControl.Stop();
                VlcControl.ResetMedia();
            };

            VlcControl.VlcMediaPlayer.EndReached += VlcMediaPlayer_EndReached;

            const string source =
                @"C:\Users\am\AppData\Roaming\PlusAMedia.Debug\wwwroot\plusamedia.com\docs\Customers\pam\536-5cd5d697116e4\digitalmenu\videos\1572558410-smart.media.ads-video1.mp4";

            VlcControl.SetMedia(new FileInfo(source));
            
            VlcControl.Play();

        }

        private void VlcMediaPlayer_EndReached( object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e )
        {
         //   Debug.WriteLine("End Reached");
        }

        private void LoadVlcConfig()
        {
            if (null == _vlcOptions)
            {
                ReadVlcConfigOptions();
            }

            if (null != _vlcOptions)
            {
                VlcControl.VlcMediaplayerOptions = _vlcOptions;
            }
        }

        private void ReadVlcConfigOptions()
        {
            var codeBase = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;

            if (codeBase.IndexOf(@"file:///", StringComparison.Ordinal) == 0)
            {
                codeBase = codeBase.Substring(@"file:///".Length);
            }

            var binPath = Path.GetDirectoryName(codeBase);

            if (binPath != null && binPath.IndexOf(@"file:\", StringComparison.Ordinal) == 0)
            {
                binPath = binPath.Substring(@"file:\".Length);
            }

            if (binPath != null)
            {
                var vlcConfigFileName = Path.Combine(binPath, "vlc.conf");

                if (File.Exists(vlcConfigFileName))
                {
                    var options = File.ReadAllLines(vlcConfigFileName);
                    _vlcOptions = options.Where(option => option.IndexOf("//", StringComparison.Ordinal) != 0)
                        .ToArray();
                }
            }
        }
    }
}
