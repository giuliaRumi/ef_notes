namespace EF_Notes_Manager.Service
{
    public class Note
    {
        public string name { get; set; }
        public long Id { get; set; }
        public string content { get; set; }

        protected Note()
        {
        }

        public Note(string name, string content)
        {
            this.name = name;
            this.content = content;
        }
    }
}
