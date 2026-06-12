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

app.MapGet("/", () => Results.Ok(new { message = "API SkyMoon funcionando com sucesso!" }));

app.MapPost("/funcionario", (JsonElement body) =>
{
    try
    {
        var funcionario = new Funcionario
        {
            Id = new Random().Next(1000, 9999),
            Nome = body.GetProperty("nome").GetString(),
            Idade = body.GetProperty("idade").GetInt32(),
            Cargo = body.GetProperty("cargo").GetString(),
            Departamento = body.GetProperty("departamento").GetString(),
            Salario = body.GetProperty("salario").GetDouble()
        };

        funcionarios[totalFuncionarios] = funcionario;
        totalFuncionarios++;

        return Results.Created($"/funcionario/{funcionario.Id}", new { funcionario });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
});

app.MapGet("/funcionario", () =>
{
    var lista = funcionarios.Take(totalFuncionarios).ToArray();
    return Results.Ok(new { funcionarios = lista });
});

app.MapGet("/funcionario/busca", (string nome) =>
{
    var lista = funcionarios
        .Take(totalFuncionarios)
        .Where(f => f != null && f.Nome != null && f.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
        .ToArray();

    return lista.Length > 0
        ? Results.Ok(new { nome, funcionarios = lista })
        : Results.NotFound(new { message = "Nenhum funcionário encontrado com esse nome." });
});

app.MapGet("/funcionario/departamento/busca", (string departamento) =>
{
    var lista = funcionarios
        .Take(totalFuncionarios)
        .Where(f => f != null && f.Departamento != null && f.Departamento.Contains(departamento, StringComparison.OrdinalIgnoreCase))
        .ToArray();

    return lista.Length > 0
        ? Results.Ok(new { departamento, funcionarios = lista })
        : Results.NotFound(new { message = "Nenhum funcionário encontrado para esse departamento." });
});

app.MapPatch("/funcionario/{id}", (int id, JsonElement body) =>
{
    var funcionario = funcionarios.FirstOrDefault(f => f != null && f.Id == id);

    if (funcionario is null)
        return Results.NotFound(new { message = "Funcionário não encontrado." });

    try
    {
        if (body.TryGetProperty("salario", out var salario))
            funcionario.Salario = salario.GetDouble();

        return Results.Ok(new { funcionario });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
});

app.MapPut("/funcionario/{id}", (int id, JsonElement body) =>
{
    var funcionario = funcionarios.FirstOrDefault(f => f != null && f.Id == id);

    if (funcionario is null)
        return Results.NotFound(new { message = "Funcionário não encontrado." });

    try
    {
        funcionario.Nome = body.GetProperty("nome").GetString();
        funcionario.Idade = body.GetProperty("idade").GetInt32();
        funcionario.Cargo = body.GetProperty("cargo").GetString();
        funcionario.Departamento = body.GetProperty("departamento").GetString();
        funcionario.Salario = body.GetProperty("salario").GetDouble();

        return Results.Ok(new { funcionario });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
});

app.MapDelete("/funcionario/{id}", (int id) =>
{
    for (int i = 0; i < totalFuncionarios; i++)
    {
        if (funcionarios[i] != null && funcionarios[i].Id == id)
        {
            var removido = funcionarios[i];

            for (int j = i; j < totalFuncionarios - 1; j++)
            {
                funcionarios[j] = funcionarios[j + 1];
            }

            funcionarios[totalFuncionarios - 1] = null!;
            totalFuncionarios--;

            return Results.Ok(new { mensagem = "Funcionário removido com sucesso.", funcionario = removido });
        }
    }

    return Results.NotFound(new { message = "Funcionário não encontrado." });
});

app.Run();
