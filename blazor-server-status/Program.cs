using blazor_server_status;
using blazor_server_status.Data;
using blazor_server_status.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InjectConfiguration();
builder.Services.InjectCacheStorage();
builder.Services.InjectServices();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapHub<ServerHub>("/serverhub");

app.MapFallbackToPage("/_Host");

app.Run();
