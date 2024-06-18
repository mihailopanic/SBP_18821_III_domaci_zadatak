using Microsoft.AspNetCore.Mvc;
using OracleWebAPIService;
using SahFederacijaLibrary;
using SahFederacijaLibrary.DTOs;
using SahFederacijaLibrary.Entiteti;

namespace OracleWebAPIService.Controllers;

[ApiController]
[Route("[controller]")]
public class SahistaController : ControllerBase
{
    [HttpGet]
    [Route("PreuzmiSveSahiste")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSveSahiste()
    {
        var (isError, sahisti, error) = await DataProvider.VratiSveSahisteAsync();

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(sahisti);
    }

    [HttpGet]
    [Route("PreuzmiSveMajstorskeKandidate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSveMajstorskeKandidate()
    {
        var (isError, majstorski_kandidati, error) = await DataProvider.VratiSveMajstorskeKandidateAsync();

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(majstorski_kandidati);
    }

    [HttpGet]
    [Route("PreuzmiSveMajstore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> VratiSveMajstore()
    {
        var (isError, majstori, error) = await DataProvider.VratiSveMajstoreAsync();

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(majstori);
    }

    [HttpPost("KreirajSahistu")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajSahistu([FromBody] SahistaView sahista)
    {
        var (isError, id, error) = await DataProvider.SacuvajSahistuAsync(sahista);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan sahista, sa ID: {id}");
    }

    [HttpPost("KreirajMajstorskogKandidata")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajMajstorskogKandidata([FromBody] Majstorski_KandidatView sahista)
    {
        var (isError, id, error) = await DataProvider.SacuvajMajstorskogKandidataAsync(sahista);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan majstorski kandidat, sa ID: {id}");
    }

    [HttpPost("KreirajMajstora")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajMajstora([FromBody] MajstorView sahista)
    {
        var (isError, id, error) = await DataProvider.SacuvajMajstoraAsync(sahista);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan majstor, sa ID: {id}");
    }

    [HttpPost("KreirajMajstoraSudiju/{sudijaID}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> KreirajMajstoraSudiju([FromBody] MajstorView sahista, int sudijaID)
    {
        var (isError, id, error) = await DataProvider.SacuvajMajstoraSudijuAsync(sahista, sudijaID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return StatusCode(201, $"Upisan majstor sudija, sa ID: {id}");
    }

    [HttpPut("IzmenaSahiste")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> IzmenaSahiste([FromBody] SahistaView sahista)
    {
        var data = await DataProvider.IzmeniSahistuAsync(sahista);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Izmenjen sahista, sa ID: {sahista.Rbr}");
    }

    [HttpPut("IzmenaMajstorskogKandidata")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> IzmenaMajstorskogKandidata([FromBody] Majstorski_KandidatView sahista)
    {
        var data = await DataProvider.IzmeniMajstorskogKandidataAsync(sahista);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Izmenjen majstorski kandidat, sa ID: {sahista.Rbr}");
    }

    [HttpPut("IzmenaMajstora")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> IzmenaMajstora([FromBody] MajstorView sahista)
    {
        var data = await DataProvider.IzmeniMajstoraAsync(sahista);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Izmenjen majstora, sa ID: {sahista.Rbr}");
    }

    [HttpDelete]
    [Route("IzbrisiSahistu/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteSahista(int id)
    {
        var data = await DataProvider.ObrisiSahistuAsync(id);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan sahista. ID: {id}");
    }

    [HttpPost]
    [Route("PoveziSahistuiTurnir/{sahistaID}/{turnirID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> LinkSahistaTurnir(int sahistaID, int turnirID)
    {
        (bool isError1, var sahista, var error1) = await DataProvider.VratiSahistuAsync(sahistaID);
        (bool isError2, var turnir, var error2) = await DataProvider.VratiTurnirAsync(turnirID);

        if (isError1 || isError2)
        {
            return StatusCode(error1?.StatusCode ?? 400, $"{error1?.Message}{Environment.NewLine}{error2?.Message}");
        }

        if (sahista == null || turnir == null)
        {
            return BadRequest("Sahista ili turnir nisu validni.");
        }

        var data = await DataProvider.DodajUcestvujeNaAsync(new UcestvujeNaView
        {
            Id = new UcestvujeNaIdView
            {
                SahistaUcestvujeNa = sahista,
                UcestvujeNaSahovski_Turnir = turnir
            }
        });

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Dodat odnos izmedju sahiste i turnira. Sahista: {sahista.Lime} {sahista.Prezime}. Turnir: {turnir.Naziv}");
    }

    [HttpDelete]
    [Route("IzbrisiUcestvujeNa/{idSahista}/{idTurnir}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteUcestvujeNa(int idSahista, int idTurnir)
    {
        var data = await DataProvider.ObrisiUcestvujeNaAsync(idSahista, idTurnir);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan odnos ucestvuje na.");
    }

    [HttpGet]
    [Route("PreuzmiSahisteTurnira/{turnirID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetSahisteTurnira(int turnirID)
    {
        (bool isError, var sahisti, var error) = DataProvider.VratiSahisteTurnira(turnirID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(sahisti);
    }

    [HttpPost]
    [Route("PoveziSahistuiPartiju/{sahistaID}/{partijaID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> LinkSahistaPartija(int sahistaID, int partijaID)
    {
        (bool isError1, var sahista, var error1) = await DataProvider.VratiSahistuAsync(sahistaID);
        (bool isError2, var partija, var error2) = await DataProvider.VratiPartijuAsync(partijaID);

        if (isError1 || isError2)
        {
            return StatusCode(error1?.StatusCode ?? 400, $"{error1?.Message}{Environment.NewLine}{error2?.Message}");
        }

        if (sahista == null || partija == null)
        {
            return BadRequest("Sahista ili partija nisu validni.");
        }

        var data = await DataProvider.DodajIgraAsync(new IgraView
        {
            Id = new IgraIdView
            {
                SahistaIgra = sahista,
                IgraPartija = partija
            }
        });

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Dodat odnos izmedju sahiste i partije. Sahista: {sahista.Lime} {sahista.Prezime}. Partija: {partija.Id}");
    }

    [HttpDelete]
    [Route("IzbrisiIgra/{idSahista}/{idPartija}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteIgra(int idSahista, int idPartija)
    {
        var data = await DataProvider.ObrisiIgraAsync(idSahista, idPartija);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan odnos igra.");
    }

    [HttpGet]
    [Route("PreuzmiSahistePartije/{partijaID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetSahistePartije(int partijaID)
    {
        (bool isError, var sahisti, var error) = DataProvider.VratiSahistePartije(partijaID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(sahisti);
    }
}

