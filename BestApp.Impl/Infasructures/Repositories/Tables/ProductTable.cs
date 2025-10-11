using Base.Abstractions.Repository;
using SQLite;

namespace BestApp.Impl.Cross.Infasructures.Repositories.Tables
{
    public class ProductTb : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool Active { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
