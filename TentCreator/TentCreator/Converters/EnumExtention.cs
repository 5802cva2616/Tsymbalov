using System;
using System.Linq;
using System.ComponentModel;

namespace TentCreator.Converters
{
    /// Статический класс, который получает знаечения из параметров.
    public static class EnumExtention
    {        
        /// <summary>
        /// Получает описание элемента Enum из атрибута Description.
        /// </summary>
        /// <param name="enumObj">Элемент Enum.</param>
        /// <returns>
        /// Возвращает описание из аттрибута Description или если оно не задано название элемента.
        /// </returns>
        public static string GetDescription(this Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

            var descriptionAttribute =
                attribArray.FirstOrDefault(item => item is DescriptionAttribute) as DescriptionAttribute;

            return descriptionAttribute != null ? descriptionAttribute.Description : enumObj.ToString();
        }
    }
}