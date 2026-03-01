using LoanTracker.WasmApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7176/") });

builder.Services.AddScoped<LoanTracker.WasmApp.Services.IndexedDbService>();
builder.Services.AddScoped<LoanTracker.WasmApp.Services.DataSeederService>();

await builder.Build().RunAsync();
