namespace Briscola.Models
{
    public class InfoGiocatore
    {
        public InfoGiocatore(Giocatore giocatore, Statistiche statistiche)
        {
            Username = $"Username: {giocatore.Username}";
            Cognome = $"Cognome: {giocatore.Cognome}";
            Nome = $"Nome: {giocatore.Nome}";
            Eta = $"Età: {giocatore.Eta}";
            PunteggioTotale = $"Punti Totali: {statistiche.PunteggioTotale}";
            PunteggioMaxPartita = $"Punti Max in partita: {statistiche.PunteggioMaxPartita}";
            PartiteVinte = $"Partite Vinte: {statistiche.PartiteVinte}";
            PartitePerse = $"Partite Perse: {statistiche.PartitePerse}";
        }

        public InfoGiocatore()
        {
            Username = $"Username:";
            Cognome = $"Cognome:";
            Nome = $"Nome:";
            Eta = $"Età:";
            PunteggioTotale = $"Punti Totali:";
            PunteggioMaxPartita = $"Punti Max in partita:";
            PartiteVinte = $"Partite Vinte:";
            PartitePerse = $"Partite Perse:";
        }

        public string Username { get; set; }

        public string Cognome { get; set; }

        public string Nome { get; set; }

        public string Eta { get; set; }

        public string PunteggioTotale { get; private set; }

        public string PunteggioMaxPartita { get; private set; }

        public string PartiteVinte { get; private set; }

        public string PartitePerse { get; private set; }
    }
}
