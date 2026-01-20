# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG VERSION=1.0.0
WORKDIR /app

# Copy solution and restore dependencies
COPY cva-back.slnx ./
COPY Directory.Build.props ./
COPY Directory.Packages.props ./

COPY src/CVA.Application.Abstractions/CVA.Application.Abstractions.csproj src/CVA.Application.Abstractions/
COPY src/CVA.Application.Contracts/CVA.Application.Contracts.csproj src/CVA.Application.Contracts/
COPY src/CVA.Application.IdentityService/CVA.Application.IdentityService.csproj src/CVA.Application.IdentityService/
COPY src/CVA.Application.ProfileService/CVA.Application.ProfileService.csproj src/CVA.Application.ProfileService/
COPY src/CVA.Application.Validators/CVA.Application.Validators.csproj src/CVA.Application.Validators/
COPY src/CVA.Domain.Interfaces/CVA.Domain.Interfaces.csproj src/CVA.Domain.Interfaces/
COPY src/CVA.Domain.Models/CVA.Domain.Models.csproj src/CVA.Domain.Models/
COPY src/CVA.Infrastructure.Auth/CVA.Infrastructure.Auth.csproj src/CVA.Infrastructure.Auth/
COPY src/CVA.Infrastructure.Common/CVA.Infrastructure.Common.csproj src/CVA.Infrastructure.Common/
COPY src/CVA.Infrastructure.Media/CVA.Infrastructure.Media.csproj src/CVA.Infrastructure.Media/
COPY src/CVA.Infrastructure.Mongo/CVA.Infrastructure.Mongo.csproj src/CVA.Infrastructure.Mongo/
COPY src/CVA.Infrastructure.Postgres/CVA.Infrastructure.Postgres.csproj src/CVA.Infrastructure.Postgres/
COPY src/CVA.Infrastructure.ResumePdf/CVA.Infrastructure.ResumePdf.csproj src/CVA.Infrastructure.ResumePdf/
COPY src/CVA.Infrastructure.Storage.S3/CVA.Infrastructure.Storage.S3.csproj src/CVA.Infrastructure.Storage.S3/
COPY src/CVA.Presentation.Auth/CVA.Presentation.Auth.csproj src/CVA.Presentation.Auth/
COPY src/CVA.Presentation.Web/CVA.Presentation.Web.csproj src/CVA.Presentation.Web/
COPY src/CVA.Tools.Common/CVA.Tools.Common.csproj src/CVA.Tools.Common/

RUN dotnet restore src/CVA.Presentation.Web/CVA.Presentation.Web.csproj

COPY . .
WORKDIR "/app/src/CVA.Presentation.Web"
RUN dotnet build "CVA.Presentation.Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CVA.Presentation.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---- Playwright: install browsers (Chromium) into image ----
ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright
RUN dotnet tool install --global Microsoft.Playwright.CLI
ENV PATH="$PATH:/root/.dotnet/tools"
RUN playwright install chromium
# -----------------------------------------------------------

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

# ---- Chromium deps for Playwright (runtime) ----
RUN apt-get update && apt-get install -y --no-install-recommends \
    libnss3 \
    libnspr4 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxfixes3 \
    libxrandr2 \
    libgbm1 \
    libasound2t64 \
    libpangocairo-1.0-0 \
    libpango-1.0-0 \
    libcairo2 \
    libdrm2 \
    libx11-6 \
    libxext6 \
    libxshmfence1 \
    libglib2.0-0 \
    libgtk-3-0 \
    ca-certificates \
    fonts-liberation \
    && rm -rf /var/lib/apt/lists/*
# -----------------------------------------------

# Playwright browsers path + copy browsers
ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

COPY --from=publish /app/publish .
COPY --from=publish /ms-playwright /ms-playwright
RUN chmod -R 755 /ms-playwright

# Use built-in non-root user for .NET 8+
USER app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CVA.Presentation.Web.dll"]