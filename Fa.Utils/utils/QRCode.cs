using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.Utils.utils
{
    public static class QRCode
    {
        public static Image GenerateQRCode(string Text)
        {
            Zen.Barcode.CodeQrBarcodeDraw QRCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            return QRCode.Draw(Text,50,25);
        }
    }
}
