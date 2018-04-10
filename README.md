## Tutorial Steps

- 



## Requirements

- [Visual Studio Code](https://code.visualstudio.com/download)
- [.NET Core SDK](https://www.microsoft.com/net/download)
- [Redis for macOS](https://medium.com/@petehouston/install-and-config-redis-on-mac-os-x-via-homebrew-eb8df9a4f298)
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