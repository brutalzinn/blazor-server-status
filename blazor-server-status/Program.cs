using Blazor.Status.Backend.Hubs;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Discord;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.InjectSeriLog();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.InjectConfiguration();
builder.Services.InjectCacheStorage();
builder.Services.InjectServices();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});


var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseResponseCompression();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
});
if (!app.Environment.IsDevelopment())
{
    //  app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapHub<ServerHub>("/serverhub");
app.MapFallbackToPage("/_Host");
app.Run();
