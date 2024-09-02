using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.Utils
{
    public static class PlaneUtils
    {
        public static XYZ ProjectPointOntoPlane(this XYZ point, Plane plane)
        {
            // Extract the normal and origin from the plane
            XYZ planeNormal = plane.Normal;
            XYZ planeOrigin = plane.Origin;

            // Vector from the plane's origin to the point
            XYZ vectorFromPlaneToPoint = point - planeOrigin;

            // Compute the distance from the point to the plane along the normal direction
            double distanceToPlane = vectorFromPlaneToPoint.DotProduct(planeNormal);

            // Compute the projection of the point onto the plane
            XYZ projectedPoint = point - distanceToPlane * planeNormal;

            return projectedPoint;
        }
    }
}
