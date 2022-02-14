using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace TestProject1
{
    public class WebScraper
    {
        WebClient client = new WebClient();

        public string Getresults(string lenguajes)
        {
            string msg = string.Empty;
            var listLanguages = UtilFactory.formatInputs(lenguajes);

            List<RESULTS> ListaResultados = new List<RESULTS>();
            List<ENGINE> ListaMotores = new List<ENGINE>();

            ListaMotores = PopulateEngine();

            if (listLanguages.Count > 0)
            {
                foreach (LANGUAGE lenguaje in listLanguages)
                {
                    foreach (ENGINE motor in ListaMotores)
                    {
                        var html = DownloadHtml(lenguaje.DESCRIPTION, motor.URL);
                        MatchCollection mc = SelectNodes(html, motor.REGEX);
                        var result = GetResultValue(mc).Replace(".", "");

                        ListaResultados.Add(new RESULTS()
                        {
                            SEARCHRESULTS = string.IsNullOrEmpty(result) ? 0 : Convert.ToInt64(result),
                            LANGUAGE = new LANGUAGE
                            {
                                DESCRIPTION = lenguaje.DESCRIPTION
                            },
                            ENGINE = motor
                        });
                    }
                }
            }

            if (ListaResultados.Count > 0)
                msg = QueryScores(listLanguages, ListaMotores, ListaResultados);
            else
                msg = "Results are not found.";

            return msg;
        }

        private string DownloadHtml(string query, string searchEngine)
        {
            if (!string.IsNullOrEmpty(query))
                return client.DownloadString(searchEngine.Replace("QUERY_SEARCH", Uri.EscapeDataString(query)));

            return null;
        }

        private MatchCollection SelectNodes(string html, string engineRegex)
        {
            return Regex.Matches(html, engineRegex, RegexOptions.Singleline);
        }

        private string GetResultValue(MatchCollection myMatchCollection)
        {
            string result = "0";

            if (myMatchCollection.Count > 0)
            {
                string value = myMatchCollection[0].Groups[1].Value;
                value = UtilFactory.clearHtml(value);
                value = UtilFactory.clearLetters(value);
                return value;
            }

            return result;
        }

        private List<ENGINE> PopulateEngine()
        {
            List<ENGINE> enginelist = new List<ENGINE>();

            enginelist.Add(new ENGINE()
            {
                DESCRIPTION = "bing",
                URL = "https://www.bing.com/search?q=QUERY_SEARCH&form=QBLH",
                REGEX = "<span class=\"sb_count\">\\s*(.+?)\\s*</span>"
            });

            enginelist.Add(new ENGINE()
            {
                DESCRIPTION = "yahoo",
                URL = "https://pe.search.yahoo.com/search?p=QUERY_SEARCH",
                REGEX = "<div class=\"compTitle fc-smoke\">\\s*(.+?)\\s*</div>"
            });

            return enginelist;
        }

        private string QueryScores(List<LANGUAGE> lenguajes, List<ENGINE> motores, List<RESULTS> resultados)
        {
            string cadena = string.Empty;
            Int64 mayorresult = 0;
            string totalwinner = string.Empty;

            foreach (var lenguaje in lenguajes)
            {
                string aux = string.Empty;
                Int64 acumresult = 0;

                foreach (var resultado in resultados)
                {
                    if (lenguaje.DESCRIPTION == resultado.LANGUAGE.DESCRIPTION)
                    {
                        aux += " " + resultado.ENGINE.DESCRIPTION + " : " + resultado.SEARCHRESULTS.ToString();
                        acumresult += resultado.SEARCHRESULTS;
                    }
                }

                if (acumresult > mayorresult)
                {
                    mayorresult = acumresult;
                    totalwinner = "Total winner: " + lenguaje.DESCRIPTION;
                }

                cadena += lenguaje.DESCRIPTION + " : " + aux + "\n";
            }

            string winners = string.Empty;

            foreach (var motor in motores)
            {
                string lenguajewinner = string.Empty;
                Int64 mayor = 0;

                foreach (var resultado in resultados)
                {
                    if (motor.DESCRIPTION == resultado.ENGINE.DESCRIPTION)
                    {
                        if (resultado.SEARCHRESULTS > mayor)
                        {
                            mayor = resultado.SEARCHRESULTS;
                            lenguajewinner = resultado.LANGUAGE.DESCRIPTION;
                        }
                    }
                }

                winners += motor.DESCRIPTION + " winner : " + lenguajewinner + "\n";
            }

            cadena = cadena + "\n" + winners + "\n" + totalwinner;

            return cadena;
        }

    }
}
