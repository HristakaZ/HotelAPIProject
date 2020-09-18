using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.Repositories;
using DataStructure;
using Hotel_API_Project.Mappers;
using Hotel_API_Project.ViewModels;
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
        private IReservationMapper iReservationMapper;
        public ReservationController(IReservationRepository iReservationRepository, IGuestRepository iGuestRepository,
            IRoomRepository iRoomRepository, IEmployeeRepository iEmployeeRepository, IUnitOfWork iUnitOfWork, HtmlEncoder htmlEncoder,
            IReservationMapper iReservationMapper)
        {
            this.iReservationRepository = iReservationRepository;
            this.iGuestRepository = iGuestRepository;
            this.iEmployeeRepository = iEmployeeRepository;
            this.iRoomRepository = iRoomRepository;
            this.iUnitOfWork = iUnitOfWork;
            this.htmlEncoder = htmlEncoder;
            this.iReservationMapper = iReservationMapper;
        }
        // GET: api/<ReservationController>
        [HttpGet]
        public List<Reservation> GetReservations()
        {
            List<Reservation> reservations = iReservationRepository.GetReservations();
            /*TO DO: encode the reservations as well when you're done with the dropdown lists(for now encode the models where you have an
              input type text(strings))*/
            reservations.ForEach(x =>
            {
                string encodedReservationGuest = htmlEncoder.Encode(x.Guest.Name);
                string encodedReservationEmployee = htmlEncoder.Encode(x.Employee.UserName);
                x.Guest.Name = encodedReservationGuest;
                x.Employee.UserName = encodedReservationEmployee;
            });
            return reservations;
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}", Name = "GetReservationByID")]
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
        [HttpPost]
        public IActionResult Post([FromBody] ReservationViewModel reservationViewModel)
        {
            try
            {
                Guest guestFromDropDownList = iGuestRepository.GetGuestByID(reservationViewModel.Guest.ID);
                reservationViewModel.Guest = guestFromDropDownList;
                EmployeeApplicationUser employeeFromDropDownList = iEmployeeRepository.GetEmployeeByID(reservationViewModel.Employee.Id);
                reservationViewModel.Employee = employeeFromDropDownList;
                Reservation reservation = new Reservation();
                reservation = iReservationMapper.MapReservationViewModelToModel(reservationViewModel, reservation);
                reservation.RoomReservations = new List<RoomReservation>();
                reservationViewModel.RoomReservation.Room = iRoomRepository.GetRoomByID
                    (reservationViewModel.RoomReservation.RoomID);
                reservationViewModel.RoomReservation.Reservation = reservation;
                reservation.RoomReservations.Add(reservationViewModel.RoomReservation);
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
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ReservationViewModel reservationViewModel)
        {
            if (reservationViewModel != null)
            {
                Guest guestFromDropDownList = iGuestRepository.GetGuestByID(reservationViewModel.Guest.ID);
                reservationViewModel.Guest = guestFromDropDownList;
                EmployeeApplicationUser employeeFromDropDownList = iEmployeeRepository.GetEmployeeByID(reservationViewModel.Employee.Id);
                reservationViewModel.Employee = employeeFromDropDownList;
                reservationViewModel.ID = id;
                Reservation reservation = iReservationRepository.GetReservationByID(reservationViewModel.ID);
                reservationViewModel.RoomReservation.Room = iRoomRepository.GetRoomByID(reservationViewModel.RoomReservation.RoomID);
                reservationViewModel.RoomReservation.Reservation = reservation;
                reservationViewModel.RoomReservation.ReservationID = reservationViewModel.RoomReservation.Reservation.ID;
                List<RoomReservation> roomReservations = reservation.RoomReservations.Where(x => x.ReservationID == reservationViewModel.ID).ToList();
                reservation = iReservationMapper.MapReservationViewModelToModel(reservationViewModel, reservation);
                reservation.RoomReservations = new List<RoomReservation>();
                reservation.RoomReservations.Add(reservationViewModel.RoomReservation);
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
        [HttpDelete("{id}")]
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
