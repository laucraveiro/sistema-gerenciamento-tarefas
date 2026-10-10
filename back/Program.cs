using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_tarefas.Data.Repositories.Usuarias;
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
builder.Services.AddSingleton<IUsuariaRepository, UsuariaRepositoryInMemory>();
//**********BANCO EM MEMORIA**********************

builder.Services.AddScoped<IUsuariaService, UsuariaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
