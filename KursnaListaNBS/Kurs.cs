namespace KursnaListaNBS
{
    public class Kurs
    {
        private string _znakValute;
        public string OznakaValute
        {
            get { return _znakValute; }
            set
            {
                if (value.Length == 3)
                {
                    _znakValute = value;
                }
            }
        }
        public int SifraValute { get; set; }
        public string NazivZemlje { get; set; }
        public int Odnos { get; set; }
        public decimal SrednjiKurs { get; set; }
        public override string ToString()
        {
            return $"{SifraValute} : {NazivZemlje} : {OznakaValute} : {Odnos} : {SrednjiKurs}";
        }
    }
}
