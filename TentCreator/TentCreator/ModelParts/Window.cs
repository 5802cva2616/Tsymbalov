using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using TentCreator.Interfaces;
using TentCreator.Enumerations;
using System.Collections.Generic;

namespace TentCreator.ModelParts
{
    /// <summary>
    /// Окна.
    /// </summary>
    public class Window : IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var tentWidth = parameters[Parameter.TentWidth].Value/2;
            var tentCenterL = parameters[Parameter.TentLength].Value/2;
            var tentCenterH = (parameters[Parameter.TentHeight].Value/1.6f)/2;
            var windowWidth = parameters[Parameter.WindowWidth].Value / 2;
            var windowHeight = parameters[Parameter.WindowHeight].Value / 2;
            var windowCount = (int) parameters[Parameter.WindowCount].Value;

            var part = (ksPart)document3D.GetPart((short)Part_Type.pNew_Part);
            if (part != null)
            {
                const float thickness = 0.5f;
                var sketchProperty = new KompasSketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneXOY,
                    NormalValue = 0.5f,
                    Operation = OperationType.BaseExtrusion,
                    DirectionType = Direction_Type.dtNormal,
                    OperationColor = Color.CadetBlue
                };

                if (windowCount == 2)
                {
                    var sketchProperty2 = new KompasSketch
                    {
                        Shape = ShapeType.Line,
                        Plane = PlaneType.PlaneXOY,
                        ReverseValue = 0.5f,
                        Operation = OperationType.BaseExtrusion,
                        DirectionType = Direction_Type.dtReverse,
                        OperationColor = Color.CadetBlue
                    };

                    // Внешняя рама.
                    sketchProperty2.PointsList.Add(new PointF(-(tentCenterL + windowWidth), tentCenterH + windowHeight));
                    sketchProperty2.PointsList.Add(new PointF(-(tentCenterL - windowWidth), tentCenterH + windowHeight));
                    sketchProperty2.PointsList.Add(new PointF(-(tentCenterL - windowWidth), tentCenterH - windowHeight));
                    sketchProperty2.PointsList.Add(new PointF(-(tentCenterL + windowWidth), tentCenterH - windowHeight));

                    sketchProperty2.SketchName = "Окошко2";
                    sketchProperty2.CreateNewOffsetSketch(part, tentWidth, true);

                }

                // Внешняя рама.
                sketchProperty.PointsList.Add(new PointF(-(tentCenterL + windowWidth), tentCenterH + windowHeight));
                sketchProperty.PointsList.Add(new PointF(-(tentCenterL - windowWidth), tentCenterH + windowHeight));
                sketchProperty.PointsList.Add(new PointF(-(tentCenterL - windowWidth), tentCenterH - windowHeight));
                sketchProperty.PointsList.Add(new PointF(-(tentCenterL + windowWidth), tentCenterH - windowHeight));

                sketchProperty.SketchName = "Окошко1";
                sketchProperty.CreateNewOffsetSketch(part, tentWidth, true);

                var sketchCutProperty = new KompasSketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneXOY,
                    NormalValue = 1f,
                    Operation = OperationType.CutExtrusion,
                    DirectionType = Direction_Type.dtNormal,
                    EndType = windowCount == 1 ? End_Type.etBlind : End_Type.etThroughAll,
                    OperationColor = Color.CadetBlue,
                };

                // Левая половина окошка.
                sketchCutProperty.PointsList.Add(new PointF(-(tentCenterL + windowWidth - thickness),
                    tentCenterH + windowHeight - thickness));
                sketchCutProperty.PointsList.Add(new PointF(-(tentCenterL + 0.2f),
                    tentCenterH + windowHeight - thickness));
                sketchCutProperty.PointsList.Add(new PointF(-(tentCenterL + 0.2f),
                    tentCenterH - windowHeight + thickness));
                sketchCutProperty.PointsList.Add(new PointF(-(tentCenterL + windowWidth - thickness),
                    tentCenterH - windowHeight + thickness));

                sketchCutProperty.AddBreakPoint();

                sketchCutProperty.SketchName = "Левая Рамка";
                sketchCutProperty.CreateNewOffsetSketch(part, tentWidth + 0.5f, true);

                var sketchCutProperty2 = new KompasSketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneXOY,
                    NormalValue = 1f,
                    Operation = OperationType.CutExtrusion,
                    DirectionType = Direction_Type.dtNormal,
                    EndType = windowCount == 1 ? End_Type.etBlind : End_Type.etThroughAll,
                    OperationColor = Color.CadetBlue,
                };

                // Правая половина окошка.
                sketchCutProperty2.PointsList.Add(new PointF(-(tentCenterL - 0.2f),
                    tentCenterH + windowHeight - thickness));
                sketchCutProperty2.PointsList.Add(new PointF(-(tentCenterL - windowWidth + thickness),
                    tentCenterH + windowHeight - thickness));
                sketchCutProperty2.PointsList.Add(new PointF(-(tentCenterL - windowWidth + thickness),
                    tentCenterH - windowHeight + thickness));
                sketchCutProperty2.PointsList.Add(new PointF(-(tentCenterL - 0.2f),
                    tentCenterH - windowHeight + thickness));

                sketchCutProperty2.AddBreakPoint();

                sketchCutProperty2.SketchName = "Правая Рамка";
                sketchCutProperty2.CreateNewOffsetSketch(part, tentWidth + 0.5f, true);
            }
        }
    }
}