# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG VERSION=1.0.0
WORKDIR /app

# Copy solution and restore dependencies
COPY CV.App.slnx ./
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY src/CVA.Application.Contracts/CVA.Application.Contracts.csproj src/CVA.Application.Contracts/
COPY src/CVA.Application.Services/CVA.Application.Services.csproj src/CVA.Application.Services/
COPY src/CVA.Application.Validators/CVA.Application.Validators.csproj src/CVA.Application.Validators/
COPY src/CVA.Domain.Interfaces/CVA.Domain.Interfaces.csproj src/CVA.Domain.Interfaces/
COPY src/CVA.Domain.Models/CVA.Domain.Models.csproj src/CVA.Domain.Models/
COPY src/CVA.Infrastructure.Common/CVA.Infrastructure.Common.csproj src/CVA.Infrastructure.Common/
COPY src/CVA.Infrastructure.Mongo/CVA.Infrastructure.Mongo.csproj src/CVA.Infrastructure.Mongo/
COPY src/CVA.Infrastructure.Postgres/CVA.Infrastructure.Postgres.csproj src/CVA.Infrastructure.Postgres/
COPY src/CVA.Presentation.Web/CVA.Presentation.Web.csproj src/CVA.Presentation.Web/
COPY src/CVA.Tools.Common/CVA.Tools.Common.csproj src/CVA.Tools.Common/

RUN dotnet restore src/CVA.Presentation.Web/CVA.Presentation.Web.csproj

COPY . .
WORKDIR "/app/src/CVA.Presentation.Web"
RUN dotnet build "CVA.Presentation.Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CVA.Presentation.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

ARG SERVICE_NAME=cva-backend
ARG VERSION=1.0.0
LABEL service.name=$SERVICE_NAME
LABEL service.version=$VERSION

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Use built-in non-root user for .NET 8+
USER app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CVA.Presentation.Web.dll"]
