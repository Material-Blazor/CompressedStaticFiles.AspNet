using CompressedStaticFiles.Example.Data;
using CompressedStaticFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

/*
 * 
 * Dynanmic response compression service that compresses
 * any responses not already served by the pre compression middleware.
 * To use this, uncomment the app.UseResponseCompression() line below.
 * 
 */
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

/*
 * 
 * Add compressed static files service 
 * 
 */
builder.Services.AddCompressedStaticFiles();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

/*
 * 
 * Remove app.UseStaticFiles middleware and replace with app.UseCompressedStaticFiles. Uncomment
 * the app.UseResponseCompression() line below to compress any responses that are not precompressed static files.
 * 
 */
//app.UseResponseCompression();
app.UseCompressedStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
