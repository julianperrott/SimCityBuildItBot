namespace SimCityBuildItBot.Bot
{
    using Common.Logging;
    using SimplePaletteQuantizer;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class CommerceResourceReader
    {
        private CaptureScreen captureScreen;
        private ILog log;
        private readonly Touch touch;
        private TwoColourPallette palleteReader = new TwoColourPallette();

        public CommerceResourceReader(ILog log, Touch touch)
        {
            this.log = log;
            this.touch = touch;
            captureScreen = new CaptureScreen(log);
        }

        public List<Bot.FactoryResource> GetRequiredResources(Bot.Location clickAt, List<Bot.FactoryResource> resources)
        {
            List<Location> resourceLocations = GetResourceLocations(resources);

            var images = GetResourceImages(clickAt, resourceLocations);

            var requiredResources = new List<Bot.FactoryResource>();

            for (int i = 0; i < resourceLocations.Count; i++)
            {
                if (palleteReader.GetClosest2Colours(images[i]).Contains("Red"))
                {
                    requiredResources.Add(resources[i]);
                }
            }

            return requiredResources;
        }

        private static List<Location> GetResourceLocations(List<FactoryResource> resources)
        {
            List<Bot.Location> resourceLocations = null;
            switch (resources.Count())
            {
                case 1:
                    resourceLocations = new List<Bot.Location>()
                    {
                        Bot.Location.Resources1_Position1,
                    }; ;
                    break;

                case 2:
                    resourceLocations = new List<Bot.Location>()
                    {
                        Bot.Location.Resources2_Position1,
                        Bot.Location.Resources2_Position2,
                    };
                    break;

                default:
                    resourceLocations = new List<Bot.Location>()
                    {
                        Bot.Location.Resources3_Position1,
                        Bot.Location.Resources3_Position2,
                        Bot.Location.Resources3_Position3
                    };
                    break;
            }

            return resourceLocations;
        }

        public List<Bitmap> GetResourceImages(Bot.Location clickAt, List<Bot.Location> resourceLocations)
        {
            var resourceLocationOffset = Constants.GetOffset(clickAt);

            touch.TouchDown();
            touch.MoveTo(Constants.GetPoint(clickAt));

            System.Threading.Thread.Sleep(300);

            var images = resourceLocations.Select(resourceLocation =>
            {
                var capturePoint = Constants.GetPoint(resourceLocation);
                return captureScreen.SnapShot("MEmu", capturePoint.X + resourceLocationOffset.X, capturePoint.Y + resourceLocationOffset.Y, new Size(70, 35));
            }).ToList();

            touch.TouchUp();
            touch.EndTouchData();

            return images;
        }
    }
}