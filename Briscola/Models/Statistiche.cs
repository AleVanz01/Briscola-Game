using System.Data;
using System.Data.OleDb;

namespace Briscola.Models
{
    public class Statistiche
    {
        public Statistiche(OleDbConnection connection, Giocatore giocatore)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter($"SELECT * FROM STATISTICHE WHERE USERNAME = '{giocatore.Username}'", connection);
            DataTable table = new DataTable("Statistiche");
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                PartiteVinte = (int)table.Rows[0][1];
                PartitePerse = (int)table.Rows[0][2];
                PunteggioTotale = (int)table.Rows[0][3];
                PunteggioMaxPartita = (int)table.Rows[0][4];
            }
        }

        public int PartiteVinte { get; private set; }

        public int PartitePerse { get; private set; }

        public int PunteggioTotale { get; private set; }

        public int PunteggioMaxPartita { get; private set; }
    }
}
