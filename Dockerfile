FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["NotesMVC/NotesMVC.csproj", "NotesMVC/"]
RUN dotnet restore "NotesMVC/NotesMVC.csproj"
COPY . .
WORKDIR "/src/NotesMVC"
RUN dotnet build "NotesMVC.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NotesMVC.csproj" -c Release -o /app

RUN apt update
RUN apt install curl -y

#NODE!!!
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash
RUN apt-get install -y nodejs

#Check node
RUN node -v
RUN npm -v

RUN npm i

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "NotesMVC.dll"]