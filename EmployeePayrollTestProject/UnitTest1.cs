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
        /// <summary>
        /// Method to add a json object to json server
        /// </summary>
        public IRestResponse AddEmployeeToJsonServer(JsonObject json)
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            //adding type as json in request and pasing the json object as a body of request
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            //Execute the request
            IRestResponse response = client.Execute(request);
            return response;

        }
        /// <summary>
        /// Test method to add a new employee to json server
        /// </summary>
        [TestMethod]
        public void TestMethodToAddEmployeeToJsonServer()
        {
            try 
            { 
            //Setting rest rquest to url and setiing method to post
            RestRequest request = new RestRequest("/employees", Method.POST);
            //object for json
            JsonObject json = new JsonObject();
            //Adding new employee details to json object
            json.Add("name", "Neymar");
            json.Add("salary", 13000);

            //calling method to add to server
            IRestResponse response = AddEmployeeToJsonServer(json);
            //deserialize json object to  class  object
            var res = JsonConvert.DeserializeObject<Employee>(response.Content);

            //Checking the response statuscode 201-created
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
