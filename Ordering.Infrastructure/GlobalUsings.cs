global using System.Data;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

global using Ordering.Domain.SeedWork;
global using Ordering.Domain.AggregatesModel.OrderAggregate;
global using Ordering.Domain.AggregatesModel.ProviderAggregate;
global using Ordering.Domain.Exceptions;

global using Ordering.Infrastructure.EntityConfigurations;
global using Ordering.Infrastructure.Idempotency;