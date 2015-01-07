using System;
using Kompas6API5;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using TentCreator.Properties;
using TentCreator.Converters;
using TentCreator.Enumerations;
using System.Collections.Generic;

using System.Text.RegularExpressions;

namespace TentCreator
{
    /// <summary>
    /// Основное окно.
    /// </summary>
    public partial class TentCreatorWindow : Form
    {
        #region - Переменные -

        /// <summary>
        /// Содержит методы для работы с параметрами модели.
        /// </summary>
        private ModelParameters _modelParameters;

        /// <summary>
        /// Содержит методы для построения модели.
        /// </summary>
        private readonly TentCreatorManager _tentCreatorManager;

        /// <summary>
        /// Список Control'ов.
        /// </summary>
        private Dictionary<Parameter, Control> _controlsDictionary;
        
        #endregion // Переменные.
        
        #region - Конструктор -

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="kompas">Интерфейс объекта КОМПАС.</param>
        public TentCreatorWindow(KompasObject kompas)
        {
            InitializeComponent();
            InitializeVariables();

            _tentCreatorManager = new TentCreatorManager(kompas);
        }

        #endregion // Конструктор.
        
        #region - Инициализация -

        /// <summary>
        ///  Инициализирует переменные.
        /// </summary>
        private void InitializeVariables()
        {
            Text = Resources.MainWindowTitle;

            _modelParameters = new ModelParameters();
            _controlsDictionary = new Dictionary<Parameter, Control>();

            var labels =
                Controls.Cast<object>().Where(control => control.GetType().Name == "Label").Cast<Label>().ToList();
            labels = new List<Label>(labels.OrderBy(label => label.Tag));
            var textBoxes =
                Controls.Cast<object>().Where(control => control.GetType().Name == "TextBox").Cast<TextBox>().ToList();
            textBoxes = new List<TextBox>(textBoxes.OrderBy(textBox => textBox.Tag));

            var comboBoxes =
                Controls.Cast<object>().Where(control => control.GetType().Name == "ComboBox").Cast<ComboBox>().ToList();
            comboBoxes = new List<ComboBox>(comboBoxes.OrderBy(comboBox => comboBox.Tag));

            var parameters = _modelParameters.Parameters.Keys.ToList();
            var parametersData = _modelParameters.Parameters.Values.ToList();

            int textBoxesIndex = 0;
            int comboBoxesIndex = 0;

            for (int i = 0; i < parametersData.Count; i++)
            {
                if (i < labels.Count)
                {
                    labels[i].Text = parameters[i].GetDescription();
                }
                Control control;

                if (parameters[i] == Parameter.RoofType)
                {
                    control = comboBoxes[comboBoxesIndex];
                    comboBoxes[comboBoxesIndex].SelectedIndex = 0;
                    comboBoxesIndex++;
                }
                else
                {
                    control = textBoxes[textBoxesIndex];
                    textBoxes[textBoxesIndex].Text = 
                        parametersData[i].Value.ToString(CultureInfo.InvariantCulture).Replace(".",",");

                  
                    textBoxesIndex++;
                }

                _controlsDictionary.Add(parameters[i], control);
            }
        }

        #endregion // Инициализация.

        #region - Private методы -

        /// <summary>
        /// Строит модель.
        /// </summary>
        private void BuildModel()
        {
            var parameters = GetModelParameters();

            if (parameters == null) return;

            // Отправляем параметры на проверку.
            var errorsList = _modelParameters.CheckData(parameters);

            // Если все поля заполнены верно.
            if (errorsList.Count == 0)
            {
                _tentCreatorManager.BuildModel(parameters);
            }
            else
            {
                var messageText = errorsList.Aggregate(string.Empty, (current, message) => current + (message + "\n"));
                MessageBox.Show(messageText, Resources.MainWindowTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Получает параметры модели.
        /// </summary>
        /// <returns>Параметры модели.</returns>
        private Dictionary<Parameter, ParameterData> GetModelParameters()
        {
            var parameters = new Dictionary<Parameter, ParameterData>();

            foreach (KeyValuePair<Parameter, Control> parameter in _controlsDictionary)
            {
                var comboBox = parameter.Value as ComboBox;
                double? value = comboBox != null ? comboBox.SelectedIndex : GetParameterValue(parameter.Value.Text); 
                if (value == null)
                {
                    MessageBox.Show(@"Поле '" + parameter.Key.GetDescription() + @"' имеет неверное значение.",
                                    Resources.MainWindowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return null;
                }

                parameters.Add(parameter.Key,
                               new ParameterData(parameter.Key.ToString(), parameter.Key.GetDescription(),
                                                 (float) value));
            }

            return parameters;
        }

        /// <summary>
        /// Преобразует текстовое значение поля в числовое.
        /// </summary>
        /// <param name="text">Текстовое значение.</param>
        /// <returns>Числовое значение.</returns>
        private double? GetParameterValue(string text)
        {
            try
            {
                return Convert.ToDouble(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion // Private методы.
        
        #region - Методы контролов на форме -

        /// <summary>
        /// Возникает в момент нажатия на кнопку.
        /// </summary>
        private void BuildButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) return;

            button.Enabled = false;
            BuildModel();
            button.Enabled = true;
        }

        /// <summary>
        /// Переменная для ограничения ввода
        /// </summary>
        private bool _nonNumberEntered;

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            _nonNumberEntered = false;

            var substr = sender.ToString().Substring(36);
            var length = (substr.Length);
            var lastIndex = substr.LastIndexOf(',', length - 1);
            if (lastIndex + 1 != 0)
            {
                // Определяет нажата ли клавиша BACKSPACE.
                if (e.KeyCode != Keys.Back)
                {
                    // Была нажата не цифра.
                    // Установить флаг true и продолжить проверить событие KeyPress.
                    if (length - 1 > lastIndex)
                    {
                        _nonNumberEntered = true;
                    }
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var regex = new Regex(@"[\b]|[0-9]");  
            // Если символ не числовой.
            if (!regex.IsMatch(e.KeyChar.ToString(CultureInfo.InvariantCulture)))
            {
                _nonNumberEntered = true;
            }
            // Если запятая не первая и одна.
            if (!string.IsNullOrEmpty(sender.ToString().Substring(36)) &&
                e.KeyChar == ',' && !sender.ToString().Substring(36).Contains(','))
            {
                return;
            }
            // Проверить флаг прежде чем произойдет событие KеyDown.
            if (_nonNumberEntered)
            {
                // Остановить символ от введения, т.к. он не является числом.
                e.Handled = true;
            }
        }

        /// <summary>
        /// Возникает в момент потери фокуса ввода.
        /// </summary>
        private void TextBox_Leave(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox == null) return;

            double result;
            textBox.Text = textBox.Text.Replace(".", ",");

            if (double.TryParse(textBox.Text, out result) == false)
            {
                MessageBox.Show(
                    @"Убедитесь, что вы ввели корректное значение параметра.",
                    Resources.MainWindowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                textBox.Focus();
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var isEnabled = comboBox.SelectedIndex == 0;
            }
        }

        #endregion // Методы контролов на форме.



    }
}
