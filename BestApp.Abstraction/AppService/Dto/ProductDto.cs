using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestApp.Abstraction.Main.AppService.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public int Quantity { get; set; }
        public decimal Cost { get; set; }        
    }
}
