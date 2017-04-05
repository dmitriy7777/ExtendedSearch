namespace ExtendedSearch.Models
{
	public class SearchResponce
	{
		public string Response { get; set; }
		public string EngName { get; set; }


		public SearchResponce(string response, string engName)
		{
			Response = response;
			EngName = engName;
		}
	}
}