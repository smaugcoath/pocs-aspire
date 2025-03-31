# Pocs.Aspire.ApiService

## Overview

The ApiService project is a RESTful API built with ASP.NET Core that exposes the domain functionality of the Pocs.Aspire application. It serves as the primary interface for clients to interact with the system.

## Architectural Responsibility

This project is responsible for:

- Exposing REST endpoints for client applications
- Handling HTTP requests and producing appropriate responses
- Converting between domain models and API DTOs
- Implementing API versioning
- Providing OpenAPI documentation
- Implementing caching strategies

## Technical Approach

The ApiService uses:

- **Minimal API style**: For lightweight, focused endpoint definitions
- **Controller-based endpoints**: For more complex API scenarios
- **API Versioning**: Supporting multiple API versions simultaneously
- **Output Caching**: Redis-backed response caching for improved performance
- **Problem Details**: [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807) compliant error responses
- **Swagger/OpenAPI**: Comprehensive API documentation

## Key Features

1. **RESTful Endpoints**:
   - Standard HTTP methods
   - Resource-based URL structure
   - Consistent response patterns

2. **API Versioning**: Support for evolving the API while maintaining compatibility

3. **Output Caching**: Improved performance through strategic response caching

4. **Error Handling**: Standardized approach using Problem Details

## Third-Party Packages

- [**Asp.Versioning.Mvc**](https://github.com/dotnet/aspnet-api-versioning/wiki): API versioning support
- [**Microsoft.AspNetCore.OutputCaching**](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-8.0): Response caching middleware
- [**Swashbuckle.AspNetCore**](https://github.com/domaindrivendev/Swashbuckle.AspNetCore): Swagger/OpenAPI documentation

## Design Considerations

- Clear separation between API contracts and domain models
- Consistent request/response patterns across all endpoints
- Strategic use of caching for performance-critical endpoints
- Comprehensive API documentation through Swagger
