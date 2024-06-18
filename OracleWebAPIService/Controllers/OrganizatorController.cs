using Microsoft.AspNetCore.Mvc;
using OracleWebAPIService;
using SahFederacijaLibrary;
using SahFederacijaLibrary.DTOs;

namespace OracleWebAPIService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrganizatorController : ControllerBase
{
    [HttpGet]
    [Route("PreuzmiSveOrganizatore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSveOrganizatore()
    {
        var (isError, organizatori, error) = await DataProvider.VratiSveOrganizatoreAsync();

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(organizatori);
    }

    [HttpPost("KreirajOrganizatora")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajOrganizatora([FromBody] OrganizatorView organizator)
    {
        var (isError, id, error) = await DataProvider.SacuvajOrganizatoraAsync(organizator);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan organizator, sa ID: {id}");
    }


    [HttpPost("KreirajOrganizatoraSudiju/{sudijaID}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajOrganizatoraSudiju([FromBody] OrganizatorView organizator, int sudijaID)
    {
        var (isError, id, error) = await DataProvider.SacuvajOrganizatoraSudijuAsync(organizator, sudijaID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan organizator sudija, sa ID: {id}");
    }

    [HttpPut("IzmenaOrganizatora")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> IzmenaOrganizatora([FromBody] OrganizatorView organizator)
    {
        var data = await DataProvider.IzmeniOrganizatoraAsync(organizator);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Izmenjen organizator, sa ID: {organizator.Id}");
    }

    [HttpDelete]
    [Route("IzbrisiOrganizatora/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteOrganizatora(int id)
    {
        var data = await DataProvider.ObrisiOrganizatoraAsync(id);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan organizator. ID: {id}");
    }

    [HttpDelete]
    [Route("IzbrisiOrganizuje/{idOrganizator}/{idTurnir}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteOrganizuje(int idOrganizator, int idTurnir)
    {
        var data = await DataProvider.ObrisiOrganizujeAsync(idOrganizator, idTurnir);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan odnos organizuje.");
    }

    [HttpPost]
    [Route("PoveziOrganizatoriTurnir/{organizatorID}/{turnirID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> LinkOrganizatorTurnir(int organizatorID, int turnirID)
    {
        (bool isError1, var organizator, var error1) = await DataProvider.VratiOrganizatoraAsync(organizatorID);
        (bool isError2, var turnir, var error2) = await DataProvider.VratiTurnirAsync(turnirID);

        if (isError1 || isError2)
        {
            return StatusCode(error1?.StatusCode ?? 400, $"{error1?.Message}{Environment.NewLine}{error2?.Message}");
        }

        if (organizator == null || turnir == null)
        {
            return BadRequest("Organizator ili turnir nisu validni.");
        }

        await DataProvider.DodajOrganizujeAsync(new OrganizujeView
        {
            Id = new OrganizujeIdView
            {
                OrganizatorOrganizuje = organizator,
                OrganizujeSahovski_Turnir = turnir
            }
        });

        return Ok($"Dodat odnos izmedju organizatora i turnira. Organizator: {organizator.Lime} {organizator.Prezime}. Turnir: {turnir.Naziv}");
    }

    [HttpGet]
    [Route("PreuzmiOrganizatoreTurnira/{turnirID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult VratiOrganizatoreTurnira(int turnirID)
    {
        (bool isError, var organizatori, var error) = DataProvider.VratiOrganizatoreTurnira(turnirID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(organizatori);
    }
}