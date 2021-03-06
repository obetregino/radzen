using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNet.OData.Query;



namespace HierarchyWithTwoDataGrid.Controllers.Northwind
{
  using Models;
  using Data;
  using Models.Northwind;

  [ODataRoutePrefix("odata/Northwind/Employees")]
  [Route("mvc/odata/Northwind/Employees")]
  public partial class EmployeesController : ODataController
  {
    private Data.NorthwindContext context;

    public EmployeesController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Employees
    [EnableQuery(MaxExpansionDepth=10)]
    [HttpGet]
    public IEnumerable<Models.Northwind.Employee> GetEmployees()
    {
      var items = this.context.Employees.AsQueryable<Models.Northwind.Employee>();

      this.OnEmployeesRead(ref items);

      return items;
    }

    partial void OnEmployeesRead(ref IQueryable<Models.Northwind.Employee> items);

    [EnableQuery(MaxExpansionDepth=10)]
    [HttpGet("{EmployeeID}")]
    public SingleResult<Employee> GetEmployee(int key)
    {
        var items = this.context.Employees.Where(i=>i.EmployeeID == key);

        return SingleResult.Create(items);
    }
    partial void OnEmployeeDeleted(Models.Northwind.Employee item);

    [HttpDelete("{EmployeeID}")]
    public IActionResult DeleteEmployee(int key) 
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var item = this.context.Employees
                .Where(i => i.EmployeeID == key)
                .Include(i => i.Orders)
                .Include(i => i.EmployeeTerritories)
                .Include(i => i.Employees1)
                .FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }                

            this.OnEmployeeDeleted(item);
            this.context.Employees.Remove(item);
            this.context.SaveChanges();

            return new NoContentResult();
        }
        catch(Exception ex) 
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnEmployeeUpdated(Models.Northwind.Employee item);

    [HttpPut("{EmployeeID}")]
    public IActionResult PutEmployee(int key, [FromBody]Models.Northwind.Employee newItem)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newItem == null || (newItem.EmployeeID != key))
            {
                return BadRequest();
            }

            this.OnEmployeeUpdated(newItem);
            this.context.Employees.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Employees
                .Where(i => i.EmployeeID == key)
                .Include(i => i.Employee1)
                .FirstOrDefault();

            return new JsonResult(itemToReturn, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc
            })
            {
                StatusCode = 200
            };
        }
        catch(Exception ex) 
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPatch("{EmployeeID}")]
    public IActionResult PatchEmployee(int key, [FromBody]Delta<Models.Northwind.Employee> patch)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = this.context.Employees.Where(i=>i.EmployeeID == key).FirstOrDefault();
            
            if (item == null)
            {
                return BadRequest();
            }

            patch.Patch(item);

            this.OnEmployeeUpdated(item);
            this.context.Employees.Update(item);
            this.context.SaveChanges();

            var itemToReturn = this.context.Employees
                .Where(i => i.EmployeeID == key)
                .Include(i => i.Employee1)
                .FirstOrDefault();

            return new JsonResult(itemToReturn, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc
            })
            {
                StatusCode = 200
            };
        }
        catch(Exception ex) 
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnEmployeeCreated(Models.Northwind.Employee item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Employee item)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item == null)
            {
                return BadRequest();
            }

            this.OnEmployeeCreated(item);
            this.context.Employees.Add(item);
            this.context.SaveChanges();

            var key = item.EmployeeID;
            var itemToReturn = this.context.Employees
                .Where(i => i.EmployeeID == key)
                .Include(i => i.Employee1)
                .FirstOrDefault();

            return new JsonResult(itemToReturn, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc
            })
            {
                StatusCode = 201
            };
        }
        catch(Exception ex) 
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
