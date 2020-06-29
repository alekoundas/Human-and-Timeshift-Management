using System;
using System.Collections.Generic;
using System.Text;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base.Interface
{
    public interface ICustomerRepository:IBaseRepository<Customer>
    {
    }
}
