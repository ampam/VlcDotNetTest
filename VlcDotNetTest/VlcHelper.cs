using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Vlc.DotNet.Forms;
using TimerX = System.Timers.Timer;

namespace VlcDotNetTest
{

    internal class VlcHelper
    {
        private static readonly string source =
            @"C:\Users\am\AppData\Roaming\PlusAMedia.Debug\wwwroot\plusamedia.com\docs\Customers\pam\536-5cd5d697116e4\digitalmenu\videos\1572558249-sm-cci-octubre09-3-video2.mp4";

        private static string[] _vlcOptions;
        public static VlcControl CreateVlcControl( Form hostForm )
        {
            var result = new VlcControl()
            {
                Location = new Point( 0, 0  ),
                Size = hostForm.Size,
                BackColor = Color.FromArgb(110, 110, 110),
                Dock = DockStyle.Fill,
            };

            result.BeginInit();

            LoadVlcConfig();
            if (null != _vlcOptions)
            {
                result.VlcMediaplayerOptions = _vlcOptions;
            }

            result.VlcLibDirectory = new DirectoryInfo(@"libvlc\win-x86");
            result.EndInit();

            hostForm.Controls.Add(result);


            result.VlcMediaPlayer.EndReached += (sender, args) =>
            {

                hostForm.BeginInvoke(new Action(() =>
                {
                     //ReCreateVlcControl( result, hostForm );
                     //RecycleVlcControl( result, hostForm );
                     ReplayMedia( result, hostForm );
                }));
            };



            result.SetMedia(new FileInfo(source));

            result.Play();

            return result;
        }

        private static void ReplayMedia( VlcControl oldControl, Form hostForm )
        {
            oldControl.Stop();

            var timer = new TimerX {Interval = 1};
            ElapsedEventHandler handler = (s, e) =>
            {
                hostForm.BeginInvoke(new Action(() =>
                {
                    timer.Enabled = false;
                    oldControl.VlcMediaPlayer.Position = 0;
                    oldControl.Play();
                }));
            };


            timer.Elapsed += handler;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private static void RecycleVlcControl( VlcControl oldControl, Form hostForm )
        {
            oldControl.Stop();
            oldControl.ResetMedia();
//            hostForm.Controls.Remove(oldControl);
//            oldControl.Dispose();

            var timer = new TimerX {Interval = 1};
            ElapsedEventHandler handler = (s, e) =>
            {
                hostForm.BeginInvoke(new Action(() =>
                {
                 //   CreateVlcControl(hostForm);
                    timer.Enabled = false;
                    oldControl.SetMedia(new FileInfo(source));
                    oldControl.Play();
                }));
            };


            timer.Elapsed += handler;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private static void ReCreateVlcControl( VlcControl oldControl, Form hostForm )
        {
            oldControl.Stop();
            oldControl.ResetMedia();
            hostForm.Controls.Remove(oldControl);
            oldControl.Dispose();

            var timer = new TimerX {Interval = 1};
            ElapsedEventHandler handler = (s, e) =>
            {
                hostForm.BeginInvoke(new Action(() =>
                {
                    CreateVlcControl(hostForm);
                    timer.Enabled = false;
                }));
            };


            timer.Elapsed += handler;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private static void VlcMediaPlayer_EndReached( object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e )
        {
            Debug.WriteLine("End Reached");
        }

        private static void ReadVlcConfigOptions()
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

        private static void LoadVlcConfig()
        {
            if (null == _vlcOptions)
            {
                ReadVlcConfigOptions();
            }

        }

    }
}
