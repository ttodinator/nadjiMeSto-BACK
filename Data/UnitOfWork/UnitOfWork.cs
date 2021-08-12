using AutoMapper;
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
        private IMapper mapper;

        public UnitOfWork(Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IRepositoryRestaurant RepositoryRestaurant => new RepositoryRestaurant(context);

        public IRepositoryUser RepositoryUser => new RepositoryUser(context,mapper);
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
