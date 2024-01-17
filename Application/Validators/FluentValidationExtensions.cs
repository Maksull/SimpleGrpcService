using FluentValidation;

namespace Application.Validators;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MatchesUlidFormat<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(BeValidUlid)
            .WithMessage("Please enter a valid Ulid format");
    }
    
    private static bool BeValidUlid(string ulidString)
    {
        return Ulid.TryParse(ulidString, out _);
    }
}