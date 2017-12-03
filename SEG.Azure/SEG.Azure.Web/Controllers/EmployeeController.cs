using AutoMapper;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using SEG.Azure.Business;
using SEG.Azure.Entities;
using SEG.Azure.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace SEG.Azure.Web.Controllers
{
    public class FunctionTraceWriter : TraceWriter
    {
        public FunctionTraceWriter() : base(System.Diagnostics.TraceLevel.Error)
        {
            
        }

        public override void Trace(TraceEvent traceEvent)
        {
            
        }
    }

    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            List<EmployeeViewModel> employeelist = new List<EmployeeViewModel>();

            try
            {
                EmployeeBAL employeebal = new EmployeeBAL();

                List<Employee> employeegenericlist = employeebal.GetEmployees(0);
                var employeeDto = Mapper.Map<List<EmployeeViewModel>>(employeegenericlist);

                return View(employeeDto);
            }
            catch (Exception ex)
            {
                
            }

            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeeViewModel employeemodel)
        {
            try
            {
                EmployeeBAL employeebal = new EmployeeBAL();
                var employeeDto = Mapper.Map<Employee>(employeemodel);

                if (ModelState.IsValid)
                {
                    employeebal.InsertEmployee(employeeDto);
                }

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                CloudQueue queue = queueClient.GetQueueReference("employeequeue");
                queue.CreateIfNotExists();
                CloudQueueMessage message = 
                    new CloudQueueMessage(employeeDto.FName + " " + employeeDto.LName + " added to blob!");
                queue.AddMessage(message);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            List<EmployeeViewModel> employeelist = new List<EmployeeViewModel>();

            try
            {
                EmployeeBAL employeebal = new EmployeeBAL();

                List<Employee> employeegenericlist = employeebal.GetEmployees(id);
                var employeeDto = Mapper.Map<EmployeeViewModel>(employeegenericlist[0]);

                return View(employeeDto);
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel employeemodel)
        {
            try
            {
                EmployeeBAL employeebal = new EmployeeBAL();
                var employeeDto = Mapper.Map<Employee>(employeemodel);

                if (ModelState.IsValid)
                {
                    employeebal.InsertEmployee(employeeDto);
                }

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                FunctionTraceWriter writer = new FunctionTraceWriter();
                writer.Level = System.Diagnostics.TraceLevel.Verbose;


                //string uri = "https://segfunctionapp.azurewebsites.net/api/DeleteEmployee?code=hmW5Ey4czBHJQNK98CuQf3Vsa6RfCMBawU1IP40ELzWE5BsDrBq5ag==" +
                //    "&name=" + id.ToString();

                string uri = "https://segfunctionapp.azurewebsites.net/api/DeleteEmployee?name=" + id.ToString();

                HttpRequestMessage msg = new HttpRequestMessage();
                msg.Method = HttpMethod.Delete;
                msg.RequestUri = new Uri(uri);

                System.Threading.Tasks.Task<HttpResponseMessage> response = SEG.Azure.Function.DeleteEmployee.Run(msg, writer);

                ViewBag.EmployeeDeleted = "Employee - " + id.ToString() + " deleted!";
            }
            catch (Exception ex)
            {
                throw ex;

                //EmployeeBAL employeebal = new EmployeeBAL();
                //employeebal.DeleteEmployee(id);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Bot()
        {
            return View();
        }
    }
}