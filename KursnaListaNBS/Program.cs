using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace KursnaListaNBS
{
    class Program
    {


        //https://nbs.rs/kursnaListaModul/srednjiKurs.faces?lang=lat
        ////*[@id="index:id31"]
        private const string NBSUrl = @"https://www.nbs.rs/export/sites/default/internet/latinica/scripts/kl_srednji.html";
        private const string NBSTabela = @"https://nbs.rs/kursnaListaModul/srednjiKurs.faces?lang=lat";
        

        public static async Task Main(string[] args)
        {
            if (args.Length > 0)
            {
                string destinationPath = args[0];
                await LoadTheLink(destinationPath);
            } else
            {
                await LoadTheLink();
            }
            
            KursnaLista novaLista = DeserializeJSON(Path.Combine(Directory.GetCurrentDirectory(), "utfjson.json"));

        }

        private static async Task LoadTheLink(string DestinationPath = "")
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage msg = await client.GetAsync(NBSTabela);
                    if (msg.IsSuccessStatusCode)
                    {
                        var clientRezults = await client.GetStringAsync(NBSTabela);
                        if (clientRezults.Length > 0)
                        {
                            DropToFile(clientRezults);
                            ParseHTML(DestinationPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var rezult = ex.Message;
                DropErrorToFile(rezult);
            }
        }

        private static void DropErrorToFile(string msg, string Destination="")
        {
            string path = string.Empty;
            
            
            if( Destination.Length > 0)
            {
                if(Directory.Exists(Destination))
                {
                    path = Destination;
                }
            }
            else
            {
                path = Directory.GetCurrentDirectory();
            }

            string filepath = Path.Combine(path, "error.txt");
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (StreamWriter sr = new StreamWriter(filepath))
            {
                sr.Write(msg);
            }
        }

        private static void DropToFile(string result)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            string filepath = Path.Combine(path, "NBSToParse.html");
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (StreamWriter sr = new StreamWriter(filepath))
            {
                sr.Write(result);
            }
        }

        private static void ParseHTML(string DstPath="")
        {
            List<string> TableHead = new List<string>();
            var htmlDoc = new HtmlDocument();
            var lista = new KursnaLista();
            string htmlPath = Path.Combine(Directory.GetCurrentDirectory(), "NBSToParse.html");
            htmlDoc.Load(htmlPath);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"index:srednjiKursLista\"]");
            try
            {

                var FormiranaNaDan = node.SelectSingleNode("//*[@id=\"index:id31\"]");
                string datumListe = FormiranaNaDan.InnerText;
                datumListe = datumListe.Remove(datumListe.Length - 1);
                lista.DatumKursneListe = DateTime.ParseExact(datumListe, "dd.MM.yyyy", new CultureInfo("sr-RS").DateTimeFormat);

                var tableHead = node.SelectSingleNode("//thead/tr");
                var tBodyId = node.SelectNodes("//*[@id=\"index:srednjiKursList:tbody_element\"]/tr");
                IspisiZaglavlje(TableHead, tableHead);
                for (int i = 0; i < tBodyId.Count; i++)
                {
                    var rtrs = tBodyId[i].ChildNodes;
                    Kurs valuta = new Kurs();
                    valuta.SifraValute = Convert.ToInt32(rtrs[0].InnerText);
                    valuta.NazivZemlje = rtrs[1].InnerText;
                    valuta.OznakaValute = rtrs[2].InnerText;
                    valuta.Odnos = Convert.ToInt32(rtrs[3].InnerText);
                    valuta.SrednjiKurs = Convert.ToDecimal(rtrs[4].InnerText);
                    lista.Lista.Add(valuta);
                }
                foreach (var item in lista.Lista)
                {
                    System.Console.WriteLine(item);
                }
                DropToJSON(lista, DstPath);
            }
            catch (Exception ex)
            {
                var rezult = ex.Message;
                DropErrorToFile(rezult);
            }
        }

        private static void DropToJSON(KursnaLista KursnaLista, string dstPath = "")
        {
            string GetCurrentDir = Directory.GetCurrentDirectory();
            if (dstPath.Length > 0 && Directory.Exists(dstPath))
            {
                GetCurrentDir = dstPath;
            }
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new UpperCaseNamingPolicy(),
                WriteIndented = true
            };
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(KursnaLista, options);
            File.WriteAllBytes(Path.Combine(GetCurrentDir, "utfjson.json"), jsonBytes);
        }

        private static void IspisiZaglavlje(List<string> TableHead, HtmlNode tableHead)
        {
            foreach (var item in tableHead.ChildNodes)
            {
                TableHead.Add(item.InnerText);
            }
            Console.Write(TableHead[0] + " | ");
            for (int i = 1; i < TableHead.Count; i++)
            {
                Console.Write(TableHead[i] + " | ");
            }
            Console.WriteLine();
        }

        private static KursnaLista DeserializeJSON(string filePath)
        {
            byte[] jsonBytes = File.ReadAllBytes(filePath);
            //string jsonString = File.ReadAllText(filePath);
            var kursnaLista = JsonSerializer.Deserialize<KursnaLista>(jsonBytes);
            return kursnaLista;
        }

    }
}
