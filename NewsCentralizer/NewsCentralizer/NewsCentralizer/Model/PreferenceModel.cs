namespace NewsCentralizer.Model
{
    public class PreferenceModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public string CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public string Tag { get; set; }
    }
}
