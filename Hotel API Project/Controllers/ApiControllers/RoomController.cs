using System;
using System.Collections.Generic;
using System.Linq;
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
    public class RoomController : ControllerBase
    {
        private IRoomRepository iRoomRepository;
        private IRoomTypeRepository iRoomTypeRepository;
        private IUnitOfWork iUnitOfWork;
        private HtmlEncoder htmlEncoder;
        private IUpdateRoomMapper iUpdateRoomMapper;
        private IUpdateRoomValidationService iUpdateRoomValidationService;
        public RoomController(IRoomRepository iRoomRepository, IRoomTypeRepository iRoomTypeRepository,
            IUnitOfWork iUnitOfWork, HtmlEncoder htmlEncoder, IUpdateRoomMapper iUpdateRoomMapper, 
            IUpdateRoomValidationService iUpdateRoomValidationService)
        {
            this.iRoomRepository = iRoomRepository;
            this.iRoomTypeRepository = iRoomTypeRepository;
            this.iUnitOfWork = iUnitOfWork;
            this.htmlEncoder = htmlEncoder;
            this.iUpdateRoomMapper = iUpdateRoomMapper;
            this.iUpdateRoomValidationService = iUpdateRoomValidationService;
        }
        // GET: api/<RoomController>
        [HttpGet, Authorize]
        public List<Room> GetRooms()
        {
            List<Room> rooms = iRoomRepository.GetRooms();
            /*encoding(against xss) at the get request, so as to store the entity column in its plain form in the database*/
            rooms.ForEach(x =>
            {
                if (x != null)
                {
                    if (x.RoomType != null)
                    {
                        string encodedRoomRoomType = htmlEncoder.Encode(x.RoomType.Name);
                        x.RoomType.Name = encodedRoomRoomType;
                    }
                }
            });
            return rooms;
        }

        // GET api/<RoomController>/5
        [HttpGet("{id}", Name = "GetRoomByID"), Authorize]
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
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public IActionResult Post([FromBody] CreateRoomViewModel createRoomViewModel)
        {
            try
            {
                Room room = new Room()
                {
                    ID = createRoomViewModel.ID,
                    Number = createRoomViewModel.Number,
                    RoomType = createRoomViewModel.RoomType,
                    RoomReservations = createRoomViewModel.RoomReservations
                };
                if (room.RoomType != null && room.RoomType.ID != 0)
                {
                    RoomType roomTypeFromDropDownList = iRoomTypeRepository.GetRoomTypeByID(room.RoomType.ID);
                    room.RoomType = roomTypeFromDropDownList;
                }
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
        [HttpPut("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Put(int id, [FromBody] UpdateRoomViewModel updateRoomViewModel)
        {
            if (updateRoomViewModel != null)
            {
                Room room = new Room();
                room = iUpdateRoomMapper.MapUpdateRoomViewModelToModel(updateRoomViewModel, room);
                iUpdateRoomValidationService.UpdateRoomValidation(room);
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
        [HttpDelete("{id}"), Authorize, ValidateAntiForgeryToken]
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
