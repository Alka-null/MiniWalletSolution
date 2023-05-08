using UtilityService.Interfaces;

namespace UtilityService
{
    public class ResponseObj: IResponseObj
    {
        public string? Message { get; set; }
        public int Code { get; set; }
        public bool IsSuccess { get; set; }
        public Object? Data { get; set; }
    }
}
