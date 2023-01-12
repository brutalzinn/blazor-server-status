using blazor_server_status.Hubs;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseHsts();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapHub<ServerHub>("/serverhub");
app.MapFallbackToPage("/_Host");
app.Run();
