using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.Utils.utils
{
    public class FileUtils
    {
        //public static string GetWriteFileName(string FileNames, string Title, string ProposedFileName)
        //{
        //    SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
        //    SaveFileDialog1.Filter = FileNames;
        //    SaveFileDialog1.Title = Title;
        //    SaveFileDialog1.FileName = ProposedFileName;
        //    if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        if (!string.IsNullOrEmpty(SaveFileDialog1.FileName))
        //        {
        //            return SaveFileDialog1.FileName;
        //        }
        //    }
        //    return null;
        //}

        //public static bool DeleteFileIfExist(string FileName)
        //{
        //    try
        //    {
        //        if (File.Exists(FileName))
        //        {
        //            File.Delete(FileName);
        //            return true;
        //        }
        //    }
        //    catch (IOException ioExp)
        //    {
        //        Console.WriteLine(ioExp.Message);
        //    }
        //    return false;
        //}

        //public static string GetReadFileName(string FileNames, string Title, string ProposedFileName)
        //{
        //    OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
        //    OpenFileDialog1.Filter = FileNames;
        //    OpenFileDialog1.Title = Title;
        //    if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        if (!string.IsNullOrEmpty(OpenFileDialog1.FileName))
        //        {
        //            return OpenFileDialog1.FileName;
        //        }
        //    }
        //    return null;
        //}
    }
}
