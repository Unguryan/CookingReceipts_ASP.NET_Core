
#FROM corebll_image as bll
#WORKDIR /categorySrc
#
#COPY ./Microservices/CategoryMicroservice/ .
#COPY --from=bll


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#FROM corebll_image:latest AS core
WORKDIR /idsSrc

COPY . .
COPY --from=corebll_image:latest ./CoreBLL/output ./DLL
#ADD ./CoreBLL/output/* /DLL

RUN dotnet restore "IDS.csproj" --disable-parallel
RUN dotnet build "IDS.csproj" -c Release -o /idsApp --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /idsApp

COPY --from=build /idsApp ./

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "IDS.dll"]



#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /categorySrc
#
#COPY ./Microservices/CategoryMicroservice/ .
#ADD ./CoreBLL/output/* /DLL
#RUN dotnet restore "Microservices/CategoryMicroservice/CategoryMicroservice.csproj" --disable-parallel
#RUN dotnet build "Microservices/CategoryMicroservice/CategoryMicroservice.csproj" -c Release -o /categoryApp --no-restore
#
#
#FROM mcr.microsoft.com/dotnet/aspnet:6.0
#WORKDIR /categoryApp
#
#COPY --from=build /categoryApp ./
#
#EXPOSE 80
#EXPOSE 443
#ENTRYPOINT ["dotnet", "CategoryMicroservice.dll"]


#COPY *.csproj CategoryService/
#COPY DLL/*.dll CategoryService/DLL
#RUN dotnet restore "CategoryService/CategoryMicroservice.csproj"
#COPY . .
#WORKDIR "/categorySrc/CategoryService"
#RUN dotnet build "CategoryMicroservice.csproj" -c Release -o /categoryApp/build
#
#FROM build AS publish
#RUN dotnet publish "CategoryMicroservice.csproj" -c Release -o /categoryApp/publish
#
#FROM base AS final
#WORKDIR /categoryApp
#COPY --from=publish /categoryApp/publish .
#ENTRYPOINT ["dotnet", "CategoryMicroservice.dll"]