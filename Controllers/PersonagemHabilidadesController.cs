using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Data;
using System.Threading.Tasks;
using RpgApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagemHabilidadesController : ControllerBase
    {
        private readonly DataContext _context;

        public PersonagemHabilidadesController(DataContext context)
        {
            _context = context;
        }

    [HttpPost]
    public async Task<IActionResult> AddPersonagemHabilidadeAsync(PersonagemHabilidade novoPersonagemHabilidade)
    {
        try
        {
            Personagem personagem = await _context.TB_PERSONAGENS
                .Include(p => p.Arma)
                .Include(p => p.PersonagemHabilidades).ThenInclude(ps => ps.Habilidade)
                .FirstOrDefaultAsync(p => p.Id == novoPersonagemHabilidade.PersonagemId);

            if (personagem == null)
                throw new SystemException("Personagem não encontrado para o Id informado.");
            
            Habilidade habilidade = await _context.TB_HABILIDADES
                .FirstOrDefaultAsync(h => h.Id == novoPersonagemHabilidade.HabilidadeId);
            if (habilidade == null)
            throw new SystemException("Habilidade não encontrada.");

            PersonagemHabilidade ph = new PersonagemHabilidade();    
            ph.Personagem = personagem;
            ph.Habilidade = habilidade;
            await _context.TB_PERSONAGENS_HABILIDADES.AddAsync(ph);
            int linhasAfetadas = await _context.SaveChangesAsync();

            return Ok(linhasAfetadas);
        }
        catch (System.Exception ex)
        {
            
            return BadRequest(ex.Message);
        }
    }





    }
}