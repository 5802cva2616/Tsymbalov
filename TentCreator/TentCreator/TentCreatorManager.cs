using Kompas6API5;
using Kompas6Constants3D;
using System.Windows.Forms;
using TentCreator.Properties;
using TentCreator.Enumerations;
using System.Collections.Generic;

namespace TentCreator
{
    /// <summary>
    /// Содержит методы для построения детали (модели).
    /// </summary>
    public class TentCreatorManager
    {
        /// <summary>
        /// Интерфейс объекта КОМПАС.
        /// </summary>
        private readonly KompasObject _kompas;

        /// <summary>
        /// Содержит методы для построения детали (модели).
        /// </summary>
        private readonly ModelBuilder _modelBuilder;

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="kompas">Интерфейс объекта КОМПАС.</param>
        public TentCreatorManager(KompasObject kompas)
        {
            _kompas = kompas;
            _modelBuilder = new ModelBuilder();
        }

        /// <summary>
        /// Вызывает построение модели.
        /// </summary>
        /// <param name="parameters">Параметры модели.</param>
        public void BuildModel(Dictionary<Parameter, ParameterData> parameters)
        {
            var document3D = (ksDocument3D)_kompas.ActiveDocument3D();

            if (document3D == null)
            {
                var result = MessageBox.Show(Resources.CreateNewDocumentText, Resources.MainWindowTitle,
                                             MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    document3D = (ksDocument3D)_kompas.Document3D();
                    document3D.Create();
                }
                else return;
            }
            else
            {
                var result = MessageBox.Show(Resources.CreateInCurrentDocumentText, Resources.MainWindowTitle,
                                             MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    document3D = (ksDocument3D)_kompas.Document3D();
                    document3D.Create();
                }

                if (result == DialogResult.Cancel) return;
            }

            ClearDocument(document3D);

            _modelBuilder.Build(document3D, parameters);
        }

        /// <summary>
        /// Очищает 3D документ.
        /// </summary>
        private void ClearDocument(ksDocument3D document3D)
        {
            var part = (ksPart)document3D.GetPart((short)Part_Type.pTop_Part);
            var entityCollection = (ksEntityCollection)part.EntityCollection(0);

            for (int i = entityCollection.GetCount() - 1; i > 0; i--)
            {
                var entity = (ksEntity)entityCollection.GetByIndex(i);
                document3D.DeleteObject(entity);
            }
        } 
    }
}