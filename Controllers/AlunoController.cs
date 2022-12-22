using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetoapi.Data;
using projetoapi.Models;

namespace projetoapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : Controller
    {
        private readonly PrimaryDbContext _context;

        public AlunoController(PrimaryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlunos(){
            var alunos = _context.Alunos;
            if(alunos != null){
                return Ok(await alunos.ToListAsync());
            }
            return BadRequest("Não foi encontrado resgistros de alunos.");
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetById(int? id){
            var aluno = new Aluno();
            if(id == null){
                return BadRequest("Não foi possível buscar aluno. O id informado é nulo.");
            }
            var alunos = _context.Alunos;
            if(alunos != null){
                aluno = await alunos.Where(a => a.Id == id).FirstOrDefaultAsync();
            }
            if(aluno == null){
                return BadRequest("Aluno não encontrado!");
            }
            return Ok(aluno);
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarAluno(Aluno aluno){
            aluno.Id = null;
            if(aluno == null) return BadRequest();
            string nomeAluno = aluno.Nome ?? "";
            if(nomeAluno.Length < 3) return BadRequest("O nome precisa conter pelo menos 3 letras");
            var alunos = _context.Alunos;
            if(alunos != null){
                await alunos.AddAsync(aluno);
                await _context.SaveChangesAsync();
                return Ok("Aluno " + aluno.Nome + " registrado com sucesso!");
            }
            return BadRequest("Não foi possível registrar o aluno!");
        }

        [HttpDelete("Deletar/{id}")]
        public async Task<IActionResult> Deletar(int? id){
            if(id == null)
                return BadRequest("Nenhum aluno selecionado para deletar!");
            var alunos = _context.Alunos;
            if(alunos == null)
                return BadRequest("Nenhum aluno encontrado para deletar!");
            var aluno = await alunos.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(aluno == null)
                return BadRequest("Aluno não encontrado!");
            alunos.Remove(aluno);
            await _context.SaveChangesAsync();
            return Ok("Aluno deletado com sucesso!");
        }

        [HttpPut("Alterar/{id}")]
        public async Task<IActionResult> Alterar(int? id, [FromBody] Aluno? aluno){
            if(id == null || aluno == null) return BadRequest("Parametros não podem ser nulos!");
            var alunos = _context.Alunos;
            if(alunos == null) return BadRequest();
            var alunoContext = await alunos.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(alunoContext == null) return BadRequest("Aluno não encontrado para alterar.");
            if(id != aluno.Id) return BadRequest("Id não pode ser diferente do Id do aluno");
            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Alterado com sucesso!");
        }

    }
}