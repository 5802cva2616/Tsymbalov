using System.Drawing;

namespace TentCreator
{
    /// <summary>
    /// Значения параметра.
    /// </summary>
    public class ParameterData
    {
        #region - Переменные -

        /// <summary>
        /// Название.
        /// </summary>
        public string Name;

        /// <summary>
        /// Значение.
        /// </summary>
        public float Value;

        /// <summary>
        /// Описание.
        /// </summary>
        public string Description;

        /// <summary>
        /// Диапазон допустимых значений.
        /// </summary>
        public PointF RangeValue;

        #endregion // Переменные.

        #region - Конструкторы -

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="name">Название.</param>
        /// <param name="value">Значение.</param>
        public ParameterData(string name, float value)
        {
            Initialize();

            Name = name;
            Value = value;
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="name">Название.</param>
        /// <param name="validValue">Диапазон допустимых значений.</param>
        public ParameterData(string name, PointF validValue)
        {
            Initialize();

            Name = name;
            RangeValue = validValue;
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="name">Название.</param>
        /// <param name="description">Описание.</param>
        /// <param name="value">Значение.</param>
        public ParameterData(string name, string description, float value)
        {
            Initialize();

            Name = name;
            Value = value;
            Description = description;
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="name">Название.</param>
        /// <param name="value">Значение.</param>
        /// <param name="validValue">Диапазон допустимых значений.</param>
        public ParameterData(string name, float value, PointF validValue)
        {
            Initialize();

            Name = name;
            Value = value;
            RangeValue = validValue;
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="name">Название.</param>
        /// <param name="description">Описание.</param>
        /// <param name="validValue">Диапазон допустимых значений.</param>
        public ParameterData(string name, string description, PointF validValue)
        {
            Initialize();

            Name = name;
            RangeValue = validValue;
            Description = description;
        }

        #endregion // Конструкторы.

        #region - Инициализация -

        /// <summary>
        /// Инициализирует переменные.
        /// </summary>
        private void Initialize()
        {
            Value = 0;
            RangeValue = new PointF();
            Description = string.Empty;
        }

        #endregion // Инициализация.
    }
}