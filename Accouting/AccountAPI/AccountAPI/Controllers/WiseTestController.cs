using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WiseTestController : ControllerBase
    {
        int countid = 0;
        // GET: api/<WiseTestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<WiseTestController>/5
        [HttpGet("{id}")]
        public int Get(int id)
        {
            countid++;
            return countid + id;
        }

        // POST api/<WiseTestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<WiseTestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WiseTestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
