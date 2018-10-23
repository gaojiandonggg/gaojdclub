using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaoJD.Club.BusinessEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GaoJD.Club.OneTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProtoController : ControllerBase
    {
        List<ProtoBufTest> protoBufTests = new List<ProtoBufTest>();
        public ProtoController()
        {
            protoBufTests.Add(new ProtoBufTest() { ID = 0, Name = "高建东", Address = "山西" });
        }

        [HttpGet]
       // [Produces("application/proto")]
        public async Task<List<ProtoBufTest>> GetListProtoBufTest()
        {
            return protoBufTests;
        }
    }
}