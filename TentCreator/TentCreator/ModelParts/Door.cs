using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using TentCreator.Interfaces;
using TentCreator.Enumerations;
using System.Collections.Generic;

namespace TentCreator.ModelParts
{
    /// <summary>
    /// Вход.
    /// </summary>
    public class Door : IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var doorWidth = parameters[Parameter.DoorWidth].Value/2;
            var doorHeight = parameters[Parameter.DoorHeight].Value;

            var part = (ksPart) document3D.GetPart((short) Part_Type.pNew_Part);
            if (part != null)
            {
                var sketchProperty = new KompasSketch
                    {
                        Shape = ShapeType.Line,
                        Plane = PlaneType.PlaneYOZ,
                        ReverseValue = 0.5f,
                        Operation = OperationType.BaseExtrusion,
                        DirectionType = Direction_Type.dtReverse,
                        OperationColor = Color.CadetBlue
                    };

                const float doorThickness = 0.5f;

                sketchProperty.PointsList.Add(new PointF(-doorWidth, 0));
                sketchProperty.PointsList.Add(new PointF(-doorWidth + doorThickness, 0));
                sketchProperty.PointsList.Add(new PointF(-doorWidth + doorThickness, -doorHeight + doorThickness));
                sketchProperty.PointsList.Add(new PointF(doorWidth - doorThickness, -doorHeight + doorThickness));
                sketchProperty.PointsList.Add(new PointF(doorWidth - doorThickness, 0));
                sketchProperty.PointsList.Add(new PointF(doorWidth, 0));
                sketchProperty.PointsList.Add(new PointF(doorWidth, -doorHeight));
                sketchProperty.PointsList.Add(new PointF(-doorWidth, -doorHeight));

                sketchProperty.SketchName = "Вход";
                sketchProperty.CreateNewSketch(part);
            }
        }
    }
}