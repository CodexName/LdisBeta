using LdisDirty.DataBaseContext;
using LdisDirty.Services;
using LdisDirty.Services.GoogleOAuthServices;
using LdisDirty.Services.RealizationServices;
using LdisDirty.SignalREngine;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddDbContext<DbContextApplication>();
builder.Services.AddSignalR();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddTransient<IS256CoderService,S256Realize>();
builder.Services.AddTransient<IGetAuthServerUrlService, GetUrlAuthServer>();
builder.Services.AddTransient<IRequestSendService, SendRequest>();
builder.Services.AddTransient<IGetUserDataService, GetUserDataRealize>();
builder.Services.AddTransient<IChatHandlerService, ChatHandlerRealize>();
builder.Services.AddHttpContextAccessor();
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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapHub<ChatsHandler>("/chats");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();