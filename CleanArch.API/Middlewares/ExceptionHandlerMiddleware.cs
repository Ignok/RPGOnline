namespace RPGOnline.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exc)
            {
                string logpath = @"..\logs.txt";
                if (!File.Exists(logpath))
                {
                    File.Create(logpath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(logpath))
                {
                    sw.WriteLine("Eror Logging");
                    sw.WriteLine("Start" + DateTime.Now);
                    sw.WriteLine("Error:" + exc.Message);
                    sw.WriteLine("Stack Trace" + exc.StackTrace);
                    sw.WriteLine("End" + DateTime.Now);
                }
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("Unexpected problem");
            }
        }
    }
}
