using Microsoft.AspNetCore.Mvc;
using Transport.Models.Responses.Hello;

namespace Transport.Controllers;

[ApiController]
[Route("api/hello-world")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public ActionResult<HelloResponse> Get()
    {
        return Ok(new HelloResponse
        {
            Message = "Hello world!"
        });
    }
}