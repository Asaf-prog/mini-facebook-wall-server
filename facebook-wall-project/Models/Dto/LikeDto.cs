public class LikeDto
{
    public int LikeId { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public int LikeCount { get; set; }
    public DateTime LikedAt { get; set; }

     public class Builder
    {
        private LikeDto _likeDto;

        public Builder()
        {
            _likeDto = new LikeDto();
        }

        public Builder WithLikeId(int likeId)
        {
            _likeDto.LikeId = likeId;
            return this;
        }

        public Builder WithPostId(int postId)
        {
            _likeDto.PostId = postId;
            return this;
        }

        public Builder WithUserId(int userId)
        {
            _likeDto.UserId = userId;
            return this;
        }

        public Builder WithLikedAt(DateTime likedAt)
        {
            _likeDto.LikedAt = likedAt;
            return this;
        }
        public Builder WithLikeCount(int likeCount)
        {
            _likeDto.LikeCount = likeCount;
            return this;
        }

        public LikeDto Build()
        {
            return _likeDto;
        }
    }
}
