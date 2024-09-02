using System.Windows;
using MoreLinq;
using RevitAddIn1.Utils;

namespace RevitAddIn1.ThucChien.BeamRebar.Model
{
    public class BeamRebarModel
    {
        public XYZ Direction { get; set; }
        public XYZ StartPointTop { get; set; }
        public XYZ EndPointTop { get; set; }
        public XYZ StartPointBot { get; set; }
        public XYZ EndPointBot { get; set; }
        public double B { get; set; }
        public double H { get; set; }
        public XYZ XVector { get; set; }
        public XYZ ZVector { get; set; }

        public FamilyInstance Beam { get; set; }

        public double Length { get; set; }
        public Plane LeftPlane { get; set; }
        public Plane RightPlane { get; set; }
        public BeamRebarModel(FamilyInstance beam, List<FamilyInstance> columns)
        {
            Beam = beam;
            var symbol = beam.Symbol;
            var solid = symbol.GetAllSolids().OrderByDescending(x => x.Volume).FirstOrDefault();

            var bb = solid.GetBoundingBox();

            H = bb.Max.Z - bb.Min.Z;
            B = bb.Max.Y - bb.Min.Y;

            XVector = -beam.GetTransform().BasisY;
            ZVector = beam.GetTransform().BasisZ;

            var curve = (beam.Location as LocationCurve).Curve;

            var clone = curve.Clone();


            foreach (var familyInstance in columns)
            {

                var columnSolid = familyInstance.GetAllSolids().OrderByDescending(x => x.Volume).FirstOrDefault();
                var rs = columnSolid.IntersectWithCurve(clone,
                       new SolidCurveIntersectionOptions()
                       { ResultType = SolidCurveIntersectionMode.CurveSegmentsOutside });


                clone = rs.GetCurveSegment(0);
            }

            var tf = beam.GetTransform();
            var max = tf.OfPoint(bb.Max);
            var normal = tf.OfVector(XYZ.BasisY);
            var min = tf.OfPoint(bb.Min);


            LeftPlane = Plane.CreateByNormalAndOrigin(normal, max);
            RightPlane = Plane.CreateByNormalAndOrigin(normal, min);


            var sp = clone.GetEndPoint(0);
            var ep = clone.GetEndPoint(1);

            StartPointTop = sp.ProjectPointOntoPlane(LeftPlane);
            EndPointTop = ep.ProjectPointOntoPlane(LeftPlane);

            StartPointBot = new XYZ(StartPointTop.X, StartPointTop.Y, min.Z);
            EndPointBot = new XYZ(EndPointTop.X, EndPointTop.Y, min.Z);


            using (var tx= new Transaction(DocumentUtils.Document,"Create line"))
            {
                tx.Start();

                DocumentUtils.Document.Create.NewModelCurve(Line.CreateBound(StartPointTop, EndPointTop),
                    SketchPlane.Create(DocumentUtils.Document,
                        Plane.CreateByNormalAndOrigin(XYZ.BasisZ, StartPointTop)));


                tx.Commit();
            }


            Direction = (EndPointTop - StartPointTop).Normalize();


            Length = (EndPointTop - StartPointTop).GetLength();
        }
    }
}
