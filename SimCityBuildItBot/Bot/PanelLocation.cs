namespace SimCityBuildItBot.Bot
{
    using System.Drawing;

    public class PanelLocation
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public Point Start { get; set; }

        public Point ImageGlobalTradePoint
        {
            get
            {
                return new Point(Start.X + 75, Start.Y + 120);
            }
        }

        public Size ImageGlobalTradeSize
        {
            get
            {
                return new Size(60, 80);
            }
        }

        public Point ImageTradeDepotPoint
        {
            get
            {
                return new Point(Start.X + 75, Start.Y + 60);
            }
        }

        public Size ImageTradeDepotSize
        {
            get
            {
                return new Size(60, 80);
            }
        }

        public int Width { get; internal set; }

        public Bitmap CroppedImage { get; set; }

        public string Item { get; set; }

        public string ItemText
        {
            get
            {
                if (string.IsNullOrEmpty(this.Item))
                {
                    return string.Empty;
                }

                if (!this.Item.Contains(@"\"))
                {
                    return this.Item;
                }

                return this.Item.Substring(this.Item.LastIndexOf(@"\") + 1);
            }
        }
    }
}