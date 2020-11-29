using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Model;
using System.Collections.Generic;

namespace API_DataAccess.DataAccess.Contracts
{
    public interface IRoleData : IRepository<Role>
    {
        List<Role> GetAll_exclude_deleted();
    }
}
