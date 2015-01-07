using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using TentCreator.Interfaces;
using TentCreator.Enumerations;
using System.Collections.Generic;

namespace TentCreator.ModelParts
{
    /// <summary>
    /// Пол.
    /// </summary>
    public class Floor : IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var tentLength = parameters[Parameter.TentLength].Value;
            var floorWidth = parameters[Parameter.FloorWidth].Value/2;
            var floorLength = parameters[Parameter.FloorLength].Value;

            var part = (ksPart)document3D.GetPart((short)Part_Type.pNew_Part);
            if (part != null)
            {
                var sketchProperty = new KompasSketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneYOZ,
                    ReverseValue = (floorLength - tentLength) / 2,
                    Operation = OperationType.BaseExtrusion,
                    DirectionType = Direction_Type.dtBoth,
                    OperationColor = Color.CadetBlue
                };
                sketchProperty.NormalValue = floorLength - sketchProperty.ReverseValue;

                //const float doorThickness = 0.5f;

                sketchProperty.PointsList.Add(new PointF(-floorWidth, 1));
                sketchProperty.PointsList.Add(new PointF(floorWidth, 1));
                sketchProperty.PointsList.Add(new PointF(floorWidth, 0));
                sketchProperty.PointsList.Add(new PointF(-floorWidth, 0));
 
                sketchProperty.SketchName = "Пол";
                sketchProperty.CreateNewSketch(part);
            }
        }
    }
}