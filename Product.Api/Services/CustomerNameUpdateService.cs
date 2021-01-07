using MediatR;
using OrderApi.Service.v1.Command;
using Product.Api.Models;
using Product.Api.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Api.Services
{
    public class CustomerNameUpdateService : ICustomerNameUpdateService
    {
        private readonly IMediator _mediator;

        public CustomerNameUpdateService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async void UpdateCustomerNameInOrders(AddUser updateCustomerFullNameModel)
        {
            try
            {
                


               dynamic  v = updateCustomerFullNameModel.FirstName;
                

                await _mediator.Send(new UpdateOrderCommand
                {
                    Orders = v
                });
            }
            catch (Exception ex)
            {
                // log an error message here

                Debug.WriteLine(ex.Message);
            }
        }
    }
}
