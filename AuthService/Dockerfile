# Build project dlls
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV TZ Africa/Johannesburg
WORKDIR /home
WORKDIR /app
COPY --from=build-env /app/out .
RUN ls -a
CMD ["dotnet", "AuthService.dll"]