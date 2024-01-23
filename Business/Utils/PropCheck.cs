using Business.Dtos;
using System.Diagnostics;

namespace Business.Utils;

public static class PropCheck
{
    public static bool CheckAllPropertiesAreSet(UserRegistrationDto registration)
    {
        try
        {
            // Get all properties of the object
            var properties = registration.GetType().GetProperties();

            // Check if all string properties have non-empty values
            return properties.All(property =>
            {
                var value = (string)property.GetValue(registration, null)!;
                return !string.IsNullOrEmpty(value);
            });

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }
}
