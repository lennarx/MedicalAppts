using System.Reflection;
using System.Runtime.Serialization;

namespace MedicalAppts.Test.IntegrationTests
{
    public class IntegrationTestHelper
    {
        private const int APPT_DEFAULT_TEST_HOUR = 10;
        private const int APPT_DEFAULT_TEST_MINUTES = 0;
        public static DateTime NextMondayAt(int hour = APPT_DEFAULT_TEST_HOUR, int minute = APPT_DEFAULT_TEST_MINUTES)
        {
            var today = DateTime.Today;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            var nextMonday = today.AddDays(daysUntilMonday);
            return new DateTime(nextMonday.Year, nextMonday.Month, nextMonday.Day, hour, minute, 0, DateTimeKind.Utc);
        }

        public static string GetEnumMemberValue(Enum enumValue)
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (member != null)
            {
                var attribute = member.GetCustomAttribute<EnumMemberAttribute>();
                if (attribute != null)
                {
                    return attribute.Value;
                }
            }
            return enumValue.ToString();
        }
    }
}
