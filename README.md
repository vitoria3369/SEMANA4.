# SEMANA4.
# Código do arquivo Program.cs
using skymoon.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

app.UseCors("AllowAll");

Funcionario[] funcionarios = new Funcionario[100];
int totalFuncionarios = 0;

app.MapGet("/", () =>
{
    return Results.Ok("API SkyMoon funcionando com sucesso!");
});

app.MapPost("/funcionario", (JsonElement body) =>
{
    Random random = new();

    Funcionario novo_funcionario = new Funcionario();

    novo_funcionario.Id = random.Next(1000, 9999);
    novo_funcionario.Nome = body.GetProperty("nome").GetString();
    novo_funcionario.Idade = body.GetProperty("idade").GetInt32();
    novo_funcionario.Cargo = body.GetProperty("cargo").GetString();
    novo_funcionario.Departamento = body.GetProperty("departamento").GetString();
    novo_funcionario.Salario = body.GetProperty("salario").GetDouble();

    funcionarios[totalFuncionarios] = novo_funcionario;
    totalFuncionarios++;

    return Results.Ok(new
    
        novo_funcionario

{
    
});

app.MapGet("/funcionario", () =>
{
    
});

app.MapPatch("/funcionario/{id}", (int id, JsonElement body) =>
{
    
});

app.MapPut("/funcionario/{id}", (int id, JsonElement body) =>
{   
    
});

app.MapDelete("/funcionario", (int id) =>
{
    
});

app.MapGet("/funcionario/departamento/busca", (string departamento) =>
{
    
});

app.MapGet("/funcionario/busca", (string nome) =>
{
   
}); */

app.Run();