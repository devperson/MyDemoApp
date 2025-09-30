using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.AppService.Dto
{
    public class Some<T>
    {
        private readonly T? value;
        

        public Some() { }
        public Some(Exception ex)
        {
            Exception = ex;
        }
        public Some(T value) 
        {
            this.value = value;
        }

        public T Value
        {
            get
            {
                if(value != null)
                    return value;
                throw new InvalidOperationException("value is null");
            }
        }

        public bool Success
        {
            get { return value != null; }
        }

        public Exception? Exception { get; set; }
    


    }
}
