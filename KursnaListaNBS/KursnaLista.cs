using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KursnaListaNBS
{
    public class KursnaLista
    {
        [JsonPropertyName("Datum")]
        public DateTimeOffset DatumKursneListe { get; set; }
        public List<Kurs> Lista { get; set; } = new List<Kurs>();
    }
}
