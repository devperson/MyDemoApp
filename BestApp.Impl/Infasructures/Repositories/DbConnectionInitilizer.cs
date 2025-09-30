using BestApp.Abstraction.General.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using Common.Abstrtactions;
using DryIoc;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    public class DbConnectionInitilizer
    {
        private readonly IDirectoryService directoryService;

        public DbConnectionInitilizer(IDirectoryService directoryService)
        {
            this.directoryService = directoryService;
        }
      

        public async Task RegisterDbConnection(IContainer container)
        {
            var path = directoryService.GetDbPath();
            var database = new SQLiteAsyncConnection(path);
            await database.CreateTableAsync<ProductTable>();

            container.RegisterInstance(database);
        }
    }
}
