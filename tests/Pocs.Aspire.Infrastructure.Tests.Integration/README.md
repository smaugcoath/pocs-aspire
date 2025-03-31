# Integration Tests

## Overview

This project contains integration tests for the Infrastructure layer, focusing on validating the actual interactions with external dependencies like databases. These tests use real implementations rather than mocks to provide higher confidence in the system's behavior.

## Technical Approach

The integration tests use:

- **Testcontainers**: To provide ephemeral PostgreSQL instances for each test
- **XUnit**: As the testing framework
- **Shouldly**: For more readable assertions
- **Entity Framework Core**: To interact with the test database

## Key Features

1. **Testcontainers Integration**:
   - Spins up Docker containers for PostgreSQL
   - Configures containers with appropriate settings
   - Handles container lifecycle (startup, shutdown)

2. **IAsyncLifetime Implementation**:
   - Properly initializes test environment before test execution
   - Cleans up resources after tests complete

3. **Real Database Interactions**:
   - Tests against actual database to verify ORM mappings
   - Validates queries against real database engine

## Design Considerations

### Current Approach

The current implementation creates and destroys a new PostgreSQL container for each test class, which provides:

- **Test Isolation**: Each test runs in a clean environment
- **No Test Interference**: Tests don't affect each other's data
- **Realistic Testing**: Tests run against actual PostgreSQL instances

### Limitations and Alternatives

While the current approach works well, it has some limitations:

1. **Performance Impact**:
   - Creating and destroying containers for each test class is resource-intensive
   - Can significantly increase test execution time in larger test suites

2. **Alternative Approaches**:
   - **Respawn**: Use a tool like [Respawn][respawn] to clean the database between tests instead of recreating it
   - **Database-per-Test**: Spin up one container but create multiple databases for parallel test execution
   - **Shared Container**: Use a single container with database resets between tests for faster execution
   - **Container Pooling**: Implement a container pool to reuse containers across tests

[respawn]: https://github.com/jbogard/Respawn
[testcontainers]: https://dotnet.testcontainers.org/
[xunit3]: https://xunit.net/
[shouldly]: https://github.com/shouldly/shouldly
