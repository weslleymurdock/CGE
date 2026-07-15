var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IBrowserStorageService, BrowserStorageService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<ILocalizer, Localizer>();
var app = builder.Build();
var sp = app.Services.CreateScope();
var bs = sp.ServiceProvider.GetRequiredService<IBrowserStorageService>();
await bs.InitializeAsync();
var userCulture = await bs.GetCurrentLanguageAsync();

CultureInfo ci = new(userCulture);
ci.SetCurrentCulture(userCulture);

await app.RunAsync();