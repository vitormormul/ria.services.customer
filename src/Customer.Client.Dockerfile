FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY ./Ria.Services.Customer.Client ./Ria.Services.Customer.Client
COPY ./Ria.Services.Customer.Web ./Ria.Services.Customer.Web

RUN dotnet restore ./Ria.Services.Customer.Client
RUN dotnet publish -c Release -o out ./Ria.Services.Customer.Client

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Ria.Services.Customer.Client.dll"]