﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
		readonly IDiscountRepository discountRepository;
		readonly ILogger<DiscountService> logger;
		readonly IMapper mapper;

		public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
		{
			this.discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var coupon = await discountRepository.GetDiscount(request.ProductName);
			if (coupon == null)
				throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

			logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

			var couponModel = mapper.Map<CouponModel>(coupon);
			return couponModel;
		}
   }
}
