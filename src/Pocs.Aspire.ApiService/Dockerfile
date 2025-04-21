# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Pocs.Aspire.ApiService/Pocs.Aspire.ApiService.csproj", "src/Pocs.Aspire.ApiService/"]
COPY ["src/Pocs.Aspire.Business/Pocs.Aspire.Business.csproj", "src/Pocs.Aspire.Business/"]
COPY ["src/Pocs.Aspire.Domain/Pocs.Aspire.Domain.csproj", "src/Pocs.Aspire.Domain/"]
COPY ["src/Pocs.Aspire.Infrastructure/Pocs.Aspire.Infrastructure.csproj", "src/Pocs.Aspire.Infrastructure/"]
COPY ["src/Pocs.Aspire.ServiceDefaults/Pocs.Aspire.ServiceDefaults.csproj", "src/Pocs.Aspire.ServiceDefaults/"]
RUN dotnet restore "./src/Pocs.Aspire.ApiService/Pocs.Aspire.ApiService.csproj"
COPY . .
WORKDIR "/src/src/Pocs.Aspire.ApiService"
RUN dotnet build "./Pocs.Aspire.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pocs.Aspire.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pocs.Aspire.ApiService.dll"]