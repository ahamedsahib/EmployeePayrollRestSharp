using EmployeePayrollRestSharpAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollTestProject
{
    [TestClass]
    public class PayrollRestSharpTest
    {
        //Initializing the restclient 
        RestClient client;

        [TestInitialize]

        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        public IRestResponse GetAllEmployee()
        {
            //Get request from json server
            RestRequest request = new RestRequest("/employees", Method.GET);
            //Requesting server and execute 
            IRestResponse response = client.Execute(request);

            return response;
        }

        [TestMethod]
        public void TestMethodToRetrieveAllEmployees()
        {
            try
            {
                
                IRestResponse response = GetAllEmployee();
                //converting response to list og objects
                var res = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
                //Check whether all contents are received or not
                Assert.AreEqual(4, res.Count);
                //Checking the response statuscode 200-ok
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                res.ForEach((x) =>
                {
                    Console.WriteLine($"id = {x.id} , name = {x.name} , salary = {x.salary}");
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
