using System.Runtime.Serialization;

namespace MedicalAppts.Core.Enums
{
    public enum MedicalSpecialty
    {
        [EnumMember(Value = nameof(CARDIOLOGIST))]
        CARDIOLOGIST,
        [EnumMember(Value = nameof(CLINICAL))]
        CLINICAL,
        [EnumMember(Value = nameof(GASTROENTEROLOGIST))]
        GASTROENTEROLOGIST,
        [EnumMember(Value = nameof(NEUROLOGIST))]
        NEUROLOGIST,
        [EnumMember(Value = nameof(DERMATOLOGIST))]
        DERMATOLOGIST,
        [EnumMember(Value = nameof(ORTHOPEDIC))]
        ORTHOPEDIC,
        [EnumMember(Value = nameof(PEDIATRICIAN))]
        PEDIATRICIAN,
        [EnumMember(Value = nameof(PSYCHIATRIST))]
        PSYCHIATRIST
    }
}
