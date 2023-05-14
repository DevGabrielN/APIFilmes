using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private readonly FilmeContext _filmeContext;
    private IMapper _mapper;

    public FilmeController(FilmeContext filmeContext, IMapper mapper)
    {
        _mapper = mapper;
        _filmeContext = filmeContext;
    }
    #region summary
    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="400">Requisição fora do padrão</response>
    #endregion
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _filmeContext.Filmes.Add(filme);
        _filmeContext.SaveChanges();
        return CreatedAtAction(nameof(RecuperarFilmePorId), new { id = filme.Id }, filme);
    }
    #region summary
    /// <summary>
    /// Recupera uma lista de filmes
    /// </summary>    
    /// <returns>IActionResult</returns>
    /// <response code="200">Requisição concluída com sucesso</response>
    #endregion
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult RecuperarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return Ok(_mapper.Map<List<ReadFilmeDto>>(_filmeContext.Filmes.Skip(skip).Take(take)));
    }
    #region summary
    /// <summary>
    /// Recupera filme por Id
    /// </summary>
    /// <param name="id">Id do filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Requisição concluída com sucesso</response>
    /// <response code="404">Não encontrado</response>
    #endregion
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult RecuperarFilmePorId(int id)
    {
        var filme = _filmeContext.Filmes.Where(x => x.Id == id).FirstOrDefault();
        if (filme == null)
        {
            return NotFound();
        }
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }
    #region summary
    /// <summary>
    /// Atualiza filme por Id
    /// </summary>
    /// <param name="id">Id do filme</param>    
    /// <returns>IActionResult</returns>
    /// <response code="204">No content (sucesso)</response>
    /// <response code="404">Não encontrado</response>
    #endregion
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _filmeContext.Filmes.FirstOrDefault(x => x.Id == id);
        if (filme == null)
        {
            return NotFound();
        }
        _mapper.Map(filmeDto, filme);
        _filmeContext.SaveChanges();
        return NoContent();
    }
    #region summary
    /// <summary>
    /// Atualiza filme parcialement por Id
    /// </summary>
    /// <param name="id">Id do filme</param>
    /// <param name="patch">Json patch</param>  
    /// <returns>IActionResult</returns>
    /// <response code="204">No content (sucesso)</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="400">Requisição fora do padrão</response>
    #endregion
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _filmeContext.Filmes.FirstOrDefault(x => x.Id == id);
        if (filme == null)
        {
            return NotFound();
        }

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);
        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(filmeParaAtualizar, filme);
        _filmeContext.SaveChanges();
        return NoContent();
    }

    #region summary
    /// <summary>
    /// Deleta filme por Id
    /// </summary>
    /// <param name="id">Id do filme</param>    
    /// <returns>IActionResult</returns>
    /// <response code="204">No content (sucesso)</response>
    /// <response code="404">Não encontrado</response>    
    #endregion
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _filmeContext.Filmes.FirstOrDefault(x => x.Id == id);
        if (filme == null)
        {
            return NotFound();
        }        
        return NoContent();
    }
}