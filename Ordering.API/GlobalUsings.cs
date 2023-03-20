global using Autofac;
global using Autofac.Extensions.DependencyInjection;

global using Dapper;

global using MediatR;
global using MediatR.Extensions.Autofac.DependencyInjection;

global using FluentValidation;

global using Serilog;
global using Serilog.Context;

global using Polly;
global using Polly.Retry;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.EntityFrameworkCore;

global using HealthChecks.UI.Client;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Diagnostics.HealthChecks;

global using System.Reflection;
global using System.Data;
global using System.Data.SqlClient;
global using System.Runtime.Serialization;

global using Ordering.Domain.Exceptions;
global using Ordering.Domain.AggregatesModel.OrderAggregate;
global using Ordering.Domain.AggregatesModel.ProviderAggregate;

global using Ordering.Infrastructure;
global using Ordering.Infrastructure.Repositories;
global using Ordering.Infrastructure.Idempotency;

global using Ordering.API;
global using Ordering.API.Application.Commands;
global using Ordering.API.Application.Commands.Dtos;
global using Ordering.API.Application.Queries;
global using Ordering.API.Application.Behaviors;
global using Ordering.API.Application.DomainServices;
global using Ordering.API.Application.Validators;
global using Ordering.API.Application.Queries.Records;

global using Ordering.API.Infrastructure;
global using Ordering.API.Infrastructure.DataServices;
global using Ordering.API.Infrastructure.Filters;
global using Ordering.API.Infrastructure.Modules;