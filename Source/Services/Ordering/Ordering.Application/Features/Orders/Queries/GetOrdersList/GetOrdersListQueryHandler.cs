﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
	public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
	{
		readonly IOrderRepository orderRepository;
		readonly IMapper mapper;

		public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
		{
			this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
		{
			var orderList = await orderRepository.GetOrdersByUserName(request.UserName);
			return mapper.Map<List<OrdersVm>>(orderList);
		}
	}
}
