ARG VERSION=3.1-alpine3.14

# Get base image and copy project file
FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS publish

WORKDIR /src

COPY ["nuget.config", "."]
COPY ["Ofgem.API.GGSS.WebApi/Ofgem.API.GGSS.WebApi.csproj", "Ofgem.API.GGSS.WebApi/"]
COPY ["Ofgem.API.GGSS.Domain/Ofgem.API.GGSS.Domain.csproj", "Ofgem.API.GGSS.Domain/"]
COPY ["Ofgem.API.GGSS.Application/Ofgem.API.GGSS.Application.csproj", "Ofgem.API.GGSS.Application/"]
COPY ["Ofgem.API.GGSS.Persistence/Ofgem.API.GGSS.Persistence.csproj", "Ofgem.API.GGSS.Persistence/"]

# Restore project
RUN dotnet restore "Ofgem.API.GGSS.WebApi/Ofgem.API.GGSS.WebApi.csproj"

# Copy restored project
COPY . .

# Publish project
RUN dotnet publish "Ofgem.API.GGSS.WebApi/Ofgem.API.GGSS.WebApi.csproj" -c Release -o /app/publish

# Create final image with entrypoint
FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS final

WORKDIR /app
COPY --from=publish /app/publish .

# Add user without root privileges
RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app

RUN apk update && apk add -q icu-libs

USER app

ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

ENTRYPOINT ["dotnet", "Ofgem.API.GGSS.WebApi.dll"]
