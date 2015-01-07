using System.Drawing;
using TentCreator.Enumerations;
using System.Collections.Generic;

namespace TentCreator
{
    /// <summary>
    /// Содержит параметры модели.
    /// </summary>
    public class ModelParameters
    {
        /// <summary>
        /// Словарь параметров.
        /// </summary>
        public Dictionary<Parameter, ParameterData> Parameters { get; private set; } 
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ModelParameters()
        {
            Initialize();
        }

        /// <summary>
        /// Инициализирует переменные.
        /// </summary>
        private void Initialize()
        {
            Parameters = new Dictionary<Parameter, ParameterData>
                {
                    {Parameter.TentWidth, new ParameterData(Parameter.TentWidth.ToString(), 40, new PointF(40, 100))},
                    {Parameter.TentHeight, new ParameterData(Parameter.TentHeight.ToString(), 35, new PointF(35, 100))},
                    {Parameter.TentLength, new ParameterData(Parameter.TentLength.ToString(), 40, new PointF(40, 100))},
                    {Parameter.DoorWidth, new ParameterData(Parameter.DoorWidth.ToString(), 10, new PointF(8, 100))},
                    {Parameter.DoorHeight, new ParameterData(Parameter.DoorHeight.ToString(), 20, new PointF(20, 39))},
                    {Parameter.WindowWidth, new ParameterData(Parameter.WindowWidth.ToString(), 10, new PointF(6, 100))},
                    {Parameter.WindowHeight, new ParameterData(Parameter.WindowHeight.ToString(), 8, new PointF(6, 100))},
                    {Parameter.WindowCount, new ParameterData(Parameter.WindowCount.ToString(), 2, new PointF(1, 2))},
                    {Parameter.FloorWidth, new ParameterData(Parameter.FloorWidth.ToString(), 42, new PointF(12, 102))},
                    {Parameter.FloorLength, new ParameterData(Parameter.FloorLength.ToString(), 42, new PointF(27, 102))},
                    {Parameter.RoofType, new ParameterData(Parameter.RoofType.ToString(), 1, new PointF(0, 1))},
                };
        }
       
        /// <summary>
        /// Проверяет корректность введенных данных.
        /// </summary>
        /// <param name="parameters">Словарь параметров для проверки.</param>
        /// <returns>Список ошибок.</returns>
        public List<string> CheckData(Dictionary<Parameter, ParameterData> parameters)
        {
            var errorList = new List<string>();

            foreach (KeyValuePair<Parameter, ParameterData> parameter in parameters)
            {
                switch (parameter.Key)
                {
                    case Parameter.TentWidth:     //Ширина
                        {
                            SetMaxValue(Parameter.DoorWidth, parameter.Value.Value / 3);
                            SetMaxValue(Parameter.WindowWidth, parameter.Value.Value / 3);
                            SetMinValue(Parameter.FloorWidth, parameter.Value.Value + 1);
                            SetMaxValue(Parameter.FloorWidth, parameter.Value.Value + 3);
                        }
                        break;

                    case Parameter.TentHeight:  //Высота
                        {
                            SetMaxValue(Parameter.DoorHeight, (int)(parameter.Value.Value / 1.6f));
                            SetMaxValue(Parameter.WindowHeight, (int)(parameter.Value.Value / 1.6f) / 2);
                        }
                        break;

                        case Parameter.TentLength:  //Длина
                        {
                            SetMinValue(Parameter.FloorLength, parameter.Value.Value + 1);
                            SetMaxValue(Parameter.FloorLength, parameter.Value.Value + 3);
                        }
                        break;
                }

                var value = parameter.Value.Value;
                var validValue = GetValidValue(parameter.Key);

                if (validValue == null) continue;

                if (!(value >= validValue.RangeValue.X && value <= validValue.RangeValue.Y))
                {
                    if (validValue.RangeValue.X <= validValue.RangeValue.Y)
                    {
                        errorList.Add("Значение параметра '" + parameter.Value.Description +
                                      "', должно лежать в диапазоне от " + validValue.RangeValue.X + " до " +
                                      validValue.RangeValue.Y + ".\n");
                    }
                }
            }

            return errorList;
        }

        /// <summary>
        /// Возвращает допустимые значения.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>Допустимое значение.</returns>
        private ParameterData GetValidValue(Parameter parameter)
        {
            if (Parameters.ContainsKey(parameter))
            {
                return Parameters[parameter];
            }

            return null;
        }

        /// <summary>
        /// Задает новое максимальное значение параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="maxValue">Новое значение.</param>
        private void SetMaxValue(Parameter parameter, float maxValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(currentParameter.RangeValue.X, maxValue));
            }
        }

        /// <summary>
        /// Задает новое минимальное значение параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="minValue">Новое значение.</param>
        private void SetMinValue(Parameter parameter, float minValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(minValue, currentParameter.RangeValue.Y));
            }
        }

        /// <summary>
        /// Задает новый диапазон значений параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="minValue">Минимальное значение.</param>
        /// <param name="maxValue">Максимальное значение.</param>
        private void SetRange(Parameter parameter, float minValue, float maxValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(minValue, maxValue));
            }
        }
    }
}