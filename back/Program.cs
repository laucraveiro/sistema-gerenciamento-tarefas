using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_tarefas.Data;
using sistema_gerenciamento_tarefas.Data.Repositories.Tarefas;
using sistema_gerenciamento_tarefas.Data.Repositories.Usuarias;
using sistema_gerenciamento_tarefas.Services.Tarefas;
using sistema_gerenciamento_tarefas.Services.Usuarias;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("ConexaoPadrao")
    ));

builder.Services.AddScoped<IUsuariaRepository, UsuariaRepository>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//**********BANCO EM MEMORIA**********************
//builder.Services.AddSingleton<IUsuariaRepository, UsuariaRepositoryInMemory>();
//**********BANCO EM MEMORIA**********************

builder.Services.AddScoped<IUsuariaService, UsuariaService>();
builder.Services.AddScoped<ITarefaService, TarefaService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapControllers();

app.Run();