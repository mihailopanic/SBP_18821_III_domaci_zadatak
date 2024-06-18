using Antlr.Runtime.Tree;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SahFederacijaLibrary;

public static class DataProvider
{
    #region Sahovski_Turnir

    // svi turniri u bazi
    public static async Task<Result<List<Sahovski_TurnirView>, ErrorMessage>> VratiSveTurnireAsync()
    {
        List<Sahovski_TurnirView> data = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Sahovski_Turnir>().ListAsync())
                           .Select(p => new Sahovski_TurnirView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o turnirima.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    // brisanje turnira na osnovu id-ija
    public async static Task<Result<bool, ErrorMessage>> ObrisiTurnirAsync(int id)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahovski_Turnir turnir = await s.LoadAsync<Sahovski_Turnir>(id);
            turnir.Sponzori?.Clear();
            turnir.OrganizatorOrganizuje?.Clear();
            turnir.Partije?.Clear();
            turnir.SahistaUcestvujeNa?.Clear();

            await s.DeleteAsync(turnir);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Greška prilikom brisanja turnira.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public static Result<Sahovski_TurnirView, ErrorMessage> VratiTurnir(int id)
    {
        Sahovski_TurnirView turnirView;

        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahovski_Turnir sah = s.Load<Sahovski_Turnir>(id);
            turnirView = new Sahovski_TurnirView(sah);

            s.Close();
        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće vratiti turnir sa zadatim ID-jem.".ToError(400);
        }

        return turnirView;
    }

    public async static Task<Result<Sahovski_TurnirView, ErrorMessage>> VratiTurnirAsync(int id)
    {
        ISession? s = null;

        Sahovski_TurnirView turnirView = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahovski_Turnir t = await s.LoadAsync<Sahovski_Turnir>(id);
            turnirView = new Sahovski_TurnirView(t);

            s.Close();
        }
        catch (Exception)
        {
            return "Nemoguće vratiti turnir sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return turnirView;
    }

    public static Result<List<UcestvujeNaView>, ErrorMessage> VratiTurnireSahiste(int sahistaRbr)
    {
        ISession? s = null;

        List<UcestvujeNaView> ucestvujeNa = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<UcestvujeNa> un = from u in s.Query<UcestvujeNa>()
                                          where u.Id.UcestvujeNaSahovski_Turnir != null && u.Id.SahistaUcestvujeNa.Rbr == sahistaRbr
                                          select u;

            // ucestvujeNa = un.Select(p => new UcestvujeNaView(p)).ToList();

            foreach (UcestvujeNa u in un)
            {
                var sahista = VratiSahistu(u.Id.SahistaUcestvujeNa?.Rbr ?? -1);
                var turnir = VratiTurnir(u.Id.UcestvujeNaSahovski_Turnir?.Id ?? -1);
                ucestvujeNa.Add(new UcestvujeNaView(u));

            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve turnire sahiste sa zadatim RBR-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return ucestvujeNa;
    }

    public static Result<List<OrganizujeView>, ErrorMessage> VratiTurnireOrganizatora(int organizatorId)
    {
        ISession? s = null;

        List<OrganizujeView> organizuje = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Organizuje> org = from o in s.Query<Organizuje>()
                                          where o.Id.OrganizujeSahovski_Turnir != null && o.Id.OrganizatorOrganizuje.Id == organizatorId
                                          select o;

            // organizuje = un.Select(p => new OrganizujeView(p)).ToList();

            foreach (Organizuje o in org)
            {
                var organizator = VratiOrganizatora(o.Id.OrganizatorOrganizuje?.Id ?? -1);
                var turnir = VratiTurnir(o.Id.OrganizujeSahovski_Turnir?.Id ?? -1);
                organizuje.Add(new OrganizujeView(o));

            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve turnire organizatora sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return organizuje;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajTurnirAsync(Sahovski_TurnirView turnir, string tip)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahovski_Turnir? t = tip switch
            {
                "TakmicarskiNormalni" => new TakmicarskiNormalni(),
                "TakmicarskiBrzopotezni" => new TakmicarskiBrzopotezni(),
                "EgzibicioniNormalni" => new EgzibicioniNormalni(),
                "EgzibicioniBrzopotezni" => new EgzibicioniBrzopotezni(),
                _ => null,
            };

            if (t != null)
            {
                t.Naziv = turnir.Naziv;
                t.Zemlja = turnir.Zemlja;
                t.Grad = turnir.Grad;
                t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
                t.Datum_Od = turnir.Datum_Od;
                t.Datum_Do = turnir.Datum_Do;
                t.Nacin_Odigravanja_Znacaj = tip;

                await s.SaveAsync(t);
                await s.FlushAsync();
            }
            else
            {
                return "Pogrešan tip turnira.".ToError(400);
            }
        }
        catch (Exception)
        {
            return "Nemoguće sačuvati turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #region TakmicarskiNormalni

    // takmicarski normalni na osnovu id-ija
    public async static Task<Result<TakmicarskiNormalniView, ErrorMessage>> VratiTakmicarskiNormalniAsync(int id)
    {
        ISession? s = null;
        TakmicarskiNormalniView t = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            TakmicarskiNormalni turnir = await s.LoadAsync<TakmicarskiNormalni>(id);

            t.Id = turnir.Id;
            t.Naziv = turnir.Naziv;
            t.Zemlja = turnir.Zemlja;
            t.Grad = turnir.Grad;
            t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
            t.Datum_Od = turnir.Datum_Od;
            t.Datum_Do = turnir.Datum_Do;
            t.Region = turnir.Region;
        }
        catch (Exception)
        {
            return "Nemoguće vratiti turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return t;
    }

    // izmena atributa turnira
    public async static Task<Result<bool, ErrorMessage>> IzmeniTakmicarskiNormalniAsync(TakmicarskiNormalniView turnir)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            TakmicarskiNormalni t = await s.LoadAsync<TakmicarskiNormalni>(turnir.Id);

            t.Id = turnir.Id;
            t.Naziv = turnir.Naziv;
            t.Zemlja = turnir.Zemlja;
            t.Grad = turnir.Grad;
            t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
            t.Datum_Od = turnir.Datum_Od;
            t.Datum_Do = turnir.Datum_Do;
            t.Region = turnir.Region;

            await s.SaveOrUpdateAsync(t);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti normalni takmicarski turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    // cuvanje turnira
    public async static Task<Result<int, ErrorMessage>> SacuvajTakmicarskiNormalniAsync(TakmicarskiNormalniView turnir)
    {
        ISession? s = null;
        int id = default;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            TakmicarskiNormalni t = new()
            {
                Id = turnir.Id,
                Naziv = turnir.Naziv,
                Zemlja = turnir.Zemlja,
                Grad = turnir.Grad,
                Godina_Odrzavanja = turnir.Godina_Odrzavanja,
                Datum_Od = turnir.Datum_Od,
                Datum_Do = turnir.Datum_Do,
                Region = turnir.Region
            };

            await s.SaveAsync(t);
            await s.FlushAsync();

            id = t.Id;
        }
        catch (Exception)
        {
            return "Nemoguće sačuvati takmicarski normalni turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return id;
    }

    #endregion

    #region TakmicarskiBrzopotezni

    public async static Task<Result<TakmicarskiBrzopotezniView, ErrorMessage>> VratiTakmicarskiBrzopotezniAsync(int id)
    {
        ISession? s = null;
        TakmicarskiBrzopotezniView t = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            TakmicarskiBrzopotezniView turnir = await s.LoadAsync<TakmicarskiBrzopotezniView>(id);

            t.Id = turnir.Id;
            t.Naziv = turnir.Naziv;
            t.Zemlja = turnir.Zemlja;
            t.Grad = turnir.Grad;
            t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
            t.Datum_Od = turnir.Datum_Od;
            t.Datum_Do = turnir.Datum_Do;
            t.Region = turnir.Region;
            t.Max_Vreme_Trajanja = turnir.Max_Vreme_Trajanja;
        }
        catch (Exception)
        {
            return "Nemoguće vratiti turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return t;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniTakmicarskiBrzopotezniAsync(TakmicarskiBrzopotezniView turnir)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            TakmicarskiBrzopotezni t = await s.LoadAsync<TakmicarskiBrzopotezni>(turnir.Id);

            t.Id = turnir.Id;
            t.Naziv = turnir.Naziv;
            t.Zemlja = turnir.Zemlja;
            t.Grad = turnir.Grad;
            t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
            t.Datum_Od = turnir.Datum_Od;
            t.Datum_Do = turnir.Datum_Do;
            t.Region = turnir.Region;
            t.Max_Vreme_Trajanja = turnir.Max_Vreme_Trajanja;

            await s.SaveOrUpdateAsync(t);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti brzopotezni takmicarski turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<int, ErrorMessage>> SacuvajTakmicarskiBrzopotezniAsync(TakmicarskiBrzopotezniView turnir)
    {
        ISession? s = null;
        int id = default;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            TakmicarskiBrzopotezni t = new()
            {
                Id = turnir.Id,
                Naziv = turnir.Naziv,
                Zemlja = turnir.Zemlja,
                Grad = turnir.Grad,
                Godina_Odrzavanja = turnir.Godina_Odrzavanja,
                Datum_Od = turnir.Datum_Od,
                Datum_Do = turnir.Datum_Do,
                Region = turnir.Region,
                Max_Vreme_Trajanja = turnir.Max_Vreme_Trajanja
            };

            await s.SaveAsync(t);
            await s.FlushAsync();

            id = t.Id;
        }
        catch (Exception)
        {
            return "Nemoguće sačuvati takmicarski brzopotezni turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return id;
    }

    #endregion

    #region EgzibicioniNormalni

    public async static Task<Result<EgzibicioniNormalniView, ErrorMessage>> VratiEgzibicioniNormalniAsync(int id)
    {
        ISession? s = null;
        EgzibicioniNormalniView t = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            EgzibicioniNormalni turnir = await s.LoadAsync<EgzibicioniNormalni>(id);

            t.Id = turnir.Id;
            t.Naziv = turnir.Naziv;
            t.Zemlja = turnir.Zemlja;
            t.Grad = turnir.Grad;
            t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
            t.Datum_Od = turnir.Datum_Od;
            t.Datum_Do = turnir.Datum_Do;
            t.Tip = turnir.Tip;
            t.Novac_Namenjen = turnir.Novac_Namenjen;
            t.Prikupljen_Iznos = turnir.Prikupljen_Iznos;
        }
        catch (Exception)
        {
            return "Nemoguće vratiti turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return t;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniEgzibicioniNormalniAsync(EgzibicioniNormalniView turnir)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            EgzibicioniNormalni t = await s.LoadAsync<EgzibicioniNormalni>(turnir.Id);

            t.Id = turnir.Id;
            t.Naziv = turnir.Naziv;
            t.Zemlja = turnir.Zemlja;
            t.Grad = turnir.Grad;
            t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
            t.Datum_Od = turnir.Datum_Od;
            t.Datum_Do = turnir.Datum_Do;
            t.Tip = turnir.Tip;
            t.Novac_Namenjen = turnir.Novac_Namenjen;
            t.Prikupljen_Iznos = turnir.Prikupljen_Iznos;

            await s.SaveOrUpdateAsync(t);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti normalni egzibicioni turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<int, ErrorMessage>> SacuvajEgzibicioniNormalniAsync(EgzibicioniNormalniView turnir)
    {
        ISession? s = null;
        int id = default;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            EgzibicioniNormalni t = new()
            {
                Id = turnir.Id,
                Naziv = turnir.Naziv,
                Zemlja = turnir.Zemlja,
                Grad = turnir.Grad,
                Godina_Odrzavanja = turnir.Godina_Odrzavanja,
                Datum_Od = turnir.Datum_Od,
                Datum_Do = turnir.Datum_Do,
                Tip = turnir.Tip,
                Novac_Namenjen = turnir.Novac_Namenjen,
                Prikupljen_Iznos = turnir.Prikupljen_Iznos
            };

            await s.SaveAsync(t);
            await s.FlushAsync();

            id = t.Id;
        }
        catch (Exception)
        {
            return "Nemoguće sačuvati egzibicioni normalni turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return id;
    }

    #endregion

    #region EgzibicioniBrzopotezni

    public async static Task<Result<EgzibicioniBrzopotezniView, ErrorMessage>> VratiEgzibicioniBrzopotezniAsync(int id)
    {
        ISession? s = null;
        EgzibicioniBrzopotezniView t = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            EgzibicioniBrzopotezni turnir = await s.LoadAsync<EgzibicioniBrzopotezni>(id);

            t.Id = turnir.Id;
            t.Naziv = turnir.Naziv;
            t.Zemlja = turnir.Zemlja;
            t.Grad = turnir.Grad;
            t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
            t.Datum_Od = turnir.Datum_Od;
            t.Datum_Do = turnir.Datum_Do;
            t.Tip = turnir.Tip;
            t.Prikupljen_Iznos = turnir.Prikupljen_Iznos;
            t.Novac_Namenjen = turnir.Novac_Namenjen;
            t.Max_Vreme_Trajanja = turnir.Max_Vreme_Trajanja;
        }
        catch (Exception)
        {
            return "Nemoguće vratiti turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return t;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniEgzibicioniBrzopotezniAsync(EgzibicioniBrzopotezniView turnir)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            EgzibicioniBrzopotezni t = await s.LoadAsync<EgzibicioniBrzopotezni>(turnir.Id);

            t.Id = turnir.Id;
            t.Naziv = turnir.Naziv;
            t.Zemlja = turnir.Zemlja;
            t.Grad = turnir.Grad;
            t.Godina_Odrzavanja = turnir.Godina_Odrzavanja;
            t.Datum_Od = turnir.Datum_Od;
            t.Datum_Do = turnir.Datum_Do;
            t.Tip = turnir.Tip;
            t.Novac_Namenjen = turnir.Novac_Namenjen;
            t.Prikupljen_Iznos = turnir.Prikupljen_Iznos;
            t.Max_Vreme_Trajanja = turnir.Max_Vreme_Trajanja;

            await s.SaveOrUpdateAsync(t);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti egzibicioni brzopotezni turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<int, ErrorMessage>> SacuvajEgzibicioniBrzopotezniAsync(EgzibicioniBrzopotezniView turnir)
    {
        ISession? s = null;
        int id = default;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            EgzibicioniBrzopotezni t = new()
            {
                Id = turnir.Id,
                Naziv = turnir.Naziv,
                Zemlja = turnir.Zemlja,
                Grad = turnir.Grad,
                Godina_Odrzavanja = turnir.Godina_Odrzavanja,
                Datum_Od = turnir.Datum_Od,
                Datum_Do = turnir.Datum_Do,
                Tip = turnir.Tip,
                Prikupljen_Iznos = turnir.Prikupljen_Iznos,
                Novac_Namenjen = turnir.Novac_Namenjen,
                Max_Vreme_Trajanja = turnir.Max_Vreme_Trajanja
            };

            await s.SaveAsync(t);
            await s.FlushAsync();

            id = t.Id;
        }
        catch (Exception)
        {
            return "Nemoguće sačuvati egzibicioni brzopotezni turnir.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return id;
    }

    #endregion

    #endregion

    #region Sahista

    public static Result<List<SahistaView>, ErrorMessage> VratiSveSahiste()
    {
        ISession? s = null;

        List<SahistaView> sahisti = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Sahista> sviSahisti = from sah in s.Query<Sahista>()
                                                    select sah;

            foreach (Sahista sah in sviSahisti)
            {
                sahisti.Add(new SahistaView(sah));
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve sahiste.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sahisti;
    }

    public static async Task<Result<List<SahistaView>, ErrorMessage>> VratiSveSahisteAsync()
    {
        List<SahistaView> data = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Sahista>().ListAsync())
                           .Select(p => new SahistaView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o sahistima.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    public async static Task<Result<int, ErrorMessage>> SacuvajSahistuAsync(SahistaView sahista)
    {
        ISession? s = null;

        int id = default;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahista sah = new()
            {
                Zemlja_Porekla = sahista.Zemlja_Porekla,
                Broj_Pasosa = sahista.Broj_Pasosa,
                Datum_Uclanjenja = sahista.Datum_Uclanjenja,
                Lime = sahista.Lime,
                Sslovo = sahista.Sslovo,
                Prezime = sahista.Prezime,
                Adresa = sahista.Adresa,
                Datum_Rodjenja = sahista.Datum_Rodjenja
            };

            id = (int)await s.SaveAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati sahistu.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return id;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniSahistuAsync(SahistaView sahista)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahista sah = await s.LoadAsync<Sahista>(sahista.Rbr);

            sah.Zemlja_Porekla = sahista.Zemlja_Porekla;
            sah.Broj_Pasosa = sahista.Broj_Pasosa;
            sah.Datum_Uclanjenja = sahista.Datum_Uclanjenja;
            sah.Lime = sahista.Lime;
            sah.Sslovo = sahista.Sslovo;
            sah.Prezime = sahista.Prezime;
            sah.Adresa = sahista.Adresa;
            sah.Datum_Rodjenja = sahista.Datum_Rodjenja;

            await s.SaveOrUpdateAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti sahistu.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<SahistaView, ErrorMessage>> VratiSahistuAsync(int rbr)
    {
        ISession? s = null;

        SahistaView sahistaView = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahista sah = await s.LoadAsync<Sahista>(rbr);
            sahistaView = new SahistaView(sah);
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sahistu sa zadatim RBR-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sahistaView;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiSahistuAsync(int rbr)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahista sah = await s.LoadAsync<Sahista>(rbr);
            sah.IgraPartija?.Clear();
            sah.UcestvujeNaSahovski_Turnir?.Clear();

            await s.DeleteAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati sahistu.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public static Result<SahistaView, ErrorMessage> VratiSahistu(int rbr)
    {
        SahistaView sahistaView;

        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahista sah = s.Load<Sahista>(rbr);
            sahistaView = new SahistaView(sah);

            s.Close();
        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće vratiti sahistu sa zadatim RBR-jem.".ToError(400);
        }

        return sahistaView;
    }

    public static Result<List<UcestvujeNaView>, ErrorMessage> VratiSahisteTurnira(int turnirId)
    {
        ISession? s = null;

        List<UcestvujeNaView> ucestvujeNa = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<UcestvujeNa> un = from u in s.Query<UcestvujeNa>()
                                          where u.Id.SahistaUcestvujeNa != null && u.Id.UcestvujeNaSahovski_Turnir.Id == turnirId
                                          select u;

            // ucestvujeNa = un.Select(p => new UcestvujeNaView(p)).ToList();

            foreach (UcestvujeNa u in un)
            {
                var sahista = VratiSahistu(u.Id.SahistaUcestvujeNa?.Rbr ?? -1);
                var turnir = VratiTurnir(u.Id.UcestvujeNaSahovski_Turnir?.Id ?? -1);
                ucestvujeNa.Add(new UcestvujeNaView(u));

            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve sahiste turnira sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return ucestvujeNa;
    }

    public static Result<List<IgraView>, ErrorMessage> VratiSahistePartije(int partijaId)
    {
        ISession? s = null;

        List<IgraView> igra = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Igra> igr = from i in s.Query<Igra>()
                                    where i.Id.SahistaIgra != null && i.Id.IgraPartija.Id == partijaId
                                    select i;

            // organizuje = un.Select(p => new OrganizujeView(p)).ToList();

            foreach (Igra i in igr)
            {
                var partija = VratiPartiju(i.Id.IgraPartija?.Id ?? -1);
                var sahista = VratiSahistu(i.Id.SahistaIgra?.Rbr ?? -1);
                igra.Add(new IgraView(i));

            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti dvojicu sahiste partije sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return igra;
    }

    #endregion

    #region Majstorski_Kandidat

    public static Result<List<Majstorski_KandidatView>, ErrorMessage> VratiSveMajstorskeKandidate()
    {
        ISession? s = null;

        List<Majstorski_KandidatView> sahisti = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Majstorski_Kandidat> sviSahisti = from sah in s.Query<Majstorski_Kandidat>()
                                              select sah;

            foreach (Majstorski_Kandidat sah in sviSahisti)
            {
                sahisti.Add(new Majstorski_KandidatView(sah));
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve majstorske kandidate.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sahisti;
    }

    public static async Task<Result<List<Majstorski_KandidatView>, ErrorMessage>> VratiSveMajstorskeKandidateAsync()
    {
        List<Majstorski_KandidatView> data = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Majstorski_Kandidat>().ListAsync())
                           .Select(p => new Majstorski_KandidatView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o majstorskim kandidatima.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajMajstorskogKandidataAsync(Majstorski_KandidatView sahista)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Majstorski_Kandidat sah = new()
            {
                Zemlja_Porekla = sahista.Zemlja_Porekla,
                Broj_Pasosa = sahista.Broj_Pasosa,
                Datum_Uclanjenja = sahista.Datum_Uclanjenja,
                Lime = sahista.Lime,
                Sslovo = sahista.Sslovo,
                Prezime = sahista.Prezime,
                Adresa = sahista.Adresa,
                Datum_Rodjenja = sahista.Datum_Rodjenja,
                Broj_Partija_Do_Zvanja = sahista.Broj_Partija_Do_Zvanja
            };

            await s.SaveOrUpdateAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati majstorskog kandidata.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniMajstorskogKandidataAsync(Majstorski_KandidatView sahista)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Majstorski_Kandidat sah = await s.LoadAsync<Majstorski_Kandidat>(sahista.Rbr);

            sah.Zemlja_Porekla = sahista.Zemlja_Porekla;
            sah.Broj_Pasosa = sahista.Broj_Pasosa;
            sah.Datum_Uclanjenja = sahista.Datum_Uclanjenja;
            sah.Lime = sahista.Lime;
            sah.Sslovo = sahista.Sslovo;
            sah.Prezime = sahista.Prezime;
            sah.Adresa = sahista.Adresa;
            sah.Datum_Rodjenja = sahista.Datum_Rodjenja;
            sah.Broj_Partija_Do_Zvanja = sahista.Broj_Partija_Do_Zvanja;

            await s.SaveOrUpdateAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti majstorskog kandidata.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<Majstorski_KandidatView, ErrorMessage>> VratiMajstorskogKandidataAsync(int rbr)
    {
        ISession? s = null;

        Majstorski_KandidatView sahistaView = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Majstorski_Kandidat sah = await s.LoadAsync<Majstorski_Kandidat>(rbr);
            sahistaView = new Majstorski_KandidatView(sah);
        }
        catch (Exception)
        {
            return "Nemoguće vratiti majstorskog kandidata sa zadatim RBR-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sahistaView;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiMajstorskogKandidataAsync(int rbr)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Majstorski_Kandidat sah = await s.LoadAsync<Majstorski_Kandidat>(rbr);
            sah.IgraPartija?.Clear();
            sah.UcestvujeNaSahovski_Turnir?.Clear();

            await s.DeleteAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati majstorskog kandidata.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion

    #region Majstor

    public static Result<List<MajstorView>, ErrorMessage> VratiSveMajstore()
    {
        ISession? s = null;

        List<MajstorView> sahisti = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Majstor> sviSahisti = from sah in s.Query<Majstor>()
                                                          select sah;

            foreach (Majstor sah in sviSahisti)
            {
                sahisti.Add(new MajstorView(sah));
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve majstore.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sahisti;
    }

    public static async Task<Result<List<MajstorView>, ErrorMessage>> VratiSveMajstoreAsync()
    {
        List<MajstorView> data = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Majstor>().ListAsync())
                           .Select(p => new MajstorView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o majstorima.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajMajstoraAsync(MajstorView sahista)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Majstor sah = new()
            {
                Zemlja_Porekla = sahista.Zemlja_Porekla,
                Broj_Pasosa = sahista.Broj_Pasosa,
                Datum_Uclanjenja = sahista.Datum_Uclanjenja,
                Lime = sahista.Lime,
                Sslovo = sahista.Sslovo,
                Prezime = sahista.Prezime,
                Adresa = sahista.Adresa,
                Datum_Rodjenja = sahista.Datum_Rodjenja,
                Datum_Zvanja = sahista.Datum_Zvanja
            };

            await s.SaveOrUpdateAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati majstora.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajMajstoraSudijuAsync(MajstorView sahista, int idSudija)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sudija sud = await s.LoadAsync<Sudija>(idSudija);

            Majstor sah = new()
            {
                Zemlja_Porekla = sahista.Zemlja_Porekla,
                Broj_Pasosa = sahista.Broj_Pasosa,
                Datum_Uclanjenja = sahista.Datum_Uclanjenja,
                Lime = sahista.Lime,
                Sslovo = sahista.Sslovo,
                Prezime = sahista.Prezime,
                Adresa = sahista.Adresa,
                Datum_Rodjenja = sahista.Datum_Rodjenja,
                Datum_Zvanja = sahista.Datum_Zvanja,
                Sudija = sud
            };

            await s.SaveOrUpdateAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati majstora.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniMajstoraAsync(MajstorView sahista)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Majstor sah = await s.LoadAsync<Majstor>(sahista.Rbr);

            sah.Zemlja_Porekla = sahista.Zemlja_Porekla;
            sah.Broj_Pasosa = sahista.Broj_Pasosa;
            sah.Datum_Uclanjenja = sahista.Datum_Uclanjenja;
            sah.Lime = sahista.Lime;
            sah.Sslovo = sahista.Sslovo;
            sah.Prezime = sahista.Prezime;
            sah.Adresa = sahista.Adresa;
            sah.Datum_Rodjenja = sahista.Datum_Rodjenja;
            sah.Datum_Zvanja = sahista.Datum_Zvanja;

            await s.SaveOrUpdateAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti majstora.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<MajstorView, ErrorMessage>> VratiMajstoraAsync(int rbr)
    {
        ISession? s = null;

        MajstorView sahistaView = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Majstor sah = await s.LoadAsync<Majstor>(rbr);
            sahistaView = new MajstorView(sah);
        }
        catch (Exception)
        {
            return "Nemoguće vratiti majstora sa zadatim RBR-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sahistaView;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiMajstoraAsync(int rbr)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Majstor sah = await s.LoadAsync<Majstor>(rbr);
            sah.IgraPartija?.Clear();
            sah.UcestvujeNaSahovski_Turnir?.Clear();

            await s.DeleteAsync(sah);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati majstora.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion

    #region Organizator

    public static Result<List<OrganizatorView>, ErrorMessage> VratiSveOrganizatore()
    {
        ISession? s = null;

        List<OrganizatorView> organizatori = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Organizator> sviOrganizatori = from org in s.Query<Organizator>()
                                              select org;

            foreach (Organizator org in sviOrganizatori)
            {
                organizatori.Add(new OrganizatorView(org));
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve organizatore.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return organizatori;
    }

    public static async Task<Result<List<OrganizatorView>, ErrorMessage>> VratiSveOrganizatoreAsync()
    {
        List<OrganizatorView> data = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Organizator>().ListAsync())
                           .Select(p => new OrganizatorView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o organizatorima.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajOrganizatoraAsync(OrganizatorView organizator)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Organizator org = new()
            {
                Jmbg = organizator.Jmbg,
                Adresa = organizator.Adresa,
                Lime  = organizator.Lime,
                Sslovo = organizator.Sslovo,
                Prezime = organizator.Prezime
            };

            await s.SaveOrUpdateAsync(org);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati organizatora.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajOrganizatoraSudijuAsync(OrganizatorView organizator, int idSudija)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sudija sud = await s.LoadAsync<Sudija>(idSudija);

            Organizator org = new()
            {
                Jmbg = organizator.Jmbg,
                Adresa = organizator.Adresa,
                Lime = organizator.Lime,
                Sslovo = organizator.Sslovo,
                Prezime = organizator.Prezime,
                Sudija = sud
            };

            await s.SaveOrUpdateAsync(org);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati organizatora.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniOrganizatoraAsync(OrganizatorView organizator)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Organizator org = await s.LoadAsync<Organizator>(organizator.Id);

            org.Jmbg = organizator.Jmbg;
            org.Adresa = organizator.Adresa;
            org.Lime = organizator.Lime;
            org.Sslovo = organizator.Sslovo;
            org.Prezime = organizator.Prezime;

            await s.SaveOrUpdateAsync(org);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti organizatora.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<OrganizatorView, ErrorMessage>> VratiOrganizatoraAsync(int id)
    {
        ISession? s = null;

        OrganizatorView organizatorView = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Organizator org = await s.LoadAsync<Organizator>(id);
            organizatorView = new OrganizatorView(org);
        }
        catch (Exception)
        {
            return "Nemoguće vratiti organizatora sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return organizatorView;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiOrganizatoraAsync(int id)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Organizator org = await s.LoadAsync<Organizator>(id);
            org.OrganizujeSahovski_Turnir?.Clear();

            await s.DeleteAsync(org);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati organizatora.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public static Result<OrganizatorView, ErrorMessage> VratiOrganizatora(int id)
    {
        OrganizatorView organizatorView;

        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Organizator organizator = s.Load<Organizator>(id);
            organizatorView = new OrganizatorView(organizator);

            s.Close();
        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće vratiti organizatora sa zadatim ID-jem.".ToError(400);
        }

        return organizatorView;
    }

    public static Result<List<OrganizujeView>, ErrorMessage> VratiOrganizatoreTurnira(int turnirId)
    {
        ISession? s = null;

        List<OrganizujeView> organizuje = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Organizuje> org = from o in s.Query<Organizuje>()
                                          where o.Id.OrganizatorOrganizuje != null && o.Id.OrganizujeSahovski_Turnir.Id == turnirId
                                          select o;

            // ucestvujeNa = un.Select(p => new UcestvujeNaView(p)).ToList();

            foreach (Organizuje o in org)
            {
                var organizator = VratiOrganizatora(o.Id.OrganizatorOrganizuje?.Id ?? -1);
                var turnir = VratiTurnir(o.Id.OrganizujeSahovski_Turnir?.Id ?? -1);
                organizuje.Add(new OrganizujeView(o));

            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve organizatore turnira sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return organizuje;
    }

    #endregion

    #region Partija

    public static Result<List<PartijaView>, ErrorMessage> VratiSvePartije()
    {
        ISession? s = null;

        List<PartijaView> partije = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Partija> svePartije = from par in s.Query<Partija>()
                                              select par;

            foreach (Partija par in svePartije)
            {
                partije.Add(new PartijaView(par));
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve partije.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return partije;
    }

    public static async Task<Result<List<PartijaView>, ErrorMessage>> VratiSvePartijeAsync()
    {
        List<PartijaView> data = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Partija>().ListAsync())
                           .Select(p => new PartijaView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o partijama.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajPartijuBezTurniraAsync(PartijaView partija, int crne, int bele, int sudija)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahista crne1 = await s.LoadAsync<Sahista>(crne);
            Sahista bele1 = await s.LoadAsync<Sahista>(bele);
            Sudija sud = await s.LoadAsync<Sudija>(sudija);

            Partija par = new()
            {
                Datum_Vreme_Odigravanja = partija.Datum_Vreme_Odigravanja,
                Rezultat_Partije = partija.Rezultat_Partije,
                Trajanje_Partije = partija.Trajanje_Partije,
                Crne_Figure = crne1,
                Bele_Figure = bele1,
                Sudija = sud
            };

            await s.SaveOrUpdateAsync(par);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati partiju.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajPartijuAsync(PartijaView partija, int idTurnir, int crne, int bele, int sudija)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahovski_Turnir sah = await s.LoadAsync<Sahovski_Turnir>(idTurnir);
            Sahista crne1 = await s.LoadAsync<Sahista>(crne);
            Sahista bele1 = await s.LoadAsync<Sahista>(bele);
            Sudija sud = await s.LoadAsync<Sudija>(sudija);

            Partija par = new()
            {
                Datum_Vreme_Odigravanja = partija.Datum_Vreme_Odigravanja,
                Rezultat_Partije = partija.Rezultat_Partije,
                Trajanje_Partije = partija.Trajanje_Partije,
                Crne_Figure = crne1,
                Bele_Figure = bele1,
                Sudija = sud,
                SahovskiTurnir = sah
            };

            await s.SaveOrUpdateAsync(par);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati partiju.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniPartijuAsync(PartijaView partija, int crne, int bele, int sudija)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Partija par = await s.LoadAsync<Partija>(partija.Id);
            Sahista crne1 = await s.LoadAsync<Sahista>(crne);
            Sahista bele1 = await s.LoadAsync<Sahista>(bele);
            Sudija sud = await s.LoadAsync<Sudija>(sudija);

            par.Datum_Vreme_Odigravanja = partija.Datum_Vreme_Odigravanja;
            par.Rezultat_Partije = partija.Rezultat_Partije;
            par.Trajanje_Partije = partija.Trajanje_Partije;
            par.Crne_Figure = crne1;
            par.Bele_Figure = bele1;
            par.Sudija = sud;

            await s.SaveOrUpdateAsync(par);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti sahistu.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<PartijaView, ErrorMessage>> VratiPartijuAsync(int id)
    {
        ISession? s = null;

        PartijaView partijaView = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Partija par = await s.LoadAsync<Partija>(id);
            partijaView = new PartijaView(par);
        }
        catch (Exception)
        {
            return "Nemoguće vratiti partiju sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return partijaView;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiPartijuAsync(int id)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Partija par = await s.LoadAsync<Partija>(id);
            par.SahistaIgra?.Clear();
            par.Potezi?.Clear();

            await s.DeleteAsync(par);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati partiju.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public static Result<PartijaView, ErrorMessage> VratiPartiju(int id)
    {
        PartijaView partijaView;

        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Partija partija = s.Load<Partija>(id);
            partijaView = new PartijaView(partija);

            s.Close();
        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće vratiti partiju sa zadatim ID-jem.".ToError(400);
        }

        return partijaView;
    }

    public static async Task<Result<List<PartijaView>, ErrorMessage>> VratiSvePartijeTurniraAsync(int turnirId)
    {
        List<PartijaView> partije = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            partije = (await s.QueryOver<Partija>().ListAsync())
                           .Where(p => p.SahovskiTurnir?.Id == turnirId)
                           .Select(p => new PartijaView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o partijama.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return partije;
    }

    public static async Task<Result<List<PartijaView>, ErrorMessage>> VratiSvePartijeSudijeAsync(int id)
    {
        List<PartijaView> data = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Partija>().ListAsync())
                           .Where(p => p.Sudija?.Id == id)
                           .Select(p => new PartijaView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o partijama.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    public static Result<List<IgraView>, ErrorMessage> VratiPartijeSahiste(int sahistaRbr)
    {
        ISession? s = null;

        List<IgraView> igra = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Igra> igr = from i in s.Query<Igra>()
                                          where i.Id.IgraPartija != null && i.Id.SahistaIgra.Rbr == sahistaRbr
                                          select i;

            // organizuje = un.Select(p => new OrganizujeView(p)).ToList();

            foreach (Igra i in igr)
            {
                var partija = VratiPartiju(i.Id.IgraPartija?.Id ?? -1);
                var sahista = VratiSahistu(i.Id.SahistaIgra?.Rbr ?? -1);
                igra.Add(new IgraView(i));

            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve partije sahiste sa zadatim RBR-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return igra;
    }

    public static async Task<Result<List<PartijaView>, ErrorMessage>> VratiPartijeSahistaAsync(int crneRbr, int beleRbr)
    {
        ISession? s = null;

        List<PartijaView> data = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Partija>().ListAsync())
                       .Where(p => (p.Crne_Figure?.Rbr == crneRbr || p.Crne_Figure?.Rbr == beleRbr) &&
                                   (p.Bele_Figure?.Rbr == crneRbr || p.Bele_Figure?.Rbr == beleRbr))
                       .Select(p => new PartijaView(p))
                       .ToList();
        }
        catch (Exception)
        {
            return "Nemoguće vratiti partiju sa zadatim ID-jevima sahista.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    #endregion

    #region Potez

    public static async Task<Result<List<PotezView>, ErrorMessage>> VratiSvePotezePartijeAsync(int id)
    {
        List<PotezView> potezi = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            potezi = (await s.QueryOver<Potez>().ListAsync())
                           .Where(p => p.Partija?.Id == id)
                           .Select(p => new PotezView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o potezima.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return potezi;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajPotezAsync(PotezView potez, int partijaId)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Partija par = await s.LoadAsync<Partija>(partijaId);

            Potez pot = new()
            {
                Broj = potez.Broj,
                Figura = potez.Figura,
                Redni_Broj = potez.Redni_Broj,
                Slovo = potez.Slovo,
                Vreme_Odigravanja = potez.Vreme_Odigravanja,
                Partija = par
            };

            await s.SaveOrUpdateAsync(pot);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati potez.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniPotezAsync(PotezView potez, int potezRbr, int partijaId)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Partija par = await s.LoadAsync<Partija>(partijaId);
            var potezId = (potezRbr, par);

            Potez pot = await s.LoadAsync<Potez>(potezId);

            pot.Broj = potez.Broj;
            pot.Slovo = potez.Slovo;
            pot.Figura = potez.Figura;
            pot.Vreme_Odigravanja = potez.Vreme_Odigravanja;

            await s.SaveOrUpdateAsync(pot);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće izmeniti potez.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiPotezAsync(int rbr, int partijaId)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Partija par = await s.LoadAsync<Partija>(partijaId);
            var potezid = (rbr, par);

            Potez pot = await s.LoadAsync<Potez>(potezid);

            await s.DeleteAsync(pot);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati potez.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion

    #region Sponzori

    public static async Task<Result<List<SponzoriView>, ErrorMessage>> VratiSveSponzoreTurniraAsync(int id)
    {
        List<SponzoriView> sponzori = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            sponzori = (await s.QueryOver<Sponzori>().ListAsync())
                           .Where(p => p.SahovskiTurnir?.Id == id)
                           .Select(p => new SponzoriView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o sponzorima.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sponzori;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajSponzoraAsync(SponzoriView sponzor, int turnirId)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahovski_Turnir sah = await s.LoadAsync<Sahovski_Turnir>(turnirId);

            Sponzori spo = new()
            {
                Sponzor = sponzor.Sponzor,
                SahovskiTurnir = sah
            };

            await s.SaveOrUpdateAsync(spo);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće dodati sponzora turniru.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public static Result<List<SponzoriView>, ErrorMessage> VratiSponzoreTurnira(int turnirId)
    {
        ISession? s = null;

        List<SponzoriView> sponzori = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Sponzori> spo = from sp in s.Query<Sponzori>()
                                          where sp.SahovskiTurnir.Id == turnirId
                                          select sp;

            // ucestvujeNa = un.Select(p => new UcestvujeNaView(p)).ToList();

            foreach (Sponzori sp in spo)
            {
                sponzori.Add(new SponzoriView(sp));

            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve sponzore turnira sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sponzori;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiSponzoraAsync(string sponzor, int turnirId)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahovski_Turnir sah = await s.LoadAsync<Sahovski_Turnir>(turnirId);
            var sponzorId = (sponzor, sah);

            Sponzori spo = await s.LoadAsync<Sponzori>(sponzorId);

            await s.DeleteAsync(spo);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati sponzora.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion

    #region Sudija

    public static Result<List<SudijaView>, ErrorMessage> VratiSveSudije()
    {
        ISession? s = null;

        List<SudijaView> sudije = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Sudija> sveSudije = from sud in s.Query<Sudija>()
                                              select sud;

            foreach (Sudija sud in sveSudije)
            {
                sudije.Add(new SudijaView(sud));
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve sudije.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sudije;
    }

    public static async Task<Result<List<SudijaView>, ErrorMessage>> VratiSveSudijeAsync()
    {
        List<SudijaView> data = new();

        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            data = (await s.QueryOver<Sudija>().ListAsync())
                           .Select(p => new SudijaView(p)).ToList();
        }
        catch (Exception)
        {
            return "Došlo je do greške prilikom prikupljanja informacija o sudijama.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return data;
    }

    public async static Task<Result<bool, ErrorMessage>> SacuvajSudijuAsync()
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sudija sud = new();

            await s.SaveOrUpdateAsync(sud);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return GetError("Nemoguće je dodati sudiju.", 404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<SudijaView, ErrorMessage>> VratiSudijuAsync(int id)
    {
        ISession? s = null;

        SudijaView sudijaView = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sudija sud = await s.LoadAsync<Sudija>(id);
            sudijaView = new SudijaView(sud);
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sudiju sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return sudijaView;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiSudijuAsync(int id)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sudija sud = await s.LoadAsync<Sudija>(id);

            await s.DeleteAsync(sud);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati sudiju.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion

    #region Igra

    public static Result<List<IgraView>, ErrorMessage> VratiIgra(int idSahista, int idPartija)
    {
        List<IgraView> igra = new();

        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Igra> igr = from i in s.Query<Igra>()
                                     where i.Id != null && i.Id.SahistaIgra != null && i.Id.SahistaIgra.Rbr == idSahista
                                     where i.Id != null && i.Id.IgraPartija != null && i.Id.IgraPartija.Id == idPartija
                                     select i;

            foreach (Igra i in igr)
            {
                igra.Add(new IgraView(i));
            }

            s.Close();

        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće vratiti odnos da li je sahista igrao partiju.".ToError(400);
        }

        return igra;
    }

    public async static Task<Result<bool, ErrorMessage>> DodajIgraAsync(IgraView igra)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Igra i = new()
            {
                Id = new IgraId
                {
                    SahistaIgra = await s.LoadAsync<Sahista>(igra.Id?.SahistaIgra?.Rbr),
                    IgraPartija = await s.LoadAsync<Partija>(igra.Id?.IgraPartija?.Id)
                }
            };

            await s.SaveOrUpdateAsync(i);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće dodati odnos izmedju sahiste i partije.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public static Result<bool, ErrorMessage> IzmeniIgra(IgraView igr)
    {
        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IgraId id = new()
            {
                SahistaIgra = s.Load<Sahista>(igr.Id?.SahistaIgra?.Rbr),
                IgraPartija = s.Load<Partija>(igr.Id?.IgraPartija?.Id)
            };

            Igra i = s.Load<Igra>(id);

            s.SaveOrUpdate(i);
            s.Flush();
            s.Close();
        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće izmeniti odnos izmedju sahiste i partije.".ToError(400);
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiIgraAsync(int idSahista, int idPartija)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahista sa = await s.LoadAsync<Sahista>(idSahista);
            Partija par = await s.LoadAsync<Partija>(idPartija);
            IgraId id = new IgraId()
            {
                SahistaIgra = sa,
                IgraPartija = par
            };
            Igra igra = await s.LoadAsync<Igra>(id);

            await s.DeleteAsync(igra);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati odnos igra.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion

    #region Organizuje

    public static Result<List<OrganizujeView>, ErrorMessage> VratiOrganizuje(int idOrganizator, int idTurnir)
    {
        List<OrganizujeView> organizuje = new();

        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Organizuje> org = from o in s.Query<Organizuje>()
                                    where o.Id != null && o.Id.OrganizatorOrganizuje != null && o.Id.OrganizatorOrganizuje.Id == idOrganizator
                                    where o.Id != null && o.Id.OrganizujeSahovski_Turnir != null && o.Id.OrganizujeSahovski_Turnir.Id == idTurnir
                                    select o;

            foreach (Organizuje o in org)
            {
                organizuje.Add(new OrganizujeView(o));
            }

            s.Close();

        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće vratiti odnos da li je organizator organizovao turnir.".ToError(400);
        }

        return organizuje;
    }

    public async static Task<Result<bool, ErrorMessage>> DodajOrganizujeAsync(OrganizujeView organizuje)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Organizuje o = new()
            {
                Id = new OrganizujeId
                {
                    OrganizatorOrganizuje = await s.LoadAsync<Organizator>(organizuje.Id?.OrganizatorOrganizuje?.Id),
                    OrganizujeSahovski_Turnir = await s.LoadAsync<Sahovski_Turnir>(organizuje.Id?.OrganizujeSahovski_Turnir?.Id)
                }
            };

            await s.SaveOrUpdateAsync(o);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće dodati odnos izmedju organizatora i turnira.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public static Result<bool, ErrorMessage> IzmeniOrganizuje(OrganizujeView org)
    {
        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            OrganizujeId id = new()
            {
                OrganizatorOrganizuje = s.Load<Organizator>(org.Id?.OrganizatorOrganizuje?.Id),
                OrganizujeSahovski_Turnir = s.Load<Sahovski_Turnir>(org.Id?.OrganizujeSahovski_Turnir?.Id)
            };

            Organizuje o = s.Load<Organizuje>(id);

            s.SaveOrUpdate(o);
            s.Flush();
            s.Close();
        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće izmeniti odnos izmedju organizatora i turnira.".ToError(400);
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiOrganizujeAsync(int idOrganizator, int idTurnir)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Organizator org = await s.LoadAsync<Organizator>(idOrganizator);
            Sahovski_Turnir sah = await s.LoadAsync<Sahovski_Turnir>(idTurnir);
            OrganizujeId id = new OrganizujeId()
            {
                OrganizatorOrganizuje = org,
                OrganizujeSahovski_Turnir = sah
            };
            Organizuje organizuje = await s.LoadAsync<Organizuje>(id);

            await s.DeleteAsync(organizuje);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati odnos organizuje.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion

    #region UcestvujeNa

    public static Result<List<UcestvujeNaView>, ErrorMessage> VratiUcestvujeNa(int idSahista, int idTurnir)
    {
        List<UcestvujeNaView> ucestvujeNa = new();

        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<UcestvujeNa> un = from u in s.Query<UcestvujeNa>()
                                    where u.Id != null && u.Id.SahistaUcestvujeNa != null && u.Id.SahistaUcestvujeNa.Rbr == idSahista
                                    where u.Id != null && u.Id.UcestvujeNaSahovski_Turnir != null && u.Id.UcestvujeNaSahovski_Turnir.Id == idTurnir
                                    select u;

            foreach (UcestvujeNa u in un)
            {
                ucestvujeNa.Add(new UcestvujeNaView(u));
            }

            s.Close();

        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće vratiti odnos da li je sahista ucestvovao na turnir.".ToError(400);
        }

        return ucestvujeNa;
    }

    public async static Task<Result<bool, ErrorMessage>> DodajUcestvujeNaAsync(UcestvujeNaView ucestvujeNa)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            UcestvujeNa u = new()
            {
                Id = new UcestvujeNaId
                {
                    SahistaUcestvujeNa = await s.LoadAsync<Sahista>(ucestvujeNa.Id?.SahistaUcestvujeNa?.Rbr),
                    UcestvujeNaSahovski_Turnir = await s.LoadAsync<Sahovski_Turnir>(ucestvujeNa.Id?.UcestvujeNaSahovski_Turnir?.Id)
                }
            };

            await s.SaveOrUpdateAsync(u);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće dodati odnos izmedju sahiste i turnira.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public static Result<bool, ErrorMessage> IzmeniUcestvujeNa(UcestvujeNaView un)
    {
        try
        {
            ISession? s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            UcestvujeNaId id = new()
            {
                SahistaUcestvujeNa = s.Load<Sahista>(un.Id?.SahistaUcestvujeNa?.Rbr),
                UcestvujeNaSahovski_Turnir = s.Load<Sahovski_Turnir>(un.Id?.UcestvujeNaSahovski_Turnir?.Id)
            };

            UcestvujeNa u = s.Load<UcestvujeNa>(id);

            s.SaveOrUpdate(u);
            s.Flush();
            s.Close();
        }
        catch (Exception)
        {
            //handle exceptions
            //throw;
            return "Nemoguće izmeniti odnos izmedju sahiste i turnira.".ToError(400);
        }

        return true;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiUcestvujeNaAsync(int idSahista, int idTurnir)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Sahista sa = await s.LoadAsync<Sahista>(idSahista);
            Sahovski_Turnir sah = await s.LoadAsync<Sahovski_Turnir>(idTurnir);
            UcestvujeNaId id = new UcestvujeNaId()
            {
                SahistaUcestvujeNa = sa,
                UcestvujeNaSahovski_Turnir = sah
            };
            UcestvujeNa ucestvujeNa = await s.LoadAsync<UcestvujeNa>(id);

            await s.DeleteAsync(ucestvujeNa);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati odnos ucestvuje na.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion
}
