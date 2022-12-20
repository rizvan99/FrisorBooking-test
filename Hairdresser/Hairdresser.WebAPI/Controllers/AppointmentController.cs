using Hairdresser.Core.Application.Implementation;
using Hairdresser.Core.Entities;
using Hairdresser.Core.Entities.DTO;
using Hairdresser.Core.Filter;
using Hairdresser.WebAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Hairdresser.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _service;
        //private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
        public AppointmentController(AppointmentService service)
        {
            _service = service;
            //_hubContext = hubContext;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAll()
        {
            try
            {
                var AppointmentList = _service.Read().ToList();
                if (AppointmentList.Count == 0)
                {
                    return NoContent();
                }
                return AppointmentList;
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("{date}")]
        public ActionResult<Appointment> Get(string date)
        {
            try
            {
                return _service.Get(DateTime.Parse(date));
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }


        }

        [HttpPost]
        //[Authorize]
        [Route("createnewappointment")]
        public ActionResult<Appointment> Post([FromBody] Appointment entity)
        {
            try
            {
                string sid = User.Claims.FirstOrDefault(type => type.Type.Equals(ClaimTypes.Sid)).Value;
                if (sid != null)
                {
                    entity.CustomerID = Int32.Parse(sid);
                    var entityFromDb = _service.Create(entity);
                    //if (entityFromDb != null)
                    //{
                    //    _hubContext.Clients.All.BroadcastMessage();
                    //}
                    return Ok(entityFromDb);
                }
                else
                {
                    return Unauthorized("Brugeren kunne ikke verificeres");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut]
        public ActionResult<Appointment> Put(DateTime id, [FromBody] Appointment entity)
        {
            try
            {
                if (id == null) return BadRequest("Id must be greater then 0");

                if (id != entity.AppointmentDateTime)
                {
                    return BadRequest("Parameter Id and category ID must be the same");
                }

                return Accepted(_service.Update(entity));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete]
        public ActionResult<Appointment> Delete([FromBody] Appointment entity)
        {
            try
            {
                var appointment = _service.Delete(entity);

                return Accepted(
                    new
                    {
                        message = "Bid was been removed ",
                        appointment
                    }
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpPost]
        [Route("checkdateavailability")]
        public ActionResult<Boolean> checkDateAvalability([FromBody] DateTime date)
        {
            try
            {
                return _service.CheckIfDateHasAvalableTimes(date);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }

        }

        [HttpPost]
        [Route("checktimeavailability")]
        public ActionResult<Boolean> checkTimeAvalability([FromBody] DateTime date)
        {
            try
            {
                return _service.CheckIfTimeIsAvalable(date);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpGet]
        [Route("generatedates")]
        public ActionResult<List<CalendarDateModel>> GenerateDates()
        {
            try
            {
                return _service.GenerateDateSchedule();
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPost]
        [Route("generatetimes")]
        public ActionResult<List<DateTime>> GenerateTimes([FromBody] DateTime dt)
        {
            try
            {
                return _service.GenerateTimeSchedule(dt);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        [HttpPut]
        [Route("completeappointment")]
        public ActionResult<Appointment> CompleteAppointment([FromBody] Appointment entity)
        {
            try
            {
                try
                {
                    return Accepted(_service.CompleteAssignemnt(entity));
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.Message);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpPut]
        [Route("cancelappointment")]
        public ActionResult<Appointment> CancelAppointment([FromBody] Appointment entity)
        {
            try
            {
                try
                {
                    return Accepted(_service.CancelAssignemnt(entity));
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.Message);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("getactiveappointmentsuser")]
        public ActionResult<List<AppointmentDto>> GetActiveAppointmentsForUser()
        {
            try
            {
                string loggedInUserId = User.Claims.FirstOrDefault(type => type.Type.Equals(ClaimTypes.Sid)).Value;

                if (loggedInUserId != null)
                {
                    int id = Int32.Parse(loggedInUserId);
                    var result = _service.GetActiveAppointmentsForUser(id);
                    return result
                        .Select(x => new AppointmentDto()
                        {
                            AppointmentDateTime = x.AppointmentDateTime,
                            Status = x.Status,
                            Customer = new UserBasicDTO
                            {
                                Firstname = x.Customer.FirstName,
                                Lastname = x.Customer.LastName,
                                UserID = x.Customer.UserID,
                            },
                            Employee = new UserBasicDTO
                            {
                                Firstname = x.Employee.FirstName,
                                Lastname = x.Employee.LastName,
                                UserID = x.Employee.UserID
                            },
                            Treatment = x.Treatment,
                        })
                        .ToList();
                }
                else
                {
                    return Unauthorized("Brugeren kunne ikke verificeres");
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet]
        [Route("getoldappointmentsuser")]
        public ActionResult<List<AppointmentDto>> GetOldAppointmentsForUser()
        {
            try
            {
                string loggedInUserId = User.Claims.FirstOrDefault(type => type.Type.Equals(ClaimTypes.Sid)).Value;

                if (loggedInUserId != null)
                {
                    int id = Int32.Parse(loggedInUserId);
                    var result = _service.GetOldAppointmentsForUser(id);
                    return result
                        .Select(x => new AppointmentDto()
                        {
                            AppointmentDateTime = x.AppointmentDateTime,
                            Status = x.Status,
                            Customer = new UserBasicDTO
                            {
                                Firstname = x.Customer.FirstName,
                                Lastname = x.Customer.LastName,
                                UserID = x.Customer.UserID,
                            },
                            Employee = new UserBasicDTO
                            {
                                Firstname = x.Employee.FirstName,
                                Lastname = x.Employee.LastName,
                                UserID = x.Employee.UserID
                            },
                            Treatment = x.Treatment,
                        })
                        .ToList();
                }
                else
                {
                    return Unauthorized("Brugeren kunne ikke verificeres");
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("fetchappointmentswithfilter")]
        public ActionResult<FilterResponse<Appointment>> GetAppointmentsWithFilter([FromBody] AppointmentFilter filter)
        {
            try
            {
                var results = _service.GetWithFilter(filter);

                return Ok(_service.GetWithFilter(filter));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
