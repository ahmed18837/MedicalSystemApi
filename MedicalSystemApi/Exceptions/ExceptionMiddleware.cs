using System.Net;

namespace MedicalSystemApi.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // نفذ الطلب
            }
            catch (Exception ex) // في حال حدوث استثناء
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(context, ex); // معالجة الاستثناء
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // تحديد نوع الاستثناء وحالة الاستجابة بناءً عليه
            context.Response.StatusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest, // خطأ في المدخلات
                KeyNotFoundException => (int)HttpStatusCode.NotFound, // العنصر غير موجود
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // الوصول غير مصرح به
                _ => (int)HttpStatusCode.InternalServerError // أخطاء غير معروفة
            };

            // إرسال رسالة الاستثناء كـ JSON
            return context.Response.WriteAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}

