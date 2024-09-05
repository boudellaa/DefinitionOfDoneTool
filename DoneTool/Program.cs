// <copyright file="Program.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // using directives outside namespace

using DoneTool.Data;
using DoneTool.Mappings;
using DoneTool.Repositories.Interfaces;
using DoneTool.Repositories.SQL;
using DoneTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Skyline.DataMiner.Utils.JsonOps.Services;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();