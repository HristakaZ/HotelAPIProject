using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.Repositories;
using DataStructure;
using Hotel_API_Project.Mappers;
using Hotel_API_Project.Services;
using Hotel_API_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_API_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private IReservationRepository iReservationRepository;
        private IGuestRepository iGuestRepository;
        private IEmployeeRepository iEmployeeRepository;
        private IRoomRepository iRoomRepository;
        private IUnitOfWork iUnitOfWork;
        private HtmlEncoder htmlEncoder;
        private ICreateReservationMapper iCreateReservationMapper;
        private IUpdateReservationMapper iUpdateReservationMapper;
        private IUpdateReservationValidationService iUpdateReservationValidationService;
        public ReservationController(IReservationRepository iReservationRepository, IGuestRepository iGuestRepository,
            IRoomRepository iRoomRepository, IEmployeeRepository iEmployeeRepository, IUnitOfWork iUnitOfWork, HtmlEncoder htmlEncoder,
            ICreateReservationMapper iCreateReservationMapper, IUpdateReservationMapper iUpdateReservationMapper,
            IUpdateReservationValidationService iUpdateReservationValidationService)
        {
            this.iReservationRepository = iReservationRepository;
            this.iGuestRepository = iGuestRepository;
            this.iEmployeeRepository = iEmployeeRepository;
            this.iRoomRepository = iRoomRepository;
            this.iUnitOfWork = iUnitOfWork;
            this.htmlEncoder = htmlEncoder;
            this.iCreateReservationMapper = iCreateReservationMapper;
            this.iUpdateReservationMapper = iUpdateReservationMapper;
            this.iUpdateReservationValidationService = iUpdateReservationValidationService;
        }
        // GET: api/<ReservationController>
        [HttpGet, Authorize]
        public IActionResult GetReservations()
        {
            List<Reservation> reservations = iReservationRepository.GetReservations();
            /*encoding(against xss) at the get request, so as to store the entity column in its plain form in the database*/
            if (reservations != null)
            {
                reservations.ForEach(x =>
                {
                    if (x != null)
                    {
                        if (x.Employee != null)
                        {
                            string encodedEmployeeName = htmlEncoder.Encode(x.Employee.UserName);
                            x.Employee.UserName = encodedEmployeeName;
                        }
                        if (x.Guest != null)
                        {
                            string encodedGuestName = htmlEncoder.Encode(x.Guest.Name);
                            x.Guest.Name = encodedGuestName;
                        }
                    }
                    if (x.Employee == null)
                    {
                        x.Employee = iEmployeeRepository.GetEmployees().Where(y => y.UserName == "NoEmployee").FirstOrDefault();
                    }
                    if (x.Guest == null)
                    {
                        x.Guest = iGuestRepository.GetGuests().Where(y => y.Name == "No Guest!").FirstOrDefault();
                    }
                });
                return Ok(reservations);
            }
            return NotFound("No reservations were found!");
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}", Name = "GetReservationByID"), Authorize]
        public IActionResult GetReservationByID(int id)
        {
            Reservation reservation = iReservationRepository.GetReservationByID(id);
            if (reservation != null)
            {
                return Ok(reservation);
            }
            else
            {
                return NotFound("Reservation with ID " + id.ToString() + " was not found.");
            }
        }

        // POST api/<ReservationController>
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public IActionResult Post([FromBody] CreateReservationViewModel createReservationViewModel)
        {
            try
            {
                foreach (RoomReservation roomReservation in createReservationViewModel.RoomReservations)
                {
                    if (roomReservation.RoomID == 0)
                    {
                        return BadRequest("The room is required");
                    }
                }
                createReservationViewModel.StartDate = createReservationViewModel.StartDate.ToUniversalTime();
                createReservationViewModel.EndDate = createReservationViewModel.EndDate.ToUniversalTime();
                Guest guestFromDropDownList = iGuestRepository.GetGuestByID(createReservationViewModel.Guest.ID);
                createReservationViewModel.Guest = guestFromDropDownList;
                EmployeeApplicationUser employeeFromDropDownList = iEmployeeRepository.GetEmployeeByID(createReservationViewModel.Employee.Id);
                createReservationViewModel.Employee = employeeFromDropDownList;
                Reservation reservation = new Reservation();
                reservation = iCreateReservationMapper.MapCreateReservationViewModelToModel(createReservationViewModel, reservation);
                iReservationRepository.CreateReservation(reservation);
                Uri uri = new Uri(Url.Link("GetReservationByID", new { Id = reservation.ID }));
                iUnitOfWork.Save();
                return Created(uri, reservation.ID.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<ReservationController>/5
        [HttpPut("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Put(int id, [FromBody] UpdateReservationViewModel updateReservationViewModel)
        {
            if (updateReservationViewModel != null)
            {
                Reservation reservation = new Reservation();
                reservation = iUpdateReservationMapper.MapUpdateReservationViewModelToModel(updateReservationViewModel, reservation);
                if (reservation.Employee.Id != 0)
                {
                    reservation.Employee = iEmployeeRepository.GetEmployeeByID(reservation.Employee.Id);
                }
                if (reservation.Guest.ID != 0)
                {
                    reservation.Employee = iEmployeeRepository.GetEmployeeByID(reservation.Employee.Id);
                }
                if (updateReservationViewModel.StartDate.HasValue && updateReservationViewModel.EndDate.HasValue)
                {
                    updateReservationViewModel.StartDate = updateReservationViewModel.StartDate.Value.ToUniversalTime();
                    updateReservationViewModel.EndDate = updateReservationViewModel.EndDate.Value.ToUniversalTime();
                }
                iUpdateReservationValidationService.UpdateReservationValidation(reservation);
                iReservationRepository.UpdateReservation(reservation);
                iUnitOfWork.Save();
                return Ok(reservation);
            }
            else
            {
                return NotFound("Reservation with ID " + id.ToString() + " was not found.");
            }
        }

        // DELETE api/<ReservationController>/5
        [HttpDelete("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Reservation reservationToDelete = iReservationRepository.GetReservationByID(id);
            if (reservationToDelete != null)
            {
                iReservationRepository.DeleteReservation(reservationToDelete.ID);
                iUnitOfWork.Save();
                return Ok(reservationToDelete);
            }
            else
            {
                return NotFound("Reservation with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
