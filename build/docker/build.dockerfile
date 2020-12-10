FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN  mkdir -p /usr/paradigm/
COPY . /usr/paradigm/
WORKDIR /usr/paradigm/
RUN dotnet restore
RUN dotnet publish -c release -o ./orm
