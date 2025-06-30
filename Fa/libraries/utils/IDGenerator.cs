using System;

namespace fa.libraries.utils
{
    class IDGenerator
    {
        public static string generateID(string prefix)
        {
            return string.Format("{0}_{1:N}", prefix, Guid.NewGuid());
        }
    }
}
