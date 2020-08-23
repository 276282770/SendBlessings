using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WXController
    {
        [HttpGet("BackCode")]
        public string BackCode(string code,string state)
        {
             
        }   
        [HttpGet("AA")]
    public string AA()
    {
        return "这是AA方法";
    }
    }

}
