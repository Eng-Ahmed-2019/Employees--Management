namespace Application.Security
{
    public static class NationalIdValidator
    {
        public static bool IsValid(string nationalId)
        {
            if (string.IsNullOrEmpty(nationalId) || nationalId.Length != 14)
                return false;

            if (!long.TryParse(nationalId, out _))
                return false;

            int century = nationalId[0] == '2' ? 1900 : nationalId[0] == '3' ? 2000 : 0;
            if (century == 0) return false;

            int year = int.Parse(nationalId.Substring(1, 2));
            int month = int.Parse(nationalId.Substring(3, 2));
            int day = int.Parse(nationalId.Substring(5, 2));
            int fullYear = century + year;

            try
            {
                var birthDate = new DateTime(fullYear, month, day);
            }
            catch
            {
                return false;
            }

            int governorate = int.Parse(nationalId.Substring(7, 2));
            if (governorate < 1 || governorate > 29)
                return false;

            int sum = 0;
            for (int i = 0; i < 13; i++)
            {
                int digit = int.Parse(nationalId[i].ToString());
                sum += digit * (14 - i);
            }
            int checksum = sum % 11 % 10;

            int lastDigit = int.Parse(nationalId[13].ToString());
            if (checksum != lastDigit)
                return false;

            return true;
        }

        public static string GetGender(string nationalId)
        {
            if (!IsValid(nationalId))
                throw new ArgumentException("الرقم القومي غير صالح");

            int genderDigit = int.Parse(nationalId.Substring(12, 1));
            return genderDigit % 2 == 0 ? "Female" : "Male";
        }

        public static DateTime GetBirthDate(string nationalId)
        {
            if (!IsValid(nationalId))
                throw new ArgumentException("الرقم القومي غير صالح");

            int century = nationalId[0] == '2' ? 1900 : 2000;
            int year = int.Parse(nationalId.Substring(1, 2));
            int month = int.Parse(nationalId.Substring(3, 2));
            int day = int.Parse(nationalId.Substring(5, 2));

            return new DateTime(century + year, month, day);
        }
    }
}