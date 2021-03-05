namespace ODataBenchmark.DataModel
{
	public class Address
    {
        public string City { get; set; }

        public string Street { get; set; }
    }

    public class CnAddress : Address
    {
        public string Postcode { get; set; }
    }

    public class UsAddress : Address
    {
        public string Zipcode { get; set; }
    }
}
