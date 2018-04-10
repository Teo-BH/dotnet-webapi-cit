## Tutorial Steps

- Create a ASP.NET Core Web API Project
  ```bash
  $ dotnet new webapi
  ```

- Run ASP.NET Core Web API Project
  ```bash
  Installing C# dependencies...
  Platform: darwin, x86_64

  Downloading package 'OmniSharp for OSX' (24026 KB) .................... Done!
  Downloading package '.NET Core Debugger (macOS / x64)' (44057 KB) .................... Done!

  Installing package 'OmniSharp for OSX'
  Installing package '.NET Core Debugger (macOS / x64)'

  Finished
  ```

- Check Web API Service on Postman

- Add Swagger for Documentation
  ```bash
  dotnet add dotnet-webapi-cit.csproj package Swashbuckle.AspNetCore

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddMvc();

    // Register the Swagger generator, defining one or more Swagger documents
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
    });
  }

  public void Configure(IApplicationBuilder app)
  {
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

    app.UseMvc();
  }
  ```

- Add Redis for cache
  ```bash

  // Startup.cs
  public void Configure(IApplicationBuilder app)
    {
      // Redis Cache
      services.AddDistributedRedisCache(options =>
      {
        options.Configuration = Configuration.GetConnectionString("RedisConnection");
        options.InstanceName = "RedisInstance";
      });

      app.UseMvc();
    }  

  // appsettings
  "ConnectionStrings": {
    "RedisConnection": "localhost"
  }

  // Controller
  private readonly IDistributedCache cache;
  
  public Controller(IDistributedCache distributedCache) 
  {
    this.cache = distributedCache;
  }

  // GET api/values/5
  [HttpGet("{id}")]
  public string Get(int id)
  {
      if (id == 0) {
          id = GetValueCache();
      }
      return $"value {id}";
  }

  private int GetValueCache() {
    const string cacheKey = "IndexValue";
    string data = cache.GetString(cacheKey);
    int result = 1;
    if (Int32.TryParse(data, out result)) {
      result += 1;
    }
  
    var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
    cache.SetString(cacheKey, result.ToString(), options);
  
    return result;
  }

  ```

- Publish on Docker
  ```bash
  // DockerFile
  FROM microsoft/aspnetcore-build:2.0 AS build-env
  WORKDIR /app

  # Copy csproj and restore as distinct layers
  COPY *.csproj ./
  RUN dotnet restore

  # Copy everything else and build
  COPY . ./
  RUN dotnet publish -c Release -o out

  # Build runtime image
  FROM microsoft/aspnetcore:2.0
  WORKDIR /app
  COPY --from=build-env /app/out .
  ENTRYPOINT ["dotnet", "dotnet-webapi-cit.dll"]

  // Build and run Docker
  $ docker build -t dotnet-webapi-cit .
  $ docker run -d -p 8080:80 --name myapp dotnet-webapi-cit
  ```

## Requirements

- [Visual Studio Code](https://code.visualstudio.com/download)
- [.NET Core SDK](https://www.microsoft.com/net/download)
- [Postman](https://www.getpostman.com/)
- [Redis for macOS](https://medium.com/@petehouston/install-and-config-redis-on-mac-os-x-via-homebrew-eb8df9a4f298)
- [ Redis Desktop Manager](https://github.com/uglide/RedisDesktopManager/releases)
- [Docker](https://docs.docker.com/install)

## Extras

- Check .NET Core SDK Version
  ```bash
  $ dotnet --version
  ```

- Start Redis
  ```bash
  $ redis-server
  ```

- Interact with Redis
  ```bash
  $ redis-cli
  redis> set foo bar
  OK
  redis> get foo
  "bar"
  ```

- Check Docker Version
  ```bash
  $ docker --version
  ```

- Check Docker Hello World
  ```bash
  $ docker run hello-world
  ```

## Requirements

- [Visual Studio Code](https://code.visualstudio.com/download)
- [.NET Core SDK](https://www.microsoft.com/net/download)
- [Postman](https://www.getpostman.com/)
- [Redis for macOS](https://medium.com/@petehouston/install-and-config-redis-on-mac-os-x-via-homebrew-eb8df9a4f298)
- [Docker](https://docs.docker.com/install)

## References

- [Introdução ao ASP.NET Core MVC no Mac, Linux ou Windows](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-mvc-app-xplat/start-mvc)
- [API Web ASP.NET Core with Swagger](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger)
- [Working with a distributed cache (Redis) in ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/performance/caching/distributed#using-a-redis-distributed-cache)
- [Medium: ASP.NET Core 2.0: implementando cache em APIs REST](https://medium.com/@renato.groffe/asp-net-core-2-0-implementando-cache-em-apis-rest-cd2df219f13b)
- [Docker Docs: .NET Core Application](https://docs.docker.com/engine/examples/dotnetcore/#view-the-web-page-running-from-a-container)