using System;
using System.Linq.Expressions;
using System.Net;
using Entities.Institution;
using InstitutionsService.Code;
using InstitutionsService.Services.RabbitMQ;
using InstitutionsService.Services.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Utils.Enums;

namespace InstitutionsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionsController : ControllerBase
    {
        protected IUnitOfWork uow { get; set; }
        protected IRabbitMQConnection rabbitMQ;

        public InstitutionsController(IUnitOfWork unitOfWork, IRabbitMQConnection rabbitMQ)
        {
            this.uow = unitOfWork;
            this.rabbitMQ = rabbitMQ;
        }

        [HttpGet]
        public ObjectResult Get(string searchText, string region, string country, int? offset, int? limit, string sort, string sortDirection)
        {
            //var mapped = JwtHelper.GetTokenValues(HttpContext.Request.Headers["Authorization"], new string[] { "userId", "sub", "companyId", "roles" });

            //int reqId = 0;
            //int.TryParse(mapped["userId"], out reqId);
            //string reqName = mapped["sub"];
            //string reqCompanyId = mapped["companyId"];

            ObjectResult response;

            try
            {
                offset = offset ?? 1;
                //limit = limit ?? GlobalVars.PAGE_SIZE;

                searchText = searchText == null ? string.Empty : searchText.Trim();
                sort = sort ?? "Id";
                SortDirection direction = sortDirection == "desc" ? SortDirection.Descending : SortDirection.Ascending;

                //// See regulation from his company or published from system
                //       && ((String.IsNullOrEmpty || (x. == reqCompanyId)) || (!x.CompanyId.HasValue && (x.IsActive == true)))
                //       && (!start.HasValue || (start.HasValue && x.CreatedAt >= start.Value.Date))
                //       && (!end.HasValue || (end.HasValue && x.CreatedAt < end.Value.Date.AddDays(1)))
                //       && (!isActive.HasValue || (isActive.HasValue && x.IsActive == isActive))
                //       );

                Expression<Func<Institution, bool>> filter = (x => x.Name.Contains(searchText)
                       );

                var institutions = uow.InstitutionRepository.getInstitutionsWithAddresses(offset, limit, filter, sort, direction, "");

                int totalCount = uow.InstitutionRepository.Count(filter);

                Request.HttpContext.Response.Headers.Add("X-Total-Count", totalCount.ToString());

                response = institutions != null ? StatusCode((int)HttpStatusCode.OK, institutions) : StatusCode((int)HttpStatusCode.NotFound, null);
            }
            catch (Exception ex)
            {
                byte[] bytes = ErrorLog.ErrorLogGenerator(ex, null, null, 0, null, null);

                rabbitMQ.LogsChannel.BasicPublish(RabbitExchanges.LOGS, "log.event.create", true, null, bytes);

                response = StatusCode((int)HttpStatusCode.InternalServerError, null);
            }

            return response;
        }
    }
}