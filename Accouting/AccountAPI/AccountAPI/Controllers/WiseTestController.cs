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
        [HttpGet("search/GetAllRecord")]
        public IEnumerable<RecordForm> GetAllRecord()
        {
            List<RecordForm> result = new List<RecordForm>();
            result = _db.GetAllRecords();
            return result;
        }
        [HttpGet("login")]
        public string Login([FromQuery] int id, string password)
        {
            return "Login Success";
        }

        [HttpPost("search/GetRecord")]
        public IEnumerable<RecordForm> GetRecord([FromBody] RecordForm r)
        {
            List<RecordForm> result = new List<RecordForm>();
            result = _db.GetRecordsBy(r);
            return result;
        }
        
        [HttpPost("search/GetRecordInRange")]
        public IEnumerable<RecordForm> GetRecordInRange([FromBody] BandRecord r)
        {
            List<RecordForm> result = new List<RecordForm>();
            result = _db.GetRecordsBy(r.r1, r.r2);
            return result;
        }

        [HttpPost("search/GetTotals")]
        public RecordForm GetTotals([FromBody] RecordForm r)
        {
            RecordForm result = new RecordForm();
            result = _db.GetTotals(r);
            return result;
        }

        [HttpPost("add/AddObject")]
        public void AddObject([FromBody] RecordForm value)
        {
            _db.Add(value);
        }

        [HttpPut("renew")]
        public void Renew([FromBody] BandRecord r)
        {
            _db.Update(r.r1, r.r2);
        }

        // DELETE api/<WiseTestController>/5
        [HttpDelete("delete")]
        public void Delete(int id)
        {
            _db.Remove(new RecordForm() { Id = id });
        }
    }
}
