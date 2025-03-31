# Pocs.Aspire.AppHost

## Overview

The AppHost project is the orchestration layer for the Pocs.Aspire distributed application using .NET Aspire. It defines and configures the services, resources, and connections that make up the complete application.

## Architectural Responsibility

This project is responsible for:

- Defining the services and resources that make up the application
- Configuring service-to-service communication
- Setting up infrastructure resources like databases and caches
- Providing local development experience with dashboard
- Configuring health checks and observability
- Enabling containerization for deployment

## Technical Approach

The AppHost project uses:

- **.NET Aspire**: For orchestrating multiple services and resources
- **Service Discovery**: For connecting services dynamically
- **Resource Management**: For databases, caches, and other infrastructure
- **Configuration Management**: For environment-specific settings
- **Containerization**: For consistent deployment across environments

## Key Features

1. **Application Composition**:
   - Service definition and configuration
   - Resource allocation and management
   - Connection management between components

2. **Resource Management**:
   - Database provisioning and configuration
   - Cache setup and management
   - Infrastructure orchestration

3. **.NET Aspire Dashboard**:
   - Visual representation of application health
   - Log aggregation and monitoring
   - Diagnostics and troubleshooting

4. **Development and Deployment**:
   - Local development environment
   - Containerization support

## Usage

To start the entire application, run:

```shell
dotnet run
```

If it is the first time, Aspire Dashboard will prompt for a token credentials. The instructions about how to get it are described.

This will start all services and resources, set up the necessary configuration, and launch the .NET Aspire dashboard.

### Known issues
- Sometimes the main application starts too fast and try to ensure the database is created, however the database instance may not be ready yet and the application crashes. Aspire is prepared for these scenarios at should retry to run it eventually. If it persists you can re-run the ApiService container from the Aspire Dashboard.
