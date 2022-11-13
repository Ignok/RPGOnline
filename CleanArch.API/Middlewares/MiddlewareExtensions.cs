namespace RPGOnline.API.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGreatErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
