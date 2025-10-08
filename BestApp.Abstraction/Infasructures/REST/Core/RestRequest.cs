using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Infasructures.REST
{
    public class RestRequest
    {
        
        public string ApiEndpoint { get; set; }
        public Priority Priority { get; set; } = Priority.NORMAL;        
        public TimeoutType TimeoutType { get; set; } = TimeoutType.Small;
        public bool CancelSameRequest { get; set; }
        public bool WithBearer { get; set; } = true;
        public object Body { get; set; }
        public int RetryCount { get; set; } = 0;
        public Dictionary<string, string> HeaderValues { get; set; }
    }
}
