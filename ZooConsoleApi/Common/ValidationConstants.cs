namespace ZooConsoleAPI.Common
{
    public class ValidationConstants
    {
        public const int NameMaxLength = 40;
        public const string CatalogNumberFormat = @"^([0-9A-Z]{12})$";
        public const int BreedMaxLength = 60;
        public const int TypeMaxLength = 30;
        public const int AgeMinValue = 0;
        public const int AgeMaxValue = 120;
        public const int GenderMaxLength = 10;
    }
}
