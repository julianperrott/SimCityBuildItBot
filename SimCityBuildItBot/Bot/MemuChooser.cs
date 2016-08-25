namespace SimCityBuildItBot.Bot
{
    using System.Linq;
    using Managed.Adb;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Forms;
    using Common.Logging.Simple;
    using System;

    public partial class MemuChooser : Form
    {
        private static Device device;
        private static Process process;

        private List<Device> devices;
        private Process[] processes;

        public MemuChooser()
        {
            InitializeComponent();
            devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);
            processes = Process.GetProcessesByName("MEmu");

            RefreshMemuDevices();
        }

        private void RefreshMemuDevices()
        {
            this.listBoxDevices.Items.Clear();
            this.listBoxProcess.Items.Clear();

            devices.ForEach(d => this.listBoxDevices.Items.Add(d.DeviceProperty));
            processes.ToList().ForEach(p =>
            {
                var rect = new CaptureScreen.Rect();
                var error = CaptureScreen.GetWindowRect(p.MainWindowHandle, ref rect);

                // adb shell am display-size 1920x1080
                long height = rect.bottom - rect.top;
                long width = rect.right - rect.left;

                this.listBoxProcess.Items.Add(p.MainWindowTitle + "   " + width + "x" + height);
            }
            );
        }

        public static Device GetDevice()
        {
            if (device != null)
            {
                return device;
            }

            List<Device> devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);

            if (devices.Count == 1)
            {
                device = devices[0];
                return device;
            }

            new MemuChooser().ShowDialog();

            return device;
        }

        public static Process GetMemuProcess()
        {
            if (process != null)
            {
                return process;
            }

            var processes = Process.GetProcessesByName("MEmu");

            if (processes.Length == 1)
            {
                process = processes[0];
                return process;
            }

            new MemuChooser().ShowDialog();

            return process;
        }

        private void listBoxDevices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            device = this.devices[this.listBoxDevices.SelectedIndex];
        }

        private void listBoxProcess_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            process = this.processes[this.listBoxProcess.SelectedIndex];
        }

        private void btnTouch_Click(object sender, System.EventArgs e)
        {
            new Touch(new NoOpLogger()).ClickAt(Bot.Location.CentreMap);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshMemuDevices();
        }
    }
}