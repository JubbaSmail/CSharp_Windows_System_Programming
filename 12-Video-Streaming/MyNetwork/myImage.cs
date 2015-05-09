using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace MyNetwork
{
    public class IWebCam
    {
        Bitmap img;

        /// <summary>
        /// Turn on WebCam
        /// </summary>
        /// <param name="handle"></param>
        public IWebCam(IntPtr handle)
        {
            Clipboard.Clear();
            if (img == null)
                Start(10, handle);
            img = (Bitmap)m_Capture();
            if (img == null)
                MessageBox.Show("The Program can't detect a Webcam device.");
        }

        public void iTurn_off_WebCam()
        {
            Stop();
        }

        ~IWebCam()
        {
            Stop();
        }

        public Image iWebCam_Image
        {
            get { return m_Capture(); }
        }

        // property variables
        int m_TimeToCapture_milliseconds = 100;
        int m_Width = 720;
        int m_Height = 576;
        int mCapHwnd;
        ulong m_FrameNumber = 10;
        //----------------------------------------
        System.Windows.Forms.IDataObject tempObj;
        Image tempImg;
        List<byte[,]> g_bckGrnd_bytes = new List<byte[,]>(20);
        //--------------------------------------------------
        #region API Declarations

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        public static extern int capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, int hwndParent, int nID);

        [DllImport("user32", EntryPoint = "OpenClipboard")]
        public static extern int OpenClipboard(int hWnd);

        [DllImport("user32", EntryPoint = "EmptyClipboard")]
        public static extern int EmptyClipboard();

        [DllImport("user32", EntryPoint = "CloseClipboard")]
        public static extern int CloseClipboard();

        #endregion

        #region API Constants

        public static uint WM_USER = 1024;

        public static uint WM_CAP_CONNECT = 1034;
        public static uint WM_CAP_DISCONNECT = 1035;
        public static uint WM_CAP_GET_FRAME = 1084;
        public static uint WM_CAP_COPY = 1054;

        public static uint WM_CAP_START = WM_USER;

        public static uint WM_CAP_DLG_VIDEOFORMAT = WM_CAP_START + 41;
        public static uint WM_CAP_DLG_VIDEOSOURCE = WM_CAP_START + 42;
        public static uint WM_CAP_DLG_VIDEODISPLAY = WM_CAP_START + 43;
        public static uint WM_CAP_GET_VIDEOFORMAT = WM_CAP_START + 44;
        public static uint WM_CAP_SET_VIDEOFORMAT = WM_CAP_START + 45;
        public static uint WM_CAP_DLG_VIDEOCOMPRESSION = WM_CAP_START + 46;
        public static uint WM_CAP_SET_PREVIEW = WM_CAP_START + 50;

        #endregion

        private int TimeToCapture_milliseconds
        {
            get
            { return m_TimeToCapture_milliseconds; }

            set
            { m_TimeToCapture_milliseconds = value; }
        }

        private int CaptureHeight
        {
            get
            { return m_Height; }

            set
            { m_Height = value; }
        }

        private int CaptureWidth
        {
            get
            { return m_Width; }

            set
            { m_Width = value; }
        }

        private ulong FrameNumber
        {
            get
            { return m_FrameNumber; }

            set
            { m_FrameNumber = value; }
        }

        //-------------------------------------------------
        private bool Start(ulong FrameNum, IntPtr handle)
        {
            try
            {
                // for safety, call stop, just in case we are already running
                Stop();
                Clipboard.Clear();
                // setup a capture window
                mCapHwnd = capCreateCaptureWindowA("WebCap", 0, 0, 0, m_Width, m_Height, handle.ToInt32(), 0);
                // connect to the capture device

                SendMessage(mCapHwnd, WM_CAP_CONNECT, 0, 0);
                SendMessage(mCapHwnd, WM_CAP_SET_PREVIEW, 0, 0);
                SendMessage(mCapHwnd, WM_CAP_DLG_VIDEOSOURCE, 0, 0);
                // set the frame number
                m_FrameNumber = FrameNum;
                // set the timer information
                return true;
            }
            catch (Exception excep)
            {
                MessageBox.Show
                    ("An error ocurred while starting the video capture. Check that your webcamera is connected properly and turned on.\r\n\n"
                    + excep.Message);
                Stop();
                return false;
            }
        }

        private void Stop()
        {
            SendMessage(mCapHwnd, WM_CAP_DISCONNECT, 0, 0);
        }
        //--------------------------------------------------
        private Image m_Capture()
        {
            try
            {
                Clipboard.Clear();
                // get the next frame;
                SendMessage(mCapHwnd, WM_CAP_GET_FRAME, 0, 0);

                // copy the frame to the clipboard
                SendMessage(mCapHwnd, WM_CAP_COPY, 0, 0);

                // paste the frame into the event args image

                // get from the clipboard
                tempObj = Clipboard.GetDataObject();
                tempImg = (System.Drawing.Bitmap)tempObj.GetData(System.Windows.Forms.DataFormats.Bitmap);
                //int t1 = DateTime.Now.Millisecond;
                GC.Collect();
                //int t2 = DateTime.Now.Millisecond;
                //int t = t2 - t1;
                /*
                * For some reason, the API is not resizing the video
                * feed to the width and height provided when the video
                * feed was started, so we must resize the image here
                */
                //x.WebCamImage = tempImg.GetThumbnailImage(m_Width, m_Height, null, System.IntPtr.Zero);
            }
            catch (Exception excep)
            {
                MessageBox.Show("An error ocurred while capturing the video image. The video capture will now be terminated.\r\n\n" + excep.Message);
                Stop(); // stop the process
            }
            return tempImg;
        }
    }
    public struct IPixel
    {
        public byte R, G, B;
        public IPixel(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }
    }

    public class IImage
    {
        Bitmap img;
        public IPixel[,] Pixels;
        public int Width;
        public int Height;

        public IImage(string file_path)
        {
            img = (Bitmap)Image.FromFile(file_path);
            Width = img.Width;
            Height = img.Height;
            m_BmpTo2D();
        }

        public IImage(Image normal_image)
        {
            img = (Bitmap)normal_image;
            Width = img.Width;
            Height = img.Height;
            m_BmpTo2D();
        }
        public IImage(int width, int height)
        {
            Width = width;
            Height = height;
            img = new Bitmap(Width, Height);
            m_BmpTo2D();
        }

        public static Image ImageFromStream(byte[] Bytes)
        {
            byte[] w = new byte[4];
            byte[] h = new byte[4];
            Array.Copy(Bytes, 0, w, 0, 4);
            Array.Copy(Bytes, 4, h, 0, 4);
            int Width = BitConverter.ToInt32(w, 0);
            int Height = BitConverter.ToInt32(h, 0);
            Bitmap bmp = new Bitmap(Width, Height);

            BitmapData bData = bmp.LockBits(new Rectangle(new Point(), bmp.Size),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(Bytes, 8, bData.Scan0, Bytes.Length - 8);
            bmp.UnlockBits(bData);
            return bmp;
        }

        public static byte[] StreamFromImage(Image image)
        {
            Bitmap img = (Bitmap)image;
            if (img.Width % 4 == 0)
                img = (Bitmap)m_CropImage(img, new Rectangle(0, 0, img.Width - (img.Width % 4), img.Height));
            //B,G,R
            BitmapData bData = img.LockBits(new Rectangle(new Point(), img.Size),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            // number of bytes in the bitmap
            int byteCount = bData.Stride * img.Height;
            byte[] bmpBytes = new byte[byteCount + 8];
            Array.Copy(BitConverter.GetBytes(img.Width), 0, bmpBytes, 0, 4);
            Array.Copy(BitConverter.GetBytes(img.Height), 0, bmpBytes, 4, 4);
            // Copy the locked bytes from memory
            Marshal.Copy(bData.Scan0, bmpBytes, 8, byteCount);
            // don't forget to unlock the bitmap!!
            img.UnlockBits(bData);
            return bmpBytes;
        }

        public Image Bitmap
        {
            get { return m_2DToBmp(); }
        }

        unsafe void m_BmpTo2D()
        {
            if (img.Width % 4 == 0)
                img = (Bitmap)m_CropImage(img, new Rectangle(0, 0, img.Width - (img.Width % 4), img.Height));
            //B,G,R
            BitmapData bData = img.LockBits(new Rectangle(new Point(), img.Size),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            // number of bytes in the bitmap
            int byteCount = bData.Stride * img.Height;
            byte[] bmpBytes = new byte[byteCount];

            // Copy the locked bytes from memory
            Marshal.Copy(bData.Scan0, bmpBytes, 0, byteCount);
            // don't forget to unlock the bitmap!!
            img.UnlockBits(bData);

            int index = 0;
            Pixels = new IPixel[img.Width, img.Height];

            for (int j = 0; j < img.Height; j++)
            {
                for (int i = 0; i < img.Width; i++)
                {
                    Pixels[i, j].R = bmpBytes[index++];
                    Pixels[i, j].G = bmpBytes[index++];
                    Pixels[i, j].B = bmpBytes[index++];
                }
            }
        }

        Image m_2DToBmp()
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            BitmapData bData = bmp.LockBits(new Rectangle(new Point(), bmp.Size),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            // Copy the bytes to the bitmap object
            byte[] bmpBytes = new byte[img.Width * img.Height * 3];
            int index = 0;
            for (int j = 0; j < img.Height; j++)
            {
                for (int i = 0; i < img.Width; i++)
                {
                    bmpBytes[index++] = Pixels[i, j].R;
                    bmpBytes[index++] = Pixels[i, j].G;
                    bmpBytes[index++] = Pixels[i, j].B;
                }
            }
            Marshal.Copy(bmpBytes, 0, bData.Scan0, bmpBytes.Length);
            bmp.UnlockBits(bData);
            return bmp;
        }

        private static Image m_CropImage(Image img, Rectangle cropArea)
        {
            if (cropArea.X < 0)
                cropArea.X = 0;
            if (cropArea.Y < 0)
                cropArea.Y = 0;
            Size new_size = cropArea.Size;
            if (cropArea.X + cropArea.Width > img.Width)
                new_size.Width = cropArea.X + cropArea.Width;
            if (cropArea.Y + cropArea.Height > img.Height)
                new_size.Height = cropArea.Y + cropArea.Height;
            Bitmap new_bitmap = new Bitmap(new_size.Width + 1, new_size.Height + 1);
            Graphics.FromImage(new_bitmap).DrawImage(img, 0, 0);

            if (cropArea.Width != 0 && cropArea.Height != 0)
            {
                return new_bitmap.Clone(cropArea,
                img.PixelFormat);
            }
            return img;
        }

        private Image m_ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            int destWidth = (int)(sourceWidth * nPercentW);
            int destHeight = (int)(sourceHeight * nPercentH);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }
    }
}
