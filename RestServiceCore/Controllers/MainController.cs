using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace RestServiceCore
{
    [Route("[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {

        private readonly ILogger<MainController> logger;

        public MainController(ILogger<MainController> logger)
        {
            this.logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MainData data)
        {
            if (data.Text == null) return BadRequest("Text required");

            await Task.Run(() => { DataStorage.Instance.Create(data); });

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            MainData data = await Task.Run(() => { return DataStorage.Instance.Read(id); });

            if (data == null) return NotFound();

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromQuery] bool createIfNotExisting, [FromBody] MainData data)
        {
            if (data.Text == null) return BadRequest("Text required");

            if ((!createIfNotExisting) && await Task.Run(() => { return DataStorage.Instance.Read(id); }) == null)
            {
                return NotFound();
            }

            data.Id = id;

            await Task.Run(() => { DataStorage.Instance.Update(data); }); 

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool errorIfNotExisting)
        {
            if (errorIfNotExisting && await Task.Run(() => { return DataStorage.Instance.Read(id); }) == null)
            {
                return NotFound();
            }

            await Task.Run(() => { DataStorage.Instance.Delete(id); });

            return Ok();
        }
    }
}
