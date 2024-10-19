FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /var/apps/UploaderBot

EXPOSE 50031

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /var/appsrcs/src
COPY ["UploaderBot/UploaderBot.csproj","UploaderBot/"]
RUN dotnet restore "UploaderBot/UploaderBot.csproj"
COPY . .
WORKDIR "/var/appsrcs/src/UploaderBot"
RUN dotnet build "UploaderBot.csproj" -c $BUILD_CONFIGURATION -o /var/apps/UploaderBot/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UploaderBot.csproj" -c $BUILD_CONFIGURATION -o /var/apps/UploaderBot/publish

FROM base AS final
WORKDIR /var/apps/UploaderBot
COPY --from=publish /var/apps/UploaderBot/publish .
ENTRYPOINT ["dotnet","UploaderBot.dll","--urls=http://localhost:5031/"]