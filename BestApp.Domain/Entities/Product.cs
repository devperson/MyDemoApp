using Base.Abstractions.Domain;

namespace BestApp.Abstraction.Domain.Entities
{
    public class Product : IEntity
    {
        //private List<Return> returns = new List<Return>();

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool Active { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        //public ProductCode Code { get; protected set; }
        //public ReadOnlyCollection<Return> Returns
        //{
        //    get
        //    {
        //        return returns.AsReadOnly();
        //    }
        //}

        public static Product Create(string name, int quantity, decimal cost)
        {
            return new Product()
            {                
                Name = name,
                Quantity = quantity,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Active = true,
                Cost = cost,
            };
        }
    }
}
