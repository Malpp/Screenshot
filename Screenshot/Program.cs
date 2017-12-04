using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Screenshot;
using Screenshot.Properties;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

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

    public class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private bool cursorIsActive;
        private Thread screenshoThread;
        public static bool formCreated = false;
        private bool screenshotDone = false;
        private bool isCanceled = false;
        private Form1 form1;
        private Point startPos;
        private Bitmap printscreen;

        private KeyboardHook hook = new KeyboardHook();
        MouseHook mouseHook = new MouseHook();

        public MyCustomApplicationContext()
        {
            hook.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Shift, Keys.C);
            hook.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Shift, Keys.E);
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            mouseHook.LeftButtonDown += MouseHookOnLeftButtonDown;
            mouseHook.RightButtonUp += MouseHookOnRightButtonUp;
            mouseHook.Install();

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

        private void MouseHookOnRightButtonUp(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            if (formCreated)
            {
                screenshotDone = true;
                isCanceled = true;
            }
        }

        private void MouseHookOnLeftButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            if (formCreated)
            {
                screenshotDone = true;
                Console.WriteLine("REEEEEEEe");
                isCanceled = false;
            }
        }

        private void Screenshot(object sender, EventArgs e)
        {
            screenshoThread = new Thread(TakeScreenshot);
            screenshoThread.Start();
            screenshoThread.Join();
            Clipboard.SetImage(printscreen);
            //trayIcon.Visible = false;
            /*
            int totalHeight = 0;
            int totalWidth = 0;
            Screen.AllScreens.ToList().ForEach(screen => totalHeight += screen.Bounds.Height);
            Screen.AllScreens.ToList().ForEach(screen => totalWidth += screen.Bounds.Width);
            Bitmap printscreen = new Bitmap(totalWidth, totalHeight);

            Graphics graphics = Graphics.FromImage(printscreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            Form form = new Form1();
            form.Show();
            form.Focus();
            */
            //save graphic variable into memory
            //printscreen.Save("C:/asd.png", ImageFormat.Png);
        }

        private void TakeScreenshot()
        {
            Console.WriteLine("Hello");
            while (!screenshotDone)
            {
                if (!formCreated)
                {
                    startPos = Cursor.Position;
                    form1 = new Form1 {Location = startPos};
                    form1.Show();
                    formCreated = true;
                }
                else if (formCreated)
                {
                    Point newPos = Cursor.Position;
                    newPos.X -= startPos.X;
                    newPos.Y -= startPos.Y;
                    form1.Size = (Size) newPos;
                }
            }
            Size finalSize = form1.Size;
            Point finalLocation = form1.Location;
            form1.Close();
            if (!isCanceled)
            {
                printscreen = new Bitmap(finalSize.Width, finalSize.Height);

                using (Graphics graphic = Graphics.FromImage(printscreen))
                {
                    graphic.CopyFromScreen(finalLocation.X, finalLocation.Y, 0, 0, finalSize);
                }
            }
            Console.WriteLine("Done");
            screenshotDone = false;
            formCreated = false;
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.Modifier == (int) ModifierKeys.Control + ModifierKeys.Shift && e.Key == Keys.E)
                Exit(sender, e);
            else if (e.Modifier == (int) ModifierKeys.Control + ModifierKeys.Shift && e.Key == Keys.C)
                Screenshot(sender, e);
        }
    }
}