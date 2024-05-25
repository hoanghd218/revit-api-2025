using System.Data.Common;

namespace RevitAddIn1.ThucChien.ColumnRebar.Model
{
    public class ColumnRebarModel
    {
        public XYZ A { get; set; }
        public XYZ B { get; set; }
        public XYZ C { get; set; }
        public XYZ D { get; set; }
        public double BotElevation { get; set; }
        public double TopElevation { get; set; }

        public XYZ XVector { get; set; }
        public XYZ YVector { get; set; }

        public Transform Transform { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public FamilyInstance Column { get; set; }
        public ColumnRebarModel(FamilyInstance column)
        {
            Column= column;
            var type = column.Symbol;
            Width = type.LookupParameter("b").AsDouble();
            Height = type.LookupParameter("h").AsDouble();

            Transform = column.GetTransform();

            A = Transform.OfPoint(new XYZ(-Width / 2, Height / 2, 0));
            B = Transform.OfPoint(new XYZ(Width / 2, Height / 2, 0));
            C = Transform.OfPoint(new XYZ(Width / 2, -Height / 2, 0));
            D = Transform.OfPoint(new XYZ(-Width / 2, -Height / 2, 0));
            XVector = Transform.OfVector(XYZ.BasisX);
            YVector = Transform.OfVector(XYZ.BasisY);
            var bb = column.get_BoundingBox(null);

            TopElevation = bb.Max.Z;
            BotElevation = bb.Min.Z;
        }
    }
}
