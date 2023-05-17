# Sticky Notes App

Sample full-stack .NET application to create and manage sticky notes. The application is divided into an ASP.NET Core Web API and a Blazor Web Assembly UI.

This application demonstrates the usage of .NET Core to build maintainable software, combining modern architecture, libraries, and tools commonly used in everyday development.

# Technologies

The Web API is built using the following technologies:

* [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0)
* [MediatR](https://github.com/jbogard/MediatR) - Mediator pattern implementation for applying the CQRS pattern.
* [AutoMapper](https://automapper.org/) - Object-object mapper for mapping API resources.
* [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - Swagger documentation generator for ASP.NET Core applications.
* [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) - The most commonly used ORM for .NET applications.
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) - Database used to store the sticky notes.
* [xUnit](https://xunit.net/) - Testing library for writing unit tests.

The UI is built using:

* [Blazor Web Assembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-7.0) - Framework for building client-side apps with .NET.
* [Bootstrap 5](https://getbootstrap.com/) - Popular CSS framework for front-end app development.
* [BlazorStrap](https://blazorstrap.io/V5/V5) - Library that implements Bootstrap components for Blazor apps.
* [BlazorTime](https://github.com/dustout/BlazorTime) - A time conversion library for Blazor that enables displaying dates and times in the browser's local time.

Additionally, the application runs on [Docker](https://www.docker.com/) containers using a [docker-compose.yml](https://docs.docker.com/compose/) file.

# Architecture

The API uses CQRS to handle requests and responses. Due to the small scope of this application, there is a single database for saving and querying data.

The API sends and receives API resources in its endpoints. Incoming resources are mapped to commands or queries, and handlers are available to save changes in the database or return desired data.

There are 5 endpoints exposed to manage sticky notes:

* A `[POST] /api/notes` route to save new sticky notes in the database.
* A `[PATCH] /api/notes/{id}` route to update sticky notes.
* A `[DELETE] /api/notes/{id}` route to remove sticky notes.
* A `[GET] /api/notes` route to query sticky notes.
* A `[GET] /api/notes/{id}` route to retrieve sticky notes by their IDs.

The list route implements a sorting and pagination pattern that I usually apply in both my personal and professional applications. This pattern is highly flexible, allowing API callers to specify a page and number of items to return in a request. It is also possible to use any valid model field for sorting. In the client-side app, I use the pattern to sort sticky notes by creation date.

The Blazor app does not use any state management library since the scope is very limited. If you require a state management library for a Blazor application, I suggest checking out [Fluxor](https://github.com/mrpmorris/Fluxor) or similar alternatives.

I have added some useful HTTP client extensions to call the API routes in client-side apps. These extensions can be beneficial if you have any kind of .NET application that interacts with other APIs.

Please note that authentication and authorization for the API routes have not been implemented in this application. You can refer to [my other repository](https://github.com/evgomes/jwt-api) to learn how to use JSON Web Tokens for authentication and authorization in APIs. Even better, you could consider using an OAuth solution to handle authentication and authorization. Take a look at [Identity Server](https://identityserver4.readthedocs.io/en/latest/) for more information.

# How to Run the Application

To run the application, you need Docker and Docker Compose installed. Open the solution in Visual Studio and run the application using Docker Compose.

Navigate to `https://localhost:5000/swagger` to view the Swagger documentation for the API routes. Access `https://localhost:5001/` to see the UI, where you can add, edit, and remove sticky notes. All data is saved in a SQL Server database running in a Docker container.

![Swagger View](https://raw.githubusercontent.com/evgomes/net-core-notes/main/images/swagger-view.png)

![UI View](https://raw.githubusercontent.com/evgomes/net-core-notes/main/images/ui-view.png)