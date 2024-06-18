using Microsoft.AspNetCore.Mvc;
using OracleWebAPIService;
using SahFederacijaLibrary;
using SahFederacijaLibrary.DTOs;
using SahFederacijaLibrary.Entiteti;

namespace OracleWebAPIService.Controllers;

[ApiController]
[Route("[controller]")]
public class PartijaController : ControllerBase
{
    [HttpGet]
    [Route("PreuzmiSvePartije")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSvePartije()
    {
        var (isError, partije, error) = await DataProvider.VratiSvePartijeAsync();

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(partije);
    }

    [HttpPost("KreirajPartiju/{turnirId}/{crne}/{bele}/{sudija}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajPartiju([FromBody] PartijaView partija, int turnirID, int crne, int bele, int sudija)
    {
        var (isError, id, error) = await DataProvider.SacuvajPartijuAsync(partija, turnirID, crne, bele, sudija);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisana partija sa turnira, sa ID: {turnirID}");
    }

    [HttpPost("KreirajPartijuBezTurnira/{crne}/{bele}/{sudija}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajPartijuBezTurnira([FromBody] PartijaView partija, int crne, int bele, int sudija)
    {
        var (isError, id, error) = await DataProvider.SacuvajPartijuBezTurniraAsync(partija, crne, bele, sudija);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisana partija bez turnira, sa ID: {id}");
    }

    [HttpPut("IzmenaPartije/{crne}/{bele}/{sudija}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> IzmenaPartije([FromBody] PartijaView partija, int crne, int bele, int sudija)
    {
        var data = await DataProvider.IzmeniPartijuAsync(partija, crne, bele, sudija);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Izmenjena partija, sa ID: {partija.Id}");
    }

    [HttpDelete]
    [Route("IzbrisiPartiju/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletePartija(int id)
    {
        var data = await DataProvider.ObrisiPartijuAsync(id);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisana partija. ID: {id}");
    }

    [HttpGet("PreuzmiSvePartijeTurnira/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSvePartijeTurnira(int id)
    {
        var (isError, partije, error) = await DataProvider.VratiSvePartijeTurniraAsync(id);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(partije);
    }

    [HttpGet("PreuzmiPotezePartije/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSvePotezePartije(int id)
    {
        var (isError, potezi, error) = await DataProvider.VratiSvePotezePartijeAsync(id);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(potezi);
    }

    [HttpGet("PreuzmiPartijeSudije/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSvePartijeSudije(int id)
    {
        var (isError, partije, error) = await DataProvider.VratiSvePartijeSudijeAsync(id);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(partije);
    }

    [HttpPost("KreirajPotez/{partijaId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajPotez([FromBody] PotezView potez, int partijaId)
    {
        var (isError, id, error) = await DataProvider.SacuvajPotezAsync(potez, partijaId);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan potez za partiju {partijaId}, sa ID: {id}");
    }

    [HttpPut("IzmenaPoteza/{potezRbr}/{partijaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> IzmenaPartije([FromBody] PotezView potez, int potezRbr, int partijaId)
    {
        var data = await DataProvider.IzmeniPotezAsync(potez, potezRbr, partijaId);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Izmenjena potez partije sa ID: {partijaId}");
    }

    [HttpDelete]
    [Route("IzbrisiPotez/{potezRbr}/{partijaId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletePotez(int potezRbr, int partijaId)
    {
        var data = await DataProvider.ObrisiPotezAsync(potezRbr, partijaId);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan potez. ID partije: {partijaId}");
    }
}


