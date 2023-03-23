global using Xunit;
global using MediatR;
global using Moq;

global using System;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Collections.Generic;

global using Microsoft.Extensions.Logging;
global using Microsoft.EntityFrameworkCore;

global using Ordering.Domain.AggregatesModel.OrderAggregate;

global using Ordering.Infrastructure;
global using Ordering.Infrastructure.Repositories;
global using Ordering.Infrastructure.Idempotency;

global using Ordering.API.Controllers;
global using Ordering.API.Application.Commands;
global using Ordering.API.Application.Commands.Dtos;

global using Ordering.UnitTests.API.Mocks;