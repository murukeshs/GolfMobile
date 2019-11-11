namespace Golf.Models
{
    public class CountryModel
    {
        public int countryId { get; set; }
        public string countryName { get; set; }
    }

    public class StateModel
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
    }
}
