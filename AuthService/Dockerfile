# Build project dlls
FROM madebyeden.azurecr.io/dotnet_ubuntu:buildimage AS build-env
WORKDIR /app
COPY *.csproj ./
COPY . ./
RUN dotnet publish -c Release -o out --self-contained false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine-amd64
ENV TZ Africa/Johannesburg
WORKDIR /home
WORKDIR /app
COPY --from=build-env /app/out .
RUN ls -a
CMD ["dotnet", "AuthService.dll"]
