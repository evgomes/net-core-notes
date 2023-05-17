# Sticky Notes App

Sample full-stack .NET application to create and manage sticky notes. The application is divided into an ASP.NET Core Web API and a Blazor Web Assembly UI. 

This application shows how to use .NET Core to build software that is easy to maintain, combining a modern architecture, libraries, and tools that you will probably use in your everyday job.

# Technologies
The Web API is built using the following technologies:

* [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0)
* [MediatR](https://github.com/jbogard/MediatR) - Mediator pattern implementation to apply the CQRS pattern.
* [AutoMapper](https://automapper.org/) - object-object mapper, to map API resources.
* [Swashbuckle.AspNetCore ](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - Swagger documentation generator for ASP.NET Core applications.
* [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) - The most common ORM for .NET applications.
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) - Database to store the sticky notes.
* [xUnit](https://xunit.net/) - Testing library, to write unit tests.

The UI is built using:

* [Blazor Web Assembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-7.0) - Framework to build client-side apps with .NET.
* [Bootstrap 5](https://getbootstrap.com/) - Popular CSS framework to write front-end apps.
* [BlazorStrap](https://blazorstrap.io/V5/V5) - Library that implements Bootstrap components for Blazor apps.
* [BlazorTime](https://github.com/dustout/BlazorTime) - A time conversion library for Blazor that allows you to display dates and times in the browsers local time.

Additionaly, the application runs on [Docker](https://www.docker.com/) containers using a [docker-compose.yml](https://docs.docker.com/compose/) file.

# Architecture

The API uses CQRS to handle requests and responses. Due to the small scope of this application, there is a single database to save and query data. 

The API sends and receives API resources in its endpoints. Incoming resources are mapped to commands or queries, and there are handlers to save changes in the database or to return desired data.

There are 5 endpoints exposed to manage sticky notes:

* A `[POST] /api/notes` route to save new sticky notes in the database.
* A `[PATCH] /api/notes/{id}` route to update sticky notes.
* A `[DELETE] /api/notes/{id}` route to remove sticky notes.
* A `[GET] /api/notes` to query sticky notes.
* A `[GET] /api/notes/{id}` to retrieve sticky notes by their IDs.

The list route implements a sort and pagination pattern that I usually apply both on my personal applications and professional application. 
The pattern is very flexible, allowing API callers to specify a page and number of items to return in a request. It is also possible to use any valid model field to apply sorting. In the client-side app, I use the pattern to sort sticky notes by creation date.

The Blazor app does not use any state management library since the scope is very limited. I suggest you to check [Fluxor](https://github.com/mrpmorris/Fluxor) or other similar library if you need a state management library for a Blazor application.

I added some useful HTTP client extensions to call the API routes in client-side app. These extensions can be useful for you if you have any kind of .NET application that calls other APIs.

Note that I did not implement authentication and authorization for the API routes. You can check [my other repository](https://github.com/evgomes/jwt-api) to see how to use JSON Web Tokens to authenticate and authorize users in APIs. Even better, you could use an OAuth solution to handle authentication and authorization. Take a look at [Identity Server](https://identityserver4.readthedocs.io/en/latest/) to see how to do it.

# How to Run the Application

You need Docker and Docker Compose to run the application. Open the solution on Visual Studio and run the application using Docker Compose. 

Navigate to `https://localhost:5000/swagger` to see the Swagger documentation for the API routes.  Navigate to `https://localhost:5001/` to see the UI. You can add, edit, and remove sticky notes in this page. All data is saved in a SQL Server database running in a Docker container.