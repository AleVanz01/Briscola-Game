using System;

namespace Test
{
    internal class Program
    {
        /*static void Main(string[] args)//Prova Login
        {
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Environment.CurrentDirectory + "\\Briscola.mdb");
            DataTable utenti;
            utenti = new DataTable("Utenti");
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM GIOCATORI", connection);
            adapter.Fill(utenti);
        }*/
        private static void Main(string[] args)//Prova partita
        {
            Giocatore g = new Giocatore("a", "a", "a", "a", "1");/*
            giocatori.Add(new Giocatore("b", "b", "b", "b", 2));
            giocatori.Add(new Giocatore("c", "c", "c", "c", 3));
            giocatori.Add(new Giocatore("d", "d", "d", "d", 4));*/

            Partita p = new Partita(3, g, "trevisane");
            //g.MazzoGiocatore = new List<Carta>();
            g.MazzoGiocatore.Add(p.Pesca(1));
            g.MazzoGiocatore.Add(p.Pesca(2));
            g.MazzoGiocatore.Add(p.Pesca(3));
            for (int i = 0; i < 21; i++)
            {
                g.CartaGiocata = g.MazzoGiocatore[0];

                if (p.giocatori[0].Username == g.Username)
                {
                    p.giocatori[0] = g;
                    if (i == 0)
                    {
                        p.giocatori[1].MazzoGiocatore.Add(p.Pesca(4));
                        p.giocatori[1].MazzoGiocatore.Add(p.Pesca(5));
                        p.giocatori[1].MazzoGiocatore.Add(p.Pesca(6));
                        //3 GIOCATORI
                        p.giocatori[2].MazzoGiocatore.Add(p.Pesca(7));
                        p.giocatori[2].MazzoGiocatore.Add(p.Pesca(8));
                        p.giocatori[2].MazzoGiocatore.Add(p.Pesca(9));
                    }
                }
                else if (p.giocatori[1].Username == g.Username)
                {
                    p.giocatori[1] = g;
                    if (i == 0)
                    {
                        p.giocatori[0].MazzoGiocatore.Add(p.Pesca(4));
                        p.giocatori[0].MazzoGiocatore.Add(p.Pesca(5));
                        p.giocatori[0].MazzoGiocatore.Add(p.Pesca(6));
                        //3 GIOCATORI
                        p.giocatori[2].MazzoGiocatore.Add(p.Pesca(7));
                        p.giocatori[2].MazzoGiocatore.Add(p.Pesca(8));
                        p.giocatori[2].MazzoGiocatore.Add(p.Pesca(9));
                    }
                }
                else
                {
                    p.giocatori[2] = g;
                    if (i == 0)
                    {
                        p.giocatori[0].MazzoGiocatore.Add(p.Pesca(4));
                        p.giocatori[0].MazzoGiocatore.Add(p.Pesca(5));
                        p.giocatori[0].MazzoGiocatore.Add(p.Pesca(6));
                        //3 GIOCATORI
                        p.giocatori[1].MazzoGiocatore.Add(p.Pesca(7));
                        p.giocatori[1].MazzoGiocatore.Add(p.Pesca(8));
                        p.giocatori[1].MazzoGiocatore.Add(p.Pesca(9));
                    }
                }


                p.GiocaTurno(i);

            }

            Console.ReadKey();
        }

    }
}
