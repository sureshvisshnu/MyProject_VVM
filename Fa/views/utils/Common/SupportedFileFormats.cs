using System.Collections.Generic;
using System.ComponentModel;


namespace fa.views.utils
{
    public enum FileExtenstionSupported
    {
        PDF,
        TXT,
        EXCEL
    };

    public class FileExtenstionConverter : TypeConverter
    {
        private static Dictionary<string, FileExtenstionSupported> FileExtMap;

        //static FileExtenstionConverter()
        //{
        //    HahMap<FileExtMap> ExtMap = new Dictionary.PDF| FileExtenstionSupported.TXT
        //        | FileExtenstionSupported.EXCEL;

        //    FileExtMap = ExtMap.GetFields(ExtMap).ToDictionary(
        //        c => GetDisplayName(c)
        //        );
        //}

        //private static string GetDisplayName(FieldInfo field, Type enumType)
        //{
        //    DisplayNameAttribute attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(typeof(DisplayNameAttribute));

        //    return (attr != null) ? attr.DisplayName : field.Name;
        //}

        //public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        //{
        //    string stringValue = value as string;

        //    if (stringValue != null)
        //    {
        //        Operation operation;
        //        if (operationMap.TryGetValue(stringValue, out operation))
        //        {
        //            return operation;
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Cannot convert '" + stringValue + "' to Operation");
        //        }
        //    }
        //}
    }

    public enum SupportedFileFormats
    {
        PDF=1,EXCEL=2,TXT=3
    }

    //public class SupportedFileFormatExtensions
    //{
    //    HashMap ExtensionMap 
    //}

}
