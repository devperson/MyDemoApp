using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Infasructures.REST
{
    public enum RestMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public enum Priority
    {
        HIGH = 0,
        NORMAL = 1,
        LOW = 2,
        NONE = 4,
    }

    public enum TimeoutType
    {
        Small = 10,
        Medium = 30,
        High = 60,
        VeryHigh = 120
    }
}
