using System;
using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using TentCreator.Interfaces;
using TentCreator.Enumerations;
using System.Collections.Generic;

namespace TentCreator.ModelParts
{
    /// <summary>
    /// Палатка.
    /// </summary>
    public class ModelBody : IModelPart
    {
        int RoofTypee = 1;
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var tentWidth = parameters[Parameter.TentWidth].Value/2;
            var tentHeight = -parameters[Parameter.TentHeight].Value;
            var tentLength = parameters[Parameter.TentLength].Value;
            var typeRoof = parameters[Parameter.RoofType].Value;

            var part = (ksPart)document3D.GetPart((short)Part_Type.pNew_Part);
            if (part != null)
            {
                var sketchProperty = new KompasSketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneYOZ,
                    NormalValue = tentLength,
                    Operation = OperationType.BaseExtrusion,
                    DirectionType = Direction_Type.dtNormal,
                    OperationColor = Color.CadetBlue
                };

                sketchProperty.PointsList.Add(new PointF(-tentWidth, 0));   // Лево низ.
                sketchProperty.PointsList.Add(new PointF(tentWidth, 0));    // Право низ.
                sketchProperty.PointsList.Add(new PointF(tentWidth, tentHeight / 1.6f));    // Право верх.
                
                //if (Math.Round(parameters[Parameter.RoofType].Value) == 1)
                if (typeRoof == RoofTypee)
                {
                    var sketchProp = new KompasSketch
                    {
                        Shape = ShapeType.Arc,
                        Plane = PlaneType.PlaneYOZ,
                        NormalValue = tentLength,
                        Operation = OperationType.BaseExtrusion,
                        DirectionType = Direction_Type.dtNormal,
                        OperationColor = Color.CadetBlue
                    };

                    sketchProp.PointsList.Add(new PointF(-tentWidth, tentHeight / 1.6f)); // (0) Лево.
                    sketchProp.PointsList.Add(new PointF(tentWidth, tentHeight / 1.6f));   // (1) Право.
                    sketchProp.PointsList.Add(new PointF(0, tentHeight / 1.6f)); // (2) Центр.
                    sketchProp.SketchName = "Крыша";
                    sketchProp.CreateNewSketch(part);
                }
                else
                {
                    sketchProperty.PointsList.Add(new PointF(0, tentHeight));
                }

                sketchProperty.PointsList.Add(new PointF(-tentWidth, tentHeight / 1.6f));   // Лево верх.
                sketchProperty.SketchName = "Палатка";
                sketchProperty.CreateNewSketch(part);

                var sketchOffsetProperty = new KompasSketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneYOZ,
                    ReverseValue = tentLength - 0.4f,
                    Operation = OperationType.CutExtrusion,
                    DirectionType = Direction_Type.dtReverse,
                    OperationColor = Color.CadetBlue
                };

                sketchOffsetProperty.PointsList.Add(new PointF(-tentWidth + 0.2f, 0 - 0.2f));   // Лево низ.
                sketchOffsetProperty.PointsList.Add(new PointF(tentWidth - 0.2f, 0 - 0.2f));    // Право низ.
                sketchOffsetProperty.PointsList.Add(new PointF(tentWidth - 0.2f, tentHeight/1.6f + 0.2f));    // Право верх.
                sketchOffsetProperty.PointsList.Add(new PointF(-tentWidth + 0.2f, tentHeight/1.6f + 0.2f));   // Лево верх.
                sketchOffsetProperty.SketchName = "Полость";
                sketchOffsetProperty.CreateNewOffsetSketch(part, 0.2f, true);

                
                
            }
        }
    }
}