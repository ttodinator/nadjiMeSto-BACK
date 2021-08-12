using Data.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRepositoryRestaurant RepositoryRestaurant { get;  }
        public IRepositoryUser RepositoryUser { get; }
        Task<bool> Complete();
        bool HasChanged();
    }
}
