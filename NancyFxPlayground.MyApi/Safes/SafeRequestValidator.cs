using FluentValidation;

namespace NancyFxPlayground.MyApi.Safes
{
    public class SafeRequestValidator : AbstractValidator<GetSafeRequest>
    {
        public SafeRequestValidator()
        {
            RuleFor(cr => cr.Id).GreaterThan(0);
        }
    }
}