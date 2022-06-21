FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /CoreBLL

COPY /Interfaces/*.csproj Interfaces/
COPY /Core/*.csproj Core/
RUN dotnet restore "Interfaces/Interfaces.csproj"
RUN dotnet restore "Core/Core.csproj"


COPY . .
WORKDIR "/CoreBLL/Core"
RUN dotnet build "Core.csproj" -c Release -o /CoreBLL/output



#COPY /Core/*.csproj ./
#
#WORKDIR /coreBLL/src/Interfaces
#RUN dotnet restore
#
#WORKDIR /coreBLL/src/Core
#RUN dotnet restore

#RUN dotnet restore 
#
#RUN dotnet publish -c Release -o out
#
#
#WORKDIR /coreBLL
#RUN dotnet build "/Interfaces/Interfaces.csproj" -c Release -o /coreBLL/output
#RUN dotnet build "/Core/Core.csproj" -c Release -o /coreBLL/output


#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#
#COPY /Interfaces/*.csproj Interfaces/
#RUN dotnet restore "Interfaces/Interfaces.csproj"
#
#
#COPY . .
#WORKDIR "/src/Interfaces"
#RUN dotnet build "Interfaces.csproj" -c Release -o /src/build