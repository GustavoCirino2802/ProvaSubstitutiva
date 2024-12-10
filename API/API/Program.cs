using Microsoft.AspNetCore.Mvc;
using API.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "Prova Substitutiva");

app.MapPost("/api/imc/cadastrar", ([FromBody] Imc imc, [FromServices] AppDataContext ctx)=>{

    Aluno? aluno = 
        ctx.Alunos.Find(imc.AlunoId);
        if (aluno == null){
            Results.NotFound();
        }

        

        imc.ResultadoImc = imc.Peso/(imc.Altura * imc.Altura);

        if (imc.ResultadoImc < 18.5){
            imc.Classificacao = "Magreza";
        }
        if (imc.ResultadoImc >= 18.5 && imc.ResultadoImc <= 24.9){
            imc.Classificacao = "Normal";
        }
        if (imc.ResultadoImc >= 25.0 && imc.ResultadoImc <= 29.9){
            imc.Classificacao = "Sobrepeso";
        }
        if (imc.ResultadoImc >= 30.0 && imc.ResultadoImc <= 39.9){
            imc.Classificacao = "Obesidade";
        }
        if (imc.ResultadoImc > 40.0){
            imc.Classificacao = "Magreza";
        }

        ctx.Imcs.Add(imc);
        ctx.SaveChanges();
        return Results.Created($"/imc/{imc.AlunoId}", imc);
        
    
});

app.MapGet("/api/imc/listar", ([FromServices] AppDataContext ctx) =>{

    if (ctx.Imcs.Any() )
    {
        return Results.Ok(ctx.Imcs.ToList());
        
    }
    return Results.NotFound();

});

app.MapGet("/api/imc/listarporaluno/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
 {
    Imc? imc = ctx.Imcs.Find(id);
    if (imc is null){
        return Results.NotFound();
    }
    return Results.Ok(imc);
 });

 app.MapPut("/api/imc/alterar/{id}", ([FromRoute] int id, [FromServices] [FromBody] Imc imcAlterado, AppDataContext ctx) =>{

    Imc? imc = ctx.Imcs.Find(id);
    if(imc is null){
        return Results.NotFound();
    }

    imc.Altura = imcAlterado.Altura;
    imc.Peso = imcAlterado.Peso;

    ctx.Imcs.Update(imc);
    ctx.SaveChanges();
    return Results.Ok(imc);

 });



//--------------------------------------------------------------------------------------------------------

app.MapPost("/api/aluno/cadastrar", ([FromBody] Aluno aluno, [FromServices] AppDataContext ctx) =>{

    ctx.Alunos.Add(aluno);
    ctx.SaveChanges();
    return Results.Created("", aluno);
});

app.MapGet("/api/aluno/listar", ([FromServices] AppDataContext ctx) =>{

    if (ctx.Alunos.Any())
    {
        return Results.Ok(ctx.Alunos.ToList());
    }
    return Results.NotFound();

});

app.MapGet("/api/aluno/buscar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
 {
    Aluno? aluno = ctx.Alunos.Find(id);
    if (aluno is null){
        return Results.NotFound();
    }
    return Results.Ok(aluno);
 });

app.Run();
