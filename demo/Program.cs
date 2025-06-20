using Microsoft.AspNetCore.Builder;
using NLWebNet.Extensions;
using NLWebNet.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add NLWebNet services
builder.Services.AddNLWebNet(options =>
{
    // Configure NLWebNet options here
    options.DefaultMode = NLWebNet.Models.QueryMode.List;
    options.EnableStreaming = true;
});

// Add OpenAPI for API documentation
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add NLWebNet middleware
app.UseNLWebNet();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<NLWebNet.Demo.Components.App>()
    .AddInteractiveServerRenderMode();

// Map NLWebNet minimal API endpoints
app.MapNLWebNet();

app.Run();
