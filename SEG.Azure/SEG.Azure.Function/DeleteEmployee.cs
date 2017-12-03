using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using SEG.Azure.Business;
using System.Threading.Tasks;

namespace SEG.Azure.Function
{
    public static class DeleteEmployee
    {
        [FunctionName("DeleteEmployee")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete",
            Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            string name = string.Empty;

            try
            {
                log.Info("SEG Delete Employee HTTP Function triggered...");

                // parse query parameter
                name = req.GetQueryNameValuePairs()
                    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                    .Value;

                // Get request body
                //dynamic data = await req.Content.ReadAsAsync<object>();

                // Set name to query string or body data
                //name = name ?? data?.name;

                int employeedeletedcount = 0;

                EmployeeBAL employeebal = new EmployeeBAL();
                employeedeletedcount = employeebal.DeleteEmployee(int.Parse(name));
            }
            catch(System.Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, 
                    "Inside catch - " + ex.StackTrace);
                //log.Info(ex.Message + ex.StackTrace);
            }

            return name == null
                    ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                    : req.CreateResponse(HttpStatusCode.OK, "Employee " + name + " deleted!");
        }
    }
}
