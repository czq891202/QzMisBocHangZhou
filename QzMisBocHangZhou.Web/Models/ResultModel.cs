namespace QzMisBocHangZhou.Web
{
    public class ResultModel<T>
    {
        public int code { get; set; } = 0;

        public string msg { get; set; } = "";

        public T data { get; set; }

        public ResultModel()
        {
        }

        public ResultModel(T Tdata)
        {
            data = Tdata;
        }
    }
}