// <copyright file="Program.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // using directives outside namespace

using DoneTool.Data;
using DoneTool.Mappings;
using DoneTool.Models.SkylineApiModels;
using DoneTool.Repositories.Interfaces;
using DoneTool.Repositories.SQL;
using DoneTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Skyline.DataMiner.Utils.JsonOps.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true);

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddDbContext<DoneToolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DoneToolConnection")));

builder.Services.AddScoped<IChecksRepository, SQLChecksRepository>();
builder.Services.AddScoped<ITaskChecklistRepository, SQLTaskChecklistRepository>();
builder.Services.AddScoped<ITaskChecksRepository, SQLTaskChecksRepository>();

builder.Services.AddScoped<JsonTaskService>();
builder.Services.AddScoped<TaskService>();

builder.Services.Configure<SkylineApiData>(builder.Configuration.GetSection("SkylineApi"));

builder.Services.AddHttpClient<SkylineApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.skyline.be/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
