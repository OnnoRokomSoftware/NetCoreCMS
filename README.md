# NetCoreCMS
NetCoreCMS is a modular theme supported Content Management System developed using ASP.Net Core 1.1. Which is also usable as web application framework.   

## Alpha

The software is complete enough for internal testing. This is typically done by people other than the software engineers who wrote it, but still within the same organization or community that developed the software.

Here is a more detailed [roadmap](https://github.com/xonaki/NetCoreCMS/wiki/Roadmap).

## Getting Started

- Clone the repository using the command `git clone https://github.com/xonaki/NetCoreCMS.git` and checkout the `master` branch. 

### Command line

- Install the latest versions (current) for both Runtime and SDK of .NET Core from this page https://www.microsoft.com/net/download/core
- Call `dotnet restore`.
- Call `dotnet build`.
- Next navigate to `D:\NetCoreCMS\NetCoreCMS.Web` or wherever your respective folder is on the command line in Administrator mode.
- Call `dotnet run`.
- Then open the `http://localhost:5000` URL in your browser.

### Visual Studio 2017

- Download Visual Studio 2017 (any edition) from https://www.visualstudio.com/downloads/
- Open `NetCoreCMS.sln`
- Ensure `NetCoreCMS.Web` is the startup project and run it
