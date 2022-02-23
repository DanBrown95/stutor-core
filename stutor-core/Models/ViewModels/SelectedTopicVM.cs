namespace stutor_core.Models.ViewModels
{
    public class SelectedTopicVM
    {
        public int TopicId { get; set; }
        public string RequestingUserId { get; set; }
        public Coordinates UserCoordinates { get; set; }
    }
}
