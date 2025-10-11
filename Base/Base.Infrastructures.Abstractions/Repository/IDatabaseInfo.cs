using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructures.Abstractions.Repository
{
    public interface IDatabaseInfo
    {
        string GetDbPath();
    }
}
