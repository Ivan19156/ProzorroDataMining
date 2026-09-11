FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ProzorroDataMining.Worker/ProzorroDataMining.Worker.csproj", "ProzorroDataMining.Worker/"]
COPY ["ProzorroDataMining.Application/ProzorroDataMining.Application.csproj", "ProzorroDataMining.Application/"]
COPY ["ProzorroDataMining.Data/ProzorroDataMining.Data.csproj", "ProzorroDataMining.Data/"]
COPY ["ProzorroDataMining.Domain/ProzorroDataMining.Domain.csproj", "ProzorroDataMining.Domain/"]
COPY ["ProzorroDataMining.Contracts/ProzorroDataMining.Contracts.csproj", "ProzorroDataMining.Contracts/"]
COPY ["ProzorroDataMining.Shared/ProzorroDataMining.Shared.csproj", "ProzorroDataMining.Shared/"]

RUN dotnet restore "ProzorroDataMining.Worker/ProzorroDataMining.Worker.csproj"

COPY . .

RUN dotnet build "ProzorroDataMining.Worker/ProzorroDataMining.Worker.csproj" -c Release -o /app/build
RUN dotnet publish "ProzorroDataMining.Worker/ProzorroDataMining.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProzorroDataMining.Worker.dll"]