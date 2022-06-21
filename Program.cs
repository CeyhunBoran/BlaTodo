using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Todo.Data;
using Todo.Models;
using Microsoft.EntityFrameworkCore;
using Todo.Services;
using Microsoft.AspNetCore.Identity;
using Todo.Areas.Identity;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<TodoUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TodoDbContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<TodoDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRazorPages();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<TodoUser>>();
builder.Services.AddScoped<TodoService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseAuthentication();
app.UseAuthorization();

app.Run();
