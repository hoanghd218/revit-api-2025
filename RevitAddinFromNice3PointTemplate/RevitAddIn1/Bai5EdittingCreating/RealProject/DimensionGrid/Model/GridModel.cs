namespace RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.Model
{
    public class GridModel
    {
        public bool IsHorizontalGrid { get; set; } = true;
        public Grid Grid { get; set; }

        public XYZ SP { get; set; }
        public XYZ EP { get; set; }
        public GridModel(Grid grid)
        {
            Grid = grid;

            var gridCurve = grid.Curve;

            var sp = gridCurve.GetEndPoint(0);
            var end = gridCurve.GetEndPoint(1);


            SP = sp;
            EP = end;
            var direction = end - sp;
            if (Math.Abs(direction.CrossProduct(XYZ.BasisX).GetLength()) < 0.0001)
            {
                IsHorizontalGrid = true;
            }
            else
            {
                IsHorizontalGrid = false;
            }

        }
    }
}
