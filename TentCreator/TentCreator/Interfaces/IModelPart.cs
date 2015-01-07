using Kompas6API5;
using TentCreator.Enumerations;
using System.Collections.Generic;

namespace TentCreator.Interfaces
{
    /// <summary>
    /// Интерфейс части модели.
    /// </summary>
    public interface IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters);
    }
}