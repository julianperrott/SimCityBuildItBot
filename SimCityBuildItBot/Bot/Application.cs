namespace SimCityBuildItBot.Bot
{
    using System.Diagnostics;
    using System.Windows.Forms;

    public class BotApplication
    {
        public static void Wait(int millisecond)
        {
            var sw = new Stopwatch();
            sw.Start();

            while (sw.ElapsedMilliseconds < millisecond)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(50);
            }
        }
    }
}