namespace NewsCentralizer.Model
{
    public class FavoriteModel
    {
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public string NewsId { get; set; }
        public NewsModel News { get; set; }
    }
}
