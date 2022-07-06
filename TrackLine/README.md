# TrackLine operations

## Database operations
~~~sh
dotnet ef migrations add --project App.DAL.EF --startup-project WebApp --context AppDbContext Initial

dotnet ef migrations remove --project App.DAL.EF --startup-project WebApp --context AppDbContext

dotnet ef database update --project App.DAL.EF --startup-project WebApp

dotnet ef database drop --project App.DAL.EF --startup-project WebApp
~~~

## Scaffolding

### Controllers

#### Web Controllers
~~~sh
cd WebApp
dotnet aspnet-codegenerator controller -name !!NAME + Controller       -actions -m  App.Domain.!!NAME    -dc AppDbContext -outDir Areas\Admin\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UserRoundResultConroller      -actions -m  App.Domain.UserRoundResult   -dc AppDbContext -outDir Areas\Admin\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
~~~

#### API Controllers
~~~sh
cd WebApp
dotnet aspnet-codegenerator controller -name !!Name + Controller     -m App.Domain.!!NAME     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name UserGameStatisticsController    -m App.Domain.UserGameStatistics    -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
~~~

#### Web Pages
~~~sh
cd WebApp
dotnet aspnet-codegenerator razorpage -m !!Name -dc AppDbContext -udl -outDir Pages/!!DomainName  --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m GameShipConfig -dc AppDbContext -udl -outDir Pages/ShipConfiguration --referenceScriptLibraries
~~~

## Add new Repository + Service
!NB not all entities need Services, but all need Repository

- From **App.Domain** add entity to **App.DAL.DTO** and **App.BLL.DTO** excluding data annotations

- Go to **App.BLL**
  - Mapper => create mapper file
  - *AutoMapperConfig* => register Mapper

- Go to **App.Contracts.DAL**
  - Repositories => create IRepository file with default and custom classes
  - *IAppUnitOfWork* => register IRepository

- Go to **App.DAL.EF**
  - Repositories => create Repository file
  - Mappers => create Mapper file
  - *AppUOW* => register Repository
  - *AutoMapperConfig* => register Mapper

- Go to **App.Contracts.BLL**
  - Services => create IService file
  - *IAppBLL* => register IService

- Go to **App.BLL**
  - Services => create Service file
  - *AppBLL* => register Service

- Additional - create ApiController using scaffolding, commands above

## Docker
### Docker db local via docker compose example
- create *docker-compose.yml* file, add it to the solution items

~~~yml
version: '3.8'

services:
  NAME-postgres:
    container_name: NAME-postgres
    image: postgres:latest
    restart: unless-stopped
    environment: 
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
    logging: 
      options:
        max-size: 10m
        max-file: "3"
    ports:
      - "5432:5432"
    volumes: 
      - NAME-postgres-volume:/var/lib/postrgesql/data

volumes:
  NAME-postgres-volume:
~~~

- right click *docker-compose.yml* => run
- connection string - "Host:localhost;Port=5432;Username=postgres;Password=postgres;database=NameDataBase"

### DB deploy example
- azure connection string - "Server=postgredistro2022.postgres.database.azure.com;Database=skillpoint;Port=5432;User Id=postgres;Password=jkfsd324.JF3;Ssl Mode=Require;Trust Server Certificate=true;"

### App deploy example
- create Dockerfile in solution items

~~~dockerfile
# grab the sdk image, create on name for it - "build"
FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /app
EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

# NB! These is example, all folder must me chenges according on the project
COPY BLL.App/*.csproj ./BLL.App/
COPY BLL.App.DTO/*.csproj ./BLL.App.DTO/
COPY BLL.Base/*.csproj ./BLL.Base/
COPY Contracts.BLL.App/*.csproj ./Contracts.BLL.App/
COPY Contracts.BLL.Base/*.csproj ./Contracts.BLL.Base/

COPY Contracts.DAL.App/*.csproj ./Contracts.DAL.App/
COPY Contracts.DAL.Base/*.csproj ./Contracts.DAL.Base/
COPY DAL.App.DTO/*.csproj ./DAL.App.DTO/
COPY DAL.App.EF/*.csproj ./DAL.App.EF/
COPY DAL.Base/*.csproj ./DAL.Base/
COPY DAL.Base.EF/*.csproj ./DAL.Base.EF/

COPY Contracts.Domain/*.csproj ./Contracts.Domain/
COPY Domain.App/*.csproj ./Domain.App/
COPY Domain.Base/*.csproj ./Domain.Base/

COPY PublicApi.DTO.v1/*.csproj ./PublicApi.DTO.v1/
COPY Extensions/*.csproj ./Extensions/

COPY WebApp/*.csproj ./WebApp/

# crate the first layer with jus nuget packages installed
RUN dotnet restore

# copy everything else and build app
COPY BLL.App/. ./BLL.App/
COPY BLL.App.DTO/. ./BLL.App.DTO/
COPY BLL.Base/. ./BLL.Base/
COPY Contracts.BLL.App/. ./Contracts.BLL.App/
COPY Contracts.BLL.Base/. ./Contracts.BLL.Base/

COPY Contracts.DAL.App/. ./Contracts.DAL.App/
COPY Contracts.DAL.Base/. ./Contracts.DAL.Base/
COPY DAL.App.DTO/. ./DAL.App.DTO/
COPY DAL.App.EF/. ./DAL.App.EF/
COPY DAL.Base/. ./DAL.Base/
COPY DAL.Base.EF/. ./DAL.Base.EF/

COPY Contracts.Domain/. ./Contracts.Domain/
COPY Domain.App/. ./Domain.App/
COPY Domain.Base/. ./Domain.Base/

COPY PublicApi.DTO.v1/. ./PublicApi.DTO.v1/
COPY Extensions/. ./Extensions/

COPY WebApp/. ./WebApp/

WORKDIR /app/WebApp

# compile the app with Release options and put files into dir "out"
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:latest AS runtime
WORKDIR /app
EXPOSE 80

# All sensetive information here (appsettings)
ENV ConnectionStrings:SqlServerConnection="Server=alpha.akaver.com,1533;User Id=student;Password=Student.Bad.password.0;Database=akaver_sportmap_123;MultipleActiveResultSets=true"

# Copy files from previous image(the "build") 
COPY --from=build /app/WebApp/out ./

# Run this command when command when container starts up (not runned during image build phase)
ENTRYPOINT ["dotnet", "WebApp.dll"]
~~~

- create .dockerignore file, link it with Dockerfile
- reference .dockerignore file in project sln

~~~docker
**/bin
**/obj
**/out
**/.vscode
**/.vs
.dotnet
.Microsoft.DotNet.ImageBuilder
~~~

- Build app into docker image
~~~sh
docker build -t IMAGENAME .
~~~

- Run it locally via docker (set envs for db if needed)
~~~sh
docker run --name INSTANCE_NAME --rm -it -p 8000:80 IMAGENAME
~~~
  - -p 8000:80 – map port 8000 (local machine) to docker port 80
  - --name webapp_docker – name of instance
  - -it – interactive (can close down with ctrl-c)
  - --rm - clean resources after closing down

#### Publish in dockerhub

- Tag your docker image (needed for publishing)

~~~sh
docker tag IMAGENAME username/IMAGENAME:latest
~~~

- Publish your docker image to registry
~~~sh
docker login -u [username] -p [password]
docker push username/IMAGENAME:latest
~~~

