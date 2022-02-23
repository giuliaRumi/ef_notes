using System;
using System.Linq;
using System.Threading.Tasks;
using EF_Notes_Manager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EF_Notes_Manager.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ApiContext _context;

        public NoteController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var notes = await _context.Notes
                .ToArrayAsync();

            return Ok(notes);
        }

        [HttpGet("{name}")]
        public ActionResult Get(string name)
        {
            var response = _context.Notes.SingleOrDefault(b => b.name == name);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var response = _context.Notes.SingleOrDefault(b => b.id == id);

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Note>> Edit(long id, Note note)
        {
            try
            {
                if (id != note.id)
                {
                    return BadRequest("Note ID mismatch");
                }

                var noteToUpdate = _context.Notes.SingleOrDefault(b => b.id == id);

                if (noteToUpdate == null)
                {
                    return NotFound($"Note with Id = {id} not found");
                }

                noteToUpdate.content = note.content;
                noteToUpdate.name = note.name;
                _context.SaveChanges();

                return CreatedAtAction(nameof(Note), new { id = note.id }, note);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var notesToDelete = _context.Notes.SingleOrDefault(b => b.id == id);

                if (notesToDelete == null)
                {
                    return NotFound($"note with id = {id} not found");
                }

                _context.Notes.Remove(notesToDelete);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
