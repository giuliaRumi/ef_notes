using System;
using System.Collections.Generic;
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

        [HttpPost]
        public IActionResult Create(NoteRequest noteRequest)
        {
            Note note = new Note(noteRequest.name, noteRequest.content);
            _context.Notes.Add(note);
            _context.SaveChanges();

            return Created(nameof(Get), NoteResponse.from(note));
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var note = _context.Notes.SingleOrDefault(n => n.Id == id);

            return Ok(NoteResponse.from(note));
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var notes = await _context.Notes
                .ToArrayAsync();

            var notesResponse = new List<NoteResponse>();

            foreach(Note n in notes)
            {
                notesResponse.Add(NoteResponse.from(n));
            }

            return Ok(notesResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NoteResponse>> Edit(long id, NoteRequest noteRequest)
        {
            try
            {

                Note noteToUpdate = _context.Notes.SingleOrDefault(n => n.Id == id);
                if (noteToUpdate == null)
                {
                    return NotFound($"Note with Id = {id} not found");
                }

                noteToUpdate.content = noteRequest.content;
                noteToUpdate.name = noteRequest.name;
                _context.SaveChanges();

                return Ok(NoteResponse.from(noteToUpdate));
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
                var notesToDelete = _context.Notes.SingleOrDefault(n => n.Id == id);

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
