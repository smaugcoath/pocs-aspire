# Pocs.Aspire Solution

## Overview

The Pocs.Aspire solution demonstrates a clean architecture implementation using .NET 8 and .NET Aspire for building distributed applications. It showcases modern development practices, clean architecture principles, and the capabilities of .NET Aspire for service orchestration.

## Architecture

This solution follows clean architecture principles with clear separation of concerns:

- **Domain Layer**: Core business entities and logic, free from external dependencies
- **Business Layer**: Application services implementing use cases and business rules
- **Infrastructure Layer**: External concerns like data persistence and external services
- **API Layer**: REST API exposing functionality to clients
- **AppHost**: Orchestration of services and resources using .NET Aspire

## Key Technologies

- **.NET 8**: Latest .NET runtime with performance improvements
- **.NET Aspire**: Cloud-ready stack for distributed applications
- **Minimal APIs**: Lightweight HTTP endpoints with reduced boilerplate
- **Entity Framework Core**: Object-relational mapping for data access
- **PostgreSQL**: High-performance open-source database
- **Redis**: In-memory data store for caching
- **Docker**: Containerization for consistent deployment

## Projects

- **Pocs.Aspire.Domain**: Core business models and repository interfaces
- **Pocs.Aspire.Business**: Application services and business logic
- **Pocs.Aspire.Infrastructure**: Repository implementations and data access
- **Pocs.Aspire.ApiService**: REST API endpoints exposing functionality
- **Pocs.Aspire.AppHost**: .NET Aspire orchestration of services and resources
- **Pocs.Aspire.ServiceDefaults**: .NET Aspire template for shared service configuration

## Getting Started

### Prerequisites

- .NET 8
- Docker Desktop or Podman


More information [here](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=windows&pivots=visual-studio#install-net-aspire-prerequisites)

### Running the Application

1. Clone the repository
2. Navigate to the solution directory
3. Run 
```shell 
dotnet run --project src/Pocs.Aspire.AppHost
```
4. Access the .NET Aspire dashboard at the provided URL

## Architectural Decisions

- **Clean Architecture**: Separation of concerns with domain at the center
- **Functional Results**: Using [LanguageExt](https://github.com/louthy/language-ext) for better error handling
- **Persistence Ignorance**: Domain layer remains independent of persistence details
- **.NET Aspire**: Modern approach to distributed application development

## Features

- **Resource Management**: Automatic provisioning of infrastructure resources
- **Health Monitoring**: Built-in health checks and dashboard
- **API Versioning**: Support for evolving APIs while maintaining compatibility
- **Output Caching**: Redis-backed response caching for improved performance
- **Structured Logging**: Comprehensive logging across all services
- **Observability**: Built-in with OpenTelemetry support.
