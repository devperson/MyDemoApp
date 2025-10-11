using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Aspect
{
    /// <summary>
    /// Prevents the decorated method from being logged.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExcludeFromLogAttribute : Attribute
    {
    }
}
