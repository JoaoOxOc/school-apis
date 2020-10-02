using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LoggingService.Models;
using LoggingService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq;
using Newtonsoft.Json;
using Utils.Enums;
using Utils.Extensions.Helper;
using Utils.Extensions;

namespace LoggingService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        ILogRepository repository { get; set; }

        public LogsController(ILogRepository logsRepository)
        {
            this.repository = logsRepository;
        }

        [HttpGet("testing")]
        public async Task<ActionResult<bool>> TestingApi()
        {
            var accessToken = Request.Headers["Authorization"];
            var audience = Request.Headers["Audience"];
            var scopes = Request.Headers["Scopes"];
            return Ok("Success from LoggingService");
        }

        [HttpGet]
        public ObjectResult Get(string searchText, int? activityType, string companyId, int? logType, DateTime? start, DateTime? end, int? offset, int? limit, string sort, string sortDirection)
        {
            var mapped = JwtHelper.GetTokenValues(HttpContext.Request.Headers["Authorization"], new string[] { "roles", "companyId" });

            ObjectResult response;

            try
            {
                bool isSysAdmin = PermissionsHelper.CheckUserIsSysAdmin(mapped["roles"]);
                bool isCompanyAdmin = PermissionsHelper.CheckUserIsCompanyAdmin(mapped["roles"]);
                if (isSysAdmin || isCompanyAdmin)
                {
                    searchText = string.IsNullOrEmpty(searchText) ? string.Empty : searchText;

                    List<Log> result = this.repository.GetFiltered(searchText, activityType, mapped["companyId"], logType, start, end, offset, limit, sort, sortDirection).ToList<Log>();

                    var totalElements = this.repository.Count(searchText, activityType, mapped["companyId"], logType, start, end, 1, null, sort);

                    Request.HttpContext.Response.Headers.Add("X-Total-Count", totalElements.ToString());

                    if (result.Count() > 0)
                    {
                        response = StatusCode((int)HttpStatusCode.OK, result);
                    }
                    else
                    {
                        response = StatusCode((int)HttpStatusCode.NotFound, new List<Log>());
                    }
                }
                else
                {
                    response = StatusCode((int)HttpStatusCode.Forbidden, null);
                }

            }
            catch (Exception ex)
            {
                response = StatusCode(500, ex.Message);
            }

            return response;
        }
    }
}