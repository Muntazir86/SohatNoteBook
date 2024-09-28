FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

# Use Linux-based image for .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/sohatNotebook.Api/sohatNotebook.Api.csproj", "src/sohatNotebook.Api/"]

# Restore dependencies
RUN dotnet restore "src/sohatNotebook.Api/sohatNotebook.Api.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/src/sohatNotebook.Api"

# Build the application
RUN dotnet build "sohatNotebook.Api.csproj" -c $configuration -o /app/build

# Publish the application
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "sohatNotebook.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sohatNotebook.Api.dll"]