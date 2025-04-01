# Pocs.Aspire Solution

## Table of Contents
- [Overview](#overview)
- [Project Purpose](#project-purpose)
- [Project Structure](#project-structure)
  - [Domain][domain-readme]
  - [Business][business-readme]
  - [Infrastructure][infrastructure-readme]
  - [API Service][apiservice-readme]
  - [AppHost][apphost-readme]
- [Architecture](#architecture)
- [Key Technologies](#key-technologies)
- [Getting Started](#getting-started)
- [Architectural Decisions](#architectural-decisions)
- [Features](#features)
- [Roadmap](#roadmap)
- [Links of Interest](#links-of-interest)
- [References](#references)

## Overview

The Pocs.Aspire solution demonstrates a clean architecture implementation using .NET 8 and .NET Aspire for building distributed applications. It showcases modern development practices, clean architecture principles, and the capabilities of .NET Aspire for service orchestration.

## Project Purpose

This is a personal proof-of-concept (POC) project that I'm using to experiment with .NET Aspire and modern architecture patterns. It serves as both a learning platform and a reference implementation for clean architecture principles in a distributed environment. Through this project, I aim to explore best practices, try new technologies, and develop my skills in modern .NET development.

## Project Structure

This solution consists of multiple projects following clean architecture principles:

- **Domain Project**: The core of the application containing business entities and rules
- **Business Project**: Application services and business logic implementation
- **Infrastructure Project**: Database access and external service integrations
- **API Service Project**: RESTful endpoints exposing application functionality
- **AppHost Project**: .NET Aspire orchestration for distributed services
- **ServiceDefaults Project**: Standard .NET Aspire template for shared service configuration

Each project (except ServiceDefaults) has its own detailed README explaining its purpose and architecture.

## Architecture

This solution follows clean architecture principles with clear separation of concerns:

- **Domain Layer**: Core business entities and logic, free from external dependencies
- **Business Layer**: Application services implementing use cases and business rules
- **Infrastructure Layer**: External concerns like data persistence and external services
- **API Layer**: RESTful interfaces exposing functionality to clients
- **AppHost**: Orchestration of services and resources using .NET Aspire

## Key Technologies

- **.NET 8**: Latest .NET runtime with performance improvements
- **.NET Aspire**: Cloud-ready stack for distributed applications
- **Minimal APIs**: Lightweight HTTP endpoints with reduced boilerplate
- **Entity Framework Core**: Object-relational mapping for data access
- **PostgreSQL**: High-performance open-source database
- **Redis**: In-memory data store for caching
- **Docker**: Containerization for consistent deployment

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker Desktop or Podman

More information about prerequisites is available in the [.NET Aspire documentation][aspire-setup].

### Running the Application

1. Clone the repository
2. Navigate to the solution directory
3. Run: 
```shell 
dotnet run --project src/Pocs.Aspire.AppHost
```
4. Access the .NET Aspire dashboard at the provided URL

## Architectural Decisions

- **Clean Architecture**: Separation of concerns with domain at the center
- **Functional Results**: Using [LanguageExt][language-ext] for better error handling
- **Persistence Ignorance**: Domain layer remains independent of persistence details
- **.NET Aspire**: Modern approach to distributed application development

## Features

- **Resource Management**: Automatic provisioning of infrastructure resources
- **Health Monitoring**: Built-in health checks and dashboard
- **API Versioning**: Support for evolving APIs while maintaining compatibility
- **Output Caching**: Redis-backed response caching for improved performance
- **Structured Logging**: Comprehensive logging across all services
- **Observability**: Built-in with OpenTelemetry support

## Roadmap

The following items represent future development directions for this POC project:

1. **Authentication and Authorization**
   - [ ] Implement identity provider integration
   - [ ] Add role-based access control
   - [ ] Secure API endpoints

1. **Advanced Testing**
   - [ ] Expand unit test coverage
   - [ ] Add integration tests using test containers
   - [ ] Implement E2E testing
   - [ ] Implement mutational testing
   - [ ] Implement architectural testing

1. **CI/CD Pipeline**
   - [ ] Set up GitHub Actions workflows
   - [ ] Implement automated testing
   - [ ] Configure container publishing
   - [ ] Add static analysis tools

1. **Event-Driven Architecture**
   - [ ] Implement secondary service to enable inter service communication exploration
   - [ ] Implement message queues or event bus
   - [ ] Create event-based communication between services
   - [ ] Add event sourcing for key domains

1. **Performance Optimization**
   - [ ] Implement comprehensive benchmarking
   - [ ] Add advanced caching strategies

1. **Advanced Deployment**
   - [ ] Kubernetes deployment configuration
   - [ ] Infrastructure as Code templates
   - [ ] Multi-environment configuration
1. **Developers Experience / QOL**
   - [ ] Enable Hot Reload
   - [ ] Autogeneration of clients for multiple technologies based on OpenAPI specification
   - [ ] Integration with Application Performance Monitoring (e.g., Prometheus, Grafana)
1. **Documentation**
   - [ ] Autogenerate documentation wiki based on XML comments
   - [ ] Better documentation for the OpenAPI 
 
1. **Upgrades**
   - [ ] Upgrade to .Net 9


## Links of Interest

### Microsoft Technologies
- [.NET Aspire Overview][aspire-overview]
- [ASP.NET Core Documentation][aspnet-core]
- [Entity Framework Core Documentation][ef-core]
- [OpenTelemetry in .NET][opentelemetry-dotnet]

### Third-Party Technologies
- [PostgreSQL Documentation][postgresql]
- [Redis Documentation][redis]
- [Docker Documentation][docker]

### NuGet Packages
- [LanguageExt][language-ext] - Functional programming extensions
- [Asp.Versioning.Mvc][api-versioning] - API versioning
- [FluentValidation][fluent-validation] - Validation library
- [Swashbuckle.AspNetCore][swashbuckle] - Swagger/OpenAPI documentation
- [Testcontainers][testcontainers] - Defines test dependencies by as code
- [Shouldly][shouldly] - Fluent API assertion framework

### Architecture References
- [Clean Architecture][clean-architecture]
- [Microservices Architecture][microservices]


[domain-readme]: ./src/Pocs.Aspire.Domain/README.md
[business-readme]: ./src/Pocs.Aspire.Business/README.md
[infrastructure-readme]: ./src/Pocs.Aspire.Infrastructure/README.md
[apiservice-readme]: ./src/Pocs.Aspire.ApiService/README.md
[apphost-readme]: ./src/Pocs.Aspire.AppHost/README.md

[aspire-overview]: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview
[aspire-setup]: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=windows&pivots=visual-studio
[aspnet-core]: https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0
[ef-core]: https://learn.microsoft.com/en-us/ef/core/
[opentelemetry-dotnet]: https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing

[postgresql]: https://www.postgresql.org/docs/
[redis]: https://redis.io/documentation
[docker]: https://docs.docker.com/

[language-ext]: https://github.com/louthy/language-ext
[api-versioning]: https://github.com/dotnet/aspnet-api-versioning
[fluent-validation]: https://docs.fluentvalidation.net/
[swashbuckle]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
[testcontainers]: https://dotnet.testcontainers.org/
[shouldly]: https://docs.shouldly.org/

[clean-architecture]: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
[microservices]: https://microservices.io/



