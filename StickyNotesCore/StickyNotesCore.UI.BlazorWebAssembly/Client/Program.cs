using BlazorStrap;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StickyNotesCore.UI.BlazorWebAssembly.Client.Clients.Notes;
using StickyNotesCore.UI.BlazorWebAssembly.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazorStrap();
builder.Services.AddScoped<INotesApiClient, NotesApiClient>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000/") });

await builder.Build().RunAsync();