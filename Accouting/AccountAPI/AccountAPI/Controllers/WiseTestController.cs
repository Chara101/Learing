using System.Data.Common;
using AccountAPI.DataStorage;
using AccountAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TestAcounting.DataStorage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WiseTestController : ControllerBase
    {
        IDataStorage _db = new MssqlCtrl();
        // GET: api/<WiseTestController>
        [HttpGet]
        public IEnumerable<RecordForm> Get()
        {
            //List<string> result = new List<string>();
            //List<RecordForm> result = _db.GetAllRecords();
            //foreach (var r in records)
            //{
            //    result.Add(r.Id.ToString() + "," + r.Date.ToString("yyyy/MM/dd") + "," + r.Category + "," + r.SubCategory + "," + r.Amount.ToString() + "," + r.SubCount.ToString() + "," + r.SubAmount.ToString() + "," + r.Comment);
            //}

            List<RecordForm> result = new List<RecordForm>();

            //result.Add(new RecordForm()
            //{
            //    Id = 1,
            //    Date = DateTime.Now,
            //    Category = "TestCategory",
            //    SubCategory = "TestSubCategory",
            //    Amount = 1000,
            //    SubCount = 1,
            //    SubAmount = 1000,
            //    Comment = "TestComment" 
            //});

            result = _db.GetAllRecords();
            
            //SqlConnection connectionstring = new SqlConnection("Server=MSI;Database=Cash;User Id=Apple;Password=ApplePen;Encrypt=true;TrustServerCertificate=True;");
            //connectionstring.Open();


            return result;
        }

        // GET api/<WiseTestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "id: " + id.ToString();
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
