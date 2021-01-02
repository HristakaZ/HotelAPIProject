using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.Repositories;
using DataStructure;
using Hotel_API_Project.Mappers;
using Hotel_API_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_API_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private IRoomTypeRepository iRoomTypeRepository;
        private IUnitOfWork iUnitOfWork;
        private HtmlEncoder htmlEncoder;
        private ICreateRoomTypeMapper iCreateRoomTypeMapper;
        private IUpdateRoomTypeMapper iUpdateRoomTypeMapper;
        public RoomTypeController(IRoomTypeRepository iRoomTypeRepository, IUnitOfWork iUnitOfWork, HtmlEncoder htmlEncoder,
            ICreateRoomTypeMapper iCreateRoomTypeMapper, IUpdateRoomTypeMapper iUpdateRoomTypeMapper)
        {
            this.iRoomTypeRepository = iRoomTypeRepository;
            this.iUnitOfWork = iUnitOfWork;
            this.htmlEncoder = htmlEncoder;
            this.iCreateRoomTypeMapper = iCreateRoomTypeMapper;
            this.iUpdateRoomTypeMapper = iUpdateRoomTypeMapper;
        }
        // GET: api/<RoomTypeController>
        [HttpGet, Authorize]
        public List<RoomType> GetRoomTypes()
        {
            List<RoomType> roomTypes = iRoomTypeRepository.GetRoomTypes();
            /*encoding(against xss) at the get request, so as to store the entity column in its plain form in the database*/
            roomTypes.ForEach(x =>
            {
                if (x != null)
                {
                    string encodedRoomTypeName = htmlEncoder.Encode(x.Name);
                    x.Name = encodedRoomTypeName;
                }
            });
            return roomTypes;
        }

        // GET api/<RoomTypeController>/5
        [HttpGet("{id}", Name = "GetRoomTypeByID"), Authorize]
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
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public IActionResult Post([FromBody] CreateRoomTypeViewModel createRoomTypeViewModel)
        {
            try
            {
                RoomType roomType = new RoomType();
                if (!string.IsNullOrEmpty(createRoomTypeViewModel.Name))
                {
                    roomType = iCreateRoomTypeMapper.MapCreateRoomTypeViewModelToModel(createRoomTypeViewModel, roomType);
                    iRoomTypeRepository.CreateRoomType(roomType);
                }
                Uri uri = new Uri(Url.Link("GetRoomTypeByID", new { Id = roomType.ID }));
                iUnitOfWork.Save();
                return Created(uri, roomType.ID.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<RoomTypeController>/5
        [HttpPut("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Put(int id, [FromBody] UpdateRoomTypeViewModel updateRoomTypeViewModel)
        {
            if (updateRoomTypeViewModel != null)
            {
                updateRoomTypeViewModel.ID = id;
                RoomType roomType = new RoomType();
                roomType = iUpdateRoomTypeMapper.MapUpdateRoomTypeViewModelToModel(updateRoomTypeViewModel, roomType);
                if (string.IsNullOrEmpty(roomType.Name))
                {
                    roomType.Name = iRoomTypeRepository.GetRoomTypeByID(roomType.ID).Name;
                }
                iRoomTypeRepository.UpdateRoomType(roomType);
                iUnitOfWork.Save();
                return Ok(roomType);
            }
            else
            {
                return NotFound("Room Type with ID " + id.ToString() + " was not found.");
            }
        }

        // DELETE api/<RoomTypeController>/5
        [HttpDelete("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            RoomType roomTypeToDelete = iRoomTypeRepository.GetRoomTypeByID(id);
            if (roomTypeToDelete != null)
            {
                iRoomTypeRepository.DeleteRoomType(roomTypeToDelete.ID);
                iUnitOfWork.Save();
                return Ok(roomTypeToDelete);
            }
            else
            {
                return NotFound("Room Type with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
