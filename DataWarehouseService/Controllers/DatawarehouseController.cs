using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Description;
using BLL.Interfaces;
using NLog;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataWarehouseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatawarehouseController : ControllerBase
    {
        
        IDatawarehouseManager _manager;
        private static ILogger<DatawarehouseController> _logger;

        public DatawarehouseController(IDatawarehouseManager manager, ILogger<DatawarehouseController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Data")]
        [Authorize]
        [ResponseType(typeof(DataTable))]
        public IActionResult Get([FromQuery] string queryName, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null, [FromQuery] int? CountryId = null)
        {
            try
            {
                var clientIdentity = AuthorizationHelper.GetClientIdentity(Request);
                //_logger.Info(clientIdentity?.UserIdentifier, Request?.RequestUri?.AbsoluteUri, Request?.Method?.Method, MethodBase.GetCurrentMethod(), null);
                var data = _manager.GetData(queryName, startDate, endDate, CountryId, clientIdentity);
                return Ok(data);
            }
            catch (UnauthorizedAccessException ex)
            {
                //_logger.LogError(ex, MethodBase.GetCurrentMethod());
                return Unauthorized();
            }
            catch (Exception ex)
            {
               // _logger.LogMethodFailure(ex, MethodBase.GetCurrentMethod());
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("DataQueries")]
        [Authorize]
        public IActionResult GetQueries()
        {
            try
            {
                 var clientIdentity = AuthorizationHelper.GetClientIdentity(Request);
                //_logger.Info(clientIdentity?.UserIdentifier, Request?.RequestUri?.AbsoluteUri, Request?.Method?.Method, MethodBase.GetCurrentMethod(), null);
               
                var data = _manager.GetDataQueries(clientIdentity);
               //var result = EntityToDataConverter.Convert<ICollection<BE.DataQuery>, ICollection<DTO.Data.DataQuery>>(data);
                return Ok(data);
            }
            catch (UnauthorizedAccessException ex)
            {
               // _logger.LogMethodFailure(ex, MethodBase.GetCurrentMethod());
                return Unauthorized();
            }
            catch (Exception ex)
            {
               // _logger.LogMethodFailure(ex, MethodBase.GetCurrentMethod());
                return BadRequest();
            }
        }
    }
}
