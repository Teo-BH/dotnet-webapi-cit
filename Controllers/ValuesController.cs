using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace dotnet_webapi_cit.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IDistributedCache cache;
  
        public ValuesController(IDistributedCache distributedCache) 
        {
            this.cache = distributedCache;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            if (id == 0) {
                id = GetValueCache();
            }
            return $"value {id}";
        }

        private int GetValueCache() {
            const string cacheKey = "IndexValue";
            string data = cache.GetString(cacheKey);
            int result = 1;
            if (Int32.TryParse(data, out result)) {
                result += 1;
            }

            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
            cache.SetString(cacheKey, result.ToString(), options);
  
            return result;
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
