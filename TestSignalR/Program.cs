using MessagePack;
using MessagePack.Formatters;
using MessagePack.Formatters.TestSignalR.Hubs;
using MessagePack.Resolvers;

using Microsoft.EntityFrameworkCore;

using TestSignalR.Data;
using TestSignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Generator tool
// https://www.nuget.org/packages/MessagePack.Generator

var formatter = new IMessagePackFormatter[]
{
    new AddPersonDtoFormatter()
};  

var resolver = CompositeResolver.Create(formatter, new[] { StandardResolver.Instance });
var resolverOptions = MessagePackSerializerOptions.Standard
    .WithResolver(resolver);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR()
    .AddMessagePackProtocol(options =>
    {
        options.SerializerOptions = resolverOptions;
    });
// Add sqlite dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<DataHub>("/data");

app.Run();