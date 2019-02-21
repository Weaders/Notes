FROM microsoft/dotnet:2.2-sdk

EXPOSE 80
EXPOSE 443

WORKDIR /src

COPY ["NotesMVC/NotesMVC.csproj", "NotesMVC/"]
COPY ["NotesMVC.Data/NotesMVC.Data.csproj", "NotesMVC.Data/"]
COPY ["NotesMVC.DomainServices/NotesMVC.DomainServices.csproj", "NotesMVC.DomainServices/"]
COPY ["NotesMVC.Services/NotesMVC.Services.csproj", "NotesMVC.Services/"]

RUN dotnet restore "NotesMVC/NotesMVC.csproj"

COPY NotesMVC NotesMVC
COPY NotesMVC.Data NotesMVC.Data
COPY NotesMVC.DomainServices NotesMVC.DomainServices
COPY NotesMVC.Services NotesMVC.Services

WORKDIR /src/NotesMVC

RUN dotnet publish "NotesMVC.csproj" -c Release -o /app

COPY NotesMVC/entrypoint.sh /app

RUN apt update
RUN apt install curl -y

#NODE!!!
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash
RUN apt-get install -y nodejs

#Check node
RUN node -v
RUN npm -v

#Install npm dependices
RUN npm i

RUN npm run buildProd

WORKDIR /app

ENTRYPOINT ["bash", "entrypoint.sh"]