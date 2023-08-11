#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/API/Cbms.Wms.Web/Cbms.Kms.Web.csproj", "src/API/Cbms.Wms.Web/"]
COPY ["src/Modules/Cbms.Kms.Application/Cbms.Kms.Application.csproj", "src/Modules/Cbms.Kms.Application/"]
COPY ["src/Modules/Cbms.Kms.Domain/Cbms.Kms.Domain.csproj", "src/Modules/Cbms.Kms.Domain/"]
COPY ["src/Modules/Cbms.Kms.Infrastructure/Cbms.Kms.Infrastructure.csproj", "src/Modules/Cbms.Kms.Infrastructure/"]
RUN dotnet restore "src/API/Cbms.Wms.Web/Cbms.Kms.Web.csproj"
COPY . .
WORKDIR "/src/src/API/Cbms.Wms.Web"
RUN dotnet build "Cbms.Kms.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cbms.Kms.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cbms.Kms.Web.dll"]
