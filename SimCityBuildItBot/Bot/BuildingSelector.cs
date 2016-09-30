namespace SimCityBuildItBot.Bot
{
    using Common.Logging;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Tesseract;

    public class BuildingSelector
    {
        private readonly ILog log;
        private readonly Touch touch;

        public BuildingSelector(ILog log, Touch touch)
        {
            this.log = log;
            this.touch = touch;
            screen = new CaptureScreen(log);
        }

        private CaptureScreen screen;
        private List<BuildingMatch> buildingMatches = BuildingMatch.Create();

        public BuildingMatch SelectABuilding(string suffixMessage)
        {
            var maxCnt = 10;
            BuildingMatch building = null;

            for (int cnt = 0; cnt < maxCnt; cnt++)
            {
                // is building selected
                building = GetBuilding();
                if (building != null)
                {
                    this.log.Info("At [" + building.Building.ToString()+"]"+ suffixMessage);
                    //this.log.Info("Exit");
                    break;
                }

                this.log.Info("No building selected, cnt = " + cnt);

                touch.ClickAt(Location.CentreMap,100);
            }

            if (building == null)
            {
                this.log.Info("Max count reached.");
                this.log.Info("Failed!");
            }

            return building;
        }

        public Bitmap SnapShotBuildingTitle()
        {
            return screen.SnapShot( 660, 120, new Size(550, 70));
        }

        public Bitmap SnapShotTradeDepotTitle()
        {
            return screen.SnapShot( 410, 90, new Size(400, 70));
        }

        public BuildingMatch GetBuilding()
        {
            var bitmap = SnapShotBuildingTitle();

            if (bitmap == null)
            {
                return null;
            }

            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                using (var pix = new BitmapToPixConverter().Convert(bitmap))
                {
                    using (var page = engine.Process(pix, PageSegMode.SingleLine))
                    {
                        return ToBuildingName(page.GetText().Trim().ToLower());
                    }
                }
            }
        }

        private BuildingMatch ToBuildingName(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
            var matches = buildingMatches.Where(b => b.Fragments.Exists(fr => text.Contains(fr))).ToList();

            return matches.OrderByDescending(m => m.Fragments.Where(fr => text.Contains(fr)).Count())
                .FirstOrDefault();
        }
    }
}