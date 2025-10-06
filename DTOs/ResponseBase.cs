namespace ECommerce.DTO
{
    public class ResponseBase<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ResponseBase(int StatusCode, string Message)
        {
            this.StatusCode = StatusCode;
            this.Message = Message;
        }
    }
}
