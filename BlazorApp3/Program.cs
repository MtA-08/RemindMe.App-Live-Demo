using BlazorApp3;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<TaskStorageService>();
builder.Services.AddLocalization();

var host = builder.Build();

var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
var savedLanguage = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "selected_language");

CultureInfo culture;
if (!string.IsNullOrEmpty(savedLanguage))
{
    culture = new CultureInfo(savedLanguage);
}
else
{
    // Fallback to browser system language
    culture = CultureInfo.CurrentCulture;
}

// 3. Apply the culture globally to the application execution threads
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await builder.Build().RunAsync();
