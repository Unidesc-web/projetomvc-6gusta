using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using projetomvc_6gusta.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace projetomvc_6gusta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Lista alunos e permite busca simples por nome
        public async Task<IActionResult> Index(string busca)
        {
            var alunos = from a in _context.Alunos select a;

            if (!string.IsNullOrEmpty(busca))
            {
                alunos = alunos.Where(a => a.Nome != null && a.Nome.ToLower().Contains(busca.ToLower()));
            }

            return View(await alunos.ToListAsync());
        }

        // Exibir formulário para cadastrar novo aluno
        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        // Receber dados do formulário e salvar novo aluno
        [HttpPost]
        public async Task<IActionResult> Criar(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aluno);
        }

        // Exibir formulário para editar aluno
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        // Receber dados do formulário e atualizar aluno
        [HttpPost]
        public async Task<IActionResult> Editar(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index");
            }
            return View(aluno);
        }

        // Confirmar exclusão
        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        // Excluir aluno
        [HttpPost, ActionName("Excluir")]
        public async Task<IActionResult> ExcluirConfirmado(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Página de privacidade
        public IActionResult Privacy()
        {
            return View();
        }

        // Página de erro padrão
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorModel);
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.Id == id);
        }
    }
}
