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
    public class RoomTypeController : ControllerBase
    {
        private IRoomTypeRepository iRoomTypeRepository;
        public RoomTypeController(IRoomTypeRepository iRoomTypeRepository)
        {
            this.iRoomTypeRepository = iRoomTypeRepository;
        }
        // GET: api/<RoomTypeController>
        [HttpGet]
        public List<RoomType> GetRoomTypes()
        {
            List<RoomType> roomTypes = iRoomTypeRepository.GetRoomTypes();
            return roomTypes;
        }

        // GET api/<RoomTypeController>/5
        [HttpGet("{id}", Name = "GetRoomTypeByID")]
        public IActionResult GetRoomTypeByID(int id)
        {
            RoomType roomType = iRoomTypeRepository.GetRoomTypeByID(id);
            if (roomType != null)
            {
                return Ok(roomType);
            }
            else
            {
                return NotFound("Room Type with ID " + id.ToString() + " was not found.");
            }
        }

        // POST api/<RoomTypeController>
        [HttpPost]
        public IActionResult Post([FromBody] RoomType roomType)
        {
            try
            {
                iRoomTypeRepository.CreateRoomType(roomType);
                Uri uri = new Uri(Url.Link("GetRoomTypeByID", new { Id = roomType.ID }));
                return Created(uri, roomType.ID.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<RoomTypeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RoomType roomType)
        {
            if (roomType != null)
            {
                roomType.ID = id;
                iRoomTypeRepository.UpdateRoomType(roomType);
                return Ok(roomType);
            }
            else
            {
                return NotFound("Room Type with ID " + id.ToString() + " was not found.");
            }
        }

        // DELETE api/<RoomTypeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            RoomType roomTypeToDelete = iRoomTypeRepository.GetRoomTypeByID(id);
            if (roomTypeToDelete != null)
            {
                iRoomTypeRepository.DeleteRoomType(roomTypeToDelete.ID);
                return Ok(roomTypeToDelete);
            }
            else
            {
                return NotFound("Room Type with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
