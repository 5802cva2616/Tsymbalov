using System.ComponentModel;

namespace TentCreator.Enumerations
{
    /// <summary>
    /// Параметры модели.
    /// </summary>
    public enum Parameter
    {
        [Description("Ширина палатки")]
        TentWidth,

        [Description("Высота палатки")]
        TentHeight,

        [Description("Длина палатки")]
        TentLength,

        [Description("Ширина входа")]
        DoorWidth,

        [Description("Высота входа")]
        DoorHeight,

        [Description("Ширина окошка")]
        WindowWidth,

        [Description("Высота окошка")]
        WindowHeight,

        [Description("Количество окошек")]
        WindowCount,

        [Description("Ширина пола")]
        FloorWidth,

        [Description("Длина пола")]
        FloorLength,

        [Description("Тип крыши")]
        RoofType
    }
}