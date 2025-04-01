# Functional Tests

## Overview

This project contains functional tests for the API Service layer, focusing on validating the behavior of REST endpoints. These tests use real application components to provide higher confidence in end-to-end system behavior.

## Technical Approach

The functional tests use:

- **.NET Aspire Testing Framework**: To orchestrate the full application stack for testing
- **XUnit**: As the testing framework
- **Shouldly**: For more readable assertions
- **HttpClient**: To interact with the running API service

## Key Features

1. **Aspire Host Integration**:
   - Boots up the full distributed application using Aspire
   - Waits for service readiness via health checks
   - Automatically wires up HTTP clients for inter-service communication

2. **IAsyncLifetime Implementation**:
   - Properly initializes and tears down the application before and after tests
   - Ensures consistent, repeatable test environments

3. **HTTP-Based Functional Testing**:
   - Sends real HTTP requests to validate API endpoints
   - Verifies expected responses including status codes and headers
   - Parses and asserts on returned content for deeper validation

## Design Considerations

### Current Approach

The current implementation runs the full application within the test context, which provides:

- **Realistic Testing**: Validates real API interactions and service wiring
- **End-to-End Coverage**: Ensures all layers, from HTTP to data access, are exercised
- **Confidence in Behavior**: Helps catch integration issues early in the development lifecycle

### Limitations and Alternatives

While this approach offers robust validation, it comes with some trade-offs:

1. **Execution Overhead**:
   - Booting the entire app for each test run consumes more resources
   - Longer test start-up and execution times compared to isolated tests

2. **Alternative Approaches**:
   - **Mocked Dependencies**: Replace internal dependencies to focus tests on specific layers
   - **Contract Testing**: Use tools like Pact to verify API contracts between services
   - **In-Memory Hosting**: Use `WebApplicationFactory` for faster, in-process tests
   - **Hybrid Strategy**: Combine unit, integration, and functional tests for layered assurance

## References

- [Write your first .NET Aspire test][write-aspire-test]
- [Access resources in .NET Aspire tests][access-aspire-resources]
- [xUnit Documentation][xunit-docs]
- [Shouldly Documentation][shouldly-docs]

[write-aspire-test]: https://learn.microsoft.com/en-us/dotnet/aspire/testing/write-your-first-test  
[access-aspire-resources]: https://learn.microsoft.com/en-us/dotnet/aspire/testing/accessing-resources  
[xunit-docs]: https://xunit.net/  
[shouldly-docs]: https://github.com/shouldly/shouldly  
