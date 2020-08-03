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
    public class GuestController : ControllerBase
    {
        private IGuestRepository iGuestRepository;
        public GuestController(IGuestRepository iGuestRepository)
        {
            this.iGuestRepository = iGuestRepository;
        }
        // GET: api/<GuestController>
        [HttpGet]
        public List<Guest> GetGuests()
        {
            List<Guest> guests = iGuestRepository.GetGuests();
            return guests;
        }

        // GET api/<GuestController>/5
        [HttpGet("{id}", Name = "GetGuestByID")]
        public IActionResult GetGuestByID(int id)
        {
            Guest guest = iGuestRepository.GetGuestByID(id);
            if (guest != null)
            {
                return Ok(guest);
            }
            else
            {
                return NotFound("Guest with ID " + id.ToString() + " was not found.");
            }
        }

        // POST api/<GuestController>
        [HttpPost]
        public IActionResult Post([FromBody] Guest guest)
        {
            try
            {
                iGuestRepository.CreateGuest(guest);
                Uri uri = new Uri(Url.Link("GetGuestByID", new { Id = guest.ID }));
                return Created(uri, guest.ID.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<GuestController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Guest guest)
        {
            if (guest != null)
            {
                guest.ID = id;
                iGuestRepository.UpdateGuest(guest);
                return Ok(guest);
            }
            else
            {
                return NotFound("Guest with ID " + id.ToString() + " was not found.");
            }
        }

        // DELETE api/<GuestController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Guest guestToDelete = iGuestRepository.GetGuestByID(id);
            if (guestToDelete != null)
            {
                iGuestRepository.DeleteGuest(guestToDelete.ID);
                return Ok(guestToDelete);
            }
            else
            {
                return NotFound("Guest with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
