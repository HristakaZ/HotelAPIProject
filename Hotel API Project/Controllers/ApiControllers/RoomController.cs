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
    public class RoomController : ControllerBase
    {
        private IRoomRepository iRoomRepository;
        private IRoomTypeRepository iRoomTypeRepository;
        private IUnitOfWork iUnitOfWork;
        private HtmlEncoder htmlEncoder;
        public RoomController(IRoomRepository iRoomRepository, IRoomTypeRepository iRoomTypeRepository,
            IUnitOfWork iUnitOfWork, HtmlEncoder htmlEncoder)
        {
            this.iRoomRepository = iRoomRepository;
            this.iRoomTypeRepository = iRoomTypeRepository;
            this.iUnitOfWork = iUnitOfWork;
            this.htmlEncoder = htmlEncoder;
        }
        // GET: api/<RoomController>
        [HttpGet]
        public List<Room> GetRooms()
        {
            List<Room> rooms = iRoomRepository.GetRooms();
            /*TO DO: encode the rooms as well when you're done with the dropdown lists(for now encode the models where you have an
              input type text(strings))*/
            rooms.ForEach(x => {
                string encodedRoomRoomType = htmlEncoder.Encode(x.RoomType.Name);
                x.RoomType.Name = encodedRoomRoomType;
            });
            return rooms;
        }

        // GET api/<RoomController>/5
        [HttpGet("{id}", Name = "GetRoomByID")]
        public IActionResult GetRoomByID(int id)
        {
            Room room = iRoomRepository.GetRoomByID(id);
            if (room != null)
            {
                return Ok(room);
            }
            else
            {
                return NotFound("Room with ID " + id.ToString() + " was not found.");
            }
        }

        // POST api/<RoomController>
        [HttpPost]
        public IActionResult Post([FromBody] Room room)
        {
            try
            {
                //TO DO for later on: move the create logic for the room roomtype in a service
                RoomType roomTypeFromDropDownList = iRoomTypeRepository.GetRoomTypeByID(room.RoomType.ID);
                room.RoomType = roomTypeFromDropDownList;
                iRoomRepository.CreateRoom(room);
                Uri uri = new Uri(Url.Link("GetRoomByID", new { Id = room.ID }));
                iUnitOfWork.Save();
                return Created(uri, room.ID.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<RoomController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Room room)
        {
            if (room != null)
            {
                //TO DO for later on: move the update logic for the room roomtype in a service
                room.ID = id;
                List<RoomType> roomTypes = iRoomTypeRepository.GetRoomTypes();
                RoomType roomTypeFromDropDownList = roomTypes.Where(x => x.ID == room.RoomType.ID).FirstOrDefault();
                room.RoomType = roomTypeFromDropDownList;
                iRoomRepository.UpdateRoom(room);
                iUnitOfWork.Save();
                return Ok(room);
            }
            else
            {
                return NotFound("Room with ID " + id.ToString() + " was not found.");
            }
        }

        // DELETE api/<RoomController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Room roomToDelete = iRoomRepository.GetRoomByID(id);
            if (roomToDelete != null)
            {
                iRoomRepository.DeleteRoom(roomToDelete.ID);
                iUnitOfWork.Save();
                return Ok(roomToDelete);
            }
            else
            {
                return NotFound("Room with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
