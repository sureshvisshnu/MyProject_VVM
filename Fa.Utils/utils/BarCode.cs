using fa.api.Log;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace fa.api.utils
{
    public static class BarCode
    {
        public static Bitmap GeneratePatientBarcode(string PatientId)
        {
            Bitmap bitMap = new Bitmap(PatientId.Length *20, 70);
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                Font oFont = new Font("Code-128", 48);
                PointF point = new PointF(2f, 2f);
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                graphics.DrawString(PatientId, oFont, blackBrush, point);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(ms, ImageFormat.Gif);
            }
            return bitMap;
        }        
        public static Image GenerateImageBarcode(string BarcodeText)
        {

            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            try
            {
                string formattedBarcodeText = BarcodeText.Trim().ToUpper();
                Image Image = b.Encode(BarcodeLib.TYPE.CODE128A, formattedBarcodeText, Color.Black, Color.White, 185, 70);
                return Image;
            }
            catch
            {
                try
                {
                    string formattedBarcodeText = BarcodeText.Trim().Remove(BarcodeText.Length - 1, 1).ToUpper();
                    Image Image = b.Encode(BarcodeLib.TYPE.CODE128A, formattedBarcodeText, Color.Black, Color.White, 185, 70);
                    return Image;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    return null!;
                }
            }
        }
        public static Image GenerateImageBarcode1(string BarcodeText)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            string formattedBarcodeText = BarcodeText.ToUpper();
            Image Image = b.Encode(BarcodeLib.TYPE.CODE128A, formattedBarcodeText, Color.Black, Color.White, 185, 40);
            return Image;
        }

        public static Image GenerateImageBarcode2(string BarcodeText)
        {
            if(BarcodeText.Length>17)
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                Image Image = b.Encode(BarcodeLib.TYPE.CODE93, BarcodeText, Color.Black, Color.White, 300, 70);
                return Image;
            }
            else
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                Image Image = b.Encode(BarcodeLib.TYPE.CODE128A, BarcodeText, Color.Black, Color.White, 300, 70);
                return Image;
            }
           
        }
    }
}
