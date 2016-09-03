namespace SimCityBuildItBot.Bot
{
    using Common.Logging;
    using Managed.Adb;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class Touch
    {
        private ConsoleOutputReceiver creciever = new ConsoleOutputReceiver();
        private Device device;
        private ILog log;

        public Touch(ILog log)
        {
            this.log = log;
            
            device = MemuChooser.GetDevice();
        }

        public void TouchDown()
        {
            device.ExecuteShellCommand("sendevent /dev/input/event7 1 330 1", creciever);
            device.ExecuteShellCommand("sendevent /dev/input/event7 3 58 1", creciever);
        }

        public void MoveTo(Point point)
        {
            device.ExecuteShellCommand("sendevent /dev/input/event7 3 53 " + point.X, creciever);
            device.ExecuteShellCommand("sendevent /dev/input/event7 3 54 " + point.Y, creciever);
            EndTouchData();
        }

        public void TouchUp()
        {
            device.ExecuteShellCommand("sendevent /dev/input/event7 1 330 0", creciever);
            device.ExecuteShellCommand("sendevent /dev/input/event7 3 58 0", creciever);
        }

        public void EndTouchData()
        {
            device.ExecuteShellCommand("sendevent /dev/input/event7 0 2 0", creciever);
            device.ExecuteShellCommand("sendevent /dev/input/event7 0 0 0", creciever);
        }

        public void ClickAt(Location location)
        {
            //log.Info("clicking " + location.ToString());

            var point = Constants.GetPoint(location);

            //this.MoveTo(point);

            System.Diagnostics.Debug.WriteLine("Clicking " + location.ToString());

            ClickAt(point);
        }

        public void LongPress(Location location, int seconds)
        {
            var point = Constants.GetPoint(location);

            this.TouchDown();
            this.MoveTo(point);

            BotApplication.Wait(seconds * 1000);

            this.TouchUp();
            this.EndTouchData();

            BotApplication.Wait(1000);
        }

        public void ClickAt(Point point)
        {
            this.TouchDown();
            this.MoveTo(point);

            this.TouchUp();
            this.EndTouchData();

            BotApplication.Wait(1000);
        }

        public void Swipe(Location downAt, Location from, Location to, int steps, bool touchUpAtTouchDownLocation)
        {
            var pointdownAt = Constants.GetPoint(downAt);
            var pointFrom = Constants.GetPoint(from);
            var pointTo = Constants.GetPoint(to);

            Swipe(pointdownAt, pointFrom, pointTo, steps, touchUpAtTouchDownLocation);
        }

        public void Swipe(Point pointdownAt, Point pointFrom, Point pointTo, int steps, bool touchUpAtTouchDownLocation)
        {
            var xStep = (pointTo.X - pointFrom.X) / steps;
            var yStep = (pointTo.Y - pointFrom.Y) / steps;

            //log.Info("Swiping from" + from.ToString() + " to " + to.ToString());
            TouchDown();
            this.MoveTo(pointdownAt);
            BotApplication.Wait(300);
            this.MoveTo(pointFrom);
            BotApplication.Wait(300);

            for (int i = 0; i < steps; i++)
            {
                pointFrom.X += xStep;
                pointFrom.Y += yStep;
                this.MoveTo(pointFrom);
            }

            if (!touchUpAtTouchDownLocation)
            {
                this.MoveTo(pointdownAt);
            }

            BotApplication.Wait(500);
            TouchUp();
            EndTouchData();
            BotApplication.Wait(500);
        }
    }
}