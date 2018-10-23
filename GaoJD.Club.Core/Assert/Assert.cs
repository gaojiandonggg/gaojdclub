using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core
{
    public static class Assert
    {
        public static void NotNull<T>(T Value, string argumentName)
        {
            if (Value == null || (Value is string && string.IsNullOrEmpty(Value as string)))
            {
                throw new ArgumentNullException(argumentName + " not null");
            }
        }
    }
}
