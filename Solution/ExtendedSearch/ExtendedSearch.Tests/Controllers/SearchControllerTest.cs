using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Effort;
using ExtendedSearch.Controllers;
using ExtendedSearch.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedSearch.Tests.Controllers
{
	[TestClass]
	public class SearchControllerTest
	{
		[TestMethod]
		public void IndexTest()
		{
			// Arrange
			var controller = new SearchController();

			// Act
			var result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void GetResultTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);
			var searchInput = new SearchInput { Searchinput = "GetResultTest" };

			// Arrange
			var controller = new SearchController(entities);
			var result = controller.GetResult(searchInput);

			// Assert
			Assert.AreNotEqual(typeof (Task<JsonResult>), result);
		}
	}
}

