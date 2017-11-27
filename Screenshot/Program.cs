using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Screenshot.Properties;

namespace Screenshot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }
    }
}

public class MyCustomApplicationContext : ApplicationContext
{
    private NotifyIcon trayIcon;

    public MyCustomApplicationContext()
    {
        // Initialize Tray Icon
        trayIcon = new NotifyIcon()
        {
            Icon = Resources.AppIcon,
            ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem("Screenshot", Screenshot, Shortcut.CtrlShiftC),
                new MenuItem("Exit", Exit)
            }),
            Visible = true
        };
    }

    private void Screenshot(object sender, EventArgs e)
    {
        //trayIcon.Visible = false;

        int totalHeight = 0;
        int totalWidth = 0;
        Screen.AllScreens.ToList().ForEach(screen => totalHeight += screen.Bounds.Height);
        Screen.AllScreens.ToList().ForEach(screen => totalWidth += screen.Bounds.Width);
        Bitmap printscreen = new Bitmap(totalWidth, totalHeight);

        Graphics graphics = Graphics.FromImage(printscreen as Image);
        graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

        MessageBox.Show(totalWidth + " " + totalHeight);
        //save graphic variable into memory
        //printscreen.Save("C:/asd.png", ImageFormat.Png);
    }

    void Exit(object sender, EventArgs e)
    {
        // Hide tray icon, otherwise it will remain shown until user mouses over it
        trayIcon.Visible = false;

        Application.Exit();
    }
}