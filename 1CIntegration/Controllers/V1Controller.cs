using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace _1CIntegration.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class V1Controller : ApiController
    {
		// GET api/v1/good 
		[Route("Good")]
		[HttpGet]
		public IHttpActionResult Good(int limit, int offset)
		{
			try
			{
				var result = new dynamic[] {
					new
					{
						id = 1,
						name = "Adidas Black White"
					}
				};
				return Ok(result);
			}
			catch (Exception error)
			{
				throw error;
			}
		}

		// GET api/v1/good/1
		[Route("good/:id")]
		[HttpGet]
		public IHttpActionResult GetGoodById(int id)
		{
			try
			{
				var result = new
				{
					name = "Adiads ...",
					location = "A100",
					image = "[base64]",
					available_sizes = new dynamic[] {
						new {
							size = 43,
						    cost = 1500,
						    count = 3
						}
					}
				};
				return Ok(result);
			}
			catch (Exception error)
			{
				throw error;
			}
		}
	}
}
