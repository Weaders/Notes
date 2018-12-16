FROM microsoft/dotnet:2.1-sdk

EXPOSE 80
EXPOSE 443

WORKDIR /src
COPY ["NotesMVC.csproj", "NotesMVC/"]
RUN dotnet restore "NotesMVC/NotesMVC.csproj"
COPY . NotesMVC
WORKDIR /src/NotesMVC

RUN dotnet build "NotesMVC.csproj" -c Release -o /app

COPY entrypoint.sh /app

RUN dotnet publish "NotesMVC.csproj" -c Release -o /app

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

WORKDIR /app

ENTRYPOINT ["bash", "entrypoint.sh"]