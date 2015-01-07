using System.ComponentModel;

namespace TentCreator.Enumerations
{
    /// <summary>
    /// ��������� ������.
    /// </summary>
    public enum Parameter
    {
        [Description("������ �������")]
        TentWidth,

        [Description("������ �������")]
        TentHeight,

        [Description("����� �������")]
        TentLength,

        [Description("������ �����")]
        DoorWidth,

        [Description("������ �����")]
        DoorHeight,

        [Description("������ ������")]
        WindowWidth,

        [Description("������ ������")]
        WindowHeight,

        [Description("���������� ������")]
        WindowCount,

        [Description("������ ����")]
        FloorWidth,

        [Description("����� ����")]
        FloorLength,

        [Description("��� �����")]
        RoofType
    }
}