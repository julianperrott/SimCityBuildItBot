namespace SimCityBuildItBot.Bot
{
    using Common.Logging;
    using System.Diagnostics;
    using System.Windows.Forms;

    public class BotApplication
    {
        public static ILog log;

        public static void Wait(int millisecond)
        {
            if (log != null && millisecond>1000)
            {
                log.Debug("Sleeping for " + (int)(millisecond / 1000) + " seconds");
            }

            var sw = new Stopwatch();
            sw.Start();

            while (sw.ElapsedMilliseconds < millisecond)
            {
                log.Trace("Sleep remaining = " + (int)( (millisecond - sw.ElapsedMilliseconds) / 1000) + " seconds");
                Application.DoEvents();
                System.Threading.Thread.Sleep(50);
            }
        }
    }
}