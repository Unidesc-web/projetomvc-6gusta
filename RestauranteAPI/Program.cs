var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

var tarefas = new List<Tarefa>();
var proximoId = 1;

app.MapGet("/tarefas", () => tarefas);

app.MapGet("/tarefas/{id}", (int id) =>
{
    var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
    return tarefa is null ? Results.NotFound() : Results.Ok(tarefa);
});

app.MapPost("/tarefas", (Tarefa nova) =>
{
    nova.Id = proximoId++;
    tarefas.Add(nova);
    return Results.Created($"/tarefas/{nova.Id}", nova);
});

app.MapPut("/tarefas/{id}", (int id, Tarefa atualizada) =>
{
    var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
    if (tarefa is null) return Results.NotFound();

    tarefa.Titulo = atualizada.Titulo;
    tarefa.Concluida = atualizada.Concluida;
    return Results.NoContent();
});

app.MapDelete("/tarefas/{id}", (int id) =>
{
    var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
    if (tarefa is null) return Results.NotFound();

    tarefas.Remove(tarefa);
    return Results.NoContent();
});

app.Run();

record Tarefa
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public bool Concluida { get; set; }
}
