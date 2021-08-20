namespace MPS.Common.DTOs
{
    public class ReturnMessageDto
    {
        public ReturnMessageDto(string message, bool status, int code)
        {
            Message = message;
            Status = status;
            Code = code;
        }
        
        public ReturnMessageDto()
        {

        }

        public string Message { get; set; }
        public bool Status { get; set; }
        public int Code { get; set; }
        public object ExtraData { get; set; }
    }
}