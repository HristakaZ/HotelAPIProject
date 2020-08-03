using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repositories;
using DataStructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_API_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private IReservationRepository iReservationRepository;
        public ReservationController(IReservationRepository iReservationRepository)
        {
            this.iReservationRepository = iReservationRepository;
        }
        // GET: api/<ReservationController>
        [HttpGet]
        public List<Reservation> GetGuests()
        {
            List<Reservation> reservations = iReservationRepository.GetReservations();
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
        public IActionResult Post([FromBody] Reservation reservation)
        {
            try
            {
                iReservationRepository.CreateReservation(reservation);
                Uri uri = new Uri(Url.Link("GetReservationByID", new { Id = reservation.ID }));
                return Created(uri, reservation.ID.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<ReservationController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Reservation reservation)
        {
            if (reservation != null)
            {
                reservation.ID = id;
                iReservationRepository.UpdateReservation(reservation);
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
                return Ok(reservationToDelete);
            }
            else
            {
                return NotFound("Reservation with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
