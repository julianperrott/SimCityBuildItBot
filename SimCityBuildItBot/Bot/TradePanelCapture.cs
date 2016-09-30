namespace SimCityBuildItBot.Bot
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TradePanelCapture
    {
        TradeWindow tradeWindow;

        public TradePanelCapture (TradeWindow tradeWindow)
        {
            this.tradeWindow = tradeWindow;
        }

        private Tuple<byte, byte> redRangeGlobalTrade = new Tuple<byte, byte>(60, 90);
        private Tuple<byte, byte> greenRangeGlobalTrade = new Tuple<byte, byte>(175, 201);
        private Tuple<byte, byte> blueRangeGlobalTrade = new Tuple<byte, byte>(200, 230);

        private Tuple<byte, byte> redRangeTradeDepot = new Tuple<byte, byte>(245, 255);
        private Tuple<byte, byte> greenRangeTradeDepot = new Tuple<byte, byte>(218, 240);
        private Tuple<byte, byte> blueRangeTradeDepot = new Tuple<byte, byte>(170, 210);

        private static Pen redPen = new Pen(Color.Red, 2);
        private Pen greenPen = new Pen(Color.Green, 2);

        public List<PanelLocation> FindPanelStartsTradeDepot(Bitmap image, List<int> tops)
        {
            return FindPanelStarts(image, tops, redRangeTradeDepot, greenRangeTradeDepot, blueRangeTradeDepot);
        }

        public List<PanelLocation> FindPanelStartsGlobalTrade(Bitmap image, List<int> tops)
        {
            return FindPanelStarts(image, tops, redRangeGlobalTrade, greenRangeGlobalTrade, blueRangeGlobalTrade);
        }

        private List<PanelLocation> FindPanelStarts(Bitmap image, List<int> tops, Tuple<byte, byte> redRange, Tuple<byte, byte> greenRange, Tuple<byte, byte> blueRange)
        {
            var result = new List<PanelLocation>();
            int lastFoundX = 0;

            tops.ForEach(y =>
            {
                PanelLocation current = null;

                for (int x = 1; x < image.Width; x++)
                {
                    var p = image.GetPixel(x, y);
                    if (InRange(p.R, redRange) && InRange(p.G, greenRange) && InRange(p.B, blueRange))
                    {
                        lastFoundX = x;

                        if (current == null)
                        {
                            current = new PanelLocation { Start = new Point(x, y) };
                        }
                    }
                    else
                    {
                        if (current != null)
                        {
                            int width = x - current.Start.X;
                            if (width > 180)
                            {
                                current.Width = width;
                                result.Add(current);
                                lastFoundX = 0;
                                current = null;
                            }
                            else
                            {
                                if (x - lastFoundX > 5)
                                {
                                    current = null;
                                }
                            }
                        }
                    }
                }
            });

            return result;
        }

        public bool InRange(byte value, Tuple<byte, byte> range)
        {
            return value >= range.Item1 && value <= range.Item2;
        }

        public List<int> FindTopsOfPanelGlobalTrade(Bitmap image)
        {
            return FindTopsOfPanel(image, redRangeGlobalTrade, greenRangeGlobalTrade, blueRangeGlobalTrade);
        }

        public List<int> FindTopsOfPanelTradeDepot(Bitmap image)
        {
            return FindTopsOfPanel(image, redRangeTradeDepot, greenRangeTradeDepot, blueRangeTradeDepot);
        }

        public List<int> FindTopsOfPanel(Bitmap image, Tuple<byte, byte> redRange, Tuple<byte, byte> greenRange, Tuple<byte, byte> blueRange)
        {
            int x1 = 160;
            var top = -1;
            var top2 = -1;

            using (var graphics = Graphics.FromImage(image))
            {
                for (int y = 50; y < 600; y++)
                {
                    var p = image.GetPixel(x1, y);
                    var pen = greenPen;
                    if (InRange(p.R, redRange) && InRange(p.G, greenRange) && InRange(p.B, blueRange))
                    {
                        if (top == -1)
                        {
                            top = y;
                        }
                        else
                        {
                            if (top2 == -1 && y > top + 330)
                            {
                                top2 = y;
                            }
                        }

                        pen = redPen;
                    }

                    graphics.DrawLine(pen, 20, y, 30, y);
                }
            }

            return new List<int> { top, top2 };
        }

        private static Bitmap cropImage(Bitmap bmpImage, Rectangle cropArea)
        {
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public List<PanelLocation> CaptureImagesGlobalTrade()
        {
            var image = this.tradeWindow.CaptureTradeWindow();

            List<int> tops = this.tradeWindow.imageTopsGlobalTrade;

            var locs = FindPanelStartsGlobalTrade(image, tops);

            locs.ForEach(p =>
            {
                var crop = cropImage(image, new Rectangle(p.ImageGlobalTradePoint, p.ImageGlobalTradeSize));
                p.CroppedImage = crop;
            });

            return locs;
        }

        public List<PanelLocation> CaptureCreateSaleItems()
        {
            var image = this.tradeWindow.CaptureForSaleInventory();

            var locs = new List<PanelLocation>();

            int startX = 90;
            int startY = 60;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    try
                    {
                        var p = new PanelLocation { Start = new Point(startX + col * 270, startY + row * 172), Row = row, Column = col };
                        p.CroppedImage = cropImage(image, new Rectangle(p.Start, p.ImageGlobalTradeSize));
                        locs.Add(p);
                    }
                    catch
                    {

                    }
                }
            }

            return locs;
        }

        public List<PanelLocation> CaptureImagesTradeDepot()
        {
            var image = this.tradeWindow.CaptureTradeWindow();

            List<int> tops = this.tradeWindow.imageTopsTradeDepot;

            var locs = FindPanelStartsTradeDepot(image, tops);

            locs.ForEach(p =>
            {
                p.CroppedImage = cropImage(image, new Rectangle(p.ImageTradeDepotPoint, p.ImageTradeDepotSize));
            });

            return locs;
        }

    }
}
