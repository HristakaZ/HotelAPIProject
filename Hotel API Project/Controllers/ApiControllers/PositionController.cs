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
    public class PositionController : ControllerBase
    {
        private IPositionRepository iPositionRepository;
        public PositionController(IPositionRepository iPositionRepository)
        {
            this.iPositionRepository = iPositionRepository;
        }
        // GET: api/<PositionController>
        [HttpGet]
        public List<PositionApplicationRole> GetGuests()
        {
            List<PositionApplicationRole> positions = iPositionRepository.GetPositions();
            return positions;
        }

        // GET api/<PositionController>/5
        [HttpGet("{id}", Name = "GetPositionByID")]
        public IActionResult GetPositionByID(int id)
        {
            PositionApplicationRole position = iPositionRepository.GetPositionByID(id);
            if (position != null)
            {
                return Ok(position);
            }
            else
            {
                return NotFound("Position with ID " + id.ToString() + " was not found.");
            }
        }

        // POST api/<PositionController>
        [HttpPost]
        public IActionResult Post([FromBody] PositionApplicationRole position)
        {
            try
            {
                iPositionRepository.CreatePosition(position);
                Uri uri = new Uri(Url.Link("GetPositionByID", new { Id = position.Id }));
                return Created(uri, position.Id.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<PositionController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PositionApplicationRole position)
        {
            if (position != null)
            {
                position.Id = id;
                iPositionRepository.UpdatePosition(position);
                return Ok(position);
            }
            else
            {
                return NotFound("Position with ID " + id.ToString() + " was not found.");
            }
        }

        // DELETE api/<PositionController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            PositionApplicationRole positionToDelete = iPositionRepository.GetPositionByID(id);
            if (positionToDelete != null)
            {
                iPositionRepository.DeletePosition(positionToDelete.Id);
                return Ok(positionToDelete);
            }
            else
            {
                return NotFound("Position with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
