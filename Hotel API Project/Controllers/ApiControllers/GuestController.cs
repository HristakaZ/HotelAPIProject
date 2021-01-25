using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.Repositories;
using DataStructure;
using Hotel_API_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet, Authorize]
        public IActionResult GetGuests()
        {
            List<Guest> guests = iGuestRepository.GetGuests();
            /*encoding(against xss) at the get request, so as to store the entity column in its plain form in the database*/
            if (guests != null)
            {
                guests.ForEach(x =>
                {
                    if (x != null)
                    {
                        string encodedGuestName = htmlEncoder.Encode(x.Name);
                        x.Name = encodedGuestName;
                    }
                });
                return Ok(guests);
            }
            return NotFound("No guests were found!");
        }

        // GET api/<GuestController>/5
        [HttpGet("{id}", Name = "GetGuestByID"), Authorize]
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
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public IActionResult Post([FromBody] CreateGuestViewModel createGuestViewModel)
        {
            try
            {
                Guest guest = new Guest()
                {
                    ID = createGuestViewModel.ID,
                    Name = createGuestViewModel.Name
                };
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
        [HttpPut("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Put(int id, [FromBody] UpdateGuestViewModel updateGuestViewModel)
        {
            if (updateGuestViewModel != null)
            {
                updateGuestViewModel.ID = id;
                if (string.IsNullOrEmpty(updateGuestViewModel.Name))
                {
                    updateGuestViewModel.Name = iGuestRepository.GetGuestByID(updateGuestViewModel.ID).Name;
                }
                Guest guest = new Guest()
                {
                    ID = updateGuestViewModel.ID,
                    Name = updateGuestViewModel.Name,
                    Reservations = iGuestRepository.GetGuestByID(updateGuestViewModel.ID).Reservations
                };
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
        [HttpDelete("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Guest guestToDelete = iGuestRepository.GetGuestByID(id);
            if (guestToDelete != null)
            {
                guestToDelete.Reservations.ForEach(x =>
                {
                    x.Guest.Name = iGuestRepository.GetGuests().Where(x => x.Name == "No Guest!").FirstOrDefault().Name;
                });
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
