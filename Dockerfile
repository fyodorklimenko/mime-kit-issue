FROM mcr.microsoft.com/dotnet/core/sdk:latest

WORKDIR /app

COPY . .

RUN dotnet restore MimeKitIssue/MimeKitTests/MimeKitTests.csproj
RUN dotnet build MimeKitIssue/MimeKitTests/MimeKitTests.csproj

ENTRYPOINT ["dotnet", "test", "MimeKitIssue/MimeKitTests/MimeKitTests.csproj"]
