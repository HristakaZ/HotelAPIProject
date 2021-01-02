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
    public class PositionController : ControllerBase
    {
        private IPositionRepository iPositionRepository;
        private IUnitOfWork iUnitOfWork;
        private HtmlEncoder htmlEncoder;
        public PositionController(IPositionRepository iPositionRepository, IUnitOfWork iUnitOfWork, HtmlEncoder htmlEncoder)
        {
            this.iPositionRepository = iPositionRepository;
            this.iUnitOfWork = iUnitOfWork;
            this.htmlEncoder = htmlEncoder;
        }
        // GET: api/<PositionController>
        [HttpGet, Authorize]
        public List<PositionApplicationRole> GetPositions()
        {
            List<PositionApplicationRole> positions = iPositionRepository.GetPositions();
            /*encoding(against xss) at the get request, so as to store the entity column in its plain form in the database*/
            positions.ForEach(x =>
            {
                if (x != null)
                {
                    string encodedPositionName = htmlEncoder.Encode(x.Name);
                    x.Name = encodedPositionName;
                }
            });
            return positions;
        }

        // GET api/<PositionController>/5
        [HttpGet("{id}", Name = "GetPositionByID"), Authorize]
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
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] CreatePositionViewModel createPositionViewModel)
        {
            try
            {
                PositionApplicationRole position = new PositionApplicationRole()
                {
                    Id = createPositionViewModel.ID,
                    Name = createPositionViewModel.Name
                };
                iPositionRepository.CreatePosition(position);
                Uri uri = new Uri(Url.Link("GetPositionByID", new { Id = position.Id }));
                iUnitOfWork.Save();
                return Created(uri, position.Id.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<PositionController>/5
        [HttpPut("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Put(int id, [FromBody] UpdatePositionViewModel updatePositionViewModel)
        {
            if (updatePositionViewModel != null)
            {
                updatePositionViewModel.ID = id;
                if (string.IsNullOrEmpty(updatePositionViewModel.Name))
                {
                    updatePositionViewModel.Name = iPositionRepository.GetPositionByID(updatePositionViewModel.ID).Name;
                }
                PositionApplicationRole position = new PositionApplicationRole()
                {
                    Id = updatePositionViewModel.ID,
                    Name = updatePositionViewModel.Name,
                    Employees = iPositionRepository.GetPositionByID(updatePositionViewModel.ID).Employees
                };
                iPositionRepository.UpdatePosition(position);
                iUnitOfWork.Save();
                return Ok(position);
            }
            else
            {
                return NotFound("Position with ID " + id.ToString() + " was not found.");
            }
        }

        // DELETE api/<PositionController>/5
        [HttpDelete("{id}"), Authorize, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            PositionApplicationRole positionToDelete = iPositionRepository.GetPositionByID(id);
            if (positionToDelete != null)
            {
                iPositionRepository.DeletePosition(positionToDelete.Id);
                //if a certain employee has this position(the position for deletion), we are setting his position to be "No Position!"
                positionToDelete.Employees.ForEach(x =>
                    x.Position = iPositionRepository.GetPositions().Where(x => x.Name == "No Position!").FirstOrDefault());
                iUnitOfWork.Save();
                return Ok(positionToDelete);
            }
            else
            {
                return NotFound("Position with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
