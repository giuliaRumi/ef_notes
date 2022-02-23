using EF_Notes_Manager.Service;

namespace EF_Notes_Manager.Controllers
{
    public class NoteResponse
    {
        public long Id { get; }
        public string Name { get; }
        public string Content { get; }

        public NoteResponse(long id, string name, string content)
        {
            this.Id = id;
            this.Name = name;
            this.Content = content;
        }

        public static NoteResponse from(Note note)
        {
            return new NoteResponse(note.Id, note.name, note.content);
        }
    }
}
