using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
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
        private IUnitOfWork iUnitOfWork;
        private HtmlEncoder htmlEncoder;
        public GuestController(IGuestRepository iGuestRepository, IUnitOfWork iUnitOfWork, HtmlEncoder htmlEncoder)
        {
            this.iGuestRepository = iGuestRepository;
            this.iUnitOfWork = iUnitOfWork;
            this.htmlEncoder = htmlEncoder;
        }
        // GET: api/<GuestController>
        [HttpGet]
        public List<Guest> GetGuests()
        {
            List<Guest> guests = iGuestRepository.GetGuests();
            guests.ForEach(x => {
                string encodedGuestName = htmlEncoder.Encode(x.Name);
                x.Name = encodedGuestName;
            });
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
                iUnitOfWork.Save();
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
                iUnitOfWork.Save();
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
                iUnitOfWork.Save();
                return Ok(guestToDelete);
            }
            else
            {
                return NotFound("Guest with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
