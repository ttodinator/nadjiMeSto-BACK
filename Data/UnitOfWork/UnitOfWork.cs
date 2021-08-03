using Data.Definition;
using Data.Implementation;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private Context context;

        public UnitOfWork(Context context)
        {
            this.context = context;

        }

        public IRepositoryRestaurant RepositoryRestaurant => new RepositoryRestaurant(context);

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;

        }

        public bool HasChanged()
        {
            throw new NotImplementedException();
        }
    }
}
