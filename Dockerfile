# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the official .NET SDK as a parent image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/InterviewTest.Api/InterviewTest.Api.csproj", "src/InterviewTest.Api/"]
COPY ["src/InterviewTest.Core/InterviewTest.Core.csproj", "src/InterviewTest.Core/"]
COPY ["src/InterviewTest.Infrastructure/InterviewTest.Infrastructure.csproj", "src/InterviewTest.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/InterviewTest.Api/InterviewTest.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/src/InterviewTest.Api"
RUN dotnet build "InterviewTest.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "InterviewTest.Api.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InterviewTest.Api.dll"]