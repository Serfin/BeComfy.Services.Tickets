FROM mcr.microsoft.com/dotnet/core/sdk:3.0
WORKDIR /app
COPY ./src/BeComfy.Services.Tickets/bin/Release/netcoreapp3.0 .
ENV ASPNETCORE_URLS http://*:5020
ENV ASPNETCORE_ENVIRONMENT Release
EXPOSE 5020
ENTRYPOINT ["dotnet", "BeComfy.Services.Tickets.dll"]