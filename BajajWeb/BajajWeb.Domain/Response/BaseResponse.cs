using BajajWeb.Domain.Enum;

namespace BajajWeb.Domain.Response
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }

        public StatusCode StatusCode { get; set; }

        public T Data { get; set; }
    }

    public interface IBaseResponse<T>
    {
        string Description { get; }

        T Data { get;}

        StatusCode StatusCode { get; }
    }
}
