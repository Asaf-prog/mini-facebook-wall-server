public class PostDto
{
    public int PostId { get; set; }
    public string Header { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public int LikeCount { get; set; }  
    public DateTime PostTime { get; set; }

    public class Builder
    {
        private PostDto _postDto;

        public Builder()
        {
            _postDto = new PostDto();
        }

        public Builder WithPostId(int postId)
        {
            _postDto.PostId = postId;
            return this;
        }

        public Builder WithHeader(string header)
        {
            _postDto.Header = header;
            return this;
        }

        public Builder WithDescription(string description)
        {
            _postDto.Description = description;
            return this;
        }

        public Builder WithPostTime(DateTime postTime)
        {
            _postDto.PostTime = postTime;
            return this;
        }

        public Builder WithUserId(int userId)
        {
            _postDto.UserId = userId;
            return this;
        }

        public Builder WithLikeCount(int likeCount)
        {
            _postDto.LikeCount = likeCount;
            return this;
        }

        public PostDto Build()
        {
            return _postDto;
        }
    }
}
