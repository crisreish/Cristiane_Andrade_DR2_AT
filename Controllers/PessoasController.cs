using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aniversarios.Models;
using System.Web;
using System.Web.Mvc;

namespace Aniversarios.Controllers
{
    public class PessoasController : Controller
    {
        private readonly AniversariosContext _context;

        public PessoasController(AniversariosContext context)
        {
            _context = context;
        }

        // GET: Pessoas
        public async Task<IActionResult> Index(string nomePesquisa)
        {
            var pessoas = await _context.Pessoa.ToListAsync();

            if (!string.IsNullOrEmpty(nomePesquisa))
            {
                pessoas = pessoas.Where(pessoa => pessoa.NomeCompletoPossui(nomePesquisa)).ToList();
            }
            pessoas.Sort(delegate (Pessoa x, Pessoa y)
            {
                return x.DiasFaltantes.CompareTo(y.DiasFaltantes);
            });

            return View(pessoas);
        }

        // GET: Pessoas/Details/5
        public async Task<IActionResult> Exibir(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.PessoaId == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // GET: Pessoas/Create
        public IActionResult Incluir()
        {
            return View();
        }

        // POST: Pessoas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Incluir([Bind("PessoaId,Nome,Sobrenome,DataNascimento")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pessoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Edit/5
        public async Task<IActionResult> Alterar(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alterar(int id, [Bind("PessoaId,Nome,Sobrenome,DataNascimento")] Pessoa pessoa)
        {
            if (id != pessoa.PessoaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pessoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExistePessoa(pessoa.PessoaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Delete/5
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.PessoaId == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExcluir(int id)
        {
            if (_context.Pessoa == null)
            {
                return Problem("Entity set 'Aniversarios.Pessoa'  is null.");
            }
            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa != null)
            {
                _context.Pessoa.Remove(pessoa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExistePessoa(int id)
        {
            return _context.Pessoa.Any(e => e.PessoaId == id);
        }
    }
}