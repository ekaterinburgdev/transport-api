using Microsoft.AspNetCore.Mvc;
using Transport.Clients;
using Transport.Models.Responses.Hello;

namespace Transport.Controllers;

[ApiController]
[Route("api/transport")]
public class TransportController : ControllerBase
{
    private EttuClient ettuClient;

    public TransportController(EttuClient ettuClient)
    {
        this.ettuClient = ettuClient;
    }
    
    [HttpGet]
    public async Task<ActionResult<Models.Transport[]>> Get()
    {
        var result = await ettuClient.GetAllTransportsAsync();
        return Ok(result);
    }
}