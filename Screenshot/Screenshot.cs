using System.Drawing;
using System.Windows.Forms;

namespace Screenshot
{
    class Screenshot
    {
        #region Public Fields
        public static bool SaveToClipboard = false;
        #endregion Public Fields

        #region Public Methods

        public static void CaptureImage(
            Point sourcePoint, Rectangle selectionRectangle)
        {
            using (Bitmap bitmap = new Bitmap(selectionRectangle.Width,
                selectionRectangle.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(sourcePoint, Point.Empty, 
                        selectionRectangle.Size);
                }
                Image img = (Image)bitmap;
                Clipboard.SetImage(img);
            }
        }

        #endregion Public Methods
    }
}