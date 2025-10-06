using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.Repositories.Tables
{
    internal class EventsTb : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string CustomValue { get; set; }
        public string Date { get; set; }

        public static EventsTb Create(string name, string action, string description, string customValue = null)
        {
            var eventObj = new EventsTb()
            {
                Name = name,
                Action = action,
                Description = description,
                Date = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss zzz"),
                CustomValue = customValue
            };
            return eventObj;
        }
    }
}
