FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Alunos.Api.App/Alunos.Api.App.csproj", "src/Alunos.Api.App/"]
COPY ["src/Alunos.Api.Domain/Alunos.Api.Domain.csproj", "src/Alunos.Api.Domain/"]
COPY ["src/Alunos.Api.Infra/Alunos.Api.Infra.csproj", "src/Alunos.Api.Infra/"]


RUN dotnet restore "src/Alunos.Api.App/Alunos.Api.App.csproj"
COPY . .
WORKDIR "/src/src/Alunos.Api.App"
RUN dotnet build "Alunos.Api.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Alunos.Api.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Alunos.Api.App.dll"]
