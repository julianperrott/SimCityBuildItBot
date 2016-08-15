namespace SimCityBuildItBot.Bot
{
    using System;
    using System.Drawing;

    public static class Constants
    {
        public static Point GetPoint(Location location)
        {
            switch (location)
            {
                case Location.CentreMap: return new Point(960, 500);
                case Location.RightButton: return new Point(1400, 130);
                case Location.LeftButton: return new Point(515, 131);
                case Location.BelowCentreMap: return new Point(945, 657);

                case Location.ButtonLeftInner1: return new Point(630, 320);
                case Location.ButtonLeftInner2: return new Point(520, 500);
                case Location.ButtonLeftInner3: return new Point(630, 680);

                case Location.ButtonLeftOuter1: return new Point(410, 320);
                case Location.ButtonLeftOuter2: return new Point(300, 500);
                case Location.ButtonLeftOuter3: return new Point(410, 680);

                case Location.ButtonRightInner1: return new Point(1280, 320);
                case Location.ButtonRightInner2: return new Point(1390, 500);
                case Location.ButtonRightInner3: return new Point(1280, 680);

                case Location.ProductionQueue: return new Point(525, 972);
                case Location.FactoryQueuePositionStart: return new Point(456, 944);
                case Location.FactoryQueuePositionend: return new Point(1471, 932);

                case Location.Resources2_Position1: return new Point(180, 420);
                case Location.Resources2_Position2: return new Point(350, 420);

                case Location.Resources3_Position1: return new Point(100, 420);
                case Location.Resources3_Position2: return new Point(260, 420);
                case Location.Resources3_Position3: return new Point(430, 420);

                case Location.Resources1_Position1: return new Point(270, 420);
            }

            throw new Exception("Unknown location!");
        }

        public static Point GetOffset(Location location)
        {
            var yOffset = 20;
            switch (location)
            {
                case Location.ButtonLeftInner1: return new Point(15, 0 + yOffset);
                case Location.ButtonLeftInner2: return new Point(615, 0 + yOffset);
                case Location.ButtonLeftInner3: return new Point(340, 80 + yOffset);

                case Location.ButtonRightInner1: return new Point(1250, 0 + yOffset);
                case Location.ButtonRightInner2: return new Point(670, 0 + yOffset);
                case Location.ButtonRightInner3: return new Point(935, 85 + yOffset);
            }
            throw new Exception("Unknown offset!");
        }
    }

    public enum Location
    {
        CentreMap,
        RightButton,
        LeftButton,
        BelowCentreMap,
        ButtonLeftInner1,
        ButtonLeftInner2,
        ButtonLeftInner3,
        ButtonLeftOuter1,
        ButtonLeftOuter2,
        ButtonLeftOuter3,

        ButtonRightInner1,
        ButtonRightInner2,
        ButtonRightInner3,

        ProductionQueue,
        FactoryQueuePositionStart,
        FactoryQueuePositionend,

        Resources2_Position1,
        Resources2_Position2,

        Resources3_Position1,
        Resources3_Position2,
        Resources3_Position3,

        Resources1_Position1,
    }
}