FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY ./src/Ria.Services.Customer.Web ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Environment variable to keep swagger available at /swagger/index.html
ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Ria.Services.Customer.Web.dll"]