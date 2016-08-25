namespace SimCityBuildItBot.Bot
{
    using Common.Logging;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class TradeWindow
    {
        private CaptureScreen captureScreen;
        private ILog log;
        public bool Debug { get; set; }
        public PictureBox PictureBox { get; set; }

        public bool StopCapture { get; set; }

        public TradeWindow(CaptureScreen captureScreen, ILog log)
        {
            this.captureScreen = captureScreen;
            this.log = log;
        }

        public int X = 287;
        public int Y = 165;
        public Size Size = new Size(1300, 800);

        public List<int> imageTopsGlobalTrade = new List<int>() { 93, 444 };
        public List<int> imageTopsTradeDepot = new List<int>() { 154, 505 };

        public int clickXExtra = 272;
        public List<int> clickY = new List<int>() { 374, 738 };

        public const ulong ResetButtonHash = 17521127647493644;

        public const ulong ConfigButtonHash = 35869570894094686;
        public const ulong TradeDepotLogoHash = 4071112760807423612;

        public Bitmap CaptureTradeWindow(CaptureScreen captureScreen)
        {
            return captureScreen.SnapShot(X, Y, Size);
        }

        public Bitmap CaptureResetButton(CaptureScreen captureScreen)
        {
            return captureScreen.SnapShot( 835, 935, new Size(55, 55));
        }

        public Bitmap CaptureConfigButton(CaptureScreen captureScreen)
        {
            return captureScreen.SnapShot( 60, 275, new Size(90, 90));
        }

        public Bitmap CaptureTradeDepotLogo(CaptureScreen captureScreen)
        {
            return captureScreen.SnapShot(320, 95, new Size(60, 65));
        }

        public Point CalcClickPointGlobalTrade(PanelLocation loc)
        {
            var y = loc.Start.Y >= imageTopsGlobalTrade[1] ? clickY[1] : clickY[0];
            var x = clickXExtra + loc.Start.X + 120;

            return new Point(x, y);
        }

        public Point CalcClickPointTradeDepot(PanelLocation loc)
        {
            var y = loc.Start.Y >= imageTopsTradeDepot[1] ? clickY[1] : clickY[0];
            var x = clickXExtra + loc.Start.X + 120;

            return new Point(x, y);
        }

        public Func<bool> IsResetButtonVisible
        {
            get
            {
                return () =>
                {
                    Func<Bitmap> getBitmap = () => { return this.CaptureResetButton(captureScreen); };

                    bool isvisible = IsImageVisible(getBitmap, TradeWindow.ResetButtonHash);
                    return isvisible;
                };
            }
        }

        public Func<bool> IsConfigButtonVisible
        {
            get
            {
                return () =>
            {
                Func<Bitmap> getBitmap = () => { return this.CaptureConfigButton(captureScreen); };

                bool isvisible = IsImageVisible(getBitmap, TradeWindow.ConfigButtonHash);
                return isvisible;
            };
            }
        }

        public Func<bool> IsTradeDepotLogoVisible
        {
            get
            {
                return () =>
            {
                Func<Bitmap> getBitmap = () => { return this.CaptureTradeDepotLogo(captureScreen); };

                bool isvisible = IsImageVisible(getBitmap, TradeWindow.TradeDepotLogoHash);
                return isvisible;
            };
            }
        }

        private bool IsImageVisible(Func<Bitmap> getImage, ulong desiredHash)
        {
            var image = getImage();

            if (this.PictureBox != null)
            {
                ImageViewer.ShowBitmap(image, this.PictureBox);
            }

            ulong hash = ImageHashing.ImageHashing.AverageHash(image);

            var isResetButtonVisible = ImageHashing.ImageHashing.Similarity(desiredHash, hash) > 90;
            if (!isResetButtonVisible)
            {
                if (Debug)
                {
                    log.Info("hash incorrect: " + hash);
                }
            }

            return isResetButtonVisible || this.StopCapture;
        }
    }
}