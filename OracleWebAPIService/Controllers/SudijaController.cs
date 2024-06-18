using Microsoft.AspNetCore.Mvc;
using OracleWebAPIService;
using SahFederacijaLibrary;
using SahFederacijaLibrary.DTOs;

namespace OracleWebAPIService.Controllers;

[ApiController]
[Route("[controller]")]
public class SudijaController : ControllerBase
{
    [HttpGet]
    [Route("PreuzmiSveSudije")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSveSudije()
    {
        var (isError, sudije, error) = await DataProvider.VratiSveSudijeAsync();

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(sudije);
    }

    [HttpPost("KreirajSudiju")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajSudiju()
    {
        var (isError, id, error) = await DataProvider.SacuvajSudijuAsync();

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan sudija, sa ID: {id}");
    }

    [HttpDelete]
    [Route("IzbrisiSudiju/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteSudija(int id)
    {
        var data = await DataProvider.ObrisiSudijuAsync(id);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan sudija. ID: {id}");
    }
}
