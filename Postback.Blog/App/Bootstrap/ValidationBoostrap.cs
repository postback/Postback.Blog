using DataAnnotationsExtensions.ClientValidation;

namespace Postback.Blog.App.Bootstrap
{
    public class ValidationBoostrap : IStartUpTask
    {
        public void Configure()
        {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();
        }
    }
}