using System;
namespace CodingFlashcard.Models
{
    public class Flashcard
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int? Timeseen { get; set; }
        public bool? Known { get; set; }
        public string Difficulty { get; set; }

        public Flashcard()
        {

        }

    }
}
