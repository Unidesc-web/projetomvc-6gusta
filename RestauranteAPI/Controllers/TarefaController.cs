using Microsoft.AspNetCore.Mvc;
using ProjetoMVC.Models;

namespace ProjetoMVC.Controllers
{
    [ApiController]
    [Route("tarefa")]
    public class TarefaController : ControllerBase
    {
        private static List<Tarefa> tarefas = new List<Tarefa>();
        private static int proximoId = 1;

        [HttpGet]
        public IActionResult Listar() => Ok(tarefas);

        [HttpGet("{id}")]
        public IActionResult Buscar(int id)
        {
            var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
            return tarefa == null ? NotFound() : Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Tarefa nova)
        {
            nova.Id = proximoId++;
            tarefas.Add(nova);
            return CreatedAtAction(nameof(Buscar), new { id = nova.Id }, nova);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] Tarefa atualizada)
        {
            var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
            if (tarefa == null) return NotFound();

            tarefa.Nome = atualizada.Nome;
            tarefa.Concluida = atualizada.Concluida;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
            if (tarefa == null) return NotFound();

            tarefas.Remove(tarefa);
            return NoContent();
        }
    }
}
