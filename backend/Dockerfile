# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY StudyQuest.API/StudyQuest.API.csproj StudyQuest.API/
RUN dotnet restore StudyQuest.API/StudyQuest.API.csproj

COPY . .
WORKDIR /src/StudyQuest.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Required for SSL connections to Supabase (Npgsql needs GSSAPI)
RUN apt-get update && apt-get install -y --no-install-recommends libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "StudyQuest.API.dll"]
