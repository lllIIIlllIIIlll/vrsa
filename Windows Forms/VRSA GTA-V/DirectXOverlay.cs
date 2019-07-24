using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Overlay.NET.Common;
using Overlay.NET.Directx;
using Overlay.NET.GTAV.Models;
using Process.NET;
using Process.NET.Memory;
using Process.NET.Windows;

namespace Overlay.NET.GTAV.Directx
{

    public class DirectXOverlay
    {

        private OverlayPlugin directXOverlay;
        private ProcessSharp _processSharp;
        private int fps;

        BackgroundWorker bw = new BackgroundWorker();

        public DirectXOverlay()
        {
            fps = 60;

            directXOverlay = new DirectXOverlayPlugin();
        }

        public void Start()
        {
            var processName = "gta5";

            var process = System.Diagnostics.Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null)
            {
                MessageBox.Show(processName + ".exe not running");
                Environment.Exit(0);
            }

            _processSharp = new ProcessSharp(process, MemoryType.Remote);

            var d3DOverlay = (DirectXOverlayPlugin)directXOverlay;
            d3DOverlay.Settings.Current.UpdateRate = 1000 / fps;
            directXOverlay.Initialize(_processSharp.WindowFactory.MainWindow);
            directXOverlay.Enable();


            bw.DoWork += Bw_DoWork;
            bw.RunWorkerAsync();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                directXOverlay.Update();
            }
        }
    }

    [RegisterPlugin("DirectXOverlay", "lllllIIIIllI", "DirectXOverlay", "0.1", "A basic DirectX Overlay")]
    public class DirectXOverlayPlugin : NET.Directx.DirectXOverlayPlugin
    {
        private readonly TickEngine _tickEngine = new TickEngine();
        public readonly ISettings<OverlaySettings> Settings = new SerializableSettings<OverlaySettings>();
        private IFirebaseClient _client;
        private Stopwatch _watch;
        private System.Timers.Timer _dataTimer;
        private User _user;
        private float _rotation;
        private int
            _displayFps,
            _font,
            _fontBold,
            _fps,
            _greenSaleBrush,
            _blueSupplyBrush,
            _exteriorBrush,
            _blackBrush,
            _whiteBrush,
            _backgroundBrush,
            _redBrush;

        public override void Initialize(IWindow targetWindow)
        {
            base.Initialize(targetWindow);

            InitializeSettings(GetType());
            InitializeData();

            OverlayWindow = new DirectXOverlayWindow(targetWindow.Handle, false);

            InitializeStyles();

            _watch = Stopwatch.StartNew();

            _rotation = 0.0f;
            _displayFps = 0;
            _fps = 0;

            _tickEngine.PreTick += OnPreTick;
            _tickEngine.Tick += OnTick;

            System.Diagnostics.Process.Start("file:///C:/Users/lllllIIIllIlIllI/Downloads/vrsa/index.html");
        }

        private void InitializeData()
        {
            _user = new User();

            _client = new FirebaseClient(new FirebaseConfig
            {
                AuthSecret = "hg3M2mMzq6ABYbevSswjIKC7tdS8vDLArsedRGiT",
                BasePath = "https://vrsa-5d0b9.firebaseio.com"
            });

            _client.OnAsync("Users/Barcode", (sender, args, context) => {
                _dataTimer.Stop();
                _dataTimer.Start();
            });

            _dataTimer = new System.Timers.Timer(5000);
            _dataTimer.Elapsed += _timer_Elapsed;
            _dataTimer.Start();
        }

        public void InitializeStyles()
        {
            _whiteBrush = RgbBrush(0, 255, 255, 255);
            _blackBrush = RgbBrush(0, 0, 0, 0);
            _backgroundBrush = RgbBrush(128, 0, 0, 0);
            _redBrush = RgbBrush(0, 9, 0, 0);
            _exteriorBrush = RgbBrush(128, 0, 0, 0);

            _greenSaleBrush = RgbBrush(200, 0, 255, 0);
            _blueSupplyBrush = RgbBrush(200, 92, 150, 240);

            _font = OverlayWindow.Graphics.CreateFont("Arial", 12);
            _fontBold = OverlayWindow.Graphics.CreateFont("Arial", 14, true);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _dataTimer.Stop();

            var response = _client.GetAsync("Users/Barcode").Result;

            var userData = response.ResultAs<UserData>();

            _user = userData.ToUser();
        }

        public void InitializeSettings(Type type)
        {
            var current = Settings.Current;

            if (current.UpdateRate == 0)
                current.UpdateRate = 1000 / 60;

            Settings.Save();
            Settings.Load();
        }

        public void DrawData()
        {
            var baseX = 30;
            var baseY = 400;

            var barHeight = 6;
            var barWidth = 200;

            var businessHeight = 60;

            var j = _user.Businesses.Count();

            OverlayWindow.Graphics.DrawText(baseX, baseY - 20, "FPS: " + _fps, _font, _whiteBrush, false);

            OverlayWindow.Graphics.FillRectangle(baseX - 5, baseY - 25, barWidth + 5, j * businessHeight + 50, _backgroundBrush);

            for (int i = 0; i < j; i++)
            {
                var business = _user.Businesses[i];

                var time = CalculateBusinessTime(business);

                OverlayWindow.Graphics.DrawText(baseX, baseY + (i * businessHeight), business.Name + " (Sale) " + time, _font, _whiteBrush);
                OverlayWindow.Graphics.DrawBarV(baseX, baseY + (i * businessHeight) + 16, barWidth, barHeight, (float)business.Sale, 1, _exteriorBrush, _greenSaleBrush);

                OverlayWindow.Graphics.DrawText(baseX, baseY + (i * businessHeight) + 18 + barHeight, business.Name + " (Supply)", _font, _whiteBrush);
                OverlayWindow.Graphics.DrawBarV(baseX, baseY + (i * businessHeight) + 34 + barHeight, barWidth, barHeight, (float)business.Supply, 1, _exteriorBrush, _blueSupplyBrush);
            }

            OverlayWindow.Graphics.DrawText(baseX, baseY + (j * businessHeight), "Greenlight on Gustav-VRSA", _fontBold, _redBrush);
        }

        private string CalculateBusinessTime(Business business)
        {
            switch (business.Name)
            {
                case "Meth":
                    var total = 5 * 60 * 60;
                    var saleRemaining = total * ((100 - business.Sale) / 100);
                    return SecondsToTime((int)Math.Round(saleRemaining));
            }
            return "";
        }

        public string SecondsToTime(int secs)
        {
            TimeSpan t = TimeSpan.FromSeconds(secs);
            string answer;
            if (t.TotalMinutes < 1.0)
            {
                answer = String.Format("{0}s", t.Seconds);
            }
            else if (t.TotalHours < 1.0)
            {
                answer = String.Format("{0}m:{1:D2}s", t.Minutes, t.Seconds);
            }
            else // more than 1 hour
            {
                answer = String.Format("{0}h:{1:D2}m:{2:D2}s", (int)t.TotalHours, t.Minutes, t.Seconds);
            }

            return answer;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (!OverlayWindow.IsVisible)
            {
                return;
            }

            OverlayWindow.Update();
            InternalRender();
        }

        private void OnPreTick(object sender, EventArgs e)
        {
            var targetWindowIsActivated = TargetWindow.IsActivated;
            if (!targetWindowIsActivated && OverlayWindow.IsVisible)
            {
                _watch.Stop();
                ClearScreen();
                OverlayWindow.Hide();
            }
            else if (targetWindowIsActivated && !OverlayWindow.IsVisible)
            {
                OverlayWindow.Show();
            }
        }

        // ReSharper disable once RedundantOverriddenMember
        public override void Enable()
        {
            _tickEngine.Interval = Settings.Current.UpdateRate.Milliseconds();
            _tickEngine.IsTicking = true;
            base.Enable();
        }

        // ReSharper disable once RedundantOverriddenMember
        public override void Disable()
        {
            _tickEngine.IsTicking = false;
            base.Disable();
        }

        public override void Update() => _tickEngine.Pulse();

        protected void InternalRender()
        {
            if (!_watch.IsRunning)
            {
                _watch.Start();
            }

            OverlayWindow.Graphics.BeginScene();
            OverlayWindow.Graphics.ClearScene();

            _rotation += 0.03f; //related to speed

            if (_rotation > 50.0f) //size of the swastika
            {
                _rotation = -50.0f;
            }

            if (_watch.ElapsedMilliseconds > 1000)
            {
                _fps = _displayFps;
                _displayFps = 0;
                _watch.Restart();
            }
            else
            {
                _displayFps++;
            }

            DrawData();

            OverlayWindow.Graphics.EndScene();
        }

        public override void Dispose()
        {
            OverlayWindow.Dispose();
            base.Dispose();
        }

        private void ClearScreen()
        {
            OverlayWindow.Graphics.BeginScene();
            OverlayWindow.Graphics.ClearScene();
            OverlayWindow.Graphics.EndScene();
        }

        private int RgbBrush(int alpha, int red, int green, int blue)
        {
            return OverlayWindow.Graphics.CreateBrush(Color.FromArgb(alpha, red, green, blue));
        }
    }

    public class OverlaySettings
    {
        public int UpdateRate { get; set; }
    }
}