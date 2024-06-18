using Microsoft.AspNetCore.Mvc;
using OracleWebAPIService;
using SahFederacijaLibrary;
using SahFederacijaLibrary.DTOs;

namespace OracleWebAPIService.Controllers;

[ApiController]
[Route("[controller]")]
public class SahovskiTurnirController : ControllerBase
{
    [HttpGet]
    [Route("PreuzmiSveTurnire")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSveTurnire()
    {
        var (isError, turniri, error) = await DataProvider.VratiSveTurnireAsync();

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(turniri);
    }

    [HttpPost("KreirajTurnir/{tipTurnira}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajTurnir([FromBody] Sahovski_TurnirView turnir, TipTurnira tipTurnira)
    {
        var (isError, success, error) = await DataProvider.SacuvajTurnirAsync(turnir, tipTurnira.ToString());

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        if (success)
        {
            return StatusCode(201, $"Upisan turnir {tipTurnira}, sa ID: {turnir.Id}");
        }
        else
        {
            return BadRequest("Neuspešan upis turnira.");
        }
    }

    [HttpPost]
    [Route("KreirajTakmicarskiNormalniTurnir")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddTakmicarskiNormalniTurnir([FromBody] TakmicarskiNormalniView sah)
    {
        var data = await DataProvider.SacuvajTakmicarskiNormalniAsync(sah);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(201, $"Uspešno dodat takmicarski normalni turnir. Naziv: {sah.Naziv}");
    }

    [HttpPost]
    [Route("KreirajTakmicarskiBrzopotezniTurnir")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddTakmicarskiBrzopotezniTurnir([FromBody] TakmicarskiBrzopotezniView sah)
    {
        var data = await DataProvider.SacuvajTakmicarskiBrzopotezniAsync(sah);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(201, $"Uspešno dodat takmicarski brzopotezni turnir. Naziv: {sah.Naziv}");
    }

    [HttpPost]
    [Route("KreirajEgzibicioniNormalniTurnir")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddEgzibicioniNormalniTurnir([FromBody] EgzibicioniNormalniView sah)
    {
        var data = await DataProvider.SacuvajEgzibicioniNormalniAsync(sah);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(201, $"Uspešno dodat egzibicioni normalni turnir. Naziv: {sah.Naziv}");
    }

    [HttpPost]
    [Route("KreirajEgzibicioniBrzopotezniTurnir")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddEgzibicioniBrzopotezniTurnir([FromBody] EgzibicioniBrzopotezniView sah)
    {
        var data = await DataProvider.SacuvajEgzibicioniBrzopotezniAsync(sah);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(201, $"Uspešno dodat egzibicioni brzopotezni turnir. Naziv: {sah.Naziv}");
    }

    [HttpPut]
    [Route("PromeniTakmicarskiNormalniTurnir")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeTakmicarskiNormalniTurnir([FromBody] TakmicarskiNormalniView sah)
    {
        (bool isError, var turnir, ErrorMessage? error) = await DataProvider.IzmeniTakmicarskiNormalniAsync(sah);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        if (turnir == null)
        {
            return BadRequest("Takmicarski normalni turnir nije validan.");
        }

        return Ok($"Uspešno ažuriran takmicarski normalni turnir. Naziv: {sah.Naziv}");
    }

    [HttpPut]
    [Route("PromeniTakmicarskiBrzopotezniTurnir")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeTakmicarskiBrzopotezniTurnir([FromBody] TakmicarskiBrzopotezniView sah)
    {
        (bool isError, var turnir, ErrorMessage? error) = await DataProvider.IzmeniTakmicarskiBrzopotezniAsync(sah);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        if (turnir == null)
        {
            return BadRequest("Takmicarski brzopotezni turnir nije validan.");
        }

        return Ok($"Uspešno ažuriran takmicarski brzopotezni turnir. Naziv: {sah.Naziv}");
    }

    [HttpPut]
    [Route("PromeniEgzibicioniNormalniTurnir")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeEgzibicioniNormalniTurnir([FromBody] EgzibicioniNormalniView sah)
    {
        (bool isError, var turnir, ErrorMessage? error) = await DataProvider.IzmeniEgzibicioniNormalniAsync(sah);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        if (turnir == null)
        {
            return BadRequest("Egzibicioni normalni turnir nije validan.");
        }

        return Ok($"Uspešno ažuriran egzibicioni normalni turnir. Naziv: {sah.Naziv}");
    }

    [HttpPut]
    [Route("PromeniEgzibicioniBrzopotezniTurnir")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeEgzibicioniBrzopotezniTurnir([FromBody] EgzibicioniBrzopotezniView sah)
    {
        (bool isError, var turnir, ErrorMessage? error) = await DataProvider.IzmeniEgzibicioniBrzopotezniAsync(sah);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        if (turnir == null)
        {
            return BadRequest("Egzibicioni brzopotezni turnir nije validan.");
        }

        return Ok($"Uspešno ažuriran egzibicioni brzopotezni turnir. Naziv: {sah.Naziv}");
    }

    [HttpDelete]
    [Route("IzbrisiTurnir/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteTurnir(int id)
    {
        var data = await DataProvider.ObrisiTurnirAsync(id);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan turnir. ID: {id}");
    }

    [HttpGet]
    [Route("PreuzmiTurnireSahiste/{sahistaID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetTurniriSahiste(int sahistaID)
    {
        (bool isError, var turniri, var error) = DataProvider.VratiTurnireSahiste(sahistaID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(turniri);
    }

    [HttpGet]
    [Route("PreuzmiTurnireOrganizatora/{organizatorID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetTurniriOrganizatora(int organizatorID)
    {
        (bool isError, var turniri, var error) = DataProvider.VratiTurnireOrganizatora(organizatorID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(turniri);
    }

    [HttpPost("KreirajSponzora/{turnirId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajSponzora([FromBody] SponzoriView sponzor, int turnirId)
    {
        var (isError, id, error) = await DataProvider.SacuvajSponzoraAsync(sponzor, turnirId);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan sponzor za turnir {turnirId}, sa ID: {id}");
    }

    [HttpGet]
    [Route("PreuzmiSponzoreTurnira/{turnirID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetSponzoriTurnira(int turnirID)
    {
        (bool isError, var sponzori, var error) = DataProvider.VratiTurnireOrganizatora(turnirID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(sponzori);
    }

    [HttpDelete]
    [Route("IzbrisiSponzora/{sponzor}/{turnirId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteSponzor(string sponzor, int turnirId)
    {
        var data = await DataProvider.ObrisiSponzoraAsync(sponzor, turnirId);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan sponzor. ID turnira: {turnirId}");
    }
}
