# Get Base SDK Image from Microsoft
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS build-env
WORKDIR /app

# Copy the CSPROJ file and restore any dependencies
COPY *.CSPROJ ./
RUN dotnet restore

# Copy the project files and build our release
COPY . ./
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "DockerAPI.dll" ]