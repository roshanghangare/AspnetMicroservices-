﻿using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviors
{
	public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		readonly ILogger<TRequest> logger;

		public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				var requestName = typeof(TRequest).Name;
				logger.LogError(ex, "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
				throw;
			}
		}
	}
}
