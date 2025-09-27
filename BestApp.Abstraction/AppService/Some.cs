using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.AppService
{
    public class Some<T>
    {
        private readonly T? value;
        

        public Some() { }
        public Some(Exception ex)
        {
            this.Exception = ex;
        }
        public Some(T value) 
        {
            this.value = value;
        }

        public T Value
        {
            get
            {
                if(this.value != null)
                    return this.value;
                throw new InvalidOperationException("value is null");
            }
        }

        public bool Success
        {
            get { return this.value != null; }
        }

        public Exception? Exception { get; set; }
    


    }
}
