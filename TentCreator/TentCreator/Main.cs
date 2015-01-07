using System;
using Kompas6API5;
using Microsoft.Win32;
using System.Windows.Forms;
using TentCreator.Properties;
using System.Runtime.InteropServices;

namespace TentCreator
{
    /// <summary>
    /// Основной класс программы.
    /// </summary>
    public class Main
    {
        #region - Переменные -

        /// <summary>
        /// Интерфейс объекта КОМПАС.
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Форма для ввода параметров.
        /// </summary>
        private TentCreatorWindow _tentCreatorWindow;

        #endregion // Переменные

        #region - Public методы -
        
        /// <summary>
        /// Возвращает название библиотеки.
        /// </summary>
        /// <returns>Название библиотеки.</returns>
        [return: MarshalAs(UnmanagedType.BStr)]
        public string GetLibraryName()
        {
            return Resources.LibraryName;
        }

        /// <summary>
        /// Срабатывает при выборе пункта меню.
        /// </summary>
        /// <param name="command">Номер выполняемой команды.</param>
        /// <param name="mode">Режим работы: 0 - обычный режим,
        ///                                  1 - вызов из дистрибутивной задачи в деморежиме,
        ///                                  2 - вызов из демоверсии.</param>
        /// <param name="kompas">Интерфейс KompasObject.</param>
        public void ExternalRunCommand([In] short command, [In] short mode,
                                       [In, MarshalAs(UnmanagedType.IDispatch)] object kompas) 
        {
            try
            {
                _kompas = (KompasObject)kompas;

                ExecuteCommand(command);
            }
            catch
            {
                MessageBox.Show(Resources.ConnectionErrorText);
            }
        }

        /// <summary>
        /// Формируем пункты меню.
        /// </summary>
        /// <param name="number">Счетчик строк.</param>
        /// <param name="itemType">Тип строки: 0 - разделитель;
        ///                                    1 - строка с названием пункта меню;
        ///                                    2 - начало выпадающего подменю;
        ///                                    3 - конец меню\подменю.</param>
        /// <param name="command">Номер команды.</param>
        /// <returns>Название пункта меню.</returns>
        [return: MarshalAs(UnmanagedType.BStr)] 
        public string ExternalMenuItem(short number, ref short itemType, ref short command)
        {
            string result = string.Empty;
            itemType = 1;

            switch (number)
            {
                case 1:
                    result = Resources.MainWindowTitle;
                    command = 1;
                    break;

                case 2:
                    command = -1;
                    itemType = 3;
                    break;
            }

            return result;
        }

        #endregion // Public методы.

        #region - Private методы -

        /// <summary>
        /// Выполняет комманду.
        /// </summary>
        /// <param name="command">Комманда.</param>
        private void ExecuteCommand(short command)
        {
            switch (command)
            {
                case 1:
                    {
                        ShowMainWindow();
                    }
                    break;
            }
        }

        /// <summary>
        /// Показывает основное окно программы.
        /// </summary>
        private void ShowMainWindow()
        {
            if (_tentCreatorWindow != null)
            {
                if (!_tentCreatorWindow.IsDisposed)
                {
                    _tentCreatorWindow.Focus();
                    return;
                }
            }

            _tentCreatorWindow = new TentCreatorWindow(_kompas);
            _tentCreatorWindow.Show(Control.FromHandle((IntPtr) _kompas.ksGetHWindow()));
        }

        #endregion // Private методы.
        
        #region - Регистрация библиотеки для COM -

        /// <summary>
        /// Эта функция выполняется при регистрации класса для COM
        /// Она добавляет в ветку реестра компонента раздел Kompas_Library,
        /// который сигнализирует о том, что класс является приложением Компас,
        /// а также заменяет имя InprocServer32 на полное, с указанием пути.
        /// Все это делается для того, чтобы иметь возможность подключить
        /// библиотеку на вкладке ActiveX.
        /// </summary>
        [ComRegisterFunction]
        public static void RegisterKompasLib(Type type)
        {
            try
            {
                RegistryKey registryKey = Registry.LocalMachine;
                string keyName = @"SOFTWARE\Classes\CLSID\{" + type.GUID.ToString() + "}";
                registryKey = registryKey.OpenSubKey(keyName, true);

                if (registryKey == null) return;

                registryKey.CreateSubKey("Kompas_Library");
                registryKey = registryKey.OpenSubKey("InprocServer32", true);

                if (registryKey != null)
                {
                    registryKey.SetValue(null,
                                         Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\mscoree.dll");
                    registryKey.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    string.Format("При регистрации класса для COM-Interop произошла ошибка:\n{0}", exception),
                    Resources.MainWindowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Отменяет регистрацию библиотеки.
        /// </summary>
        [ComUnregisterFunction]
        public static void UnregisterKompasLib(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine;
            string keyName = @"SOFTWARE\Classes\CLSID\{" + type.GUID.ToString() + "}";
            RegistryKey subKey = registryKey.OpenSubKey(keyName, true);

            if (subKey != null)
            {
                subKey.DeleteSubKey("Kompas_Library");
                subKey.Close();
            }
        }

        #endregion // Регистрация библиотеки для COM.
    }
}
