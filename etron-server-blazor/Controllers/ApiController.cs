using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EtronServer.Core.Helpers;

namespace EtronServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController: ControllerBase
    {
        public ApiController()
        {

        }


        [HttpGet]
        [Route("version")]
        public string GetAssemblyVersion()
        {
            return AssemblyHelper.GetAssemblyVersion();
        }
    }

    [ApiController]
    [Route("/api/v1/games")]
    public class GameController: ControllerBase
    {
        public GameController()
        {
            
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await Task.FromResult<IActionResult>(
                Ok(new 
                {
                    Id = id,
                    Pin = 29596,
                    CreatedDate = DateTime.UtcNow
                })
            );
        }
    }
}