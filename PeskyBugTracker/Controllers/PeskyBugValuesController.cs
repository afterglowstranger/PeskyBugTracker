//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeskyBugTracker.Data;
using PeskyBugTracker.Models;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PeskyBugTracker.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class PeskyBugValuesController : ApiController //ControllerBase
    {
        private readonly PeskyBugTrackerContext _context;

        public PeskyBugValuesController(PeskyBugTrackerContext context)
        {
            _context = context;
        }

        // GET: api/<ValuesController>
        //[HttpGet]
        //public Task<List<PeskyBug>> Get()
        //{
        //    var bugs = _context.PeskyBugs.Where(b => b.State != PeskyBug.eState.Closed).ToList();
        //    foreach (var item in bugs)
        //    {
        //        item.AssignedToName = _context.Agents.Where(a => a.Id == item.AssignedTo).FirstOrDefault().FullName();
        //    }

        //    var jsonBugs = Newtonsoft.Json.JsonConvert.SerializeObject(bugs);
        //    //ViewBag.BugsList = "Open";

        //    //return new string[] { "value1", "value2" };
        //    return Request.CreateResponse<PeskyBug>(HttpStatusCode.Created, bugs);
        //}





        // GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        [HttpGet]
        public ResponseModel GetAllOpenBugs()
        {
            List<PeskyBug> bugData = new List<PeskyBug>();

            ResponseModel _responseModel = new ResponseModel();

            var response = _context.PeskyBugs.Where(b => b.State != PeskyBug.eState.Closed).ToList();

            foreach (var bug in response) { 
                bug.Notes = _context.Notes.Where(m => m.PeskyBugID == bug.ID).ToList();
            }

            _responseModel.Date = response;
            _responseModel.Status = true;
            _responseModel.Message = "Data Received successfully";
            return _responseModel;
        }


        // GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        [HttpGet]
        public ResponseModel GetAllBugs()
        {
            List<PeskyBug> bugData = new List<PeskyBug>();

            ResponseModel _responseModel = new ResponseModel();

            var response = _context.PeskyBugs.ToList();

            foreach (var bug in response)
            {
                bug.Notes = _context.Notes.Where(m => m.PeskyBugID == bug.ID).ToList();
            }

            _responseModel.Date = response;
            _responseModel.Status = true;
            _responseModel.Message = "Data Received successfully";
            return _responseModel;
        }





        // GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        public ResponseModel GetBug(Guid id)
        {
            ResponseModel _responseModel = new ResponseModel();

            var response = _context.PeskyBugs.Where(b => b.ID == id).FirstOrDefault();

            if (response != null)
            {
                response.Notes = _context.Notes.Where(m => m.PeskyBugID == id).ToList();
                _responseModel.Date = response;
                _responseModel.Status = true;
                _responseModel.Message = "Data Received successfully";
            }
            else {
                _responseModel.Date = response;
                _responseModel.Status = true;
                _responseModel.Message = "Bug not found";
            }

            
            return _responseModel;
            //return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }

    public class ResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public object Date { set; get; }
    }
}
