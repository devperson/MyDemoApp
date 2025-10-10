using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.REST
{
    internal class ResponseBase
    {
        public bool Success => string.IsNullOrEmpty(ErrorCode);
        public string ErrorCode { get; set; }
    }
}
