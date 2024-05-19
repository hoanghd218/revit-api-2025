using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai6Geometry.CreatePilesFromCad.Model
{
    public class CadCurveModel
    {
        public string Layer { get; set; }
        public Curve Curve { get; set; }

        public CadCurveModel(Arc arc)
        {
            var graphicsStyle = arc.GraphicsStyleId.ToElement() as GraphicsStyle;
            Layer = graphicsStyle.GraphicsStyleCategory.Name;
            Curve = arc;

        }
    }
}
