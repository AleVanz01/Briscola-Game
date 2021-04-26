using System.Collections.Generic;
using System.Data.OleDb;

namespace Briscola.Models
{
    public class Giocatore
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Cognome { get; set; }

        public string Nome { get; set; }

        public string Eta { get; set; }

        public int Punti { get; set; }

        public List<Carta> MazzoPunti { get; set; }

        public List<Carta> MazzoGiocatore { get; set; }

        public Carta CartaGiocata { get; set; }

        public Giocatore(string username, string psw, string nome = "", string cognome = "", string eta = "")
        {
            Username = username;
            Password = psw;
            Nome = nome;
            Cognome = cognome;
            Eta = eta;
            MazzoGiocatore = new List<Carta>();
            MazzoPunti = new List<Carta>();
        }

        public Giocatore()
        {

        }

        public Statistiche GetPlayerStats(OleDbConnection connection)
        {
            Statistiche stats = new Statistiche(connection, this);
            return stats;
        }
    }
}
