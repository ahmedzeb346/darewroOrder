using Product.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Api.Services
{
   public interface ICustomerNameUpdateService
    {
      public void UpdateCustomerNameInOrders(AddUser updateCustomerFullNameModel);
    }
}
